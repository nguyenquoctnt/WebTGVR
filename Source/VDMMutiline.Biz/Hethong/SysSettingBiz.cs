using VDMMutiline.Dao;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Biz
{
    public class SysSettingBiz
    {
        public void GetAllSettingUser(SettingUserParam param)
        {
            var dao = new SettingDao();
            dao.GetAllSettingUser(param);
        }
        public void UpdataSettingUser(SettingUserParam param)
        {
            var dao = new SettingDao();
            dao.UpdataSettingUser(param.SettingUser);
        }
        public void GetAllSettingGroup(SettingGroupParam param)
        {
            var dao = new SettingDao();
            dao.GetAllSettingGroup(param);
        }
        public void UpdataSettingGroup(SettingGroupParam param)
        {
            var dao = new SettingDao();
            dao.UpdataSettingGroup(param.SettingGroup);
        }
        public void Update(SettingSystemParam param)
        {
            var endep = param.SysSettingSystem;
            var dao = new SettingDao();
            dao.Update(endep);
        }
        public void SetupDisplayForm(SettingSystemParam param)
        {
            var dao = new SettingDao();
            param.SettingSystemInfo = dao.GetInfo(param.Id);
        }
        public void GetInfoBykey(SettingSystemParam param)
        {
            var dao = new SettingDao();
            param.SettingSystemInfo = dao.GetInfoBykey(param.Key);
        }
        public void GetAll(SettingSystemParam param)
        {
            var dao = new SettingDao();
            dao.GetAll(param);
        }
    }
}
