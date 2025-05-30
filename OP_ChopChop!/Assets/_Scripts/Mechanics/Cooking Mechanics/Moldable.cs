using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class Moldable : MonoBehaviour
{
    [SerializeField] GameObject[] _moldedStages;
    [SerializeField] RiceIngredient _rice;
    [SerializeField] int _moldLimitPerStage;
   
    private int _moldCounter, _moldStageIndex;
    private HashSet<IXRSelectInteractor> _interactors = new HashSet<IXRSelectInteractor>();
    

/*    protected override void OnEnable()
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
    }*/

    void Start()
    {
        _moldCounter = 0;
        _moldStageIndex = 0;
    }
#region Functions

    public void InsertInteractor(SelectEnterEventArgs args)
    {
        Debug.LogWarning("Meow interactor works");
        _interactors.Add(args.interactorObject);
    }

    public void MoldEvent()
    {
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
                                        "rice mold 02");
        
        if(_grabInteractable)
        {
            foreach(var interactor in _interactors)
            {
                //interactionManager.SelectEnter(interactor, _grabInteractable);
            }
        }
    }

#endregion
}

#region Enumerations

    public enum MoldType // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {
        NOT_MOLDED,
        UNDER_MOLDED,
        PERFECT,
        OVER_MOLDED
    }

#endregion