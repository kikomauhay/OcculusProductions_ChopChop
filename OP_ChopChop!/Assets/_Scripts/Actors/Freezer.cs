using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{

    List<Ingredient> _ingredients;
    const int MAX_CAPACITY = 3;

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing == null) return;

        if (!ing.IsFresh)
        {
            SoundManager.Instance.PlaySound("wrong", SoundGroup.GAME);
            return;
        }
        if (_ingredients.Count >= MAX_CAPACITY)
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
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient ing = other.gameObject.GetComponent<Ingredient>();

        if (ing == null) return;

        // removes the ingredient to the freezer & changes its decay rate
        ing.Unstored();
        _ingredients.Remove(ing);

        SoundManager.Instance.PlaySound(Random.value > 0.5f ?
                                        "door closed 01" : "door closed 02",
                                        SoundGroup.APPLIANCES);
    }
}
