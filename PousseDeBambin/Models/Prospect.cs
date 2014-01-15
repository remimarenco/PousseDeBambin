using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PousseDeBambin.Models
{
    [Table("Prospect")]
    public class Prospect
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Votre adresse email est requise")]
        [EmailAddress]
        [Display(Name = "Votre Adresse Mail")]
        public string EmailAddress { get; set; }

        [StringLength(30, ErrorMessage = "Votre prénom ne peut excéder 30 caractères")]
        [Display(Name = "Votre Prénom")]
        public string FirstName { get; set; }

        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Votre retour, vos impressions...ou une envie de nous parler ?")]
        public string Comment { get; set; }
    }
}
