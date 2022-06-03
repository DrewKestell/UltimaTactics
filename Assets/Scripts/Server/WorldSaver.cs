#if SERVER_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public class WorldSaver : MonoBehaviour
{
    [SerializeField] private int saveFrequencyInSeconds;

#if SERVER_BUILD
    public static WorldSaver Instance;

    private static Dictionary<int, WorldSaved> trackedObjects = new();
    private float saveTimer = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        saveTimer += Time.deltaTime;

        if (saveTimer >= saveFrequencyInSeconds)
        {
            SaveWorld();
            saveTimer = 0;
        }    
    }

    public void SaveWorld()
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        foreach (var obj in trackedObjects)
        {
            obj.Value.Serialize();
        }
        stopwatch.Stop();
        
        var e = new SerializableChatMessage(ChatMessage.WorldSaveEnd, ChatMessageType.System, "SYSTEM", stopwatch.ElapsedMilliseconds.ToString());
        ConnectionManager.Instance.ChatMessageClientRpc(e);
    }

    public void TrackObject(int instanceId, WorldSaved obj)
    {
        if (!trackedObjects.ContainsKey(instanceId))
        {
            trackedObjects.Add(instanceId, obj);
        }
    }

    public void UntrackObject(int instanceId, WorldSaved obj)
    {
        if (trackedObjects.ContainsKey(instanceId))
        {
            trackedObjects.Remove(instanceId);
        }
    }
#endif
}
#endif
