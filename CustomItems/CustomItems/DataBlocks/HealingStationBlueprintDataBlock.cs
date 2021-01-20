using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Fougerite;
using UnityEngine;

namespace CustomItems.DataBlocks
{
    public class HealingStationBlueprintDataBlock : BlueprintDataBlock
    {
        public HealingStationBlueprintDataBlock()
        {
            name = "Healing Station Blueprint";
            icon = "content/item/tex/BlueprintIcon";
            _maxUses = 1;
            _spawnUsesMin = 1;
            _spawnUsesMax = 1;
            _minUsesForDisplay = 1;
            _maxCondition = 100;
            _splittable = false;
            isResearchable = false;
            isRepairable = false;
            isRecycleable = true;
            doesLoseCondition = false;
            category = ItemCategory.Blueprint;
            Combinations = new CombineRecipe[0];
            transientMode = 0;
            resultItem = CustomItems.Items.Find(i => i.name == "Healing Station");
            numResultItem = 1;

            ingredients = new[]
            {
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Large Medkit"), amount = 10},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Copper"), amount = 15},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Electronic Junk"), amount = 8},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Low Quality Metal"), amount = 20},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Batteries"), amount = 12}
            };

            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481381);
        }
    }
}