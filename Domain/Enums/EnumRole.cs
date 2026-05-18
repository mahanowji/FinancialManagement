using System.ComponentModel.DataAnnotations;

namespace CmsKit.Domain.Enums
{
    public enum EnumRole
    {
        [Display(Name = "ادمین", Description = "دسترسی کامل به تمامی بخش‌های سیستم")]
        Admin = 1,

        [Display(Name = "مشاور", Description = "امکان بررسی و مدیریت کاربران، بدون دسترسی به تنظیمات اصلی")]
        Advisor = 2,

        [Display(Name = "کارمند", Description = "نقش کمکی مشاور")]
        Staff = 3,

        [Display(Name = "کلاینت", Description = "دسترسی فقط برای کار های محدود")]
        Client = 4,

    }
}
