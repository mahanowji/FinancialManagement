using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum EnumTaskStatus
    {
        [Display(Name = "در انتظار", Description = "تسک ایجاد شده و در صف انجام قرار دارد")]
        Pending = 1,

        [Display(Name = "در حال انجام", Description = "تسک توسط مشاور یا کارکنان در حال پیگیری است")]
        InProgress = 2,

        [Display(Name = "تکمیل شده", Description = "عملیات مربوط به تسک با موفقیت پایان یافته است")]
        Completed = 3,

        [Display(Name = "لغو شده", Description = "تسک به دلایل مدیریتی یا درخواست کلاینت متوقف شده است")]
        Cancelled = 4,

        [Display(Name = "معوقه", Description = "زمان سررسید تسک گذشته و هنوز تکمیل نشده است")]
        Overdue = 5
    }
}
