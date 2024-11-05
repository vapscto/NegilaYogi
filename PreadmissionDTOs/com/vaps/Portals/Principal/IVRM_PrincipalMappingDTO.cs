using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
   public class IVRM_PrincipalMappingDTO:CommonParamDTO
    {
        public long IPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool IPR_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }


        public long IPRC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool IRPC_ActiveFlag { get; set; }


        public long IPRS_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IRPS_ActiveFlag { get; set; }


        public Array stafflist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array clsslist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array alldata { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public Array principlelist { get; set; }
        public Array stafflist2 { get; set; }
        public Array getprincplstafdata { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editprincclaslist { get; set; }
        public Array modalclaslist { get; set; }
        public Array editprincstafflist { get; set; }
        public IVRM_PrincipalMappingDTO[] classlst { get; set; }


    }
}
