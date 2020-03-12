using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class Event
    {
        public Event()
        {
            UserEvent = new HashSet<UserEvent>();
        }

        public int EventId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? Time { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }
        public string Description { get; set; }

        public ICollection<UserEvent> UserEvent { get; set; }
    }
}
