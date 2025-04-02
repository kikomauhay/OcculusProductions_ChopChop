using System.Collections;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    [SerializeField] Transform _mouthTransform;

    public Vector3 TargetSeat { get; set; }
    public int SeatIndex { get; set; }

    float _customerSpeed = 2f;

    

    void LateUpdate() => transform.position = Vector3.MoveTowards(transform.position,
                                                                  TargetSeat,
                                                                  _customerSpeed * Time.deltaTime);
                                                                  
    public void TriggerEating() => SpawnManager.Instance.SpawnVFX(VFXType.RICE,
                                                                  _mouthTransform,
                                                                  3f);


    public IEnumerator RandomMeowing() 
    {
        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                       "cat enter 01" : 
                                       "cat enter 02",
                                        SoundGroup.CUSTOMER); 
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f));

            SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                       "cat enter 01" : 
                                       "cat enter 02",
                                        SoundGroup.CUSTOMER); 
        }
    }
}

