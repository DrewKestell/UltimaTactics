using System.ComponentModel.DataAnnotations;
using UnityEngine;

public enum ItemName
{
    [Display(Name = "None")]
    None,

    [Display(Name = "Mace")]
    Mace,

    [Display(Name = "Maul")]
    Maul,

    [Display(Name = "Shirt")]
    Shirt,

    [Display(Name = "Belt")]
    Belt
}
