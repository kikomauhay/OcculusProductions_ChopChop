using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine;

public class Moldable : MonoBehaviour
{
    public ActionBasedController Left, Right;

    [SerializeField] GameObject[] _moldedStages;


    [SerializeField] GameObject _perfectMold, _smokeVFX;
    [SerializeField] Rice _rice;
    [SerializeField] int _moldLimitPerStage;
   
    int _moldCounter, _moldStageIndex;

    void Awake()
    {
        Left = ControllerManager.Instance.LeftController;
        Right = ControllerManager.Instance.RightController;
    }
    void Start()
    {
        _moldCounter = 0;
        _moldStageIndex = 0;

        _moldLimitPerStage = 5; // test
        StartCoroutine(test()); // test
    }

    void Update()
    {

        // "levels up" the molded rice and resets the mold count
        if (_moldCounter >= _moldLimitPerStage)
        {
            _moldCounter = 0;

            if (_moldStageIndex < _moldedStages.Length)
            { // prevents out of range errors
                _moldStageIndex++;
                _rice.OnRiceMolded?.Invoke(_moldStageIndex);
            }

            MoldInstantiate(_moldedStages[_moldStageIndex]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CheckGrip(Left))
        {
            Debug.Log("Left Detected");
            if (Right != null && CheckGrip(Right))
            {
                Debug.Log("Molding");
                _moldCounter++;
            }
        }
    }

    bool CheckGrip(ActionBasedController controller) 
    {
        return controller.selectAction.action.ReadValue<float>() > 0.5f;
    }

    void MoldInstantiate(GameObject moldPrefab)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Destroy(gameObject);
        Instantiate(moldPrefab, pos, rot);
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log($"Mold counter: {_moldCounter}");
    }
}
