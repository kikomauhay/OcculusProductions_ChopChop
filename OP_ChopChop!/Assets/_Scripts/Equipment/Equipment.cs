using UnityEngine;

public enum TrashableType { FOOD, INGREDIENT, EQUIPMENT }

public abstract class Equipment : MonoBehaviour 
{
    public TrashableType TrashType => _trashType;

    protected Vector3 _startPosition;

    [SerializeField] protected TrashableType _trashType;

    protected virtual void Start() 
    {
        _startPosition = transform.position;
        _trashType = TrashableType.EQUIPMENT;
    }

    protected void Reset() => Reposition();
    protected void Reposition() => transform.position = _startPosition;
}
