using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PousseDeBambin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<List> Lists { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit être long d'au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation du nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne sont pas identiques.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit être long d'au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation du nouveau mot de passe")]
        [Compare("Password", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne sont pas identiques.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Votre prénom est obligatoire")]
        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Votre nom est obligatoire")]
        [DataType(DataType.Text)]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "L'adresse mail est obligatoire")]
        [EmailAddress]
        [Display(Name = "Votre Adresse Mail")]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numéro de téléphone")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Le numéro et le nom de la rue est obligatoire")]
        [Display(Name = "Numéro et nom de la rue")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Le code postal est obligatoire")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Code postal")]
        public int Zipcode { get; set; }

        [Required(ErrorMessage = "La ville est obligatoire")]
        [Display(Name = "Ville")]
        public string City { get; set; }

        [Required(ErrorMessage = "Le pays est obligatoire")]
        [Display(Name = "Pays")]
        public string Country { get; set; }

        public virtual ICollection<List> Lists { get; set; }
    }
}
