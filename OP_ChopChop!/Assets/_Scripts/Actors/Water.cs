using UnityEngine;

public class Water : MonoBehaviour
{
    void OnEnable() => 
        SoundManager.Instance.PlaySound("tap water", SoundGroup.APPLIANCES);

    // stops playing the water sfx
    void OnDisable() => 
        SoundManager.Instance.SoundSource.Stop();    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
            other.gameObject.GetComponent<Sponge>().SetWet();
            
        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

            if (!handWash.IsWet)
                handWash.ToggleWet();
        }
    }
}
