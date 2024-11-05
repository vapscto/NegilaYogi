using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_Programs_112_DTO
    {
        public long NCACMPR112_Id { get; set; }
        public long NCACPR112F_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string NCACMPR112_DiplomaCertName { get; set; }
        public long NCACMPR112_IntroYear { get; set; }
        public bool NCACMPR112_DiscontinuedFlg { get; set; }
        public long NCACPMR112_DiscontinuedYear { get; set; }
        public string NCACMPR112_DiscontinuedReason { get; set; }
        public bool NCACMPR112_ActiveFlg { get; set; }
        public long NCACMPR112_CreatedBy { get; set; }
        public long NCACMPR112_UpdatedBy { get; set; }
        public DateTime NCACMPR112_CreatedDate { get; set; }
        public DateTime NCACMPR112_UpdatedDate { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public long NCACPR112_Id { get; set; }
        public bool NCACPR112_ActiveFlg { get; set; }
        public long NCACPR112_CreatedBy { get; set; }
        public long NCACPR112_UpdatedBy { get; set; }
        public DateTime NCACPR112_CreatedDate { get; set; }
        public DateTime NCACPR112_UpdatedDate { get; set; }
        public long NCACPR112_Year { get; set; }
        public DateTime NCACPR112_Date { get; set; }
        public long ASMAY_Id { get; set; }
        public long discontinuedid { get; set; }
        public string ASMAY_Year { get; set; }
        public string discontinuedname { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMB_BranchCode { get; set; }
        public string NCACPR112_DeptName { get; set; }
        public long NCACPR112_RevisionYear { get; set; }
        public long NCACPR112_RevcarriedSyllabusYerars { get; set; }
        public long UserId { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array editlist { get; set; }
        public Array MasterCourseList { get; set; }
        public Array masterbranchList { get; set; }
        public Array yearlist { get; set; }
        public Array discontinuedyearlist { get; set; }
        public Array alldata { get; set; }
        public Array reportlist { get; set; }
        public Array mappedProgram { get; set; }
        public Array programlist { get; set; }
        public Array mappedlistdata { get; set; }
        public Array edittab2data { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public bool? cfileapproved { get; set; }
        public string cfilestatus { get; set; }
        public bool? cfileactive { get; set; }
        public long cfileid { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public NAAC_AC_Programs_112_DTO[] filelist { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array institutionlist { get; set; }
        public Array coursebrnchlist { get; set; }
        public int countInst { get; set; }
        public string InstitutionTypeFlg { get; set; }
        public string MI_SchoolCollegeFlag { get; set; }
        public bool? NCACPR112_ApprovedFlg { get; set; }
        public string NCACPR112_StatusFlg { get; set; }
        public bool? NCACPR112F_ApprovedFlg { get; set; }
        public string NCACPR112F_StatusFlg { get; set; }


        public long NCACPR112C_Id { get; set; }
        public string NCACPR112C_Remarks { get; set; }
        public long NCACPR112C_RemarksBy { get; set; }
        public string NCACPR112C_StatusFlg { get; set; }
        public bool NCACPR112C_ActiveFlag { get; set; }
        public long? NCACPR112C_CreatedBy { get; set; }
        public DateTime? NCACPR112C_CreatedDate { get; set; }
        public DateTime? NCACPR112C_UpdatedDate { get; set; }
        public long? NCACPR112C_UpdatedBy { get; set; }


        public long NCACPR112FC_Id { get; set; }
        public string NCACPR112FC_Remarks { get; set; }
        public long NCACPR112FC_RemarksBy { get; set; }
        public bool NCACPR112FC_ActiveFlag { get; set; }
        public long? NCACPR112FC_CreatedBy { get; set; }
        public DateTime? NCACPR112FC_CreatedDate { get; set; }
        public long? NCACPR112FC_UpdatedBy { get; set; }
        public DateTime? NCACPR112FC_UpdatedDate { get; set; }
        public string NCACPR112FC_StatusFlg { get; set; }


        public string Remarks { get; set; }
        public string UserName { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        //added by sanjeeev
       public excelfile[] advimppln { get; set; }
        public string message { get; set; }
        public Array modal { get; set; }
        public Array modalduplicate { get; set; }
    }
    //added by sanjeeev
    public class excelfile
    {
        public string CertificateName { get; set; }
        public string YearOfIntroduction { get; set; }
        public string ProgramName { get; set; }
        public string BranchName { get; set; }
    }
}
