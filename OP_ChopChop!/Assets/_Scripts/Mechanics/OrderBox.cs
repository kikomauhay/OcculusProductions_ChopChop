using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class OrderBox : MonoBehaviour
{
    [SerializeField] private GameObject _fishPrefab;

    private XRInteractionManager _interactionManager;

    private void Start()
    {
        if(_interactionManager == null)
            _interactionManager = FindObjectOfType<XRInteractionManager>();
    }

    public void OpenBox(SelectEnterEventArgs args)
    {
        if (_fishPrefab == null) return;

        Destroy(transform.parent.gameObject);

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 2f);
        GameObject fishSlab = SpawnManager.Instance.SpawnObject(_fishPrefab,
                                                                transform,
                                                                SpawnObjectType.INGREDIENT);
        XRGrabInteractable _grabInteractable = fishSlab.GetComponent<XRGrabInteractable>();

        if(_grabInteractable)
            _interactionManager.SelectEnter(args.interactorObject, _grabInteractable);
        

    }
}
