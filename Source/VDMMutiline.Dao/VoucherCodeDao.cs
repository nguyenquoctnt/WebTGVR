using System;
using System.Collections.Generic;
using System.Linq;
using VDMMutiline.SharedComponent.Constants;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class VoucherCodeDao : BaseDao
    {
        #region Action
        public string Insert(tbl_VoucherCode item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                item.CreateDate = DateTime.Now;
                item.Status = false;
                dbContext.tbl_VoucherCodes.InsertOnSubmit(item);
                var datenow = DateTime.Now;
                var code = Guid.NewGuid().ToString().GetHashCode().ToString("X").ToUpper();
                dbContext.SubmitChanges();
                var dbItem = dbContext.tbl_VoucherCodes.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Code = code;
                }
                dbContext.SubmitChanges();
                return code;
            }
        }
        public void UpdateStatus(int id, bool status, string bookingcode)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_VoucherCodes.FirstOrDefault(sitem => sitem.Id == id);
                if (dbItem != null)
                {
                    dbItem.Status = true;
                    dbItem.DateStatus = DateTime.Now;
                    dbItem.BookCode = bookingcode;
                    dbContext.SubmitChanges();
                }
            }
        }
        public bool Delete(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_VoucherCodes.FirstOrDefault(sitem => sitem.Id == id);
                if (dbItem != null)
                {
                    dbContext.tbl_VoucherCodes.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }
        #endregion
        #region Query
        public tbl_VoucherCode GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_VoucherCodes
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public tbl_VoucherCode Checkbycode(string code, List<string> ListuserinSite)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_VoucherCodes
                            where n.Code.Trim().ToUpper() == code.Trim().ToUpper()
                            && ListuserinSite.Contains(n.UserCreate)
                            && n.FormDate.Value.Date <= DateTime.Now.Date && n.ToDate.Value.Date >= DateTime.Now.Date
                            && n.Status == false
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(VoucherCodeParam param)
        {
            var fiter = param.VoucherCodeFilter;
            if (fiter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from n in dbContext.tbl_VoucherCodes
                                where (string.IsNullOrEmpty(fiter.Search) || (n.Email.Contains(fiter.Search)))
                                && (fiter.FormDate.HasValue == false || n.CreateDate.Value.Date >= fiter.FormDate)
                                && (fiter.ToDate.HasValue == false || n.CreateDate.Value.Date <= fiter.ToDate)
                                && (fiter.Status.HasValue == false || n.Status == fiter.Status)
                                && fiter.ListuserinSite.Contains(n.UserCreate)
                                select new VoucherCodeInfo()
                                {
                                    Id = n.Id,
                                    Code = n.Code,
                                    CreateDate = n.CreateDate,
                                    Status = n.Status,
                                    FormDate = n.FormDate,
                                    ToDate = n.ToDate,
                                    DateStatus = n.DateStatus,
                                    UserCreate = n.UserCreate,
                                    Email = n.Email,
                                    Valued = n.Valued,
                                    BookCode = n.BookCode,
                                    Note = n.Note
                                };

                    if (param.PagingInfo != null)
                    {
                        param.PagingInfo.RecordCount = query.Count();
                        param.VoucherCodeInfos = query.OrderByDescending(z => z.CreateDate).Skip(param.PagingInfo.RowStart).Take(param.PagingInfo.PageSize).ToList();
                    }
                    else
                    {
                        param.VoucherCodeInfos = query.OrderByDescending(z => z.CreateDate).ToList();
                    }
                }
            }
        }
        #endregion
    }
}
