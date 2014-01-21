using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PousseDeBambin.Models
{
    [Table("List")]
    public class List
    {
        public int ListId { get; set; }

        [Required(ErrorMessage = "Le nom de la liste est obligatoire")]
        [Display(Name = "Nom de la liste")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "La description ne peut dépasser 1000 caractères")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime BeginningDate { get; set; }
        
        [Required (ErrorMessage = "La date de naissance de l'enfant est obligatoire")]
        [Display(Name = "Livraison du futur bébé par une cigogne (date de naissance)")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        
        /* TODO: La liste appartiendra à un ou l'utilisateur anonyme */
        public virtual ApplicationUser UserProfile {get; set;}

        public virtual ICollection<Gift> Gifts {get; set;}
    }
}