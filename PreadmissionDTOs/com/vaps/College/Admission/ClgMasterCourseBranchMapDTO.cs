using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgMasterCourseBranchMapDTO : CommonParamDTO
    {
        public long AMCOBM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool AMCOBM_ActiveFlg { get; set; }

        public Array MasterCourseList { get; set; }

        public Array masterbranchList { get; set; }

        public bool returnval { get; set; }
        public Array grid { get; set; }

        public string AMCO_CourseName { get; set;}

        public string AMB_BranchName { get; set; }
        public Array mastersemesterlist { get; set; }
        public semester_list[] semester_list { get; set; }
        public string message { get; set; }
        public Array semesterbrancklist { get; set; }

        public long AMCOBMS_Id { get; set; }       
        public long AMSE_Id { get; set; }
        public bool AMCOBMS_ActiveFlg { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array editdata { get; set; }


        public string AMCOBM_Code { get; set; }
        public bool? AMCOBM_CBCSFlg { get; set; }
        public bool? AMCOBM_ElectiveFlg { get; set; }
        public long? AMCOBM_CBCSIntroYear { get; set; }
        public string AMCOBM_FileName { get; set; }
        public string AMCOBM_FilePath { get; set; }
        public long? AMCOBM_ElectiveIntroYear { get; set; }

        public long cbcsyearid { get; set; }
        public long electiveyearid { get; set; }
        public string cbcsyearname { get; set; }
        public string electiveyearname { get; set; }
        public Array cbcsyearlist { get; set; }
        public Array electiveyearlist { get; set; }

    }

    public class semester_list
    {
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }

    }
}
