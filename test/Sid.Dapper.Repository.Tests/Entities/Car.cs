﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("Cars")]
    public class Car : Entity
    {
        public int UserId { get; set; }

        public string CarName { get; set; }

        public IList<CarOption> Options { get; set; }

        public IList<CarAddOn> AddOns { get; set; }
    }
}
