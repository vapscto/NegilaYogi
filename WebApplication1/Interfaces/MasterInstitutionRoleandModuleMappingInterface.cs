using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MasterInstitutionRoleandModuleMappingInterface
    {
        InstitutionRolePrivilegesDTO SaveInstitution_Module_Page(InstitutionRolePrivilegesDTO enqu);

        InstitutionRolePrivilegesDTO GetDropdownList(InstitutionRolePrivilegesDTO enqu);
        InstitutionRolePrivilegesDTO GetModuleDropdownList(int id);
        
        MasterRolePreviledgeDTO getPagedetailsByModuleId(int id);

        MasterRolePreviledgeDTO getPagedetailsByRoleTypeId(MasterRolePreviledgeDTO data);

        

        InstitutionRolePrivilegesDTO deleterec(InstitutionRolePrivilegesDTO id);
    }
}
