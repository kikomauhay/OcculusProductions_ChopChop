using System.Collections;
using UnityEngine;

public class CustomerActions : MonoBehaviour
{
    #region Members

    [SerializeField] private Transform _mouthTransform;
    public int SeatIndex { get; set; }
    public const float MIN_MEOW_WAIT = 20f;
    public const float MAX_MEOW_WAIT = 25f;

    #endregion

    #region Public

    public void TriggerEating() =>
        SpawnManager.Instance.SpawnVFX(VFXType.RICE,
                                       _mouthTransform, 3f);
    public IEnumerator RandomMeowing()
    {
        // initiat cat sound
        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "cat enter 01" :
                                        "cat enter 02");

        // constant meowing until the GameObject is destroyed
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(MIN_MEOW_WAIT, MAX_MEOW_WAIT));

            SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                            "cat enter 01" :
                                            "cat enter 02");
        }
    }
    
    #endregion 
}

