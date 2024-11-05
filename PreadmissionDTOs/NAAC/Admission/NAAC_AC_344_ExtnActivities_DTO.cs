using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_344_ExtnActivities_DTO
    {
        public long NCACET343_Id { get; set; }
        public long NCACET344STF_Id { get; set; }
        public long NCACET344STFF_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACET343_TypeOfActivity { get; set; }
        public string NCACET343_SchemeName { get; set; }
        public DateTime NCACET343_ActivityDate { get; set; }
        public string NCACET343_OrgAgency { get; set; }
        public string NCACET343_Place { get; set; }
        public string NCACET343_Duration { get; set; }
        public long NCACET343_Year { get; set; }
        public long NCACET343_NoOfStudents { get; set; }
        public bool NCACET343_ActiveFlg { get; set; }
        public long NCACET343_CreatedBy { get; set; }
        public long NCACET343_UpdatedBy { get; set; }
        public DateTime NCACET343_CreatedDate { get; set; }
        public DateTime NCACET343_UpdatedDate { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public Array branchlist { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public Array semisterlist { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public Array sectionlist { get; set; }
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public Array studentlist { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string NCACET343S_Role { get; set; }
        public string AMCO_CourseName { get; set; }
        public int AMCO_Order { get; set; }
        public long UserId { get; set; }
        public selectdStudentlist[] selectdStudentlist { get; set; }
        public long NCACSA343S_Id { get; set; }
        public bool retval { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }
        public long NCACET343S_Id { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long ACMST_Id { get; set; }
        public string AMCST_AdmNo { get; set; }
        public Array mappedstudentlist { get; set; }
        public bool duplicate { get; set; }
        public NAAC_AC_344_ExtnActivities_DTO[] filelist { get; set; }
        public NAAC_AC_344_ExtnActivities_DTO[] filelist_student { get; set; }
        public Array viewdocument_MainActUploadFiles { get; set; }
        public Array viewdocument_StudentActUploadFiles { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public long NCACET343F_Id { get; set; }
        public long NCACET343SF_Id { get; set; }
        public Array editMainSActFileslist { get; set; }
        public Array editStudentActFileslist { get; set; }
        public Array filldepartment { get; set; }

        public long HRMDES_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string empname { get; set; }
        public long NCACET344STFId { get; set; }
        public long HRME_Id { get; set; }
        public string NCACET344STF_Role { get; set; }
        public bool NCACET344STF_ActiveFlg { get; set; }
        public long NCACET343_NoOEmployee { get; set; }
        public selectdStafflist[] selectdStafflist { get; set; }
        public NAAC_AC_344_ExtnActivities_DTO[] filelist_staff { get; set; }
        public Array stafftlist { get; set; }
        public Array filldesignation { get; set; }
        public Array viewdocument_StaffActUploadFiles { get; set; }
        public Array editStaffActFileslist { get; set; }
        public Array empdatarole { get; set; }
        public Array empdeptSelectedId { get; set; }
        public Array empDesSelectedId { get; set; }
        public Array staffmodaldata { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string NCACET343F_StatusFlg { get; set; }
        public bool NCACET343F_ActiveFlg { get; set; }
        public string NCACET343SF_StatusFlg { get; set; }
        public bool NCACET343SF_ActiveFlg { get; set; }
        public Array commentlist { get; set; }

        public long NCACET344C_Id { get; set; }
        public string NCACET344C_Remarks { get; set; }
        public long? NCACET344C_RemarksBy { get; set; }
        public string NCACET344C_StatusFlg { get; set; }
        public bool? NCACET344C_ActiveFlag { get; set; }
        public long? NCACET344C_CreatedBy { get; set; }
        public DateTime? NCACET344C_CreatedDate { get; set; }
        public long? NCACET344C_UpdatedBy { get; set; }
        public DateTime? NCACET344C_UpdatedDate { get; set; }

        public long NCACET343FC_Id { get; set; }
        public string NCACET343FC_Remarks { get; set; }
        public long NCACET343FC_RemarksBy { get; set; }
        public bool NCACET343FC_ActiveFlag { get; set; }
        public long NCACET343FC_CreatedBy { get; set; }
        public DateTime NCACET343FC_CreatedDate { get; set; }
        public long NCACET343FC_UpdatedBy { get; set; }
        public DateTime NCACET343FC_UpdatedDate { get; set; }
        public string NCACET343FC_StatusFlg { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCACET343_StatusFlg { get; set; }
        public string NCACET344STF_StatusFlg { get; set; }
        public string NCACET344STFF_StatusFlg { get; set; }
        public string NCACET344STFF_ActiveFlg { get; set; }

    }   
    public class selectdStudentlist
    {
        public long AMCST_Id { get; set; }
    }
   
}
