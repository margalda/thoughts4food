using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    public class RecipeIngredientEntity : TableEntity
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Category { get; set; }
        public string RecipeName { get; set; }

        public RecipeIngredientEntity() { }

        public RecipeIngredientEntity(string name, double quantity, string category, string recipeName)
        {
            Name = name;
            Quantity = quantity;
            Category = category;
            RecipeName = recipeName;

            PartitionKey = Category;
            RowKey = $"{RecipeName}_{Name}";
        }
    }
}