using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PousseDeBambin.Models
{
    [Table("Gift")]
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }

        [Required(ErrorMessage = "Le nom de l'objet est obligatoire")]
        [StringLength(100, ErrorMessage="Le nom de l'objet ne doit pas dépasser 100 caractères")]
        [Display(Name = "Nom de l'objet")]
        public string Name {get; set;}

        [StringLength(1000, ErrorMessage = "La description ne peut dépasser 1000 caractères")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description de l'objet")]
        public string Description {get; set;}

        [Required(ErrorMessage = "Le prix de l'objet est obligatoire et est un nombre")]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix de l'objet")]
        public double Price {get; set;}

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Adresse de l'image du cadeau (url)")]
        public string ImageUrl {get; set;}

        [Required(ErrorMessage = "L'adresse internet de l'objet est obligatoire")]
        [StringLength(1000, ErrorMessage = "L'adresse de la page internet du cadeau ne peut excéder 1000 caractères")]
        [DataType(DataType.Url)]
        [Display(Name = "Adresse de la page internet du cadeau (url)")]
        public string WebSite {get; set;}

        public int ListID { get; set; }
        public virtual List List { get; set; }
    }
}
