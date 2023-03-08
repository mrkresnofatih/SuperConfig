using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperConfig.Models
{
    public class ConfigDeleteRequestModel
    {
        [Required]
        public string Key { get; set; }
    }
}
