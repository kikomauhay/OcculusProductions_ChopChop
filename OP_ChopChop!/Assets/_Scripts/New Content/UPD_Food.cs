using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// This will act as the UNPLATED version of the food
/// This needs to be combined with a plate to make it into a dish
/// 
/// </summary>

[RequireComponent(typeof(Trashable))]
public class UPD_Food : MonoBehaviour
{

#region Members

#region Properties

    public FoodCondition Condition => _foodCondition;
    public DishPlatter OrderType => _orderType;
    public float Score => _foodScore;

#endregion
#region Private

    [Header("Food Attrbutes")]
    [SerializeField] private FoodCondition _foodCondition;
    [SerializeField] private DishPlatter _orderType;
    [SerializeField] private float _foodScore; 

    [Header("Food Materials")]
    [SerializeField] private Material _dirtyOSM;
    [SerializeField] private Material _rottenMat, _moldyMat;

    private Renderer _rend;

#endregion

#endregion

#region Methods

#region Unity

    private void Awake() 
    {
        _rend = GetComponent<Renderer>();

        if (GameManager.Instance.CurrentShift == GameShift.Training)
            OnBoardingHandler.Instance.OnTutorialEnd += () => Destroy(gameObject);
    }
    private void Start()
    {
        if (_orderType == DishPlatter.EMPTY)
            Debug.LogError("OrderType is set to empty!");

        _foodCondition = FoodCondition.CLEAN;
    }
    protected void OnCollisionEnter(Collision other)
    {
        // food -> sponge
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Sponge sponge = other.gameObject.GetComponent<Sponge>();

            if (_foodCondition != FoodCondition.CLEAN) 
            {
                sponge.SetDirty();
                Debug.LogWarning($"{name} contaminated {sponge.name}");
            }
        }

        // food -> ingredient
        if (other.gameObject.GetComponent<Ingredient>() != null)
        {
            Ingredient ing = other.gameObject.GetComponent<Ingredient>();

            if (_foodCondition != FoodCondition.CLEAN)
            {
                ing.SetMoldy();
                Debug.LogWarning($"{name} contaminated {ing.name}");
                return;
            }
            else if (!ing.IsFresh) 
            {
                SetMoldy();
                Debug.LogWarning($"{ing.name} contaminated {name}");
                return;
            }
        }

        // food -> another food
        if (other.gameObject.GetComponent<UPD_Food>() != null)
        {
            UPD_Food food = other.gameObject.GetComponent<UPD_Food>();

            if (_foodCondition != FoodCondition.CLEAN)
            {
                food.SetMoldy();
                Debug.LogWarning($"{name} contaminated {food.name}");
                return;
            }
            else if (food.Condition != FoodCondition.CLEAN)
            {
                SetMoldy();
                Debug.LogWarning($"{food.name} contaminated {name}");
                return;
            }
        }

        // food -> equipment
        if (other.gameObject.GetComponent<Equipment>() != null)
        {
            Equipment eq = other.gameObject.GetComponent<Equipment>();

            if (_foodCondition != FoodCondition.CLEAN)
            {
                eq.SetDirty();
                Debug.LogWarning($"{name} contaminated {eq.name}");
                return;
            }
            else if (!eq.IsClean)
            {
                SetMoldy();
                Debug.LogWarning($"{eq.name} contaminated {name}");
                return;
            }
        }
    }
    protected void Oestroy()
    {
        if (GameManager.Instance.CurrentShift == GameShift.Training)
            OnBoardingHandler.Instance.OnTutorialEnd -= () => Destroy(gameObject);
    }

#endregion
#region Public

    public void SetRotten()
    {
        if (_foodCondition == FoodCondition.MOLDY) 
        {
            Debug.LogError($"{gameObject.name} is already moldy!");
            return;
        }
        
        _foodCondition = FoodCondition.ROTTEN; 
        _rend.materials = new Material[] { _rottenMat, _dirtyOSM };
    }
    public void SetMoldy()
    {
        if (_foodCondition == FoodCondition.ROTTEN) 
        {
            Debug.LogError($"{gameObject.name} is already rotten!");
            return;
        }

        _foodCondition = FoodCondition.ROTTEN; 
        _rend.materials = new Material[] { _moldyMat, _dirtyOSM };
    }
    public void SetFoodScore(float score) => _foodScore = score;
    public void PickUpFood()
    {
        string soundName = string.Empty;

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0: soundName = "food grabbed 01"; break;
            case 1: soundName = "food grabbed 02"; break;
            case 2: soundName = "food grabbed 03"; break;
            default: break;
        }

        SoundManager.Instance.PlaySound(soundName);
    }

#endregion

    #endregion

    private IEnumerator CO_StartRotting()
    {
        yield return new WaitForSeconds(10f);
        SetRotten();
    }
}
