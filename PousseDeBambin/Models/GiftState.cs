using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PousseDeBambin.Models
{
    public enum State
    {
        BOUGHT, NOT_BOUGHT
    }

    [Table("GiftState")]
    public class GiftState
    {
        [Key]
        public int GiftStateID { get; set; }

        [ForeignKey("Gift")]
        public int GiftID { get; set; }
        public virtual Gift Gift { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string BuyerName { get; set; }

        [DataType(DataType.MultilineText)]
        public string BuyerText { get; set; }
    }
}