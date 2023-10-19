using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VDMMutiline.Core.Cache;
using VDMMutiline.Core.Helper;

namespace VDMMutiline.Core.UserSecurity
{
    public class UserSecurity : IUserSecurity
    {
        private ICache _userCacher;

        public UserSecurity(ICache _cache)
        {
            _userCacher = _cache;
        }

        public CultureInfo GetCultureInfo()
        {
            CultureInfo userCache = (CultureInfo)_userCacher.GetUserCache("CurrentLanguage");
            if (userCache == null)
            {
                userCache = new CultureInfo("vi-VN");
                SetCultureInfo(userCache);
            }
            return userCache;
        }

        public IUser GetCurrentUser()
        {
            return (IUser)_userCacher.GetUserCache("userIdentity");
        }

        public bool HasRole(string roleName)
        {
            Func<Role, bool> predicate = null;
            IUser currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("User can not be null.");
            }
            if (currentUser.RoleList != null)
            {
                if (predicate == null)
                {
                    predicate = r => r.Name.Equals(roleName);
                }
                return (currentUser.RoleList.Where(predicate).Count() > 0);
            }
            return false;
        }

        public void SetCultureInfo(CultureInfo _ci)
        {
            _userCacher.SetUserCache("CurrentLanguage", _ci);
        }

        public void SetUser(IUser user)
        {
            if (user != null)
            {
                List<Role> list = new List<Role>();
                IEnumerable itemOfEntity = ReflectionHelper.GetItemOfEntity(user, "AspNetRoles") as IEnumerable;
                if (itemOfEntity != null)
                {
                    foreach (object obj3 in itemOfEntity)
                    {
                        if (ReflectionHelper.GetItemOfEntity(obj3, "Name") is string str)
                        {
                            Role item = new Role
                            {
                                Name = str
                            };
                            list.Add(item);
                        }
                    }
                }
                user.RoleList = list;
                _userCacher.SetUserCache("userIdentity", user);
            }
        }
    }
}
