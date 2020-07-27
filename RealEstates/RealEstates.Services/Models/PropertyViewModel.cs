namespace RealEstates.Services.Models
{
    public class PropertyViewModel 
    {
        public string District { get; set; }
        //tova e denormalizirano property - samo s vajnata info za originalniq District ot
        //originalniq Model class RealEstateProperty

        public int Size { get; set; }

        public int? Year { get; set; }
       
        public int Price { get; set; }
        
        public string PropertyType { get; set; } //denormalizirano property
        
        public string BuildingType { get; set; } //denormalizirano property

        public string Floor { get; set; } //combinirano property - shte napravq 2/6, t.e. 2-ri ot 6
    }
}
