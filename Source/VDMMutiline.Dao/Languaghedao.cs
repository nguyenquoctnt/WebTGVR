using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDMMutiline.SharedComponent.Entities;

namespace VDMMutiline.Dao
{
   public class Languaghedao:BaseDao
    {
       public List<tbl_Language> GetList()
       {
           using (var dbContext = new ContextDataContext(ConnectionString))
           {
               return dbContext.tbl_Languages.ToList();
           }
       }
    }
}
