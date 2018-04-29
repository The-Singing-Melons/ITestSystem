﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Itest.Data.Models.Abstracts
{
    public class DataModel : IDeletable, IEditable
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}

