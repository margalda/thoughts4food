using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    public enum Measurement
    {
        Unit,
        Teaspoon,
        Cup,
        Kg,
    };

    public enum Category
    {
        SaucesAndSpices,
        MeatAndPasta,
        SnacksAndCookies,
        Baking,
        Dairy,
        FruitsAndVegetables
    };

    public class RecipeIngredientEntity : TableEntity
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Category { get; set; }
        public string Measurement { get; set; }
        public string RecipeName { get; set; }

        public RecipeIngredientEntity() { }

        public RecipeIngredientEntity(string name, double quantity, string category, string measurement, string recipeName)
        {
            Name = name;
            Quantity = quantity;
            Category = category;
            Measurement = measurement;
            RecipeName = recipeName;

            PartitionKey = Category;
            RowKey = $"{RecipeName}_{Name}";
        }
    }
}