using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
#region Members

    [SerializeField] private GameObject _currentPrefab, _nextPrefab;
    [SerializeField] Snap _snap;

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

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform);

            // ternary operator syntax -> condition ? val_if_true : val_if_false
            SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                            "fish slice 01" : "fish slice 02");
        }

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
        StartCoroutine(DoCutting());
    }

    private void Remove(SelectEnterEventArgs args)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        IsAttached = false;
        GetComponent<Collider>().isTrigger = false;
    }

    IEnumerator DoCutting()
    {  
        yield return null;
        SpawnManager.Instance.SpawnObject(_nextPrefab,
                                          transform,
                                          SpawnObjectType.INGREDIENT);
        yield return null;
        Destroy(gameObject);

        _snap.ResetTrigger();
    }
}