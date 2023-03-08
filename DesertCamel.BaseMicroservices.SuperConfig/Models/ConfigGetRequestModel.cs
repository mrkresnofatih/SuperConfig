using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperConfig.Models
{
    public class ConfigGetRequestModel
    {
        [Required]
        public string Key { get; set; }
    }
}
