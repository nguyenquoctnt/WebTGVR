using System.ComponentModel;


namespace ApiClient.Common.Enums
{
    public enum SleevesupApiErrorEnum
    {
        [Description("Username already exists")]
        DUPLICATE_USER = 0,
        [Description("Application Name not found")]
        INVALID_APPLICATION_NAME = 1,
        [Description("Email is invalid")]
        INVALID_EMAIL = 2,
        [Description("Username cannot be blank")]
        USERNAME_BLANK = 3,
        [Description("No data to process")]
        NO_DATA = 4,
        [Description("Mobile Number is not numeric")]
        NOT_NUMERIC_MOBILE = 5,
        [Description("date of birth is invalid")]
        DOB_INVALID = 6,
        [Description("Passcode must be 4 digits")]
        PASSCODE_INVALID = 7,
        [Description("Password must be between 8 and 32 characters and contain at least 1 number,uppercase,lowercase and symbol")]
        PASSWORD_INVALID = 8,
        [Description("Please try another passcode")]
        PASSCODE_IN_USE = 9
    }
}
