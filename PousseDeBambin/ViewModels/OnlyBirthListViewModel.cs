using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PousseDeBambin.ViewModels
{
    public class OnlyBirthListViewModel
    {
        [Required(ErrorMessage = "La date de naissance de l'enfant est obligatoire")]
        [Display(Name = "Arrivée de la future pousse de bambin")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
    }
}