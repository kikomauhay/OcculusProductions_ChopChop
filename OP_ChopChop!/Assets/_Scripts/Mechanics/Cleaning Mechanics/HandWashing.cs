using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{

    //had another epiphany, gani pag kagising can you put all the coroutines inside cleanmanager na lang.
    //maiwan lang dito should be the ontrigger stay pala

    public int CleanRate { get; private set; }

    [SerializeField] bool _isDirty,_isWet;
    [SerializeField] Collider _handWashCollider;
    [SerializeField] float _timer;

    void Start()
    {
        CleanRate = 100;    
        _isDirty = true;
        _isWet = false;

        StartCoroutine(DirtifyHands());

        Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {

        if (CleanRate <= 0)
            _isDirty = true;
        else _isDirty = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Water"))
        {
            _isWet = true;
            StartCoroutine(WetToggle());
        }

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                other.gameObject.GetComponent<Ingredient>().Contaminate();
            }
        }
    }

    IEnumerator DirtifyHands()
    {
        if (_isDirty) yield break;
         
        
        while (CleanRate > 70)
        {
            yield return new WaitForSeconds(_timer);
            CleanRate -= Random.Range(3, 5);
            
        }

        _isDirty = false;
        Debug.Log($"Hand Dirty is {_isDirty}");
    }

    IEnumerator WetToggle()
    {
        yield return new WaitForSeconds(5F);
        _isWet = false;
    }
}
