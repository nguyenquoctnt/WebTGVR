using VDMMutiline.SharedComponent.Entities;
using System.Collections.Generic;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class AspNetGroupsInfo : AspNetGroup
    {
        public List<AspNetRoleGroup> RoleAssigned { get; set; }
    }
}