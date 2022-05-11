#if !CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class PlayerController : NetworkBehaviour
{
    // Server-only serialized fields, also accessible in editor.
    public string Foo;

    // Server-only methods and unserialized data.
    // We don't use the || UNITY_EDITOR
    // exception here, to ensure we don't conflict with the
    // server file's method definitions.

#if !CLIENT_BUILD
    void OnStartup()
    {
    }

    void Update()
    {        
    }
    #endif
}
#endif