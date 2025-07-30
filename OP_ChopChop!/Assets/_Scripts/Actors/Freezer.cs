using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Freezer : MonoBehaviour
{
    #region SerializeField

    [SerializeField] private bool _isTutorial;
    [SerializeField] private List<Ingredient> _ingredients;
    [SerializeField] private GameObject _icySmoke;

    [Header("Magnet Reaction")]
    [SerializeField] private Transform snapToPoint;
    [SerializeField] private Transform pointToSnap;
    [SerializeField] private float _snapSpeed;

    #endregion
    #region Private

    private NEW_TutorialComponent _tutorialComponent;
    private bool _tutorialPlayed = false;

    #endregion

    #region Unity

    private void Start()
    {
        OnBoardingHandler.Instance.OnTutorialEnd += EndTutorial; // it's also getting unsubbed in the function
        _tutorialComponent = GetComponent<NEW_TutorialComponent>();
        _icySmoke.SetActive(false);
    }

    private void Update()
    {
        ToggleVFX();
    }
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        // onboarding will only trigger at the correct index
        if (!_tutorialComponent.IsInteractable &&  
            !_tutorialComponent.IsCorrectIndex())
        {
            return;
        } 
        
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
    }
    private void OnTriggerExit(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        // onboarding will only trigger at the correct index
        if (!_tutorialComponent.IsInteractable && 
            !_tutorialComponent.IsCorrectIndex())
        {
            return;
        }

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
            OnBoardingHandler.Instance.AddOnboardingIndex();
            OnBoardingHandler.Instance.PlayOnboarding();
            _tutorialPlayed = true;
            _isTutorial = false;
        }
    }
    public void DoorSnapToBody()
    {
        // if distance between 2 objects is close, 
        // object1.transform.position = object2.transform.position

        float distanceThreshold = 0.5f;
        float pointToPointDist = Vector3.Distance(pointToSnap.position,
                                                  snapToPoint.position);

        Debug.Log($"Distance Calculated: {pointToPointDist}");

        if (pointToPointDist <= distanceThreshold)
        {

            Rigidbody rb = pointToSnap.GetComponentInParent<Rigidbody>();
            XRGrabInteractable grab = pointToSnap.GetComponentInParent<XRGrabInteractable>();

            if (grab == null && rb == null) return;

            grab.enabled = false;
            rb.isKinematic = true;

            pointToSnap.position = Vector3.MoveTowards(pointToSnap.position,
                                                       snapToPoint.position,
                                                       Time.deltaTime * _snapSpeed);
            if (pointToPointDist < 0.01F)
            {
                grab.enabled = true;
                rb.isKinematic = false;
            }

            _icySmoke.gameObject.SetActive(false);
        }
        else
        {
            _icySmoke.gameObject.SetActive(true);
        }
    }

    private void ToggleVFX()
    {
        float pointToPointDist = Vector3.Distance(pointToSnap.position,
                                          snapToPoint.position);
        if (pointToPointDist < 0.5F)
        {
            _icySmoke.SetActive(false);
        }
        else
            _icySmoke.SetActive(true);

    }

    private void EndTutorial() => StartCoroutine(CO_DisableTutorial());
    private IEnumerator CO_DisableTutorial()
    {
        if (!_isTutorial) yield break;

        yield return null;

        _isTutorial = false;
        OnBoardingHandler.Instance.OnTutorialEnd -= EndTutorial;
    }

#endregion
}
