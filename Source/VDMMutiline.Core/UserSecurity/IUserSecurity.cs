using System.Globalization;

namespace VDMMutiline.Core.UserSecurity
{
    public interface IUserSecurity
    {
        CultureInfo GetCultureInfo();
        IUser GetCurrentUser();
        bool HasRole(string roleName);
        void SetCultureInfo(CultureInfo _ci);
        void SetUser(IUser user);
    }
}
