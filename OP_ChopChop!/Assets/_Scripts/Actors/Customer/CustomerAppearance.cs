using UnityEngine;

[RequireComponent(typeof(CustomerOrder), typeof(CustomerActions))]
public class CustomerAppearance : MonoBehaviour 
{     

#region Customer_Variants

    [Header("Customer Material Renderers")]
    [SerializeField] MeshRenderer _ears; 
    [SerializeField] MeshRenderer _tail, _body;

    [Tooltip("0 = Calico, 1 = Siamese, 2 = Tabby, 3 = Torbie, 4 = Tuxedo")] 
    [SerializeField] SkinVariant[] _skinVariants;

#endregion

    void Awake()
    {        
        int i = Random.Range(0, _skinVariants.Length);

        _body.material = _skinVariants[i].BodyMaterial;
        _ears.material = _skinVariants[i].EarVariants[Random.Range(0, _skinVariants[i].EarVariants.Length)];
        _tail.material = _skinVariants[i].TailVariants[Random.Range(0, _skinVariants[i].TailVariants.Length)];
    }
}

[System.Serializable]
public struct SkinVariant
{
    public Material BodyMaterial;
    public Material[] EarVariants, TailVariants; 
}