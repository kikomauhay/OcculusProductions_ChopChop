using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CustomerOrder), typeof(CustomerActions))]
public class CustomerAppearance : MonoBehaviour 
{     

#region Customer_Variant_Members

    [Header("Customer Material Renderers")]
    [SerializeField] SpriteRenderer _face;
    [SerializeField] MeshRenderer _ears, _tail, _body;

    [Tooltip("0 = Calico, 1 = Siamese, 2 = Tabby, 3 = Torbie, 4 = Tuxedo")] 
    [SerializeField] SkinVariant[] _skinVariants;

    [SerializeField] FaceVariant _faceVariant;
    [SerializeField] Sprite[] _faces; // test


#endregion

    void Awake()
    {        
        int i = Random.Range(0, _skinVariants.Length);

        _body.material = _skinVariants[i].BodyMaterial;
        _ears.material = _skinVariants[i].EarVariants[Random.Range(0, _skinVariants[i].EarVariants.Length)];
        _tail.material = _skinVariants[i].TailVariants[Random.Range(0, _skinVariants[i].TailVariants.Length)];

        _face.sprite = _faceVariant.NeutralFace;
    }

    void Update() => test();

    void test()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _face.sprite = null;
            _face.sprite = _faces[Random.Range(0, _faces.Length)];

            ResetSkin();

            Debug.LogWarning("Randomized customer face!");
        }
    }

    void ResetSkin()
    {
        _body.material = null;
        _ears.material = null;
        _tail.material = null;

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

[System.Serializable]
public struct FaceVariant
{
    public Sprite NeutralFace, HappyFace, MadFace, DisgustFace, ChewingFace;
}