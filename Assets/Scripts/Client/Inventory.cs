#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;

public partial class Inventory : NetworkBehaviour
{
#if CLIENT_BUILD

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Items.OnDictionaryChanged += Values_OnListChanged;

        foreach (var item in Items)
        {
            var e = new ItemAddedToInventoryEvent(item.Key, item.Value);
            PubSub.Instance.Publish(this, e);
        }
    }

    private void Values_OnListChanged(NetworkDictionaryEvent<int, SerializableItem> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkDictionaryEvent<int, SerializableItem>.EventType.Add:
                var addEvent = new ItemAddedToInventoryEvent(changeEvent.Key, changeEvent.Value);
                PubSub.Instance.Publish(this, addEvent);
                break;
            case NetworkDictionaryEvent<int, SerializableItem>.EventType.Remove:
                var removeEvent = new ItemRemovedFromInventoryEvent(changeEvent.Key);
                PubSub.Instance.Publish(this, removeEvent);
                break;
        }
    }
#endif
}
#endif
