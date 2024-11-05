using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CLGAdm_College_RegNo_FormatDTO
    {
        public long ACRF_Id { get; set; }
        public long MI_Id { get; set; }
        public bool ACRF_CollegeCodeFlg { get; set; }
        public int ACRF_CCOrderFlg { get; set; }
        public bool ACRF_AYCodeFlg { get; set; }
        public int ACRF_AYCodeOrderFlg { get; set; }
        public bool ACRF_BranchCodeFlg { get; set; }
        public int ACRF_BranchCodeOrderFlg { get; set; }
        public string ACRF_NumericWidth { get; set; }
        public int ACRF_SLNo { get; set; }
        public string ACRF_StartingNo { get; set; }
        public bool ACRF_PrefilZeroFlg { get; set; }

        public bool duplicateval { get; set; }
        public bool returnval { get; set; }
        public Array datalist { get; set; }
    }
}
