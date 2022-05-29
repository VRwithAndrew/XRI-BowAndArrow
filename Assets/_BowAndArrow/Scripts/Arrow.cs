using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    [SerializeField] private float speed = 2000.0f;

    private new Rigidbody rigidbody;
    private ArrowCaster caster;

    private bool launched = false;

    private RaycastHit hit;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        caster = GetComponent<ArrowCaster>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject is Notch notch)
        {
            if (notch.CanRelease)
                LaunchArrow(notch);
        }
    }

    private void LaunchArrow(Notch notch)
    {
        launched = true;
        ApplyForce(notch.PullMeasurer);
        StartCoroutine(LaunchRoutine());
    }

    private void ApplyForce(PullMeasurer pullMeasurer)
    {
        rigidbody.AddForce(transform.forward * (pullMeasurer.PullAmount * speed));
    }

    private IEnumerator LaunchRoutine()
    {
        // Set direction while flying
        while (!caster.CheckForCollision(out hit))
        {
            SetDirection();
            yield return null;
        }

        // Once the arrow has stopped flying
        DisablePhysics();
        ChildArrow(hit);
        CheckForHittable(hit);
    }

    private void SetDirection()
    {
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }

    private void DisablePhysics()
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    private void ChildArrow(RaycastHit hit)
    {
        transform.SetParent(hit.transform);
    }

    private void CheckForHittable(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out IArrowHittable hittable))
            hittable.Hit(this);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return base.IsSelectableBy(interactor) && !launched;
    }
}
