using System.ComponentModel.DataAnnotations;

public enum ItemModifier : byte
{
    [Display(Name = "Damage Increase")]
    DamageIncrease,

    [Display(Name = "Lower Reagent Cost")]
    LowerReagentCost,

    [Display(Name = "Hit Point Increase")]
    HitPointIncrease
}
