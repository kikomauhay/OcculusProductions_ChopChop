using UnityEngine;

public class Trashable : MonoBehaviour
{
    public TrashableType TrashTypes => _trashType;
    [SerializeField] private TrashableType _trashType;
}

#region Enumeration

    public enum TrashableType
    {
        EQUIPMENT,
        INGREDIENT,
        FOOD,
    }

#endregion