using System.Reflection;

namespace CustomItems.DataBlocks
{
    public class TurretBlueprintDataBlock : BlueprintDataBlock
    {
        public TurretBlueprintDataBlock()
        {
            name = "Turret Blueprint";
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
            resultItem = CustomItems.Items.Find(i => i.name == "Turret");
            numResultItem = 1;

            ingredients = new[]
            {
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Iron Shards"), amount = 300},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "556 Ammo"), amount = 500},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Batteries"), amount = 8},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Electronic Junk"), amount = 10},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Copper"), amount = 8},
                new IngredientEntry
                    {Ingredient = CustomItems.Items.Find(i => i.name == "Diamonds"), amount = 50}
            };

            craftingDuration = 8;
            RequireWorkbench = true;

            var type = GetType().BaseType.BaseType.BaseType.BaseType;
            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
            uniqueIDField.SetValue(this, 134481426);
        }
    }
}