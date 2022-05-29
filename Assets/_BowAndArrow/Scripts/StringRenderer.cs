using UnityEngine;

[ExecuteInEditMode]
public class StringRenderer : MonoBehaviour
{
    [Header("Render Positions")]
    [SerializeField] private Transform start;
    [SerializeField] private Transform middle;
    [SerializeField] private Transform end;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // While in editor, make sure the line renderer follows bow
        if (Application.isEditor && !Application.isPlaying)
            UpdatePositions();
    }

    private void OnEnable()
    {
        Application.onBeforeRender += UpdatePositions;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdatePositions;
    }

    private void UpdatePositions()
    {
        // Set positions of line renderer, middle position is the notch attach transform
        lineRenderer.SetPositions(new Vector3[] { start.position, middle.position, end.position });
    }
}
