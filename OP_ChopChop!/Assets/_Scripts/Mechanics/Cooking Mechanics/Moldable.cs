using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class Moldable : XRBaseInteractable
{
    [SerializeField] GameObject[] _moldedStages;
    [SerializeField] RiceIngredient _rice;
    [SerializeField] int _moldLimitPerStage;
   
    private int _moldCounter, _moldStageIndex;
    private HashSet<IXRSelectInteractor> _interactors = new HashSet<IXRSelectInteractor> ();

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(InsertInteractor);
        selectExited.AddListener(MoldEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(InsertInteractor);
        selectExited.RemoveListener(MoldEvent);
    }

    void Start()
    {
        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<XRInteractionManager> ();
        }
        _moldCounter = 0;
        _moldStageIndex = 0;
    }
#region Functions

    void InsertInteractor(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _interactors.Add(args.interactorObject);
    }

    void MoldEvent(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if(_interactors.Count >= 1)
        {
            _moldCounter++; 
            // "levels up" the molded rice and resets the mold count
            if (_moldCounter >= _moldLimitPerStage)
            {
                _moldCounter = 0;

                //added this to test if it will stop molding other rice
                if (_moldStageIndex < _moldedStages.Length) // prevents out of range errors
                { 
                    this._moldStageIndex++;
                    this._rice.OnRiceMolded?.Invoke(_moldStageIndex);
                }

                MoldInstantiate(_moldedStages[_moldStageIndex]);
            }
        }
    }
    
    void MoldInstantiate(GameObject moldPrefab)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Destroy(gameObject);
        GameObject newRice = Instantiate(moldPrefab, pos, rot);
        XRGrabInteractable _grabInteractable = newRice.GetComponent<XRGrabInteractable>();
        SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                        "rice mold 01" : 
                                        "rice mold 02",
                                        SoundGroup.FOOD);
        
        if(_grabInteractable && interactionManager != null)
        {
            foreach(var interactor in _interactors)
            {
                interactionManager.SelectEnter(interactor, _grabInteractable);
            }
        }
    }

#endregion
}