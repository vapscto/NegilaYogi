using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_TParticipation_113_DTO
    {

        public long NCACTP113_Id { get; set; }
        public long NCACTP113F_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACTP113_BodyName { get; set; }
        public long NCACTP113_ParticipatedYear { get; set; }
        public string NCACTP113_FileName { get; set; }
        public string NCACTP113_FilePath { get; set; }
        public bool NCACTP113_ActiveFlg { get; set; }
        public long NCACTP113_CreatedBy { get; set; }
        public long NCACTP113_UpdatedBy { get; set; }
        public DateTime? NCACTP113_CreatedDate { get; set; }
        public DateTime? NCACTP113_UpdatedDate { get; set; }
        public DateTime? NCACTP113_PDate { get; set; }
        public long cfileid { get; set; }
        public long UserId { get;set; }
        public long HRMDES_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long HRMD_Id { get; set; }
        public string empname { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public Array yearlist { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public Array emplist { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public string msg { get; set; }
        public Array alldata { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array reportlist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set;}
        public Array institutionlist { get; set;}

        public NAAC_AC_TParticipation_113_DTO[] emplstdata { get; set; }
        public NAAC_AC_TParticipation_113_DTO[] filelist { get; set; }

        public long NCAC424EXP_Id { get; set; }
        public string NCACTP113F_StatusFlg { get; set; }
        public long NCAC424EXPF_Id { get; set; }
        public string NCACTP113_StatusFlg { get; set; }

        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }

        public string UserName { get; set; }
        public long NCACTP113C_Id { get; set; }
        public string NCACTP113C_Remarks { get; set; }
        public long NCACTP113C_RemarksBy { get; set; }
        public string NCACTP113C_StatusFlg { get; set; }
        public bool NCACTP113C_ActiveFlag { get; set; }
        public long NCACTP113C_CreatedBy { get; set; }
        public DateTime NCACTP113C_CreatedDate { get; set; }
        public long NCACTP113C_UpdatedBy { get; set; }
        public DateTime NCACTP113C_UpdatedDate { get; set; }


        public long NCACTP113FC_Id { get; set; }
        public string NCACTP113FC_Remarks { get; set; }
        public long NCACTP113FC_RemarksBy { get; set; }
        public bool NCACTP113FC_ActiveFlag { get; set; }
        public long NCACTP113FC_CreatedBy { get; set; }
        public DateTime NCACTP113FC_CreatedDate { get; set; }
        public long NCACTP113FC_UpdatedBy { get; set; }
        public DateTime NCACTP113FC_UpdatedDate { get; set; }
        public string NCACTP113FC_StatusFlg { get; set; }


        public string Remarks { get; set; }
        public long filefkid { get; set; }

        public string NCACTP113_Remarks { get; set; }
        public bool? NCACTP113_ApprovedFlg { get; set; }
        public bool? NCACTP113F_ApprovedFlg { get; set; }
        public string NCACTP113F_Remarks { get; set; }
        //added by sanjeev 
        public staffParticiPateExcel[] advimppln { get; set; }
    }
    public class staffParticiPateExcel
    {
        public string ParticipationYear { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Employeecode { get; set; }
        public string Date { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Body { get; set; }
    }
}
