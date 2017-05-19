using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("RecordsForInsertAsync")]
    public class RecordsForInsertAsync : Entity
    {
        public string Name { get; set; }
    }
}
