using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeMultiHoursAttendanceEntryDTO
    {
        public long ACSA_Id { get; set; }
        public long ACALU_Id { get; set; }
        public long AMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public string AMB_BranchType { get; set; }
        public int AMB_StudentCapacity { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMB_AidedUnAided { get; set; }
        public long AMSE_Id { get; set; }
        public long ACQC_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public long ACQ_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_Year { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string ACQC_CategoryName { get; set; }

        public decimal ACSCD_SeatPer { get; set; }
        public long ACSCD_SeatNos { get; set; }
        public string ACSCD_Remarks { get; set; }
        public bool ACSCD_ActiveFlg { get; set; }
        public long ACSCD_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public long ACAB_Id { get; set; }
        public string ACAB_BatchName { get; set; }
        public int ACAB_StudentStrength { get; set; }

        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long ACYST_RollNo { get; set; }
        public bool returnval { get; set; }

        //---------------------------
        public string username { get; set; }
        public long userId { get; set; }
        public string flag { get; set; }
        public long roleId { get; set; }
        public string rolename { get; set; }
        public string message { get; set; }
        public long Emp_Code { get; set; }
        //--------------------------------
        public Array getYear { get; set; }
        public Array getCourse { get; set; }
        public Array getBranch { get; set; }
        public Array getBranchDetails { get; set; }
        public Array getSection { get; set; }
        public Array getSemester { get; set; }
        public Array getSubject { get; set; }
        public Array getBatch { get; set; }
        public Array getPeriod { get; set; }
        public Array getStudentdetails { get; set; }
        public Array getSeatCategory { get; set; }
        public Array getQuota { get; set; }
        public Array subjectlist { get; set; }
        public Array getSeatsdetails { get; set; }
        public Array getSeattotal { get; set; }
        public long[] AMSE_Sem { get; set; }
        public Array getSeatsdetails1 { get; set; }
        public CollegeMultiAttendanceEntryTempDTO[] ClgAttendanceEntryTempDTO { get; set; }
        public string period { get; set; }
        public long TTMP_Id { get; set; }
        public DateTime? ACSA_AttendanceDate { get; set; }
        public string ACSA_Regular_Extra { get; set; }
        public string networkip { get; set; }
        public string check_attendance_entrytype { get; set; }
        public CollegeMultiAttendanceEntryTTPeriodTempDTO[] ClgAttendanceEntryTTPeriodTempDTO { get; set; }
        public selectedbranchlist[] selectedbranchlist { get; set; }
        public Array getStudentfinaldetails { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string employeename { get; set; }
        public string TotalPresent { get; set; }
        public string totalabsent { get; set; }
        public string totalcount { get; set; }
        public Array getpreviewdata { get; set; }
        public bool Multibranch { get; set; }
        public long flagdelete { get; set; }

    }

    public class CollegeMultiAttendanceEntryTempDTO
    {
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public bool? Selected { get; set; }
        public long ACSA_Id { get; set; }
        public long? ACSAS_Id { get; set; }
        public decimal? pdays { get; set; }
        public long TTMP_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public CollegeMultiAttendanceEntryTTPeriodTempDTO1[] sub_list { get; set; }

    }

    public class CollegeMultiAttendanceEntryTTPeriodTempDTO
    {
        public long TTMP_Id { get; set; }
        public string TTMP_PeriodName { get; set; }
    }

    public class CollegeMultiAttendanceEntryTTPeriodTempDTO1
    {
        public bool? Selected { get; set; }
        public long ACSA_Id { get; set; }
        public long? ACSAS_Id { get; set; }
        public decimal? pdays { get; set; }
        public long TTMP_Id { get; set; }
    }

    public class CollegeAutoScheduler
    {
        public long AMCST_MobileNo { get; set; }
        public string subjects { get; set; }
        public string Course { get; set; }
        public string AMCST_FirstName { get; set; }
        public DateTime ACSA_AttendanceDate { get; set; }
        public long AMCST_Id { get; set; }
    }

    public class selectedbranchlist
    {
        public long AMB_Id { get; set; }
    }
}
