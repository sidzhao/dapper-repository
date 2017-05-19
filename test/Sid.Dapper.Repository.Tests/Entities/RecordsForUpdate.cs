using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("RecordsForUpdate")]
    public class RecordsForUpdate : Entity
    {
        public string Name { get; set; }
    }
}
