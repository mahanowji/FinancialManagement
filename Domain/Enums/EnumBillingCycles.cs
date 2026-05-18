using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum EnumBillingCycles
    {
        [Display(Name = "یکبار", Description = "پرداخت فقط یکبار است")]
        OneTime = 1,

        [Display(Name = "ماهانه", Description = "پرداخت به صورت ماهانه است")]
        Monthly = 2,


        [Display(Name = "چهارماهه", Description = "پرداخت به صورت چهارماهه است")]
        Quarterly = 3,

        [Display(Name = "سالانه", Description = "پرداخت به صورت سالانه است")]
        Yearly = 4,
    }
}
