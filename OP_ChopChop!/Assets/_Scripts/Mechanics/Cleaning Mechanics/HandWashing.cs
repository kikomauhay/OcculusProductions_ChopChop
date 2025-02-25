using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{

    //had another epiphany, gani pag kagising can you put all the coroutines inside cleanmanager na lang.
    //maiwan lang dito should be the ontrigger stay pala

    public int CleanRate { get; private set; }

    [SerializeField] bool _isDirty;
    [SerializeField] float _timer;

    void Start()
    {
        CleanRate = 100;    
        _isDirty = true;

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
        if (other.gameObject.GetComponent<Sponge>() == null) return;

        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                
            }
        }


        //change sponge into soap or something along the way
        //same sht with plate, velocity things
        //instantiate bubble vfx
        //set dirty to false after a few seconds of cleaning

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
}
