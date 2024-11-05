using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeServiceRecordReportDTO :MasterEmployeeDTO
    {
     // public long MI_Id { get; set; }
    //    public string retrunMsg { get; set; }
     //  public long roleId { get; set; }
        //Type
    //   public string Type { get; set; }

        public string FormatType { get; set; }

        public string DOBJL { get; set; }

        public string AllOrIndividual { get; set; }

      
            // public long? HRMET_Id { get; set; }
            // public long? HRME_Id { get; set; }



        public string TypeOrEmployee { get; set; }

        public bool? Working { get; set; }
        public bool? Left { get; set; }


        public Array employeedropdown { get; set; }

        public Array employeeTypedropdown { get; set; }


        public Array employeeDetails { get; set; }

        public string AWL { get; set; }
        public string FIAL { get; set; }

        public string HRMGT_EmployeeGroupType { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string IMC_CasteName { get; set; }
        public string IVRMMR_Name { get; set; }

        public InstitutionDTO institutionDetails { get; set; }

        public string EmployeeContactNo { get; set; }
        public string EmployeeEmailId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }


    }
}
