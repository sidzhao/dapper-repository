using System.ComponentModel.DataAnnotations;
using Sid.Dapper.Repository.Attributes;

namespace Sid.Dapper.Repository.Tests.Entities
{
    public class Entity
    {
        [Key, Identity]
        public int Id { get; set; }
    }
}
