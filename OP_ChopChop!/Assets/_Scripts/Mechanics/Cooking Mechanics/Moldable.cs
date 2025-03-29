using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class Moldable : MonoBehaviour
{
    public ActionBasedController Left, Right;

    [SerializeField] GameObject[] _moldedStages;
    [SerializeField] RiceIngredient _rice;
    [SerializeField] int _moldLimitPerStage;
   
    int _moldCounter, _moldStageIndex;
    IXRSelectInteractor _interactor;

    void Start()
    {
        Left = ControllerManager.Instance.LeftController;
        Right = ControllerManager.Instance.RightController;
        _moldCounter = 0;
        _moldStageIndex = 0;
    }

    void Update()
    {
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
 #region Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (CheckGrip(Left))
            _interactor = Right.GetComponent<XRDirectInteractor>();

        else if(CheckGrip(Right))
            _interactor = Left.GetComponent<XRDirectInteractor>();

        if (_interactor != null)
            _interactor.selectEntered.AddListener(MoldEvent);
    }

    private void OnTriggerExit(Collider other)
    {
        _interactor.selectEntered.RemoveListener(MoldEvent);
        _interactor = null;
    }

#endregion

#region Functions

    void MoldEvent(SelectEnterEventArgs args) => _moldCounter++;
    
    bool CheckGrip(ActionBasedController controller) =>
        controller.selectAction.action.ReadValue<float>() > 0.5f;
    
    void MoldInstantiate(GameObject moldPrefab)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Destroy(gameObject);
        Instantiate(moldPrefab, pos, rot);

        SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                        "rice mold 01" : 
                                        "rice mold 02",
                                        SoundGroup.FOOD);    
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log($"Mold counter: {_moldCounter}");
    }

#endregion
}