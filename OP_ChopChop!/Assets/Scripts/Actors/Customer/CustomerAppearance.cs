using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CustomerOrder), typeof(CustomerActions))]
public class CustomerAppearance : MonoBehaviour 
{
    [SerializeField] GameObject[] _ears;
    [SerializeField] GameObject[] _tails;
    [SerializeField] GameObject[] _faces; // not yet used 

    void Awake()
    {
        _ears[Random.Range(0, _ears.Length)].SetActive(true);
        _tails[Random.Range(0, _tails.Length)].SetActive(true);
    }

    void Start() => StartCoroutine(test());
   

    IEnumerator test() {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            foreach (GameObject e in _ears)
                e.SetActive(false);

            foreach (GameObject t in _tails)
                t.SetActive(false);
            
            Awake();
            Debug.Log("Randomized customer face!");
        }
    }

}
