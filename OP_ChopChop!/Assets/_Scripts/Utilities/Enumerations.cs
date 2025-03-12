using UnityEngine;
public class Enumerations : MonoBehaviour {}

public enum GameShift 
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
        MAKI_SALMON, 
        MAKI_TUNA 
    }
    public enum IngredientType // will expand later
    { 
        RICE, 
        TUNA, 
        SALMON, 
        SEAWEED 
    }
    public enum IngredientState 
    { 
        DEFAULT, 
        EXPIRED, 
        CONTAMINATED, 
        STORED
    }
    public enum SliceType 
    { 
        THICK, 
        THIN, 
        SLAB 
    }
    public enum FishType // will expand expand later
    { 
        SALMON, 
        TUNA 
    }
    public enum MoldType
    {
        UNMOLDED,
        GOOD,
        PERFECT,
        BAD
    }

#endregion

#region Attributes

public enum StorageType 
    { 
        CHILLER, 
        FREEZER 
    }
    public enum TrashableType 
    { 
        FOOD, 
        INGREDIENT, 
        EQUIPMENT 
    }
    public enum CatVariant 
    { 
        CALICO, 
        SIAMESE, 
        TABBY, 
        TORBIE, 
        TUXEDO 
    }

#endregion