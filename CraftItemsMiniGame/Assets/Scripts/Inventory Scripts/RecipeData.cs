using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    public List<ItemData> requiredItems; 
    public ItemData resultItem; 
    public float successChance = 100f; 
}
