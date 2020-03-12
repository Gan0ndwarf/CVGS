using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace CVGS.Models
{
    public class MySignInManager : SignInManager<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CVGSContext _db;
        private readonly IHttpContextAccessor _contextAccessor;

        public MySignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<SignInManager<ApplicationUser>> logger, CVGSContext dbContext, IHttpContextAccessor httpContextAccessor, IOptions<IdentityOptions> optionsAccessor = null, IAuthenticationSchemeProvider scheme = null)
                : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, scheme)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _httpContextAccessor = httpContextAccessor;
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            SqlConnection SqlConn = new SqlConnection("Data Source=.;Initial Catalog=CVGS;Integrated Security=True");
            SqlCommand sqlcomm = new SqlCommand("uspLogin", SqlConn);

            try
            {
                SqlConn.Open();
                sqlcomm.CommandType = CommandType.StoredProcedure;

                SqlParameter userNameParam = new SqlParameter("@pUsername", SqlDbType.NVarChar);
                userNameParam.Direction = ParameterDirection.Input;
                userNameParam.Size = 50;
                userNameParam.Value = userName;
                sqlcomm.Parameters.Add(userNameParam);

                SqlParameter passwordParam = new SqlParameter("@pPassword", SqlDbType.NVarChar);
                passwordParam.Direction = ParameterDirection.Input;
                passwordParam.Size = 50;
                passwordParam.Value = password;
                sqlcomm.Parameters.Add(passwordParam);

                SqlParameter messageParam = new SqlParameter("@responseMessage", SqlDbType.NVarChar);
                messageParam.Direction = ParameterDirection.Output;
                messageParam.Size = 250;
                messageParam.Value = null;
                sqlcomm.Parameters.Add(messageParam);

                SqlParameter idParam = new SqlParameter("@id", SqlDbType.NVarChar);
                idParam.Direction = ParameterDirection.Output;
                idParam.Size = 250;
                idParam.Value = null;
                sqlcomm.Parameters.Add(idParam);

                sqlcomm.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            User user = null;
            int? userId = null;

            if (sqlcomm.Parameters["@id"].Value.GetType() != typeof(DBNull))
            {
                userId = Convert.ToInt32(sqlcomm.Parameters["@id"].Value);
            }
                

            if (userId != null)
            {

                IQueryable<User> myUsers = from u in _db.User
                                           where u.UserId == userId
                                           select u;

                user = myUsers.FirstOrDefault();

                if (user.EmployeeFlag)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("isEmp", true.ToString());
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetString("isEmp", false.ToString());
                }
                _httpContextAccessor.HttpContext.Session.SetString("userId", user.UserId.ToString());

                List<Game> games = new List<Game>();
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("cart", games);
                _httpContextAccessor.HttpContext.Session.SetString("cartCount", games.Count().ToString());

                _httpContextAccessor.HttpContext.Session.SetString("subTotal", "0.00");
                _httpContextAccessor.HttpContext.Session.SetString("tax", "0.00");
                _httpContextAccessor.HttpContext.Session.SetString("total", "0.00");
            }

            return base.PasswordSignInAsync(userName, password, isPersistent, true);

        }


        public override async Task SignOutAsync()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

            await base.SignOutAsync();

        }
    }
}
