using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class RiceSpawn : XRBaseInteractable
{
    #region SerializeField

    [SerializeField] private GameObject _ricePrefab, _riceMesh, _riceSpwnCollider;
    [SerializeField] private Transform _spawnpoint;
    [SerializeField] private bool _isTutorial;
    [SerializeField] private float _decrementHeightCount;

    #endregion
    #region Private

    private int _spawnCount;
    private bool _riceSpawned;
    private Vector3 _riceSpwnColliderPos;
    [SerializeField] private NEW_TutorialComponent _tutorialComponent;

    #endregion

    #region Unity

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

        _tutorialComponent = GetComponent<NEW_TutorialComponent>();

        _riceSpawned = false;
        _spawnCount = 7;
        _riceSpwnColliderPos = transform.position;
    }
    private void Update()
    {
        if (_spawnCount <= 0)
        {
            Debug.LogWarning($"Rice left: {_spawnCount}");
            _riceMesh.gameObject.SetActive(false);
        }
    }

    #endregion
    #region Rice Spawning
    public void ResetRice()
    {
        _spawnCount = 7;
        _riceMesh.gameObject.SetActive(true);
        _riceMesh.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        _riceSpwnCollider.gameObject.transform.position = _riceSpwnColliderPos;
    }
    private void RiceEvent(SelectEnterEventArgs args)
    {
        Debug.LogWarning($"Rice left: {_spawnCount}");

        if (!_tutorialComponent.IsInteractable && 
            !_tutorialComponent.IsCorrectIndex())
        {
            return;
        }

        if (_riceSpawned || _spawnCount <= 0) return;

        _riceSpawned = true;

        GameObject newRice = _isTutorial ?
                             Instantiate(_ricePrefab, transform.position, transform.rotation) :
                             SpawnManager.Instance.SpawnObject(_ricePrefab, _spawnpoint,
                                                               SpawnObjectType.INGREDIENT);

        XRGrabInteractable _grabInteractable = newRice.GetComponent<XRGrabInteractable>();
        interactionManager.SelectEnter(args.interactorObject, _grabInteractable);

        base.OnSelectEntered(args);
        StartCoroutine(ResetRiceSpawned());

        if (!_isTutorial)
        {
            Debug.LogWarning("Decrement Not Running");
            _spawnCount--;
            _riceMesh.gameObject.transform.localPosition -= new Vector3(0, _decrementHeightCount, 0);
            _riceSpwnCollider.gameObject.transform.localPosition -= new Vector3(0, _decrementHeightCount, 0);
        }
    }
    private IEnumerator ResetRiceSpawned()
    {
        yield return new WaitForSeconds(4f);
        _riceSpawned = false;
    }

    #endregion
}
