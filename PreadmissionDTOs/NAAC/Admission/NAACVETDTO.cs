using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACVETDTO
    {
        public long NCAC515VET_Id { get; set; }
        public long NCAC515VETF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC515VET_Year { get; set; }
        public string NCAC515VET_VETProgramName { get; set; }
        public long NCAC515VET_NoOfStudents { get; set; }
        public string NCAC515VET_FileName { get; set; }
        public string NCAC515VET_StatusFlg { get; set; }
        public string NCAC515VET_FilePath { get; set; }
        public bool NCAC515VET_ActiveFlg { get; set; }
        public long NCAC515VET_CreatedBy { get; set; }
        public long NCAC515VET_UpdatedBy { get; set; }
        public DateTime NCAC515VET_CreatedDate { get; set; }
        public DateTime NCAC515VET_UpdatedDate { get; set; }
        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public Array institutionlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }

    }
}
