using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ClientStatusEntity
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; } = null!;
    }
}
