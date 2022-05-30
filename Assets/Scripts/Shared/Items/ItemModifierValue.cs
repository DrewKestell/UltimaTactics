using System;

[Serializable]
public struct ItemModifierValue
{
    public ItemModifier Modifier;
    public float Value;

    public override string ToString()
    {
        return $"Modifier={{ {Modifier}, {Value} }}";
    }
}
