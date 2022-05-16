#if SERVER_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class PlayerController : NetworkBehaviour
{
    // Server-only serialized fields, also accessible in editor.
    [SerializeField] private float rotateSpeed;

    // Server-only methods and unserialized data.
    // We don't use the || UNITY_EDITOR
    // exception here, to ensure we don't conflict with the
    // server file's method definitions.

#if SERVER_BUILD
    private CharacterController characterController;

    private void OnStartup()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!characterController.isGrounded)
        {
            var moveY = new Vector3(0, Physics.gravity.y, 0);
            characterController.Move(moveY * Time.deltaTime);
        }

        characterController.Move(GetMoveVector());
        transform.Rotate(0.0f, turnMagnitude.Value * rotateSpeed, 0.0f);
    }
#endif
}
#endif