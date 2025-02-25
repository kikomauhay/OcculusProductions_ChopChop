using UnityEngine;
public class Enumerations : MonoBehaviour {}


#region Spawning

public enum FoodItemType { INGREDIENT, FOOD, DISH }
public enum VFXType { SMOKE, BUBBLE, SPARKLE, STINKY }

#endregion

#region Food_Types

public enum DishType { NIGIRI_SALMON, NIGIRI_TUNA, MAKI_SALMON, MAKI_TUNA }
public enum IngredientType { RICE, TUNA, SALMON, SEAWEED } // will expand later

public enum IngredientState { DEFAULT, ROTTEN, MOLDY, TRASHED, STORED }
public enum FreshnessRating { FRESH, LESS_FRESH, EXPIRED }

#endregion

#region Attributes

public enum StorageType { CHILLER, FREEZER } 
public enum TrashableType { FOOD, INGREDIENT, EQUIPMENT }

public enum CatVariant { CALICO, SIAMESE, TABBY, TORBIE, TUXEDO }

#endregion