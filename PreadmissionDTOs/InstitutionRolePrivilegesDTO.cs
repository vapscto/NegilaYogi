using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class InstitutionRolePrivilegesDTO : CommonParamDTO
    {
        public long IVRMIRP_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long IVRMIMP_Id { get; set; }
        public bool IVRMIRP_AddFlag { get; set; }
        public bool IVRMIRP_UpdateFlag { get; set; }
        public bool IVRMIRP_DeleteFlag { get; set; }
        public bool IVRMIRP_ReportFlag { get; set; }
        public bool IVRMIRP_SearchFlag { get; set; }
        public bool IVRMIRP_ProcessFlag { get; set; }

        //extra fields
       
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMP_PageName { get; set; }
        public string IVRMRT_Role { get; set; }


        public long IVRMIM_Id { get; set; }


        public Array InstitutionDropDown { get; set; }
        public Array RoleDropDown { get; set; }
        public Array ModuleDropDown { get; set; }
        public Array InstitutionRolePrivilegesList { get; set; }
        public Array PageDropDown { get; set; }

        //// Exra

        public long ivrmmP_Id { get; set; }
        public bool ivrmrP_AddFlag { get; set; }
        public bool ivrmrP_UpdateFlag { get; set; }
        public bool ivrmrP_DeleteFlag { get; set; }
        public bool ivrmrP_ReportFlag { get; set; }
        public bool ivrmrP_ProcessFlag { get; set; }
        public bool ivrmrP_SearchFlag { get; set; }

        public long roleId { get; set; }


        public string returnval { get; set; }

        public InstitutionRolePrivilegesDTO[] savetmpdata { get; set; }
        public InstitutionRolePrivilegesDTO[] privilagedata { get; set; }

        public InstitutionRolePrivilegesDTO[] menuid  { get; set; }

        //  public long IVRMMP_Id { get; set; }
    }
}
