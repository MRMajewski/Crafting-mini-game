using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();

    public Recipe GetRecipeByItems(List<ItemData> items)
    {
        foreach (Recipe recipe in recipes)
        {
            if (HasRequiredItems(recipe, items))
            {
                return recipe;
            }
        }
        return null;
    }

    private bool HasRequiredItems(Recipe recipe, List<ItemData> items)
    {
        foreach (ItemData requiredItem in recipe.requiredItems)
        {
            if (!items.Contains(requiredItem))
            {
                return false;
            }
        }
        return true;
    }
}
