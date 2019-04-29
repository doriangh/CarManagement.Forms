using System;

namespace TestProiectLicenta.Models
{
    public class CarDetail
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime ITP { get; set; }
        public DateTime RoadTax { get; set; }
        public bool WinterTires { get; set; }
        public DateTime OilChange { get; set; }
        public int InsuranceValue { get; set; }
        public int RoadTaxValue { get; set;}
        public int TaxValue { get; set; }
    }
}