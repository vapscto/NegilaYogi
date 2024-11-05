using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_VAC_DTO
    {
        public long NCACVAC132_Id { get; set; }
        public long NCACVAC132F_Id { get; set; }
        public long NCACVAC132DF_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACVAC132_CourseName { get; set; }
        public string NCACVAC132_CourseCode { get; set; }
        public long NCACVAC132_IntroYear { get; set; }
        public bool NCACVAC132_DiscontinuedFlg { get; set; }
        public long NCACVAC132_DiscontinuedYear { get; set; }
        public bool NCACVAC132_ActiveFlg { get; set; }
        public long NCACVAC132_CreatedBy { get; set; }

        //comment 
        public long NCACVAC132C_Id { get; set; }
        public string NCACVAC132C_Remarks { get; set; }
        public long? NCACVAC132C_RemarksBy { get; set; }
        public string NCACVAC132C_StatusFlg { get; set; }
        public bool? NCACVAC132C_ActiveFlag { get; set; }
        public long? NCACVAC132C_CreatedBy { get; set; }
        public DateTime? NCACVAC132C_CreatedDate { get; set; }
        public long? NCACVAC132C_UpdatedBy { get; set; }
        public DateTime? NCACVAC132C_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCACVAC132F_StatusFlg { get; set; }
        public bool NCACVAC132F_ActiveFlg { get; set; }
        public string message { get; set; }
        public long NCACVAC132FC_Id { get; set; }
        public string NCACVAC132_StatusFlg { get; set; }
        public string NCACVAC132FC_Remarks { get; set; }
        public long? NCACVAC132FC_RemarksBy { get; set; }
        public bool? NCACVAC132FC_ActiveFlag { get; set; }
        public long? NCACVAC132FC_CreatedBy { get; set; }
        public DateTime? NCACVAC132FC_CreatedDate { get; set; }
        public long? NCACVAC132FC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132FC_UpdatedDate { get; set; }
        public string NCACVAC132FC_StatusFlg { get; set; }


        public long NCACVAC132_UpdatedBy { get; set; }
        public DateTime? NCACVAC132_CreatedDate { get; set; }
        public DateTime? NCACVAC132_UpdatedDate { get; set; }
        public string discontinuedyear { get; set; }
        public long NCACVAC132D_Id { get; set; }
        public DateTime? NCACVAC132D_Date { get; set; }
        public long NCACVAC132D_Year { get; set; }
        public long NCACVAC132D_NoOfStudentsEnr { get; set; }
        public long NCACVAC132D_NoOfStdCompleted { get; set; }
        public bool NCACVAC132D_ActiveFlg { get; set; }
        public long NCACVAC132D_CreatedBy { get; set; }
        public long NCACVAC132D_UpdatedBy { get; set; }
        public DateTime? NCACVAC132D_CreatedDate { get; set; }
        public DateTime? NCACVAC132D_UpdatedDate { get; set; }


        // comment for detail mapping
        public long NCACVAC132DC_Id { get; set; }
        public string NCACVAC132DC_Remarks { get; set; }
        public long? NCACVAC132DC_RemarksBy { get; set; }
        public string NCACVAC132DC_StatusFlg { get; set; }
        public bool? NCACVAC132DC_ActiveFlag { get; set; }
        public long? NCACVAC132DC_CreatedBy { get; set; }
        public DateTime? NCACVAC132DC_CreatedDate { get; set; }
        public long? NCACVAC132DC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DC_UpdatedDate { get; set; }
        public long NCACVAC132DFC_Id { get; set; }
        public string NCACVAC132DFC_Remarks { get; set; }
        public long? NCACVAC132DFC_RemarksBy { get; set; }
        public bool? NCACVAC132DFC_ActiveFlag { get; set; }
        public long? NCACVAC132DFC_CreatedBy { get; set; }
        public DateTime? NCACVAC132DFC_CreatedDate { get; set; }
        public long? NCACVAC132DFC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DFC_UpdatedDate { get; set; }
        public string NCACVAC132DFC_StatusFlg { get; set; }
        public string NCACVAC132DF_StatusFlg { get; set; }
        public bool? NCACVAC132DF_ActiveFlg { get; set; }
        public string NCACVAC132D_StatusFlg { get; set; }
        public string NCACVAC132DS_StatusFlg { get; set; }

        public long NCACVAC132DS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool NCACVAC132DS_CompletedFlg { get; set; }
        public long NCACVAC132DS_CompletedYear { get; set; }
        public bool NCACVAC132DS_ActiveFlg { get; set; }
        public long NCACVAC132DS_CreatedBy { get; set; }
        public long NCACVAC132DS_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DS_CreatedDate { get; set; }
        public DateTime? NCACVAC132DS_UpdatedDate { get; set; }
        public long discontid { get; set; }
        public long detailsyearid { get; set; }
        public long completedyearid { get; set; }
        public string completedyearname { get; set; }
        public string discontyearnm { get; set; }
        public string detailsyearname { get; set; }
        public int ASMAY_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public string studentname { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string completedyear { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public long UserId { get; set; }
        public Array introyearlist { get; set; }
        public Array discontyearlist { get; set; }
        public Array detailsyearlist { get; set; }
        public Array completedyearlist { get; set; }
        public Array ccourseNamelist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array alldata { get; set; }
        public Array alldatatab2 { get; set; }
        public Array mappedstudentlist { get; set; }
        public Array studentlist { get; set; }
        public Array studentedit { get; set; }
        public Array editlisttab2 { get; set; }
        public Array completeflage { get; set; }
        public Array reportlist { get; set; }
        public Array countOfStudentEntry { get; set; }
        public Array get_Continuedflagdata { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public NAAC_AC_VAC_DTO[] studentlstdata { get; set; }
        public long AMCO_Id { get; set; }
        public Array viewuploadfliesstudent { get; set; }
        public Array viewuploadfliesmain { get; set; }
        public Array deletemainfile { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewstudentlist { get; set; }
        public NAAC_AC_VAC_DTO[] filelist { get; set; }
        public NAAC_AC_VAC_DTO[] filelist_student { get; set; }
        public Array editMainSActFileslist { get; set; }
        public Array editStudentActFileslist { get; set; }
        public Array institutionlist { get; set; }
        public Array studentlist1 { get; set; }
        //added by sanjeev
        public NACCVACEXCEL[] advimppln { get; set; }
        public Array modalExcel { get; set; }

    }
    public class NACCVACEXCEL
    {
        public string IntroductionYear { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
    }
    public class filelist_student
    {
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
    }
}
