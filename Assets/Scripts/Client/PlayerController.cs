#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public partial class PlayerController : NetworkBehaviour
{
#if CLIENT_BUILD
    private Animator animator;
    private ThirdPersonCamera thirdPersonCamera;

    private bool readyToDrawWeapons = true;
    private float combatStartTime;
    private bool pointerOverContextMenu = false;
    private bool pointerOverUi = false;

    // input state
    private bool leftClickHeld;
    private bool rightClickHeld;

    private void OnStartup()
    {
        animator = GetComponent<Animator>();

        var camComponent = GameObject.Find("ThirdPersonCamera").GetComponent<ThirdPersonCamera>();
        if (!IsLocalPlayer)
        {
            camComponent.enabled = false;
        }
        else
        {
            thirdPersonCamera = camComponent;
            thirdPersonCamera.PlayerTransform = transform;
        }

        // TODO: Player and UICanvas are spawned at runtime which makes it difficult to wire up these event listeners through the Unity Editor
        // as a workaround, wire them up here when the Player object is spawned. UI will already be initialized here.
        var chatPanel = GameObject.Find("ChatPanel").GetComponent<ChatPanel>();
        var playerInputComponent = GetComponent<PlayerInput>();
        playerInputComponent.currentActionMap.FindAction("Enter").performed += chatPanel.OnEnter;
    }

    private void OnUpdate()
    {
        var playerInputComponent = GetComponent<PlayerInput>();
        var inputDevice = playerInputComponent.devices[1];
        var deviceId = inputDevice.deviceId;
        var inputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
        var raycastResult = inputModule.GetLastRaycastResult(deviceId);
        pointerOverContextMenu = raycastResult.gameObject?.name == "ContextMenuText";

        pointerOverUi = EventSystem.current.IsPointerOverGameObject();

        if (combatStartTime != 0 && Time.time > combatStartTime + 3)
        {
            combatStartTime = 0;
            readyToDrawWeapons = true;
            SheathSwordAndShield();
        }

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Attack();
        //}

        Move();
    }

    private void OnFixedUpdate()
    {

    }

    // Input
    public void OnTurn(InputAction.CallbackContext inputValue)
    {
        turnMagnitude.Value = inputValue.ReadValue<float>();
    }

    public void OnWalk(InputAction.CallbackContext inputValue)
    {
        walkMagnitude.Value = inputValue.ReadValue<float>();
    }

    public void OnCameraZoom(InputAction.CallbackContext inputValue)
    {
        if (pointerOverUi)
            return;

        thirdPersonCamera?.Zoom(inputValue.ReadValue<Vector2>().normalized.y);
    }

    public void OnLeftClick(InputAction.CallbackContext inputValue)
    {
        if (pointerOverContextMenu)
            return;

        ContextMenu.Instance.Deactivate();

        leftClickHeld = inputValue.ReadValueAsButton();
    }

    public void OnRightClick(InputAction.CallbackContext inputValue)
    {
        if (pointerOverUi)
            return;

        rightClickHeld = inputValue.ReadValueAsButton();

        if (!rightClickHeld)
        {
            turnMagnitude.Value = 0.0f;
        }
    }

    public void OnMoveMouse(InputAction.CallbackContext inputValue)
    {
        if (pointerOverUi)
            return;

        // right-click + drag -> rotate player and camera
        if (rightClickHeld)
        {
            var value = inputValue.ReadValue<Vector2>();

            var deltaX = value.x;
            turnMagnitude.Value = deltaX;

            var deltaY = value.y;
            thirdPersonCamera?.Tilt(deltaY);

            if (thirdPersonCamera != null)
                thirdPersonCamera.FollowPlayerRotation = true;
        }

        // left-click + drag -> rotate camera around player without rotating player
        else if (leftClickHeld)
        {
            var value = inputValue.ReadValue<Vector2>();
            thirdPersonCamera?.UpdateOrbitPosition(value);
        }
    }

    private void Move()
    {
        if (turnMagnitude.Value == 0 && walkMagnitude.Value == 0)
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Speed", walkMagnitude.Value, 0.1f, Time.deltaTime);

            if (thirdPersonCamera != null && !thirdPersonCamera.FollowPlayerRotation)
            {
                if (leftClickHeld)
                {
                    if (thirdPersonCamera != null)
                        thirdPersonCamera.transform.position = thirdPersonCamera.transform.position + GetMoveVector();
                }
                else
                {
                    if (thirdPersonCamera != null && !thirdPersonCamera.LockingToPlayer)
                    {
                        thirdPersonCamera.LockToPlayer();
                    }
                }
            }
        }
    }

    private void Attack()
    {
        combatStartTime = Time.time;

        if (readyToDrawWeapons)
        {
            readyToDrawWeapons = false;
            UnsheathSwordAndShield();
        }
    }

    private void UnsheathSwordAndShield()
    {
        animator.SetTrigger("DrawSwordAndShield");
        animator.CrossFade("CombatMove", 0.5f, 0, CurrentMovementAnimationTime);
    }

    private void SheathSwordAndShield()
    {
        animator.SetTrigger("SheathSwordAndShield");
        animator.CrossFade("Move", 0.5f, 0, CurrentMovementAnimationTime);
    }

    private float CurrentMovementAnimationTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
#endif
}
#endif