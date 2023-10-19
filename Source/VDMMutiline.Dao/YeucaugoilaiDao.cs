using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;
using VDMMutiline.SharedComponent.EntityInfo;
using VDMMutiline.SharedComponent.Params;

namespace VDMMutiline.Dao
{
    public class YeucaugoilaiDao : BaseDao
    {
        #region Action
        public void Insert(tbl_yeucaugoilai item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                dbContext.tbl_yeucaugoilais.InsertOnSubmit(item);
                dbContext.SubmitChanges();
            }
        }

        public void Update(tbl_yeucaugoilai item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_yeucaugoilais.FirstOrDefault(sitem => sitem.Id == item.Id);
                if (dbItem != null)
                {
                    dbItem.Dagoilai = item.Dagoilai;
                    dbItem.NgayGoilai = item.NgayGoilai;
                    dbItem.Nguoigoilai = item.Nguoigoilai;
                    dbContext.SubmitChanges();
                }
            }
        }

        public bool Delete(int item)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var dbItem = dbContext.tbl_yeucaugoilais.FirstOrDefault(en => en.Id == item);

                if (dbItem != null)
                {
                    dbContext.tbl_yeucaugoilais.DeleteOnSubmit(dbItem);
                    dbContext.SubmitChanges();
                }
            }
            return true;
        }


        #endregion
        #region Query
        public tbl_yeucaugoilai GetbyId(int id)
        {
            using (var dbContext = new ContextDataContext(ConnectionString))
            {
                var query = from n in dbContext.tbl_yeucaugoilais
                            where n.Id == id
                            select n;
                return query.FirstOrDefault();
            }
        }
        public void Search(YeucaugoilaiParam param)
        {
            if (param.YeucaugoilaiFilter != null)
            {
                using (var dbContext = new ContextDataContext(ConnectionString))
                {
                    var query = from item in dbContext.tbl_yeucaugoilais
                                select new YeucaugoilaiInfo()
                                {
                                    Id = item.Id,
                                    Ngaytao = item.Ngaytao,
                                    Hoten = item.Hoten,
                                    SDT = item.SDT,
                                    Thoigiangoi = item.Thoigiangoi,
                                    Goilaingay = item.Goilaingay,
                                    Noidung = item.Noidung,
                                    Dagoilai = item.Dagoilai,
                                    NgayGoilai = item.NgayGoilai,
                                    Nguoigoilai = item.Nguoigoilai,
                                    NguoiDaxem = item.NguoiDaxem
                                };

                    if (param.PagingInfo != null)
                    {
                        var infopage = param.PagingInfo;
                        param.PagingInfo.RecordCount = query.Count();
                        param.YeucaugoilaiInfos =
                            query.OrderByDescending(z => z.Ngaytao).Skip(infopage.RowsSkip).Take(infopage.PageSize).ToList();
                    }
                    else
                    {
                        param.YeucaugoilaiInfos = query.OrderByDescending(z => z.Ngaytao).ToList();

                    }

                }
            }
        }

        #endregion
    }
}
