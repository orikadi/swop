using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace swop.Models
{
    public class ApartmentScore
    {
        [Key]
        public int ApartmentScoreId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public User ScoreByUser { get; set; }
        [Range(1,5)]
        public double Score { get; set; }
    }
}