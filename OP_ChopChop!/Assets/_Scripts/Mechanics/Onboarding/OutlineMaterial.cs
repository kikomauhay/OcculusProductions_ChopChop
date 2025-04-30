using UnityEngine;

// FIX ONBOARDING 

public class OutlineMaterial : MonoBehaviour
{
    [SerializeField] private Material _outlineMat;
    private Material _mat;
    private Renderer _rend;
    
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _mat = _rend.material;
    }

    public void EnableHighlight() => _rend.materials = new Material[] { _mat, _outlineMat };
    public void DisableHighlight() => _rend.materials = new Material[] { _mat };  
}
