using System.Reflection;

namespace CustomItemsClient.DataBlocks
{
    public class RespawnFlagBlueprintDataBlock : BlueprintDataBlock
    {
        public RespawnFlagBlueprintDataBlock()
        {
            name = "Respawn Flag Blueprint";
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
            resultItem = DatablockDictionary.GetByName("Respawn Flag");
            numResultItem = 1;

            ingredients = new[]
            {
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Iron Shards"), amount = 500},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Cloth"), amount = 50},
                new IngredientEntry
                    {Ingredient = DatablockDictionary.GetByName("Copper"), amount = 8},
            };

            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481389);
        }
    }}