using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    public Item ChestItem = new();
    public Item LegsItem = new();
    public Item FeetItem = new();
    public Item HeadItem = new();
    public Item[] BackpackItems = new();
}
