using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs.Category
{
    public class CategoryWithProductsDTO
    {

        [JsonProperty("category")]
        public string CategoryName { get; set; }

        public int ProductsCount { get; set; }

        public string AveragePrice { get; set; }

        public string TotalRevenue { get; set; }

    }
}
