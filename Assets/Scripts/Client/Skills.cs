#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class Skills : WorldSaved
{
#if CLIENT_BUILD
    public override void OnNetworkSpawn()
    {
        Values.OnDictionaryChanged += Values_OnListChanged;

        var e = new InitializeSkillsPanelEvent(Values.ToDictionary());
        PubSub.Instance.Publish(this, e);
    }

    private void Values_OnListChanged(NetworkDictionaryEvent<int, float> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkDictionaryEvent<int, float>.EventType.Value:
                var e = new SkillChangedEvent((SkillName)changeEvent.Key, changeEvent.PreviousValue, changeEvent.Value);
                PubSub.Instance.Publish(this, e);
                break;
        }
    }
#endif
}
#endif
