using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Crafting/RecipeDatabase")]
public class RecipeDatabase : ScriptableObject
{
    public List<RecipeData> recipes = new List<RecipeData>();

    public RecipeData GetRecipeByItems(List<ItemData> itemsToCraft)
    {
        foreach (RecipeData recipe in recipes)
        {
            if (HasRequiredItems(recipe, itemsToCraft))
            {
                return recipe;
            }
        }
        return null;
    }

    public RecipeData GetRecipeForItems(List<ItemData> itemsToCraft)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.requiredItems.Count == itemsToCraft.Count)
            {
                List<ItemData> requiredItemsCopy = new List<ItemData>(recipe.requiredItems);

                bool allItemsMatched = true;

                foreach (var item in itemsToCraft)
                {
                    if (requiredItemsCopy.Contains(item))
                    {
                        requiredItemsCopy.Remove(item);
                    }
                    else
                    {
                        allItemsMatched = false;
                        break;
                    }
                }

                if (allItemsMatched && requiredItemsCopy.Count == 0)
                {
                    return recipe;
                }
            }
        }
        return null; 
    }

    private bool HasRequiredItems(RecipeData recipe, List<ItemData> itemsToCraft)
    {
        if (recipe.requiredItems.Count != itemsToCraft.Count)
            return false;

        List<ItemData> requiredItemsCopy = new List<ItemData>(recipe.requiredItems);

        foreach (ItemData requiredItem in itemsToCraft)
        {
            if (!requiredItemsCopy.Remove(requiredItem));
            {
                return false;
            }
        }
        return requiredItemsCopy.Count == 0;
    }
}
