using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum EnumRoles
    {
        [Display(Name = "ادمین", Description = "دسترسی کامل به تمامی بخش‌ها")]
        Admin = 1,

        [Display(Name = "مشاور", Description = "امکان بررسی و مدیریت محتوا و کاربران.")]
        Advisor = 2,

        [Display(Name = "کارمند", Description = "نیروی کمکی مشاور")]
        Staff = 3,

        [Display(Name = "مشتری", Description = "مشتری در سیستم")]
        Client = 3,

    }
}
