using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PousseDeBambin.ViewModels
{
    public class InfosListViewModel
    {
        public int ListId { get; set; }

        [Required(ErrorMessage = "Le nom de la liste est obligatoire")]
        [Display(Name = "Nom de la liste")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "La description ne peut dépasser 1000 caractères")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}