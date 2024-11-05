using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACMasterSportsCADTO
    {
        public long NCAC533SPCAA_Id { get; set; }
        public long NCAC533SPCAAF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC533SPCAA_Year { get; set; }
        public string NCAC533SPCAA_NameOfActivities { get; set; }
        public bool NCAC533SPCAA_ActiveFlg { get; set; }
        public long NCAC533SPCAA_CreatedBy { get; set; }
        public long NCAC533SPCAA_UpdatedBy { get; set; }
        public DateTime NCAC533SPCAA_CreatedDate { get; set; }
        public DateTime NCAC533SPCAA_UpdatedDate { get; set; }

        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }
        public string NCAC533SPCAA_ActType { get; set; }
        public string NCAC533SPCAA_ActLevel { get; set; }

        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editfiles { get; set; }
        public string NCAC533SPCAA_StatusFlg { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
    }
}
