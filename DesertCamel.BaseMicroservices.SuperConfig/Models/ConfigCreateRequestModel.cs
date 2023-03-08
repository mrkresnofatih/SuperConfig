using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperConfig.Models
{
    public class ConfigCreateRequestModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
