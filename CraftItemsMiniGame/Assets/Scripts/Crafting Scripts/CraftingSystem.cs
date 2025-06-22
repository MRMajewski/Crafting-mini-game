using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] 
    private RecipeDatabase recipeDatabase;

    public bool TryCraft(List<ItemData> itemsToCraft, out ItemData resultItem, out string resultMessage)
    {
        resultItem = null;

        RecipeData recipe = recipeDatabase.GetRecipeForItems(itemsToCraft);
        if (recipe == null)
        {
            resultMessage = "No matching recipe found!";
            return false;
        }

        float roll = UnityEngine.Random.Range(0f, 1f);
        if (roll <= recipe.successChance)
        {
            resultItem = recipe.resultItem;
            resultMessage = $"Crafting successful! Created {recipe.resultItem.itemName}";
            return true;
        }
        else
        {
            resultMessage = "Crafting failed!";
            return false;
        }
    }

    public RecipeData GetMatchingRecipe(List<ItemData> itemsToCraft)
    {
        return recipeDatabase.GetRecipeForItems(itemsToCraft);
    }
}