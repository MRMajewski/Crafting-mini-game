using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public List<RecipeData> recipes = new List<RecipeData>();

    public RecipeData GetRecipeByItems(List<ItemData> items)
    {
        foreach (RecipeData recipe in recipes)
        {
            if (HasRequiredItems(recipe, items))
            {
                return recipe;
            }
        }
        return null;
    }
    public RecipeData GetRecipeForItems(List<ItemData> items)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.requiredItems.Count == items.Count)
            {
                bool match = true;
                foreach (var item in recipe.requiredItems)
                {
                    if (!items.Contains(item))
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return recipe;
                }
            }
        }
        return null;
    }


    private bool HasRequiredItems(RecipeData recipe, List<ItemData> items)
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
