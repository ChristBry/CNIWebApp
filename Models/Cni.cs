using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CNIWebApp.Models
{
    public class Cni
    {
        // La clé primaire (auto-incrémentée par MySQL)
        [Key]
        public int Id { get; set; }

        // Champ pour le Numéro de la CNI
        [Required(ErrorMessage = "Le numéro de la CNI est obligatoire.")]
        [StringLength(10)]
        [Display(Name = "Numéro CNI / CNI Number")]
        public string Num_cni { get; set; }

        // Champ pour le Numéro d'identification nationale
        [Required(ErrorMessage = "Le numéro d'identification est obligatoire.")]
        [StringLength(15)]
        [Display(Name = "Identifiant unique")]
        public string Num_identifiant { get; set; }

        // Champ pour le Nom
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom / Surname")]
        public string Nom { get; set; }

        // Champ pour le Prénom
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Prénom / Last name")]
        public string Prenom { get; set; }

        // Champ pour le Nom du pere
        [Required(ErrorMessage = "Le nom du père est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom du père / Father name")]
        public string Pere { get; set; }

        // Champ pour le Nom de la mere
        [Required(ErrorMessage = "Le nom de la mère est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom de la mère / Mother Name")]
        public string Mere { get; set; }

        // Date de naissance
        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance / Date of birth")]
        public DateTime Date_naissance { get; set; }

        // Champ pour le Sexe
        [Required(ErrorMessage = "Le sexe est obligatoire.")]
        [StringLength(15)]
        [Display(Name = "Sexe / Sex")]
        public string Sexe { get; set; }

        // Champ pour le Lieu de naissance
        [Required(ErrorMessage = "Le lieu de naissance est obligatoire.")]
        [StringLength(50)]
        [Display(Name = "Lieu de naissance / Place of birth")]
        public string Lieu_naissance { get; set; }

        // Champ pour la profession
        [Required(ErrorMessage = "La profession est requis.")]
        [StringLength(120)]
        [Display(Name = "Profession / Occupation")]
        public string Profession { get; set; }

        // Champ pour la taille
        [Required(ErrorMessage = "La taille est requis.")]
        [Range(0.50, 2.50, ErrorMessage = "La taille doit être entre 0.50m et 2.50m.")]
        [Display(Name = "Taille / Height")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Format invalide (ex: 1.82)")]
        public double Taille { get; set; }

        // Champ pour l'adresse
        [Required(ErrorMessage = "L'adresse est obligatoire.")]
        [StringLength(120)]
        [Display(Name = "Adresse / Address")]
        public string Adresse { get; set; }

        // Champ pour s.p.s.m
        [Required(ErrorMessage = "Le spsm est obligatoire.")]
        [StringLength(20)]
        [Display(Name = "SPSM")]
        public string spsm { get; set; }

        // Champ pour le poste d'identification
        [Required(ErrorMessage = "Le poste d'identification est obligatoire.")]
        [StringLength(10)]
        [Display(Name = "Poste d'identification")]
        public string Poste_identification { get; set; }

        // Date de délivrance de la carte
        [Required(ErrorMessage = "La date de délivrance est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de délivrance / Date of issue")]
        public DateTime Date_delivrance { get; set; }

        // Date d'expiration de la carte
        [Required(ErrorMessage = "La date d'expiration est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date d'expiration / Date of expiry")]
        public DateTime Date_expiration { get; set; }

        // Champ pour le nom de l'image
        [Required(ErrorMessage = "Photo obligatoire.")]
        [Display(Name = "Photo d'identité")]
        public string? Image { get; set; }

    }
}
