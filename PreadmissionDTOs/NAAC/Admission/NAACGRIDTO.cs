using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACGRIDTO
    {
        public long NCAC516GRI_Id { get; set; }
        public long NCAC516GRIF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC516GRI_Year { get; set; }
        public string NCAC516GRI_GRIAPP { get; set; }
        public string NCAC516GRI_GRIRED { get; set; }
        public string NCAC516GRI_AvgTime { get; set; }
        public string NCAC516GRI_FileDesc { get; set; }
        public string NCAC516GRI_FileName { get; set; }
        public string NCAC516GRI_FilePath { get; set; }
        public bool NCAC516GRI_ActiveFlg { get; set; }
        public long NCAC516GRI_CreatedBy { get; set; }
        public long NCAC516GRI_UpdatedBy { get; set; }
        public DateTime NCAC516GRI_CreatedDate { get; set; }
        public DateTime NCAC516GRI_UpdatedDate { get; set; }
        public long UserId { get; set; }

        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public Array alldatalist1 { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool NCAC516GRI_AdpOfguidelinesofRegbodiesFlg { get; set; }
        public bool NCAC516GRI_StusgrvOnline_OR_offlineFlg { get; set; }
        public bool NCAC516GRI_CommitteewithminutesFlg { get; set; }
        public bool NCAC516GRI_RecordOfActionTakenFlg { get; set; }
        public Array editfiles { get; set; }
        public string NCAC516GRI_StatusFlg { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }

    }
}
