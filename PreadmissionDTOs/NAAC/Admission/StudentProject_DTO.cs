using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class StudentProject_DTO
    {
        public long NCACSPR133_Id { get; set; }
        public long NCACSPR133F_Id { get; set;}
        public long MI_Id { get; set; }
        public long NCMCSP134_Year { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long NCACSPR133_Year { get; set;}
        public bool returnval { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string NCACSPR133_ProjectName { get; set; }
        public string AMB_BranchName { get; set;}

        public string NCACSPR133_FileName { get; set; }
        public string NCACSPR133_FilePath { get; set; }
        public string NCACSPR133F_Filedesc { get; set; }
        public bool NCACSPR133_ActiveFlg { get; set; }

        public long NCACSPR133_CreatedBy { get; set; }
        public long NCACSPR133_UpdatedBy { get; set; }
        public DateTime? NCACSPR133_CreatedDate { get; set; }
        public DateTime? NCACSPR133_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array studentlist { get; set; }
        public Array alldata { get; set; }     
        public string studentname { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public long NCMCSP134_Id { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingFieldVisits { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingClinical { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingResearchProjects { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingIndustryVisits { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingCommunityPostings { get; set; }
        public StudentProject_DTO[] filelist { get; set; }
        public Array editlist { get; set; }
        public Array reportlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array institutionlist { get; set; }
        public Array gridlist { get; set; }

        public StudentProject_DTO[] studentlstdata { get; set; }
        public string NCACSPR133F_StatusFlg { get; set; }
        public bool? NCACSPR133F_ApprovedFlg { get; set; }
        public bool? NCACSPR133F_ActiveFlg { get; set; }
        public long cfileid { get; set; }

        public long NCACSPR133C_Id { get; set; }
        public string NCACSPR133C_Remarks { get; set; }
        public long NCACSPR133C_RemarksBy { get; set; }
        public string NCACSPR133C_StatusFlg { get; set; }
        public long? NCACSPR133C_CreatedBy { get; set; }
        public long? NCACSPR133C_UpdatedBy { get; set; }
        public DateTime? NCACSPR133C_CreatedDate { get; set; }
        public bool? NCACSPR133C_ActiveFlag { get; set; }



        public long NCACSPR133FC_Id { get; set; }
        public string NCACSPR133FC_Remarks { get; set; }
        public long? NCACSPR133FC_RemarksBy { get; set; }
        public bool NCACSPR133FC_ActiveFlag { get; set; }
        public long? NCACSPR133FC_CreatedBy { get; set; }
        public long? NCACSPR133FC_UpdatedBy { get; set; }
        public DateTime? NCACSPR133FC_CreatedDate { get; set; }
        public DateTime? NCACSPR133FC_UpdatedDate { get; set; }
        public string NCACSPR133FC_StatusFlg { get; set; }

        public string UserName { get; set; }

        public Array commentlist1 { get; set; }
        public Array commentlist { get; set; }
        public string NCMCSP134_StatusFlg { get; set; }

        public bool NCMCSP134F_ActiveFlg { get; set; }
        public string NCMCSP134F_StatusFlg { get; set; }
        public string NCACSPR133_StatusFlg { get; set; }


        public long NCMCSP134C_Id { get; set; }
        public string NCMCSP134C_Remarks { get; set; }
        public long NCMCSP134C_RemarksBy { get; set; }
        public string NCMCSP134C_StatusFlg { get; set; }
        public bool NCMCSP134C_ActiveFlag { get; set; }
        public long NCMCSP134C_CreatedBy { get; set; }
        public DateTime? NCMCSP134C_CreatedDate { get; set; }
        public long NCMCSP134C_UpdatedBy { get; set; }
        public DateTime? NCMCSP134C_UpdatedDate { get; set; }


        public long NCMCSP134FC_Id { get; set; }
        public string NCMCSP134FC_Remarks { get; set; }
        public long NCMCSP134FC_RemarksBy { get; set; }
        public bool NCMCSP134FC_ActiveFlag { get; set; }
        public long? NCMCSP134FC_CreatedBy { get; set; }
        public DateTime? NCMCSP134FC_CreatedDate { get; set; }
        public long? NCMCSP134FC_UpdatedBy { get; set; }
        public DateTime? NCMCSP134FC_UpdatedDate { get; set; }
        public string NCMCSP134FC_StatusFlg { get; set; }
        public long NCMCSP134F_Id { get; set; }

        public string NCMCSP134F_Remarks { get; set; }
        public bool? NCMCSP134F_ApprovedFlg { get; set; }
        public string NCMCSP134_Remarks { get; set; }
        public bool? NCMCSP134_ApprovedFlg { get; set; }
        public bool? NCMCSP134_ActiveFlag { get; set; }
        //added by sanjeev
        public Array modalexcelfile { get; set; }
        public Array duplicatfile { get; set; }
        public excelfile5ttt[] advimppln { get; set; }
    }
    public class excelfile5ttt
    {
        public string ParticipationYear { get; set; }
        public string Course { get; set; }
        public string Branch { get; set; }
        public string ProjectName { get; set; }
        public string AdmisionNumber { get; set; }
    }
}
