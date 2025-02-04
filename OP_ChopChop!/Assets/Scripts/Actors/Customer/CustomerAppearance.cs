using UnityEngine;

[RequireComponent(typeof(CustomerOrder), typeof(CustomerActions))]
public class CustomerAppearance : MonoBehaviour 
{
    [SerializeField] GameObject[] _ears;
    [SerializeField] GameObject[] _tails;
    [SerializeField] GameObject[] _faces; // not yet used 

    void Start() 
    {
        _ears[Random.Range(0, _ears.Length)].SetActive(true);
        _tails[Random.Range(0, _tails.Length)].SetActive(true);

        InvokeRepeating("test", 0f, 1f);
    }

    void test() {
        foreach (GameObject e in _ears) 
            e.SetActive(false);
            
        foreach (GameObject t in _tails)
            t.SetActive(false);

        Start();
        Debug.Log("Randomized customer face!");
    }

}
