using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWashing : MonoBehaviour
{

    //had another epiphany, gani pag kagising can you put all the coroutines inside cleanmanager na lang.
    //maiwan lang dito should be the ontrigger stay pala

    [SerializeField] bool _isDirty;
    [SerializeField] bool _coroutineTriggered;
    [SerializeField] int _cleanCounter;
    [SerializeField] float _timer;

    private void FixedUpdate()
    {
        if(!_coroutineTriggered)
            StartCoroutine(DecayRate());

        if (_cleanCounter <= 0)
            _isDirty = true;
        else _isDirty = false;

        Debug.Log("Hand Dirty?" +_isDirty);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            //change sponge into soap or something along the way
            //same sht with plate, velocity things
            //instantiate bubble vfx
            //set dirty to false after a few seconds of cleaning
        }
    }

    IEnumerator DecayRate()
    {
        if (_coroutineTriggered) yield break;
        _coroutineTriggered = true;

        while (_cleanCounter > 0)
        {
            yield return new WaitForSeconds(_timer);
            _cleanCounter -= 5;
        }
        _coroutineTriggered = false;
    }
}
