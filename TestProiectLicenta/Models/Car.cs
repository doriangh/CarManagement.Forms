namespace TestProiectLicenta.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Make { get; set; }
        public string Manufacturer { get; set; }
        public string Plant { get; set; }
        public string ModelYear { get; set; }
        public string SequentialNumber { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string Drive { get; set; }
        public string NumberofSeats { get; set; }
        public string NumberofDoors { get; set; }
        public string Steering { get; set; }
        public string EngineDisplacement { get; set; }
        public string EngineCylinders { get; set; }
        public string Transmission { get; set; }
        public string NumberofGears { get; set; }
        public string Engine { get; set; }
        public string Made { get; set; }
        public string Color { get; set; }
        public string Fuel { get; set; }
        public string Cc { get; set; }
        public string Power { get; set; }
        public string Emissions { get; set; }
        public string Odometer { get; set; }
        //public List<string> Equipment { get; set; }
        public string Vin { get; set; }
        public string License { get; set; }
        public string FullName => Make + ' ' + Model;
        public string CarImage { get; set; }
    }
}
