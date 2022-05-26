using System;
using System.Collections.Generic;
using UnityEngine;

public class PubSub : MonoBehaviour
{
    public static PubSub Instance;

    private readonly Dictionary<Type, Dictionary<int, Delegate>> eventSubscriptions = new();

    public PubSub()
    {
        Instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Subscribe<T>(MonoBehaviour monoBehaviour, Action<T> action) where T : struct, IEvent
    {
        var eventType = typeof(T);
        if (!eventSubscriptions.ContainsKey(eventType))
        {
            eventSubscriptions.Add(eventType, new Dictionary<int, Delegate>());
        }
        if (!eventSubscriptions[eventType].ContainsKey(monoBehaviour.GetInstanceID()))
        {
            Debug.Log($"{monoBehaviour.name}:{monoBehaviour.GetInstanceID()} subscribed to event type {eventType} at {DateTime.Now}.");
            eventSubscriptions[eventType].Add(monoBehaviour.GetInstanceID(), action);
        }
    }

    public void Unsubscribe<T>(MonoBehaviour monoBehaviour) where T : struct, IEvent
    {
        var eventType = typeof(T);
        if (eventSubscriptions.ContainsKey(eventType))
        {
            if (eventSubscriptions[eventType].ContainsKey(monoBehaviour.GetInstanceID()))
            {
                Debug.Log($"{monoBehaviour.name}:{monoBehaviour.GetInstanceID()} unsubscribed to event type {eventType} at {DateTime.Now}.");
                eventSubscriptions[eventType].Remove(monoBehaviour.GetInstanceID());
            }
        }
    }

    public void Publish<T>(MonoBehaviour monoBehaviour, T e) where T : struct, IEvent
    {
        var eventType = typeof(T);
        Debug.Log($"{monoBehaviour.name}:{monoBehaviour.GetInstanceID()} published event type {eventType} at {DateTime.Now}.");

        if (!eventSubscriptions.ContainsKey(eventType))
        {
            eventSubscriptions.Add(eventType, new Dictionary<int, Delegate>());
        }

        var subs = eventSubscriptions[eventType];
        foreach (var sub in subs)
        {
            (sub.Value as Action<T>).Invoke(e);
        }
    }
}

public interface IEvent
{ 
}

public struct LoginSuccessfulEvent : IEvent
{
    public LoginSuccessfulEvent(CharacterListItem[] characters)
    {
        Characters = characters;
    }

    public CharacterListItem[] Characters { get; }
}

public struct ReceivedCharacterCreationAssetsEvent : IEvent
{
}

public struct CharacterCreationSuccessfulEvent : IEvent
{
    public CharacterCreationSuccessfulEvent(CharacterListItem character)
    {
        Character = character;
    }

    public CharacterListItem Character;
}

public struct EnterWorldSuccessfulEvent : IEvent
{
    public EnterWorldSuccessfulEvent(int characterId)
    {
        CharacterId = characterId;
    }

    public int CharacterId { get; }
}

public struct RequestCharacterAssetsSuccessfulEvent : IEvent
{
    public RequestCharacterAssetsSuccessfulEvent(CharacterAssets characterAssets)
    {
        CharacterAssets = characterAssets;
    }

    public CharacterAssets CharacterAssets { get; }
}