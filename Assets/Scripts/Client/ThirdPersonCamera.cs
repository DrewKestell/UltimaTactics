#if CLIENT_BUILD || UNITY_EDITOR
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    const float cameraDistanceMin = 15.0f;
    const float cameraDistanceMax = 35.0f;
    const float verticalOffsetMin = 0.0f;
    const float verticalOffsetMax = 40.0f;

#if CLIENT_BUILD
    public Transform PlayerTransform;
    public bool FollowPlayerRotation { get; set; } = true;
    public bool LockingToPlayer { get; set; } = false;

    [SerializeField] private float cameraDistance = 23.0f;
    [SerializeField] private float zoomModifier = 2.0f;
    [SerializeField] private float verticalOffset = 8.0f;
    [SerializeField] private float tiltModifier = 0.1f;
    [SerializeField] private int interpolationFrameCount = 450;

    private Vector3 interpolationStartPosition;
    private int elapsedInterpolationFrames = 0;

    private void Start()
    {
    }

    private void Update()
    {
        if (PlayerTransform != null)
        {
            if (LockingToPlayer)
            {
                var interpolationRatio = Mathf.SmoothStep(0.0f, 1.0f, (float)elapsedInterpolationFrames++ / interpolationFrameCount);
                transform.position = Vector3.Lerp(interpolationStartPosition, GetCameraFollowPosition(), interpolationRatio);
                transform.LookAt(PlayerTransform);
                if (transform.position == GetCameraFollowPosition())
                {
                    elapsedInterpolationFrames = 0;
                    LockingToPlayer = false;
                    FollowPlayerRotation = true;
                }
            }

            if (FollowPlayerRotation)
            {
                transform.position = GetCameraFollowPosition();
                transform.LookAt(PlayerTransform);
            }
            else if (!LockingToPlayer)
            {
                var delta = (PlayerTransform.transform.position - transform.position).normalized * cameraDistance;
                transform.position = PlayerTransform.position - delta;
            }
        }
    }

    public void Zoom(float amount)
    {
        var newDistance = cameraDistance - (amount * zoomModifier);
        cameraDistance = Mathf.Clamp(newDistance, cameraDistanceMin, cameraDistanceMax);
    }

    public void Tilt(float amount)
    {
        var newOffset = verticalOffset - (amount * tiltModifier);
        verticalOffset = Mathf.Clamp(newOffset, verticalOffsetMin, verticalOffsetMax);
    }

    public void UpdateOrbitPosition(Vector2 amount)
    {
        FollowPlayerRotation = false;

        transform.RotateAround(PlayerTransform.position, transform.up, amount.x);
        transform.LookAt(PlayerTransform);
        transform.RotateAround(PlayerTransform.position, transform.right, -amount.y * tiltModifier);
    }

    public void LockToPlayer()
    {
        interpolationStartPosition = transform.position;
        LockingToPlayer = true;
    }

    private Vector3 GetCameraFollowPosition()
    {
        return PlayerTransform.position - (PlayerTransform.forward * cameraDistance) + new Vector3(0.0f, verticalOffset, 0.0f);
    }
#endif
}
#endif