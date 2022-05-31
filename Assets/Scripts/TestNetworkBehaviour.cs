using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestNetworkBehaviour : NetworkBehaviour
{
    public NetworkVariable<SerializableItem> Foo;

    public override void OnNetworkSpawn()
    {
        
    }
}
