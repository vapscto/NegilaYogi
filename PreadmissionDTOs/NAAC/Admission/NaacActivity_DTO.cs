using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NaacActivity_DTO
    {
        public long NCACSA343_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public int AMB_Order { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRME_Id { get; set; }
        public string empname { get; set; }
        public Array institutionlist { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long UserId { get; set; }
        public int? HRME_EmployeeOrder { get; set; }

        public long NCACSA343_NoOfTeachers { get; set; }
        public string NCACSA343_TypeOfActivity { get; set; }
        public DateTime? NCACSA343_ActivityDate { get; set; }
        public string NCACSA343_OrgAgency { get; set; }
        public string NCACSA343_Place { get; set; }
        public string NCACSA343_Duration { get; set; }
        public long NCACSA343_Year { get; set; }
        public long NCACSA343_NoOfStudents { get; set; }
        public string NCACSA343_FileName { get; set; }
        public string NCACSA343_FilePath { get; set; }
        public bool NCACSA343_ActiveFlg { get; set; }
        public long NCACSA343_CreatedBy { get; set; }
        public long NCACSA343_UpdatedBy { get; set; }
        public DateTime NCACSA343_CreatedDate { get; set; }



        public long NCACSA343S_Id { get; set; }
        public string NCACSA343S_Role { get; set; }
        public string NCACSA343S_FileName { get; set; }
        public string NCACSA343S_FilePath { get; set; }
        public bool NCACSA343S_ActiveFlg { get; set; }
        public long NCACSA343S_CreatedBy { get; set; }
        public long NCACSA343S_UpdatedBy { get; set; }
        public DateTime NCACSA343S_CreatedDate { get; set; }
        public DateTime NCACSA343S_UpdatedDate { get; set; }


        public long NCACSA343E_Id { get; set; }
        public string NCACSA343E_Role { get; set; }
        public string NCACSA343E_FileName { get; set; }
        public string NCACSA343E_FilePath { get; set; }
        public bool NCACSA343E_ActiveFlg { get; set; }
        public long NCACSA343E_CreatedBy { get; set; }
        public long NCACSA343E_UpdatedBy { get; set; }
        public DateTime NCACSA343E_CreatedDate { get; set; }
        public DateTime NCACSA343E_UpdatedDate { get; set; }




        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array stafftlist { get; set; }
        public Array studentRoledata { get; set; }
        public Array empdatarole { get; set; }
        public Array empdeptSelectedId { get; set; }
        public Array empDesSelectedId { get; set; }

        public bool returnval { get; set; }

        public NaacActivity_DTO[] selectdStudentlist { get; set; }
        public NaacActivity_DTO[] selectdStafflist { get; set; }
        public Array alldata1 { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public string msg { get; set; }

        public Array studentmodaldata { get; set; }
        public Array staffmodaldata { get; set; }
        public Array editcourse { get; set; }
        public Array editbranch { get; set; }
        public Array editsemeste { get; set; }
        public Array editsection { get; set; }
        public Array editstudetdata { get; set; }

        public long NCACSA343F_Id { get; set; }
        public long NCACSA343SF_Id { get; set; }
        public long NCACSA343EF_Id { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }

        public Naac_CommonFiles_DTO[] filelist { get; set; }
        public filelist_student[] filelist_student { get; set; }
        public filelist_staff[] filelist_staff { get; set; }

        public Array editMainSActFileslist { get; set; }
        public Array editStudentActFileslist { get; set; }
        public Array editStaffActFileslist { get; set; }
        public Array viewdocument_MainActUploadFiles { get; set; }
        public Array viewdocument_StudentActUploadFiles { get; set; }
        public Array viewdocument_StaffActUploadFiles { get; set; }

    }
    //public class filelist_student
    //{
    //    public string cfilename { get; set; }
    //    public string cfilepath { get; set; }
    //    public string cfiledesc { get; set; }
    //}
    public class filelist_staff
    {
       
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
    }

}
