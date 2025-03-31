using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class RiceSpawn : XRBaseInteractable
{

    [SerializeField] GameObject _ricePrefab;
    [SerializeField] Transform _spawnPoint;

    private bool _riceSpawned;

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(RiceEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(RiceEvent);
    }

    private void Start()
    {
        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<XRInteractionManager>();
        }
        _riceSpawned = false;
    }

    void RiceEvent(SelectEnterEventArgs args)
    {
        if (!_riceSpawned)
        {
            _riceSpawned = true;

            GameObject _newRice = SpawnManager.Instance.SpawnObject(_ricePrefab,
                                                                    _spawnPoint,
                                                                    SpawnObjectType.INGREDIENT);

            XRGrabInteractable _grabInteractable = _newRice.GetComponent<XRGrabInteractable>();
            interactionManager.SelectEnter(args.interactorObject, _grabInteractable);

            base.OnSelectEntered(args);
            StartCoroutine(ResetBool());
        }
    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(4F);
        _riceSpawned = false;    
    }
}
