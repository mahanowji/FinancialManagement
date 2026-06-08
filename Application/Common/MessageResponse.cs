namespace Application.Common;

public static class MessageResponse
{

    public const string UserNotFound = "یوزر مورد نظر یافت نشد";
    public const string AlreadyHasPrimaryWallet = "یوزر مورد نظر قبلا یک کیف پول به عنوان کیف پول اصلی ثبت کرده، بیشتر از یک کیف پول اصلی مجاز نیست";
    public const string UserProfileNotFound = " پروفایل یوزر مورد نظر یافت نشد";
    public const string RoleNotFound = "نقش مورد نظر یافت نشد";
    public const string UserRoleNotFound = "نقش منتسب به کاربر یافت نشد";
    public const string PhoneNumberNotValid = "شماره تلفن ولید نیست";
    public const string RegisteredBefore = "این شماره تلفن قبلا ثبت شده است، لطفا با شماره دیگری ثبت نام کنید";
    public const string loginfailed = "لاگین نا موفق";
    public const string ChangePasswordFailed = "بازیابی رمز عبور ناموفق";
    public const string PostNotFound = "پست مورد نظر یافت نشد";
    public static string UserAlreadyHasRole(string roleName)
            => $"the user already has the role {roleName}, in order to assign a new role, delete the previous one";




}