using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterRolePreviledgeDTO : CommonParamDTO
    {
        public long IVRMRP_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long IVRMMP_Id { get; set; }
        public bool IVRMRP_AddFlag { get; set; }
        public bool IVRMRP_UpdateFlag { get; set; }
        public bool IVRMRP_DeleteFlag { get; set; }
        public bool IVRMRP_ProcessFlag { get; set; }

        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }

        public bool IVRMRP_ReportFlag { get; set; }

        public Array fillroletype { get; set; }
        public Array fillmodulepagesdata { get; set; }
        public Array firstgrigdata { get; set; }
        public Array allsaveddata { get; set; }

        public Array fillinstitution { get; set; }
        public Array thirdgriddata { get; set; }

        public long IVRMMAP_Id { get; set; }

        public long IVRMUMALP_Id { get; set; }
        public bool? IVRMUMALP_AddFlg { get; set; }
        public bool? IVRMUMALP_UpdateFlg { get; set; }
        public bool? IVRMUMALP_DeleteFlg { get; set; }
        public Array previosgriddata { get; set; }

        public MasterRolePreviledgeDTO[] savetmpdata { get; set; }

        public MasterRolePreviledgeDTO[] notsavetmpdata { get; set; }

        public MasterRolePreviledgeDTO[] previoussavetmpdata { get; set; }

        //public MasterPageDTO[] getmanypages { get; set; }
        public string returnval { get; set; }

        public bool returnvalDelete  { get; set; }

        public Array previousgrid { get; set; }

        public MasterPageModuleMappingDTO masterPageMappingDTO { get; set; }
        public MasterRoleTypeDTO masterRoleTypeDTO { get; set; }

        public string ivrmmP_PageName { get; set; }

        public string ivrmM_ModuleName { get; set; }

        public string Institutename { get; set; }

        public string ivrmrT_Role { get; set; }

        public long pagemoduleidd { get; set; }


        public string IVRMMAP_AppPageName  { get; set; }

        public long IVRMRMAP_Id { get; set; }


        public long IVRMR_Id { get; set; }

        public MasterPageDTO[] pagename { get; set; }
        public Array enq { get; set; }

        public Array privilegesList { get; set; }
        public Array ModulePagesList { get; set; }
        public Array RoleList { get; set; }

        public string prename { get; set; }
        public string searchname { get; set; }

        public long ivrmP_Id { get; set; }

        public long IVRMM_Id { get; set; }

        public long MI_Id { get; set; }

        public string stafname { get; set; }

        public InstitutionRolePrivilegesDTO InstitutionRolePrivilegesDTO { get; set; }


    }
}
