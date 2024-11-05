using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.IVRM
{
    public class ClgNoticeBoardDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long IVRMRT_Id { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMCO_Id { get; set; }      
        public long AMCST_Id { get; set; }      
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }                  
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public int AMCO_Order { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public long INTBCB_Id { get; set; }    
        public bool INTBCB_ActiveFlag { get; set; }

        public long INTB_Id { get; set; }  
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public bool INTB_DispalyDisableFlg { get; set; }
        public long INTBCSTF_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public bool INTBCSTF_ActiveFlag { get; set; }

        public Array staffnoticedetails { get; set; }
        public Array course_list { get; set; }
        public Array branch_list { get; set; }
        public Array sem_list { get; set; }
        public Array noticelist { get; set; }
        public Array noticedetails { get; set; }    
        public Array editdetails { get; set; }
        public Array coursebranchsem { get; set; }

        
        public CourseArrayDTO[] courseArray { get; set; }
        public BranchArrayDTO[] branchArray { get; set; }
        public SemesterArrayDTO[] semesterArray { get; set; }

        public coursearray1[] coursearray { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

        public string flag { get; set; }
        public string type { get; set; }

        public Array reportlist { get; set; }
        public Array view_array { get; set; }

        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public Array deptList { get; set; }
        public Array designation { get; set; }

        public long HRMD_Id { get; set; }
        public string HRMD_Departmentname { get; set; }
        public Array get_userEmplist { get; set; }
        public bool INTB_ToStaffFlg { get; set; }
        public bool INTB_ToStudentFlg { get; set; }
       
        public employeearraylistDTO[] employeearraylist { get; set; }
        public FilePath_Array1[] FilePath_Array { get; set; }

        public Array deviceArray { get; set; }

        public long FMG_Id { get; set; }    
        public string FMG_Name { get; set; }

        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }

        public Array fee_group { get; set; }
        public Array fee_heads { get; set; }
        public bool fee_def { get; set; }
        public defgrparaay1[] defarray { get; set; }
        public defheadaraay1[] defheadarray { get; set; }
        public studentarray1[] studentarray { get; set; }
        public Array studentlist { get; set; }
        public Array fillyear { get; set; }
        public Array departmentList { get; set; }
    }

    //added by roopA
    public class studentarray1
    {
        public long AMCST_Id { get; set; }

    }
    public class defheadaraay1
    {
        public long FMH_Id { get; set; }
    }
    public class defgrparaay1
    {
        public long FMG_Id { get; set; }
    }
    public class DepartmentDTO
    {
        public int HRMDC_ID { get; set; }
    }
    public class DesignationDTO
    {
        public long HRMDES_Id { get; set; }
    }

    public class employeearraylistDTO
    {
        public long HRME_Id { get; set; }
    }
    //
    public class coursearray1
    {
        public long AMCO_Id { get; set; }
    
    }
    public class CourseArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
    }
    public class BranchArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class SemesterArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }
    public class FilePath_Array1
    {
        public string INTBFL_FilePath { get; set; }
        public string FileName { get; set; }
    }
}
