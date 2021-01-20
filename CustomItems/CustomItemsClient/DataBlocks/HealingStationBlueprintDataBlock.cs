using System.Collections.Generic;
using System.Reflection;

namespace CustomItemsClient.DataBlocks
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
            resultItem = DatablockDictionary.GetByName("Healing Station");
            numResultItem = 1;

            ingredients = new[]
            {
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Large Medkit"), amount = 10},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Copper"), amount = 15},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Electronic Junk"), amount = 8},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Low Quality Metal"), amount = 20},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Batteries"), amount = 12}
            };

            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481381);
        }
    }
}