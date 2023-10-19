using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class EventLogDao : BaseDao
    {
        #region Action
        public int Insert(EventLog item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.EventLogs.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.ID;
            }
        }
        public void Update(EventLog item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.EventLogs.FirstOrDefault(sitem => sitem.ID == item.ID);
                if (dbItem != null)
                {

                    dbItem.ID = item.ID;
                    dbItem.IPDangnhap = item.IPDangnhap;
                    dbItem.Agent = item.Agent;
                    dbItem.IPXForward = item.IPXForward;
                    dbItem.IdNguoitao = item.IdNguoitao;
                    dbItem.Ngaytao = item.Ngaytao;
                    dbItem.Ngaysua = item.Ngaysua;
                    dbItem.Ghichu = item.Ghichu;
                    dbItem.Trangthai = item.Trangthai;
                    dbItem.Thietbi = item.Thietbi;
                    dbItem.MangXH = item.MangXH;
                    dbItem.LogLevel = item.LogLevel;
                    dbItem.LogType = item.LogType;

                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(EventLog item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.EventLogs.FirstOrDefault(en => en.ID == item.ID);

                if (dbItem != null)
                {
                    dbContext.EventLogs.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public EventLog GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.EventLogs
                            where n.ID == id
                            select n;
                return query.FirstOrDefault();
            }
        }

        public EventLogInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.EventLogs
                            where n.ID == id
                            select new EventLogInfo()
                            {
                                ID = n.ID,
                                IPDangnhap = n.IPDangnhap,
                                Agent = n.Agent,
                                IPXForward = n.IPXForward,
                                IdNguoitao = n.IdNguoitao,
                                Nguoitao = n.Nguoitao,
                                Ngaytao = n.Ngaytao,
                                Ngaysua = n.Ngaysua,
                                Ghichu = n.Ghichu,
                                Trangthai = n.Trangthai,
                                Thietbi = n.Thietbi,
                                MangXH = n.MangXH,
                                LogLevel = n.LogLevel,
                                LogType = n.LogType,

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(EventLogParam param)
        {
            var filter = param.EventLogFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.EventLogs
                            where (filter.Id.HasValue == false || n.ID == filter.Id) && (filter.ItemId.HasValue ==false || filter.ItemId == 0 || n.LogLevel == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.Nguoitao.Contains(filter.Search))
                            select new EventLogInfo
                            {
                                ID = n.ID,
                                IPDangnhap = n.IPDangnhap,
                                Agent = n.Agent,
                                IPXForward = n.IPXForward,
                                IdNguoitao = n.IdNguoitao,
                                Nguoitao = n.Nguoitao,
                                Ngaytao = n.Ngaytao,
                                Ngaysua = n.Ngaysua,
                                Ghichu = n.Ghichu,
                                Trangthai = n.Trangthai,
                                Thietbi = n.Thietbi,
                                MangXH = n.MangXH,
                                LogLevel = n.LogLevel,
                                LogType = n.LogType,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.EventLogInfos = query.OrderByDescending(p => p.Ngaytao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.EventLogInfos = query.OrderByDescending(p => p.Ngaytao).ToList();

                }
            }
        }
        public void GetAll(EventLogParam param)
        {
            var filter = param.EventLogFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.EventLogs
                            where (filter.Id.HasValue == false || n.ID == filter.Id) && (filter.ItemId.HasValue == false || filter.ItemId == 0 || n.LogLevel == filter.ItemId)
                            && (string.IsNullOrEmpty(filter.Search) || n.Nguoitao.Contains(filter.Search))
                            select new EventLogInfo
                            {
                                ID = n.ID,
                                IPDangnhap = n.IPDangnhap,
                                Agent = n.Agent,
                                IPXForward = n.IPXForward,
                                IdNguoitao = n.IdNguoitao,
                                Nguoitao = n.Nguoitao,
                                Ngaytao = n.Ngaytao,
                                Ngaysua = n.Ngaysua,
                                Ghichu = n.Ghichu,
                                Trangthai = n.Trangthai,
                                Thietbi = n.Thietbi,
                                MangXH = n.MangXH,
                                LogLevel = n.LogLevel,
                                LogType = n.LogType,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.EventLogInfos = query.OrderByDescending(p => p.Ngaytao).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.EventLogInfos = query.OrderByDescending(p => p.Ngaytao).ToList();

                }
            }
        }


        #endregion
    }
}
