using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACQualifyDTO
    {
        public long NCAC523QAMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC523QAMA_ExamName { get; set; }
        public string NCAC523QAMA_ExamDes { get; set; }
        public bool NCAC523QAMA_ActiveFlg { get; set; }
        public long NCAC523QAMA_CreatedBy { get; set; }
        public long NCAC523QAMA_UpdatedBy { get; set; }
        public DateTime NCAC523QAMA_CreatedDate { get; set; }
        public DateTime NCAC523QAMA_UpdatedDate { get; set; }
        public long NCAC523QE_Id { get; set; }
        public long NCAC523QEF_Id { get; set; }
      
        public long NCAC523QE_Year { get; set; }
        public long NCAC523QE_NoOfStudents { get; set; }
        public long NCAC523QE_NoOfStudentsappearing { get; set; }
        public string NCAC523QE_FileDesc { get; set; }
        public string NCAC523QE_FileName { get; set; }
        public string NCAC523QE_FilePath { get; set; }
        public string NCAC523QE_StatusFlg { get; set; }
        public bool NCAC523QE_ActiveFlg { get; set; }
        public long NCAC523QE_CreatedBy { get; set; }
        public long NCAC523QE_UpdatedBy { get; set; }
        public DateTime NCAC523QE_CreatedDate { get; set; }
        public DateTime NCAC523QE_UpdatedDate { get; set; }
        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }

        public Array alldatatab2 { get; set; }
        public Array institutionlist { get; set; }
        public Array examlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
    }
}
