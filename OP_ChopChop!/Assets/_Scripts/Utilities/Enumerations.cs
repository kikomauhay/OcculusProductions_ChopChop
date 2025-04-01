using UnityEngine;
public class Enumerations : MonoBehaviour {}

/// <summary> -WHAT DOES THIS SCRIPT DO-
/// 
/// Anything script that uses enums is here
/// Don't change shit willy-nilly, you might break parts of the game
/// 
/// </summary>


#region Spawning

    public enum SpawnObjectType 
    { 
        INGREDIENT, 
        FOOD, 
        DISH, 
        CUSTOMER, 
        VFX 
    }
    public enum VFXType // & destroyTime
    { 
        SMOKE,   // 1s
        BUBBLE,  // 3s
        SPARKLE, // 5s
        STINKY,  // 5s
        RICE,    // 3s
        SPLASH   // 4s
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
    public enum IngredientType // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {    
        RICE, 
        SALMON,
        TUNA
    }
    public enum IngredientState // IN A CERTAIN ORDER (DON'T RE-ORDER)
    { 
        DEFAULT, 
        EXPIRED, 
        CONTAMINATED, 
        STORED
    }
    public enum MoldType // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {
        UNMOLDED,
        GOOD,
        PERFECT,
    BAD
}

#endregion

#region Others

    public enum GameDifficulty
    { 
        EASY, 
        NORMAL,
        HARD
    }

    public enum GameShift // IN A CERTAIN ORDER (DON'T RE-ORDER)
    {
        DEFAULT,
        TRAINING,
        PRE_SERVICE,
        SERVICE,
        POST_SERVICE
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
    public enum FaceVariant
    {
        NEUTRAL,
        HAPPY,
        MAD,
        SUS
    }
    public enum SoundGroup
    { 
        EQUIPMENT,
        APPLIANCES,
        FOOD,
        GAME,
        VFX,
        CUSTOMER
    }
#endregion