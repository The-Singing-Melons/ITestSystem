using System;

namespace Itest.Data.Models.Abstracts
{
    public interface IEditable
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
