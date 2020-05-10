using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace swop.Models
{
    public class Cycle
    {
        [Key]
        public int CycleId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<UserCycle> UserCycles { get; set; } //many to many
    }
}