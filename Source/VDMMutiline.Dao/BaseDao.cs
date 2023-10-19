namespace VDMMutiline.Dao
{
    public class BaseDao
    {
        public static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["Vdm_MutilineConnectionString"].ConnectionString ?? "";
    }
}
