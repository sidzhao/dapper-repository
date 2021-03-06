﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Sid.Dapper.Repository.Tests.Entities
{
    [Table("CarOptions")]
    public class CarOption : Entity
    {
        public int CarId { get; set; }

        public string OptionName { get; set; }

        public CarOptionImage Image { get; set; }
    }
}
