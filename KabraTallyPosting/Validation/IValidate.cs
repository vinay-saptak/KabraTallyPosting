using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;

namespace KabraTallyPosting.Validation
{
    public interface IValidate
    {
        ValidationResult Validate(int companyId, DateTime journeyDate);

    }
}
