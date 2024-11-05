using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class MastersModule_DTO
    {
        public long ISMMMD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public long ISMMMDDE_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }
        public bool ISMMMD_ActiveFlag { get; set; }
        public long ISMMMD_CreatedBy { get; set; }
        public long ISMMMD_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string empname { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }
        public Array deptlist { get; set; }
        public Array projectlist { get; set; }
        public Array modulelist { get; set; }
        public Array emplist { get; set; }
        public Array emplistHead { get; set; }
        public string headEmpname { get; set; }
        public long headId { get; set; }
        public bool ISMMMDDE_ActiveFlag { get; set; }
        public long IVRMMMDDE_ModuleIncharge { get; set; }
        public bool IVRMMMDDE_ModuleHeadFlg { get; set; }

        public string HRMD_DepartmentName { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public MastersModule_DTO[] developerlist { get; set; }

        public MastersModule_DTO[] developerheadlist { get; set; }
        public Array editlist { get; set; }
        public Array developerlistd { get; set; }
        public Array masterModulesname { get; set; }

        public string modulehead1 { get; set; }
        public string modulehead2 { get; set; }
        public string modulehead3 { get; set; }
        public string developerName1 { get; set; }
        public string developerName2 { get; set; }
        public string developerName3 { get; set; }
    }
}
