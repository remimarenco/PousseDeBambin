using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PousseDeBambin.Models
{
    [Table("CloseRelative")]
    public class CloseRelative
    {
        [Key]
        public int CloseRelativeId { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public string Address { get; set; }

        public string Address_plus { get; set; }

        public string Town { get; set; }

        [DataType(DataType.PostalCode)]
        public string Country { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistredDate { get; set; }

        [ForeignKey("Gift")]
        public int GiftID { get; set; }
        public virtual Gift Gift { get; set; }
    }
}