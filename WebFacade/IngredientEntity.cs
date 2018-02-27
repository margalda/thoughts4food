using Microsoft.WindowsAzure.Storage.Table;

namespace WebFacade
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

    public enum Time
    {
        Minutes,
        Hours
    }

    public enum Cuisine
    {
        Italian,
        Chinese,
        American,
        Israeli,
        French,
        Mexican,
        Thai,
        Indian,
        Japanese,
        Vietnamese,
    }

    public class IngredientEntity : TableEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }

        public IngredientEntity()
        {
        }

        public IngredientEntity(string name, string category)
        {
            Name = name;
            Category = category;

            PartitionKey = Category;
            RowKey = Name;
        }
    }
}