using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace swop.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; } //foreign key
        public User User { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int State { get; set; } // 0-ongoing, 1-completed, 2-failed
    }
}