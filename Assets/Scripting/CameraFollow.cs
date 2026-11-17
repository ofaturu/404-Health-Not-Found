
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    [Range(0.1f, 20f)]
    public float smoothSpeed = 5f;

    public float offsetX = 0f;
    public float offsetY = 2.5f;
    public float offsetZ = -10f;

    [Header("Horizontal Bounds")]
    public bool useMinX = true;
    public float minX = 2.2f;

    [Header("Vertical Bounds")]
    public bool useVerticalBounds = true;
    public float minY = -2f;
    public float maxY = 8f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetX = target.position.x + offsetX;
        float targetY = target.position.y + offsetY;

        if (useMinX)
            targetX = Mathf.Max(targetX, minX);

        if (useVerticalBounds)
            targetY = Mathf.Clamp(targetY, minY, maxY);

        Vector3 desiredPosition = new Vector3(
            targetX,
            targetY,
            offsetZ
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
