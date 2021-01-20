using System.Reflection;

namespace CustomItems.DataBlocks
{
    public class LootboxBlueprintDataBlock : BlueprintDataBlock
    {
        public LootboxBlueprintDataBlock()
        {
            name = "Lootbox Blueprint";
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
            resultItem = CustomItems.Items.Find(i => i.name == "Lootbox");
            numResultItem = 1;

            ingredients = new[]
            {
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Low Quality Metal"), amount = 20},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Leather"), amount = 80},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Metal Fragments"), amount = 80},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Gunpowder"), amount = 70}
            };

            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481383);
        }
    }
}