using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour
{
    //Standardized script for activating colliders on the hand, will draft it up and will ask help from isagani to clean up the code later
    //prolly rename this script to clean manager or smth
    [SerializeField] Collider[] _handWashColliders;
    [SerializeField] Collider[] _KitchenWashColliders;
    [SerializeField] int _kitchenDecay;
    [SerializeField] float _timer;
    [SerializeField] bool _coroutineTriggered; //temp bool, please change logic for coroutine if there is a more efficient way

    private void Start() // Should we use awake instead?
    {
        if (_handWashColliders == null && _KitchenWashColliders == null) return;
        for (int i = 0; i < _handWashColliders.Length; i++)
        {
            if (_handWashColliders[i])
                _handWashColliders[i].enabled = false;
        }
        for (int i = 0; i < _KitchenWashColliders.Length; i++)
        {
            if (_KitchenWashColliders[i])
                _KitchenWashColliders[i].enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if( !_coroutineTriggered )
            StartCoroutine(DecayRate());
        Debug.Log(_kitchenDecay);

        if (_handWashColliders == null && _KitchenWashColliders == null) return;

        if (_kitchenDecay < 100)
        {
            ActivateKitchenWashColliders();
        }
        else DeactivateKitchenWashColliders();
    }
    #region Private Functions
    private void ActivateKitchenWashColliders()
    {
        if (_KitchenWashColliders == null) return;  
        for (int i =0;i < _KitchenWashColliders.Length;i++)
        {
            if (_KitchenWashColliders[i])
                 _KitchenWashColliders[i].enabled = true;
        }
        // update texture of kitchen, if di kaya spark na lng siya
        // make sure smelly vfx comes out of the colliders to signify the player that their kitchen needs cleaning
    }
    private void DeactivateKitchenWashColliders()
    {
        if (_KitchenWashColliders == null) return;
        for(int i = 0; i<_KitchenWashColliders.Length;i++)
        {
            if (_KitchenWashColliders[i])
                _KitchenWashColliders[i].enabled = false;
        }
    }
    #endregion

    #region Public Functions
    public void ActivateHandWashColliders()
    {
        if (_handWashColliders == null) return;
        for (int i = 0; i < _handWashColliders.Length; i++)
        {
            if(_handWashColliders[i])
                _handWashColliders[i].enabled = true;
        }
    }
    public void DeactivateHandWashColliders()
    {
        if (_handWashColliders == null) return;
        for (int i = 0; i <= _handWashColliders.Length; i++)
        {
            if( _handWashColliders[i])
                _handWashColliders[i].enabled = false;
        }
    }
    #endregion

    IEnumerator DecayRate()
    {
        if (_coroutineTriggered) yield break;
        _coroutineTriggered = true;

        while (_kitchenDecay > 0)
        {
            yield return new WaitForSeconds(_timer);
            _kitchenDecay -= 5;
        }
        _coroutineTriggered = false;
    }
}
