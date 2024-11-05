using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class MC_121_IntDept_Course_DTO
    {

        public long NMC121IDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long NMC121IDCF_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long UserId { get; set; }
        public long cfileid { get; set; }
        public long NMC121IDC_NoOfCourse { get; set; }
        public string NCACSP123F_FileName { get; set; }
        public string NCACSP123F_FilePath { get; set; }
        public string NCACSP123F_Filedesc { get; set; }  
        public string msg { get; set; }       
        public string AMCO_CourseName { get; set; }
        public string HRMD_DepartmentName { get; set; }        
        public string ASMAY_Year { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }

        public bool NMC121IDC_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }

        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array departmentlist { get; set; }
        public Array editcourselist { get; set; }       
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public Array alldata { get; set; }
        public Array mappedstudentlist { get; set; }
        public Array editlist { get; set; }
        public Array reportlist { get; set; }
        public bool? NMC121IDCF_ApprovedFlg { get; set; }
        public MC_121_IntDept_Course_DTO[] filelist { get; set; }
        public string NMC121IDCF_StatusFlg { get; set; }

        public long NMC121IDCFC_Id { get; set; }
        public string NMC121IDCFC_Remarks { get; set; }
        public long? NMC121IDCFC_RemarksBy { get; set; }
        public bool? NMC121IDCFC_ActiveFlag { get; set; }
        public long? NMC121IDCFC_CreatedBy { get; set; }
        public DateTime? NMC121IDCFC_CreatedDate { get; set; }
        public long? NMC121IDCFC_UpdatedBy { get; set; }
        public DateTime? NMC121IDCFC_UpdatedDate { get; set; }
        public string NMC121IDCFC_StatusFlg { get; set; }

        public bool? NMC121IDC_ApprovedFlg { get; set; }
        public string NMC121IDC_Remarks { get; set; }
        public long NMC121IDCC_Id { get; set; }
        public string NMC121IDCC_Remarks { get; set; }
        public long? NMC121IDCC_RemarksBy { get; set; }
        public string NMC121IDCC_StatusFlg { get; set; }
        public bool? NMC121IDCC_ActiveFlag { get; set; }
        public long? NMC121IDCC_CreatedBy { get; set; }
        public DateTime? NMC121IDCC_CreatedDate { get; set; }
        public long? NMC121IDCC_UpdatedBy { get; set; }
        public DateTime? NMC121IDCC_UpdatedDate { get; set; }

        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NMC121IDCF_Remarks { get; set; }
        public string NMC121IDC_StatusFlg { get; set; }

    }
}
