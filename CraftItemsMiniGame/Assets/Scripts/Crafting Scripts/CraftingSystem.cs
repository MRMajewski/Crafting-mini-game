using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; private set; }

    [SerializeField] 
    private RecipeDatabase recipeDatabase;
    //   public Inventory inventory;

    //  [SerializeField]
    //   private CraftingUI craftingUI;

    public event Action OnItemCrafted;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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



    //public ItemData Craft(List<ItemData> itemsToCraft)
    //{
    //    foreach (var item in itemsToCraft)
    //    {
    //        Debug.Log(item.itemName);
    //    }

    //    RecipeData recipe = recipeDatabase.GetRecipeForItems(itemsToCraft);

    //    if (recipe != null)
    //    {
    //        Debug.Log($"Recipe found: {recipe.resultItem.itemName}");

    //        float roll = Random.Range(0f, 1f);
    //        Debug.Log($"Roll: {roll}, Success Chance: {recipe.successChance}");

    //        if (roll <= recipe.successChance)
    //        {
    //            foreach (var item in recipe.requiredItems)
    //            {
    //                bool removed = inventory.RemoveItem(item.itemName);
    //                if (removed)
    //                {
    //                    Debug.Log($"{item.itemName} removed from inventory.");
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"{item.itemName} could not be removed!");
    //                }
    //            }

    //            craftingUI.ResultInfoText.text = $"Crafting successful! Created {recipe.resultItem.itemName}";
    //            return recipe.resultItem;
    //        }
    //        else
    //        {
    //            craftingUI.ResultInfoText.text = "Crafting failed!";
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        craftingUI.ResultInfoText.text = "No matching recipe found!";
    //        return null;
    //    }
    //}

    public RecipeData GetMatchingRecipe(List<ItemData> itemsToCraft)
    {
        return recipeDatabase.GetRecipeForItems(itemsToCraft);
    }
}