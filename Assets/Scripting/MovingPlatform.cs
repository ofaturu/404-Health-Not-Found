using UnityEngine;

[DefaultExecutionOrder(100)]
public class MovingPlatform : MonoBehaviour
{
    private Vector3 previousPosition;

    public Vector3 PlatformDelta { get; private set; }

    private void Awake()
    {
        previousPosition = transform.position;
        PlatformDelta = Vector3.zero;
    }

    private void LateUpdate()
    {
        Vector3 currentPosition = transform.position;

        PlatformDelta = currentPosition - previousPosition;
        previousPosition = currentPosition;
    }

}