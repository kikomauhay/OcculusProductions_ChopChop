using UnityEngine;

// FIX ONBOARDING 

public class OutlineMaterial : MonoBehaviour
{
    [SerializeField] private Material _outlineMat;
    private Renderer _rend;
    private Material _cleanMat;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _cleanMat = _rend.material;       
    }
    
    public void EnableHighlight() 
    {
        // this will only change when the tutorial is active
        if (!GameManager.Instance.TutorialDone)
            _rend.materials = new Material[] { _cleanMat, _outlineMat };
    }
    public void DisableHighlight() 
    {
        // this will only change when the tutorial is active
        if (!GameManager.Instance.TutorialDone)
            _rend.materials = new Material[] { _cleanMat };
    }
}
