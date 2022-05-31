#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController : NetworkBehaviour
{
#if CLIENT_BUILD
    private Animator animator;
    private ThirdPersonCamera thirdPersonCamera;

    private bool readyToDrawWeapons = true;
    private float combatStartTime;

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

    private void OnFixedUpdate()
    {

    }

    // Input
    private void OnTurn(InputValue inputValue)
    {
        turnMagnitude.Value = inputValue.Get<float>();
    }

    private void OnWalk(InputValue inputValue)
    {
        walkMagnitude.Value = inputValue.Get<float>();
    }
    
    private void OnCameraZoom(InputValue inputValue)
    {
        thirdPersonCamera?.Zoom(inputValue.Get<Vector2>().normalized.y);
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
            turnMagnitude.Value = 0.0f;
        }
    }

    private void OnMoveMouse(InputValue inputValue)
    {
        // right-click + drag -> rotate player and camera
        if (rightClickHeld)
        {
            var value = inputValue.Get<Vector2>();

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
            var value = inputValue.Get<Vector2>();
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

    private void ClientMove()
    {
        
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