using System.Collections.Generic;

namespace VDMMutiline.Core.UserSecurity
{
    public interface IUser
    {
        List<Role> RoleList { get; set; }
    }
}
