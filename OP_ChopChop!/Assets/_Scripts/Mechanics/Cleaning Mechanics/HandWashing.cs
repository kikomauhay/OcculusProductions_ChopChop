using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    [SerializeField] bool _isDirty;
    [SerializeField] bool _coroutineTriggered;
    [SerializeField] int _handCleanlinessCounter;
    [SerializeField] float _timer;
    [SerializeField] CleanManager _cleanManager;

    private void FixedUpdate()
    {
        if(!_coroutineTriggered)
            StartCoroutine(DecayRate());

        if (_handCleanlinessCounter < 90)
        {
            _isDirty = true;
            _cleanManager.ToggleHandWashColliders();
        }
            
        else _isDirty = false;

        Debug.Log("Hand Dirty?" +_isDirty);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Sponge>() != null)
        {
            //change sponge into soap or something along the way
            //same sht with plate, velocity things
            //instantiate bubble vfx
        }
    }

    void IncreaseCleanliness()
    {
        _handCleanlinessCounter += Random.Range(5, 10);
        if (_handCleanlinessCounter >= 100)
        {
            _isDirty = false;
            _handCleanlinessCounter = 100;
        }
    }

    IEnumerator DecayRate()
    {
        if (_coroutineTriggered) yield break;
        _coroutineTriggered = true;

        while (_handCleanlinessCounter > 0)
        {
            yield return new WaitForSeconds(_timer);
            _handCleanlinessCounter -= 5;
        }
        _coroutineTriggered = false;
    }
}
