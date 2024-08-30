using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; private set; }
    public RecipeDatabase recipeDatabase;
    public Inventory inventory;
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
    public void Craft(List<ItemData> itemsToCraft)
    {
        RecipeData recipe = recipeDatabase.GetRecipeForItems(itemsToCraft);

        if (recipe != null)
        {
            float roll = Random.Range(0f, 1f);
            if (roll <= recipe.successChance)
            {
                foreach (var item in recipe.requiredItems)
                {
                    inventory.RemoveItem(item.itemName);
                }

                inventory.AddItem(recipe.resultItem.itemName);
                Debug.Log($"Crafting successful! Created {recipe.resultItem.itemName}");
            }
            else
            {
                Debug.Log("Crafting failed!");
            }
        }
        else
        {
            Debug.Log("No matching recipe found!");
        }
    }
}