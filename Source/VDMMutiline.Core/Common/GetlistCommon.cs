using System.Collections.Generic;
using VDMMutiline.Biz;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;
using System.Linq;

namespace VDMMutiline.Core
{
    public class GetlistCommon
    {

        public static List<DmTinhthanhInfo> Getlisttinhthanh()
        {
            var param = new DmTinhthanhParam() { DmTinhthanhFilter = new DmTinhthanhFilter() };
            var biz = new DmTinhthanhBiz();
            biz.GetAll(param);
            return param.DmTinhthanhInfos;
        }
        public static List<DmTinhthanhInfo> GetlisttinhthanhByQuocgia(int idquocgia)
        {
            var param = new DmTinhthanhParam() { DmTinhthanhFilter = new DmTinhthanhFilter() { Idquocgia = idquocgia } };
            var biz = new DmTinhthanhBiz();
            biz.Search(param);
            return param.DmTinhthanhInfos;
        }
        public static List<DmHuyenthiInfo> Gethuyenthibytinhthanh(int? idtinhthanh)
        {
            var param = new DmHuyenthiParam() { DmHuyenthiFilter = new DmHuyenthiFilter { Idtinhthanh = idtinhthanh } };
            var biz = new DmHuyenthiBiz();
            biz.Search(param);
            return param.DmHuyenthiInfos;
        }

        public static List<DmXaphuongInfo> GetXaphuongbyHuyenthi(int? idhuyen)
        {
            var param = new DmXaphuongParam() { DmXaphuongFilter = new DmXaphuongFilter { IdHuyen = idhuyen } };
            var biz = new DmXaphuongBiz();
            biz.Search(param);
            return param.DmXaphuongInfos;
        }

        public static List<DmQuocgiaInfo> Getlistquocgia()
        {
            var param = new DmQuocgiaParam() { DmQuocgiaFilter = new DmQuocgiaFilter() };
            var biz = new DmQuocgiaBiz();
            biz.GetAll(param);
            return param.DmQuocgiaInfos;
        }

        public static AspNetUserInfo Getinforuser(string username)
        {
            var biz = new AspNetUsersBiz();
            return biz.GetInfoByLoginName(username);
        }
        public static bool CheckGroupUser(int GroupId, string username)
        {
            var biz = new AspNetUsersBiz();
            var objuser = biz.GetInfoByLoginName(username);
            if (objuser == null)
                return false;
            var groupbyuserparam = new AspNetUserGroupsParam() { userId = objuser.Id };
            var bizgroup = new AspNetUserGroupsBiz();
            bizgroup.GetAllByusername(groupbyuserparam);
            var listidquyen = groupbyuserparam.AspNetUserGroupsInfos.Select(z => z.GroupId).ToList();
            return listidquyen.Where(z => z == GroupId).Count() > 0;
        }
        public static bool CheckGroupUser(int GroupId, int? userID)
        {
            var groupbyuserparam = new AspNetUserGroupsParam() { userId = userID ?? 0 };
            var bizgroup = new AspNetUserGroupsBiz();
            bizgroup.GetAllByusername(groupbyuserparam);
            var listidquyen = groupbyuserparam.AspNetUserGroupsInfos.Select(z => z.GroupId).ToList();
            return listidquyen.Where(z => z == GroupId).Count() > 0;
        }
    }
}