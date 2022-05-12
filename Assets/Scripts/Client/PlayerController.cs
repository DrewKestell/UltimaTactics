#if !SERVER_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class PlayerController : NetworkBehaviour
{
    // Client-only serialized fields, also accessible in editor.
    [SerializeField] private int moveSpeed;
    [SerializeField] private int rotateSpeed;

    // Client-only methods and unserialized data.
    // We don't use the || UNITY_EDITOR
    // exception here, to ensure we don't conflict with the
    // server file's method definitions.

#if !SERVER_BUILD
    private CharacterController characterController;
    private Animator animator;

    private bool readyToDrawWeapons = true;
    private float combatStartTime;

    private void OnStartup()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnUpdate()
    {
        if (combatStartTime != 0 && Time.time > combatStartTime + 3)
        {
            combatStartTime = 0;
            readyToDrawWeapons = true;
            SheathSwordAndShield();
        }

        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        Move();
    }

    private void Move()
    {
        if (!characterController.isGrounded)
        {
            var moveY = new Vector3(0, Physics.gravity.y, 0);
            characterController.Move(moveY * Time.deltaTime);
        }

        var moveX = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveX == Vector3.zero)
            Idle();
        else
            Move(moveX);
    }

    private void Idle()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Move(Vector3 moveX)
    {
        animator.SetBool("IsMoving", true);
        float magnitude = Mathf.Clamp01(moveX.magnitude);
        animator.SetFloat("Speed", magnitude, 0.1f, Time.deltaTime);
        characterController.Move(moveSpeed * Time.deltaTime * moveX);

        var rotation = Quaternion.LookRotation(moveX);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
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