using System.Collections;
using UnityEngine;

public class CustomerAppearance : MonoBehaviour 
{
    public Sprite[] MadFaces => _madFaces;

#region Members

    [Header("Customer Material Renderers")]
    [SerializeField] SpriteRenderer _face;
    [SerializeField] MeshRenderer _ears, _tail, _body;

    [Tooltip("0 = Calico, 1 = Siamese, 2 = Tabby, 3 = Torbie, 4 = Tuxedo")] 
    [SerializeField] SkinVariant[] _skinVariants;

    [Header("Face Types")] 
    [SerializeField] Sprite[] _reactionFaces; // 0 = neutral, 1 = happy, 2 = sus
    [SerializeField] Sprite[] _chewingFaces;  // 0-1 = normal, 2-3 = sus
    [SerializeField] Sprite[] _madFaces;      // 0 = angry, 1 = angrier, 2 = angriest

#endregion

    void Start()
    {        
        int i = Random.Range(0, _skinVariants.Length);

        _body.material = _skinVariants[i].BodyMaterial;
        _ears.material = _skinVariants[i].EarVariants[Random.Range(0, _skinVariants[i].EarVariants.Length)];
        _tail.material = _skinVariants[i].TailVariants[Random.Range(0, _skinVariants[i].TailVariants.Length)];

        _face.sprite = _reactionFaces[0];
    }
    public void ChangeEmotion(FaceVariant type)
    {
        switch (type)
        {
            case FaceVariant.NEUTRAL:
                _face.sprite = _reactionFaces[0];
                break;
            
            case FaceVariant.HAPPY:
                _face.sprite = _reactionFaces[1];
                break;
            
            case FaceVariant.MAD:
                _face.sprite = _madFaces[0];
                break;

            case FaceVariant.SUS:
                _face.sprite = _reactionFaces[2];
                break;

            default: break;
        }
    }

    public IEnumerator DoChweing(float patienceRate)
    {
        yield return new WaitForSeconds(1f);

        if (patienceRate > 50) // is happy is a customer pateince meter or 50+
        {
            _face.sprite = _chewingFaces[0];    
            yield return new WaitForSeconds(1f);

            _face.sprite = _chewingFaces[1];
            yield return new WaitForSeconds(1f);

            _face.sprite = _chewingFaces[0];
            yield return new WaitForSeconds(1f);

            yield break;
        }

        _face.sprite = _chewingFaces[3];
        yield return new WaitForSeconds(1f);

        _face.sprite = _chewingFaces[4];
        yield return new WaitForSeconds(1f);

        _face.sprite = _chewingFaces[3];
        yield return new WaitForSeconds(1f);
    }
}

[System.Serializable]
public struct SkinVariant
{
    public Material BodyMaterial;
    public Material[] EarVariants, TailVariants; 
}