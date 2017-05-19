using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("Roles")]
    public class Role : Entity
    {
        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
 