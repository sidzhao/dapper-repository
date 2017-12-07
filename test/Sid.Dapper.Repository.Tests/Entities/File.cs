using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Sid.Dapper.Repository.Attributes.LogicalDelete;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("Files")]
    public class File : Entity
    {
        public byte[] Content { get; set; }
    }
}
