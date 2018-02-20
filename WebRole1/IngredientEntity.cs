using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    public enum Category
    {
        SaucesAndSpices,
        MeatAndPasta,
        SnacksAndCookies,
        Baking,
        Dairy,
        FruitsAndVegetables
    }

    public class IngredientEntity : TableEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }

        public IngredientEntity() { }

        public IngredientEntity(string name, string category)
        {
            Name = name;
            Category = category;

            PartitionKey = Category;
            RowKey = Name;
        }
    }
}