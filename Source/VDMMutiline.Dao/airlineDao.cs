using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class AirlineDao : BaseDao
    {
        #region Action
        public int Insert(tbl_airline item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_airlines.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tbl_airline item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_airlines.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Code = item.Code;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tbl_airline item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_airlines.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.tbl_airlines.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_airline GetbyID(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_airlines
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public AirlineInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_airlines
                            where n.Id == id
                            select new AirlineInfo()
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,

                            };
                return query.FirstOrDefault();
            }
        }
        public AirlineInfo GetbyCode(string Code)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_airlines
                            where n.Code == Code
                            select new AirlineInfo()
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,
                                Pictures=n.Pictures

                            };
                return query.FirstOrDefault();
            }
        }
        public void Search(AirlineParam param)
        {
            var filter = param.AirlineFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_airlines
                            where (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                            select new AirlineInfo()
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,

                            };

                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.AirlineInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.AirlineInfos = query.OrderByDescending(z => z.Id).ToList();

                }
            }
        }

        #endregion
    }
}
