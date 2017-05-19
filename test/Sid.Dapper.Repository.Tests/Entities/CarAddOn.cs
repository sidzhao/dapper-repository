﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("CarAddOns")]
    public class CarAddOn : Entity
    {
        public int CarId { get; set; }

        public string OptionName { get; set; }
    }
}
