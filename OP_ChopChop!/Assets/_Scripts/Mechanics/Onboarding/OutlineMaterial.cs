using UnityEngine;

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

    public void EnableHighlight() => _rend.materials = new Material[] { _cleanMat, _outlineMat };
    public void DisableHighlight() => _rend.materials = new Material[] { _cleanMat };
}
