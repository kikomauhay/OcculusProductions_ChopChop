using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Sponge>() != null)
        {
            Sponge sponge = other.gameObject.GetComponent<Sponge>();
            
            if (!sponge.IsWet) 
                sponge.ToggleWetness();
        }

        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            HandWashing handWash = other.gameObject.GetComponent<HandWashing>();

            if (!handWash.IsWet)
                handWash.ToggleWet();
        }
    }
}
