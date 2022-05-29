using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetTransform = null;

    [Header("Position")]
    [SerializeField] private bool trackPosition = true;
    [Range(0, 1)] [SerializeField] private float translateSmoothTime = 0.3f;

    [Header("Rotation")]
    [SerializeField] private bool trackRotation = true;
    [Range(0, 1)] [SerializeField] public float rotationSmoothTime = 0.3f;

    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private Vector3 previousPosition = Vector3.zero;
    private Quaternion previousRotation = Quaternion.identity;

    private Vector3 currentVelocity = Vector3.zero;
    private float currentAngularVelocity = 0;

    private void Start()
    {
        TeleportToTarget();
        SetPrevious();
    }

    private void TeleportToTarget()
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

    private void SetPrevious()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void Update()
    {
        SetTargets();

        if (trackPosition)
            UpdatePosition();

        if (trackRotation)
            UpdateRotation();
    }

    private void SetTargets()
    {
        targetPosition = targetTransform.position;
        targetRotation = targetTransform.rotation;
    }

    private void UpdatePosition()
    {
        previousPosition = Vector3.SmoothDamp(previousPosition, targetPosition, ref currentVelocity, translateSmoothTime);
        transform.position = previousPosition;
    }

    private void UpdateRotation()
    {
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        float maxDegrees = angle - Mathf.SmoothDamp(angle, 0, ref currentAngularVelocity, rotationSmoothTime);

        previousRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegrees);
        transform.rotation = previousRotation;
    }
}
