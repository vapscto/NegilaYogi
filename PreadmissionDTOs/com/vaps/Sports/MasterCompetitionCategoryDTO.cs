using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterCompetitionCategoryDTO:CommonParamDTO
    {
        public long SPCCMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMCC_CompitionCategory { get; set; }
        public string SPCCMCC_CCDesc { get; set; }
        public bool SPCCMCC_CCAgeFlag { get; set; }
        public long SPCCMCC_FromCCAgeYear { get; set; }
        public long SPCCMCC_FromCCAgeMonth { get; set; }
        public long SPCCMCC_FromCCAgeDays { get; set; }
        public bool SPCCMCC_CCWeightFlag { get; set; }
        public decimal SPCCMCC_CCWeight { get; set; }
        public bool SPCCMCC_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array competitionCategoryList { get; set; }
        public Array editDetails { get; set; }

        public long SPCCMCC_ToCCAgeYear { get; set; }
        public long SPCCMCC_ToCCAgeMonth { get; set; }
        public long SPCCMCC_ToCCAgeDays { get; set; }

        public bool retval { get; set; }

    }
}
