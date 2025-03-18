using UnityEngine;
public class Enumerations : MonoBehaviour {}

public enum GameShift // CORRECT ORDER
{ 
    DEFAULT,
    PRE_PRE_SERVICE,
    PRE_SERVICE,
    SERVICE, 
    POST_SERVICE
}

#region Spawning

    public enum SpawnObjectType 
    { 
        INGREDIENT, 
        FOOD, 
        DISH, 
        CUSTOMER, 
        VFX 
    }
    public enum VFXType 
    { 
        SMOKE, 
        BUBBLE, 
        SPARKLE, 
        STINKY 
    }

#endregion

#region Food_Types

    public enum DishType 
    { 
        NIGIRI_SALMON, 
        NIGIRI_TUNA, 
        SASHIMI_SALMON, 
        SASHIMI_TUNA 
    }
    public enum IngredientType // IN A CERTAIN ORDER
    {    
        RICE, 
        SALMON,
        TUNA
    }
    public enum IngredientState // IN A CERTAIN ORDER
    { 
        DEFAULT, 
        EXPIRED, 
        CONTAMINATED, 
        STORED
    }
    public enum MoldType // IN A CERTAIN ORDER
    {
        UNMOLDED,
        GOOD,
        PERFECT,
        BAD
    }

#endregion

#region Others

    public enum StorageType 
    { 
        CHILLER, 
        FREEZER 
    }
    public enum TrashableType 
    { 
        EQUIPMENT,
        INGREDIENT, 
        FOOD, 
        DISH
    }
    public enum CatVariant 
    { 
        CALICO, 
        SIAMESE, 
        TABBY, 
        TORBIE, 
        TUXEDO 
    }
    public enum ShoppingCart
    {
        SALMON,
        TUNA,
        RICE,
        SEAWEED
    }


#endregion