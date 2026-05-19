using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum EnumTaskPriority
    {
        [Display(Name = "کم", Description = "تسک‌های روتین اداری که فوریت زمانی ندارند")]
        Low = 1,

        [Display(Name = "متوسط", Description = "تسک‌های استاندارد مربوط به سرویس‌دهی جاری کلاینت")]
        Medium = 2,

        [Display(Name = "زیاد", Description = "موارد با اهمیت بالا مثل بررسی قراردادهای جدید یا مدارک حساس")]
        High = 3,

        [Display(Name = "فوری", Description = "موارد بحرانی که باید در سریع‌ترین زمان ممکن (ASAP) انجام شوند")]
        Urgent = 4
    }

}
