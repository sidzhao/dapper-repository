using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("RecordsForInsert")]
    public class RecordsForInsert : Entity
    {
        public string Name { get; set; }
    }
}
