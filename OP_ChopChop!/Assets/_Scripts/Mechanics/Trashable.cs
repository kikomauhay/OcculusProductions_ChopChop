using UnityEngine;

public class Trashable : MonoBehaviour
{
    public TrashableType TrashTypes => _trashType;

    [SerializeField] TrashableType _trashType;
}
