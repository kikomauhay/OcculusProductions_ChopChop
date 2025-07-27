using System.Collections;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    [SerializeField] private Transform _mouthTransform;

    public int SeatIndex { get; set; }
                                                                  
    public void TriggerEating() => 
        SpawnManager.Instance.SpawnVFX(VFXType.RICE,
                                       _mouthTransform, 3f);

    public IEnumerator RandomMeowing() 
    {
        SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                        "cat enter 01" : 
                                        "cat enter 02");
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f));

            SoundManager.Instance.PlaySound(Random.value > 0.5f ? 
                                            "cat enter 01" : 
                                            "cat enter 02");
        }
    }
}

