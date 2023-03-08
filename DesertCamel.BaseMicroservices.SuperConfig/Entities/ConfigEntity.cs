using System.ComponentModel.DataAnnotations;

namespace DesertCamel.BaseMicroservices.SuperConfig.Entities
{
    public class ConfigEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
