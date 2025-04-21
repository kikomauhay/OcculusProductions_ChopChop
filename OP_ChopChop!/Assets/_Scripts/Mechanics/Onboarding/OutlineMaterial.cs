using UnityEngine;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            EnableHighlight();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DisableHighlight();
        }
    }

    public void EnableHighlight() => _rend.materials = new Material[] { _mat, _outlineMat };
    public void DisableHighlight() => _rend.materials = new Material[] { _mat };  
}
