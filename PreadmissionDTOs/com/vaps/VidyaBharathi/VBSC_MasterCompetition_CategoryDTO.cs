using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class VBSC_MasterCompetition_CategoryDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public string returnval { get; set; }
        public Array Master_trust { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long MT_Id { get; set; }
        public string VBSCMCC_CompetitionCategory { get; set; }
        public string VBSCMCC_CCDesc { get; set; }
        public bool VBSCMCC_CCAgeFlag { get; set; }
        public long VBSCMCC_CCAgeFromYear { get; set; }
        public long VBSCMCC_CCAgeFromMonth { get; set; }
        public long VBSCMCC_CCAgeFromDays { get; set; }
        public long VBSCMCC_CCAgeToYear { get; set; }
        public long VBSCMCC_CCAgeToMonth { get; set; }
        public long VBSCMCC_CCAgeToDays { get; set; }
        public bool VBSCMCC_CCWeightFlag { get; set; }
        public int VBSCMCC_CCFromWeight { get; set; }
        public int VBSCMCC_CCToWeight { get; set; }
        public bool VBSCMCC_CCClassFlg { get; set; }
        public bool VBSCMCC_ActiveFlag { get; set; }
        public string MO_Name { get; set; }
        public Array Year { get; set; }
        public Array Month { get; set; }
        public Array getReport { get; set; }
        public Array ClassArray { get; set; }
        public Array ClasslistArray { get; set; }
    }
}
