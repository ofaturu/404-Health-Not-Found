using UnityEngine;

public class KeyFloat : MonoBehaviour
{
    [Header("Floating Effect")]
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotation Effect")]
    [SerializeField] private bool rotateObject = false;
    [SerializeField] private float rotationSpeed = 30f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed)
                       * floatHeight;

        transform.localPosition = startPosition
                                  + Vector3.up * offset;

        if (rotateObject)
        {
            transform.Rotate(
                0f,
                0f,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}