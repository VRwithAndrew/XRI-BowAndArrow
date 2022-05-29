using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Notch : XRSocketInteractor
{
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0.25f;

    public Bow Bow { get; private set; }
    public PullMeasurer PullMeasurer { get; private set; }

    public bool CanRelease => PullMeasurer.PullAmount > releaseThreshold;

    protected override void Awake()
    {
        base.Awake();
        Bow = GetComponentInParent<Bow>();
        PullMeasurer = GetComponentInChildren<PullMeasurer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PullMeasurer.selectExited.AddListener(ReleaseArrow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
    }

    public void ReleaseArrow(SelectExitEventArgs args)
    {
        if (hasSelection)
            interactionManager.SelectExit(this, firstInteractableSelected);
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (Bow.isSelected)
            UpdateAttach();
    }

    public void UpdateAttach()
    {
        // Move attach when bow is pulled, this updates the renderer as well
        attachTransform.position = PullMeasurer.PullPosition;
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up
        return QuickSelect(interactable) && CanHover(interactable) && interactable is Arrow && Bow.isSelected;
    }

    private bool QuickSelect(IXRSelectInteractable interactable)
    {
        // This lets the Notch automatically grab the arrow
        return !hasSelection || IsSelecting(interactable);
    }

    private bool CanHover(IXRSelectInteractable interactable)
    {
        if (interactable is IXRHoverInteractable hoverInteractable)
            return CanHover(hoverInteractable);

        return false;
    }
}
