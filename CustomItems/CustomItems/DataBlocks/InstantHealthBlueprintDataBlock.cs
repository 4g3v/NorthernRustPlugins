using System.Reflection;

namespace CustomItems.DataBlocks
{
    public class InstantHealthBlueprintDataBlock : BlueprintDataBlock
    {
        public InstantHealthBlueprintDataBlock()
        {
            name = "Adrenaline Rush Blueprint";
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
            resultItem = CustomItems.Items.Find(i => i.name == "Adrenaline Rush");
            numResultItem = 2;

            var ingredient1 = new IngredientEntry {Ingredient = DatablockDictionary.GetByName("Cloth"), amount = 3};
            var ingredient2 = new IngredientEntry {Ingredient = DatablockDictionary.GetByName("Blood"), amount = 3};
            var ingredient3 = new IngredientEntry
            {
                Ingredient = DatablockDictionary.GetByName("Small Rations"), amount = 1
            };
            var ingredient4 = new IngredientEntry
            {
                Ingredient = DatablockDictionary.GetByName("Large Medkit"), amount = 2
            };

            ingredients = new IngredientEntry[] {ingredient1, ingredient2, ingredient3, ingredient4};
            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481340);
        }
    }
}