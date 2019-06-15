﻿namespace TestProiectLicenta.Models
{
    public class ListCarAttributes
    {
        public int CarId { get; set; }
        public string FullName { get; set; }
        public string ModelYear { get; set; }
        public string RemainingItp { get; set; }
        public string RemainingRoadTax { get; set; }
        public string RemainingOilChange { get; set; }
        public string RoadTaxValue { get; set; }
        public string InsuranceValue { get; set; }
        public string TaxValue { get; set; }
        public string CarImage { get; set; }
        public int Price { get; set; }

        public string InsuranceColor { get; set; }
        public string InsuranceIcon { get; set; }

        public string ITPColor { get; set; }
        public string ITPIcon { get; set; }

        public string RoadTaxColor { get; set; }
        public string RoadTaxIcon { get; set; }

        public string OilColor { get; set; }
        public string OilIcon { get; set; }

        public string TiresColor { get; set; }
        public string TiresIcon { get; set; }
    }
}