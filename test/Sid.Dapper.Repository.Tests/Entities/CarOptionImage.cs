using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("CarOptionImages")]
    public class CarOptionImage : Entity
    {
        public int CarOptionId { get; set; }

        public string Name { get; set; }
    }
}
 