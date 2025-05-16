using UnityEngine;

public class FollowAndOrbitCamera : MonoBehaviour
{
    [Header("Target & Offset")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);

    [Header("Camera Movement")]
    public float followSpeed = 5f;
    public float orbitSpeed = 50f;

    [Header("Orbit Limits")]
    public float minYaw = -30f;
    public float maxYaw = 30f;

    [Header("Starting Rotation")]
    public Vector3 startingRotationEuler = new Vector3(0f, 0f, 0f); // X, Y, Z angles

    private float currentYaw;
    private Quaternion baseRotation;

    void Start()
    {
        // Store the initial rotation as a Quaternion
        baseRotation = Quaternion.Euler(startingRotationEuler);
        currentYaw = 0f; // All orbiting is now relative to baseRotation
    }

    void LateUpdate()
    {
        // Input: Mouse drag
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            currentYaw += mouseX * orbitSpeed * Time.deltaTime;
        }

        // Input: Touch drag
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float touchX = Input.GetTouch(0).deltaPosition.x;
            currentYaw += touchX * 0.1f;
        }

        // Clamp the orbit
        currentYaw = Mathf.Clamp(currentYaw, minYaw, maxYaw);

        // Apply rotation: starting rotation + current yaw
        Quaternion orbitRotation = Quaternion.Euler(0, currentYaw, 0);
        Quaternion combinedRotation = baseRotation * orbitRotation;

        // Calculate final camera position
        Vector3 rotatedOffset = combinedRotation * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smooth move and look at player
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position);
    }
}
