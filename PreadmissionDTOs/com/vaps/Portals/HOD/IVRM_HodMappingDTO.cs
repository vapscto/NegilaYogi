using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.HOD
{
   public class IVRM_HodMappingDTO:CommonParamDTO
    {
        public long IHOD_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IHOD_ActiveFlag { get; set; }

        public long IHODC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool IHODC_ActiveFlag { get; set; }

        public long IHODS_Id { get; set; }
        public bool IHODS_ActiveFlag { get; set; }
        
        public Array hodlist { get; set; }
        public Array stafflist { get; set; }
        public Array clsslist { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array sectionlist { get; set; }
        public Array hrmdlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }


        public long[] secidlist { get; set; }
        public IVRM_HodMappingDTO[] classlst { get; set; }
        public Array modalstaflist { get; set; }
        public Array edithodlist { get; set; }
        public Array stafflist2 { get; set; }
        public long staff_id { get; set; }
        public long hod_id { get; set; }
        public Array gethodstafdata { get; set; }
        public Array modalclaslist { get; set; }
        public Array edithodstaflist { get; set; }

    }
 }
