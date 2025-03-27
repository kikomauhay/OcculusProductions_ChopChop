using System.Collections;
using UnityEngine;

public class HandWashing : MonoBehaviour
{
    public int CleanRate { get; private set; }
    public bool IsWet { get; private set; }

    [SerializeField] bool _isDirty;
    [SerializeField] Collider _handWashCollider;
    [SerializeField] float _timer;

    void Start()
    {
        CleanRate = 100;    
        _isDirty = true;
        IsWet = false;

        StartCoroutine(DirtifyHands());

        Debug.Log($"Hand Dirty is {_isDirty}");
    }

    private void FixedUpdate()
    {

        if (CleanRate <= 0)
            _isDirty = true;
        else _isDirty = false;

        if (IsWet)
            StartCoroutine(WetToggle());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            if (_isDirty)
            {
                other.gameObject.GetComponent<Ingredient>().Contaminate();
            }
        }

        if (other.gameObject.GetComponent<HandWashing>() != null)
        {
            SpawnManager.Instance.SpawnVFX(VFXType.BUBBLE, 
                                                   transform, 
                                                   3F);

            //Increment Clean rate by random float
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
        IsWet = false;
    }
}
