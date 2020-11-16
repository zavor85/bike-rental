namespace BikeRental
{
    public class Bike
    {
        public Bike(string id, decimal pricePerMinute)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
        }
        
        public string Id { get; }
        public decimal PricePerMinute { get; }
        public bool IsRented { get; set; }
    
    }
}
