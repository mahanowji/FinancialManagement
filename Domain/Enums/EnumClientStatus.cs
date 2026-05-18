using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EnumClientStatus
    {
        [Display(Name = "فعال", Description = "وضعیت فعال")]
        Active = 1,

        [Display(Name = "غیر فعال", Description = "وضعیت غیر فعال")]
        DeActive = 2,


        [Display(Name = "بسته", Description = "وضعیت بسته شده")]
        Closed = 3,
    }
}
