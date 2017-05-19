using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sid.Dapper.Repository.Attributes;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("DishOptions")]
    public class DishOption
    {
        [Key, Identity]
        public int DishOptionId { get; set; }

        public int DishId { get; set; }

        public string Option { get; set; }
    }
}
