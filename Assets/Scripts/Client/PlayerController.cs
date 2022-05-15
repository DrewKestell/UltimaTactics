#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public partial class PlayerController : NetworkBehaviour
{
    // Client-only serialized fields, also accessible in editor.
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    // Client-only methods and unserialized data.
    // We don't use the || UNITY_EDITOR
    // exception here, to ensure we don't conflict with the
    // server file's method definitions.

#if CLIENT_BUILD
    private CharacterController characterController;
    private Animator animator;
    private ThirdPersonCamera thirdPersonCamera;

    private bool readyToDrawWeapons = true;
    private float combatStartTime;

    private float turnMagnitude;
    private float walkMagnitude;

    // input state
    private bool leftClickHeld;
    private bool rightClickHeld;

    private void OnStartup()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        thirdPersonCamera = GameObject.Find("ThirdPersonCamera").GetComponent<ThirdPersonCamera>();
        thirdPersonCamera.PlayerTransform = transform;
    }

    private void OnUpdate()
    {
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

    // Input
    private void OnTurn(InputValue inputValue)
    {
        turnMagnitude = inputValue.Get<float>();
    }

    private void OnWalk(InputValue inputValue)
    {
        walkMagnitude = inputValue.Get<float>();
    }
    
    private void OnCameraZoom(InputValue inputValue)
    {
        thirdPersonCamera.Zoom(inputValue.Get<Vector2>().normalized.y);
    }

    private void OnLeftClick(InputValue inputValue)
    {
        leftClickHeld = inputValue.isPressed;
    }

    private void OnRightClick(InputValue inputValue)
    {
        rightClickHeld = inputValue.isPressed;

        if (!rightClickHeld)
        {
            turnMagnitude = 0.0f;
        }
    }

    private void OnMoveMouse(InputValue inputValue)
    {
        // right-click + drag -> rotate player and camera
        if (rightClickHeld)
        {
            var value = inputValue.Get<Vector2>();

            var deltaX = value.x;
            turnMagnitude = deltaX;

            var deltaY = value.y;
            thirdPersonCamera.Tilt(deltaY);

            thirdPersonCamera.FollowPlayerRotation = true;
        }

        // left-click + drag -> rotate camera around player without rotating player
        else if (leftClickHeld)
        {
            var value = inputValue.Get<Vector2>();
            thirdPersonCamera.UpdateOrbitPosition(value);
        }
    }

    private void Move()
    {
        if (!characterController.isGrounded)
        {
            var moveY = new Vector3(0, Physics.gravity.y, 0);
            characterController.Move(moveY * Time.deltaTime);
        }

        if (turnMagnitude == 0 && walkMagnitude == 0)
            Idle();
        else
            ClientMove();
    }

    private void Idle()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void ClientMove()
    {
        var moveVector = moveSpeed * Time.deltaTime * walkMagnitude * transform.forward;
        animator.SetBool("IsMoving", true);
        animator.SetFloat("Speed", walkMagnitude, 0.1f, Time.deltaTime);
        characterController.Move(moveVector);

        transform.Rotate(0.0f, turnMagnitude * rotateSpeed, 0.0f);

        if (!thirdPersonCamera.FollowPlayerRotation)
        {
            if (leftClickHeld)
            {
                thirdPersonCamera.transform.position = thirdPersonCamera.transform.position + moveVector;
            }
            else
            {
                if (!thirdPersonCamera.LockingToPlayer)
                {
                    thirdPersonCamera.LockToPlayer();
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