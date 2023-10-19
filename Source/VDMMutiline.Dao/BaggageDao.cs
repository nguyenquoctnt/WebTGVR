using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class BaggageDao : BaseDao
    {
        #region Action
        public int Insert(tblBaggage item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tblBaggages.InsertOnSubmit(item);
                dbContext.SubmitChanges();
                return item.Id;
            }
        }
        public void Update(tblBaggage item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tblBaggages.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Id = item.Id;
                    dbItem.Name = item.Name;
                    dbItem.Code = item.Code;
                    dbItem.UpdateDate = item.UpdateDate;
                    dbItem.UpdateBy = item.UpdateBy;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(tblBaggage item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tblBaggages.FirstOrDefault(en => en.Id == item.Id);

                if (dbItem != null)
                {
                    dbContext.tblBaggages.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tblBaggage GetbyID(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblBaggages
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public BaggageInfo GetInfo(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblBaggages
                            where n.Id == id
                            select new BaggageInfo()
                            {
                                Id = n.Id,
                                Code = n.Code,
                                Name = n.Name,

                            };
                return query.FirstOrDefault();
            }
        }
        public BaggageInfo GetbyCode(string Code)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblBaggages
                            where n.Code == Code
                            select new BaggageInfo()
                            {
                                Id = n.Id,
                                Airline = n.Airline,
                                Code = n.Code,
                                Name = n.Name,
                                Price = n.Price,
                                Value = n.Value,
                                CreateDate = n.CreateDate,
                                CreateBy = n.CreateBy,
                                UpdateDate = n.UpdateDate,
                                UpdateBy = n.UpdateBy,
                            };
                return query.FirstOrDefault();
            }
        }
        public List<BaggageInfo> GetlistByAirlineCode(string AirlineCode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblBaggages
                            where n.Airline == AirlineCode
                            select new BaggageInfo()
                            {
                                Id = n.Id,
                                Airline = n.Airline,
                                Code = n.Code,
                                Name = n.Name,
                                Price = n.Price,
                                Value = n.Value,
                                CreateDate = n.CreateDate,
                                CreateBy = n.CreateBy,
                                UpdateDate = n.UpdateDate,
                                UpdateBy = n.UpdateBy,

                            };
                return query.OrderByDescending(z => z.Id).ToList(); ;
            }
        }
        public void Search(BaggageParam param)
        {
            var filter = param.BaggageFilter;
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tblBaggages
                            where (string.IsNullOrEmpty(filter.Search) || n.Name.Contains(filter.Search))
                            select new BaggageInfo()
                            {
                                Id = n.Id,
                                Airline = n.Airline,
                                Code = n.Code,
                                Name = n.Name,
                                Price = n.Price,
                                Value = n.Value,
                                CreateDate = n.CreateDate,
                                CreateBy = n.CreateBy,
                                UpdateDate = n.UpdateDate,
                                UpdateBy = n.UpdateBy,

                            };
                if (param.PagingInfo != null)
                {
                    param.PagingInfo.RecordCount = query.Count();
                    param.BaggageInfos = query.OrderByDescending(z => z.Id).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                }
                else
                {
                    param.BaggageInfos = query.OrderByDescending(z => z.Id).ToList();
                }
            }
        }
        #endregion
    }
}
