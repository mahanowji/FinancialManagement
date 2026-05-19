using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum CommunicationType
    {
        [Display(Name = "تماس تلفنی", Description = "ثبت تماس‌های ورودی یا خروجی")]
        PhoneCall = 1,

        [Display(Name = "ایمیل", Description = "مکاتبات رسمی ایمیلی")]
        Email = 2,

        [Display(Name = "جلسه حضوری", Description = "ملاقات‌های داخل دفتر یا خارج از سازمان")]
        InPersonMeeting = 3,

        [Display(Name = "جلسه آنلاین", Description = "ویدیو کنفرانس یا جلسات مجازی")]
        OnlineMeeting = 4,

        [Display(Name = "پیام سیستمی", Description = "پیام‌های تبادل شده در Client Portal")]
        PortalMessage = 5
    }
}
