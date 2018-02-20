using Microsoft.WindowsAzure.Storage.Table;

namespace WebRole1
{
    // We want to store data in an Azure table. Each entity (row)
    // that we add requires a unique row key and a non-unique partition key.
    // These live in the base class TableServiceEntity from which we derive.
    // We add the properties that we want to store in the table.
    // We also have to add our own code to generate a unique row key.

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

    public class RecipeEntity : TableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double PreperationTime { get; set; }
        public string Cuisine { get; set; }

        public RecipeEntity() { }

        public RecipeEntity(string name, string description, double preperationTime, string cuisine)
        {
            Name = name;
            Description = description;
            PreperationTime = preperationTime;
            Cuisine = cuisine;

            PartitionKey = Cuisine;
            RowKey = Name;
        }
    }
}