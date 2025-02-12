using UnityEngine;

public class Sponge : MonoBehaviour
{
    public bool IsWet;

    void Start()
    {
        IsWet = false;
    }

    public void Dried() => IsWet = false;
    public void Wet() => IsWet = true;   
}
