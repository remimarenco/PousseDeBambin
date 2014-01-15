using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PousseDeBambin.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [Required]
        [Display(Name = "Numéro et nom de la rue")]
        public string Street { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Code postal")]
        public int Zipcode { get; set; }

        [Required]
        [Display(Name = "Ville")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Pays")]
        public string Country { get; set; }

        //public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}