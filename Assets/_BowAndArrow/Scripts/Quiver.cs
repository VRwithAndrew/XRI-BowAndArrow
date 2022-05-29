using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Quiver : XRBaseInteractable
{
    [SerializeField] private GameObject arrowPrefab;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        CreateAndSelectArrow(args);
    }

    private void CreateAndSelectArrow(SelectEnterEventArgs args)
    {
        // Create arrow, force into interacting hand
        Arrow arrow = CreateArrow(args.interactorObject.transform);
        interactionManager.SelectEnter(args.interactorObject, arrow);
    }

    private Arrow CreateArrow(Transform orientation)
    {
        // Create arrow, and get arrow component
        GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
        return arrowObject.GetComponent<Arrow>();
    }
}
