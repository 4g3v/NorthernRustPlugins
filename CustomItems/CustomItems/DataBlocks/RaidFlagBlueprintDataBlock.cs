//using System.Reflection;
//
//namespace CustomItems.DataBlocks
//{
//    public class RaidFlagBlueprintDataBlock : BlueprintDataBlock
//    {
//        public RaidFlagBlueprintDataBlock()
//        {
//            name = "Raid Flag Blueprint";
//            icon = "content/item/tex/BlueprintIcon";
//            _maxUses = 1;
//            _spawnUsesMin = 1;
//            _spawnUsesMax = 1;
//            _minUsesForDisplay = 1;
//            _maxCondition = 100;
//            _splittable = false;
//            isResearchable = false;
//            isRepairable = false;
//            isRecycleable = true;
//            doesLoseCondition = false;
//            category = ItemCategory.Blueprint;
//            Combinations = new CombineRecipe[0];
//            transientMode = 0;
//            resultItem = CustomItems.Items.Find(i => i.name == "Raid Flag");
//            numResultItem = 1;
//
//            ingredients = new[]
//            {
//                new IngredientEntry
//                    {Ingredient = CustomItems.Items.Find(i => i.name == "Iron Shards"), amount = 100},
//                new IngredientEntry
//                    {Ingredient = CustomItems.Items.Find(i => i.name == "Cloth"), amount = 50}
//            };
//
//            craftingDuration = 8;
//            RequireWorkbench = true;
//
//            var type = GetType().BaseType.BaseType.BaseType.BaseType;
//            var uniqueIDField = type.GetField("_uniqueID", BindingFlags.NonPublic | BindingFlags.Instance);
//            uniqueIDField.SetValue(this, 134481402);
//        }
//    }
//}