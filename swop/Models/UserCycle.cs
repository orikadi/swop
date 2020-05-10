using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace swop.Models
{
    public class UserCycle
    {
        [Key]
        public int UserCycleId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Cycle")]
        public int CycleId { get; set; }
        public Cycle Cycle { get; set; }
        public bool IsLocked { get; set; }
    }
}