using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
#region Members

    [SerializeField] private GameObject _currentPrefab, _nextPrefab;
    [SerializeField] int _fishCuts;

    Collider _snap;

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
                other.gameObject.GetComponent<Knife>().IncrementUseCounter();
                return;
            }

            SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
            SoundManager.Instance.PlaySound("knife chop");
            SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                            "fish slice 01" : 
                                            "fish slice 02");
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

        SpawnManager.Instance.SpawnVFX(VFXType.SMOKE, transform, 1f);
        StartCoroutine(DoCutting());
    }

    private void Remove(SelectEnterEventArgs args)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        IsAttached = false;
        GetComponent<Collider>().isTrigger = false;
    }

    public void SetSnap(Collider snap) => _snap = snap;

    IEnumerator DoCutting()
    {  
        yield return null;

        for (int i = 0; i < _fishCuts; i++)
        {
            Transform t = transform;

            //still wonky, just store the transform somewhere and then pass it on to here
            t.position = SpawnManager.Instance.FishSliceSpawnPoint.position;

            SpawnManager.Instance.SpawnObject(_nextPrefab, t, SpawnObjectType.INGREDIENT);
        }

        _snap.gameObject.GetComponent<Snap>().CallReset();
        yield return null;
        Destroy(gameObject);
    }
}