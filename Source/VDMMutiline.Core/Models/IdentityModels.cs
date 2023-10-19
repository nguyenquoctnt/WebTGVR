using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VDMMutiline.Core.Models
{
    public class ActRole : IdentityUserRole<int>
    {
        
    }

    public class ActClaim : IdentityUserClaim<int>
    {
    }

    public class ActLogin : IdentityUserLogin<int>
    {
    }

    public class Role : IdentityRole<int, ActRole>
    {
        public string RoleCategory { get; set; }
        public Role() { }
        public Role(string name) { Name = name; }
    }

    public class UserStore : UserStore<VDMUser, Role, int,
        ActLogin, ActRole, ActClaim>
    {
        public UserStore(VDMDbContext dbContext) : base(dbContext)
        {

        }
    }

    public class RoleStore : RoleStore<Role, int, ActRole>
    {
        public RoleStore(VDMDbContext dbContext) : base(dbContext)
        {
        }
    }

    // You can add profile data for the user by adding more properties to your LwUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class VDMUser : IdentityUser<int, ActLogin, ActRole, ActClaim>
    {

        public DateTime? Birthday { get; set; }
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public UserStatus Status { get; set; }
        public string RegisterIP { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Google { get; set; }
        public bool? LaKhachHang { get; set; }
        public string TenCongTy { get; set; }
        public int? IdQuanhuyen { get; set; }
        public int? IdTinh { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public string Passport { get; set; }
        public string PassportPic1 { get; set; }
        public string PassportPic2 { get; set; }
        public string UrlDomain1 { get; set; }
        public string UrlDomain2 { get; set; }
        public string UrlDomain3 { get; set; }
        public string Gmap { get; set; }
        public bool? Isdelete { get; set; }
        public int? ParentId { get; set; }
        public int? SiteId { get; set; }
        public string LogoUrl { get; set; }
        public string LogoMbUrl { get; set; }
        public string Baner { get; set; }
        public int? StyleID { get; set; }
        public bool? IsSSLDomain2 { get; set; }
        public bool? IsSSLDomain3 { get; set; }
        public bool? IsSSLDomain2MB { get; set; }
        public bool? IsSSLDomain3MB { get; set; }
        public bool? IsIssueTicket { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDes { get; set; }
        public string SeoKey { get; set; }
       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(VDMUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }

    public class VDMDbContext : IdentityDbContext<VDMUser, Role, int,
        ActLogin, ActRole, ActClaim>
    {
        private VDMDbContext()
            : base("VDMContext")
        {
        }

        public static VDMDbContext Create()
        {
            return new VDMDbContext();
        }
    }
}



