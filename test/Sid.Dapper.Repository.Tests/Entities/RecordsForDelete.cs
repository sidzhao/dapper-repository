using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("RecordsForDelete")]
    public class RecordsForDelete : Entity
    {
        public string Name { get; set; }
    }
}
