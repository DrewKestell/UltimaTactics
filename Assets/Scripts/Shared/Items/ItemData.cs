using System.Text;

public struct ItemData
{
    public ItemScriptableObject ItemBase;
    public ItemModifierValue[] Modifiers;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(ItemBase.ToString());
        sb.Append(" ");

        foreach (var modifier in Modifiers)
        {
            sb.Append($"{modifier} ");
        }

        return sb.ToString();
    }
}
