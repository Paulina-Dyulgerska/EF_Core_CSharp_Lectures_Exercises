using Newtonsoft.Json;

namespace BookShop.DataProcessor.ExportDto
{
   public class AuthorExportDTO
    {
        [JsonProperty("AuthorName")]
        public string AuthorName { get; set; }

        [JsonProperty("Books")]
        public AuthorExportBookDTO[] Books { get; set; }
    }
}
