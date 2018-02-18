using System;
using System.Collections.Generic;

namespace WebRole1
{
    // We want to store data in an Azure table. Each entity (row)
    // that we add requires a unique row key and a non-unique partition key.
    // These live in the base class TableServiceEntity from which we derive.
    // We add the properties that we want to store in the table, in this case
    // two strings called Name and Body. We also have to add our own code
    // to generate a unique row key.

    public enum Time
    {
        Minutes,
        Hours
    }

    public enum Kind
    {
        Meat,
        Dairy,
        MeatAndDairy,
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


    public class RecipeEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double PreperationTime { get; set; }
        public Time Time { get; set; }
        public Kind Kind { get; set; }
        public Cuisine Cuisine { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public RecipeEntity(string name, string description, double preperationTime, Time time, Kind kind, Cuisine cuisine)
        { 
            Name = name;
            Description = description;
            PreperationTime = preperationTime;
            Time = time;
            Kind = kind;
            Cuisine = cuisine;
            Ingredients = new List<Ingredient>();

            PartitionKey = cuisine.ToString();
            RowKey = $"{DateTime.MaxValue.Ticks - DateTime.Now.Ticks:10}_{name}";
        }

        public void AddIngredients(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }
    }
}