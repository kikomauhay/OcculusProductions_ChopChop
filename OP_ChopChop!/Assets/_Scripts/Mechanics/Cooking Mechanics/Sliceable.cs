using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
#region Members

    // will change this into an array later
    [SerializeField] private GameObject _currentPrefab, _nextPrefab;

    IXRSelectInteractor _interactor;

    int _chopCounter;
    public bool IsAttached { get; set; }

#endregion

#region Methods

    void Start()
    {
        _chopCounter = 0;
        IsAttached = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Knife>() == null) return;

        if (other.gameObject.GetComponent<ActionBasedController>() && IsAttached)
            _interactor = other.gameObject.GetComponent<XRDirectInteractor>();

        if (IsAttached)
        {
            _chopCounter++;

            if (_chopCounter >= 5)
            {
                Sliced();
                return;
            }

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE,
                                           transform);

            // ternary operator syntax -> condition ? val_if_true : val_if_false
            SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                            "fish slice 01" :
                                            "fish slice 02");
            // Debug.LogWarning("Chopping");
        }

        // wtf does this do
        if (_interactor != null)
            _interactor.selectEntered.AddListener(Remove);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ActionBasedController>())
        {
            // commented this out cuz this was producing a lot of errors
            // _interactor.selectEntered.RemoveListener(Remove);
            _interactor = null;
        }
    }

    #endregion

    void Sliced()
    {
        if (_currentPrefab == null) return;

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform);

        SpawnManager.Instance.SpawnObject(_nextPrefab,
                                          transform,
                                          SpawnObjectType.INGREDIENT);

        SoundManager.Instance.PlaySound("knife chop");
        Debug.Log("SLICED!");

        // this doens't destroy the thing even
        Destroy(gameObject);
    }

    private void Remove(SelectEnterEventArgs args)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        Collider collider = this.GetComponent<Collider>();

        Debug.Log("Removing item");
        rb.isKinematic = false;
        IsAttached = false;
        collider.isTrigger = false;

        //if not attaching to hand, uncomment code below
        /*        AttachToHand(this.gameObject, _interactor);*/

    }

    /*    private void AttachToHand(GameObject _sliceable, IXRSelectInteractor _interactor)
        {
            XRGrabInteractable _grabInteractable = _sliceable.GetComponent<XRGrabInteractable>();
            XRInteractionManager _interactionManager = _grabInteractable.interactionManager as XRInteractionManager;
            if (_interactionManager == null
                && _interactor is MonoBehaviour interactorObject)
            {
                _interactionManager = interactorObject.GetComponentInParent<XRInteractionManager>();
            }
            if (_grabInteractable != null
                && _interactionManager != null)
            {
                _interactionManager.SelectEnter(_interactor, _grabInteractable);
            }
            else
            {
                Debug.LogError("Spawned object does not have an XRGrabInteractable component.");
            }
        }*/
}