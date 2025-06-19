using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
#region Members

    [SerializeField] private bool _isTutorial;
    [SerializeField] private List<Ingredient> _ingredients;

    [Header("Magnet Reaction")]
    [SerializeField] private Transform snapToPoint;
    [SerializeField] private Transform pointToSnap;
    [SerializeField] private float _snapSpeed;

    private bool _tutorialPlayed = false;

#endregion

#region Methods

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing == null) return;

        if (!ing.IsFresh)
        {
            SoundManager.Instance.PlaySound("wrong");
            return;
        }

        // adds the ingredient to the freezer & changes its decay rate
        ing.Stored();
        _ingredients.Add(ing);

        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "door opened 01" : 
                                        "door opened 02");
        /*
        if (_isTutorial)
            GetComponent<OutlineMaterial>().DisableHighlight();
        */
    }
    private void OnTriggerExit(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing == null) return;

        // removes the ingredient to the freezer & changes its decay rate
        _ingredients.Remove(ing);
        ing.Unstored();
        
        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "door closed 01" : 
                                        "door closed 02");

        if (!_isTutorial) return;

        if (!_tutorialPlayed)   
        {
            // StartCoroutine(OnBoardingHandler.Instance.Onboarding04());
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();
            _tutorialPlayed = true;
        }     
    }
    
    //Logic for this, if distance between 2 objects is close, object1.transform.position = object2.transform.position
    public void DoorSnapToBody()
    {
        float pointToPointDist = Vector3.Distance(pointToSnap.position,
                                                  snapToPoint.position);

        Debug.Log($"Distance Calculated: {pointToPointDist}");

        if (pointToPointDist <= 0.5f)
        {
            Rigidbody rb = pointToSnap.GetComponentInParent<Rigidbody>();
            XRGrabInteractable grab = pointToSnap.GetComponentInParent<XRGrabInteractable>();

            if (grab == null && rb == null) return;

            grab.enabled = false;
            rb.isKinematic = true;

            pointToSnap.position = Vector3.MoveTowards(pointToSnap.position,
                                                       snapToPoint.position, 
                                                       Time.deltaTime * _snapSpeed);
            if(pointToPointDist < 0.01F)
            {
                grab.enabled = true;
                rb.isKinematic = false;
            }

        }
    }

#endregion
}
