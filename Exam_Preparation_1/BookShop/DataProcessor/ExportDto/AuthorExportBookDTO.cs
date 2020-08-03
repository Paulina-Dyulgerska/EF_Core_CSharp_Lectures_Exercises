using Newtonsoft.Json;

namespace BookShop.DataProcessor.ExportDto
{
    public class AuthorExportBookDTO
    {
        [JsonProperty("BookName")]
        public string BookName { get; set; }

        [JsonIgnore]
        public decimal BookRealPrice { get; set; }

        [JsonProperty("BookPrice")]
        public string BookPrice => $"{this.BookRealPrice.ToString():f2}";
    }
}
