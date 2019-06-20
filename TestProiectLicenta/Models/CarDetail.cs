using System;

namespace TestProiectLicenta.Models
{
    public class CarDetail
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Itp { get; set; }
        public DateTime RoadTax { get; set; }
        public int RoadTaxPeriod { get; set; }
        public bool WinterTires { get; set; }
        public int Price { get; set; }
        public int OilChange { get; set; }
        public DateTime Insurance { get; set; }
        public int InsurancePeriod { get; set; }
        public int InsuranceValue { get; set; }
        public int RoadTaxValue { get; set; }
        public int TaxValue { get; set; }

    }
}