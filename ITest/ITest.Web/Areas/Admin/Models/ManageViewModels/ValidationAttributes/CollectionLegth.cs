using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ITest.Web.Areas.Admin.Models.ManageViewModels.ValidationAttributes
{
    public class CollectionLegth : ValidationAttribute
    {
        private int min;

        public CollectionLegth(int min)
        {
            this.min = min;
        }

        public override bool IsValid(object value)
        {
            var list = value as ICollection;
            if (list != null)
            {
                return list.Count >= this.min;
            }
            return false;
        }
    }
}
