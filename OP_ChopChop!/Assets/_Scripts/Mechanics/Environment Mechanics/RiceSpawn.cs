using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class RiceSpawn : XRBaseInteractable
{
    [SerializeField] private GameObject _ricePrefab;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private bool _isTutorial;

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
            interactionManager = FindObjectOfType<XRInteractionManager>();
        
        _riceSpawned = false;
    }

    private void RiceEvent(SelectEnterEventArgs args)
    {
        if (_riceSpawned) return;

        _riceSpawned = true;

        GameObject newRice = _isTutorial ? 
                             Instantiate(_ricePrefab, transform.position, transform.rotation) :
                             SpawnManager.Instance.SpawnObject(_ricePrefab, _spawnpoint,
                                                               SpawnObjectType.INGREDIENT);

        XRGrabInteractable _grabInteractable = newRice.GetComponent<XRGrabInteractable>();
        interactionManager.SelectEnter(args.interactorObject, _grabInteractable);

        base.OnSelectEntered(args);
        StartCoroutine(ResetRiceSpawned());
        
        if (_isTutorial)
            StartCoroutine(OnBoardingHandler.Instance.Onboarding06());
    }

    private IEnumerator ResetRiceSpawned()
    {
        yield return new WaitForSeconds(4f);
        _riceSpawned = false;    
    }
}
