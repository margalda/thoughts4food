namespace WebRole1
{
    public enum Measurement
    {
        Unit,
        Teaspoon,
        Cup,
        Kg,
    };

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public Measurement Measurement { get; set; }

        public Ingredient(string name, double quantity, Measurement measurement)
        {
            Name = name;
            Quantity = quantity;
            Measurement = measurement;
        }
    }
}