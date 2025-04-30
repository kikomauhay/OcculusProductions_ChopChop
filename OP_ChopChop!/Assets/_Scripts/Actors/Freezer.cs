using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
#region Members

    [SerializeField] private bool _isTutorial;
    [SerializeField] private List<Ingredient> _ingredients;

    private bool _tutorialPlayed = false;

#endregion

#region Methods

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing == null) return;

        if (!ing.IsFresh)
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            return;
        }

        // adds the ingredient to the freezer & changes its decay rate
        ing.Stored();
        _ingredients.Add(ing);

        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "door opened 01" : "door opened 02",
                                        SoundGroup.APPLIANCES);
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
                                        "door closed 01" : "door closed 02",
                                        SoundGroup.APPLIANCES);

        if (!_isTutorial) return;

        if (!_tutorialPlayed)
        {
            StartCoroutine(OnBoardingHandler.Instance.Onboarding04());
            _tutorialPlayed = true;
        }     
    }

#endregion
}
