using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class QuestionPaperTypeDTO 
    {
        public long OTQPTYP_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long HRMP_Id { get; set; }
        public string retrunMsg { get; set; }
        public string HRMP_Position { get; set; }
        public string OTQPTYP_QuestionPaperName { get; set; }
        public string OTQPTYP_QuestionPaperDesc { get; set; }
        public bool OTQPTYP_ActiveFlg { get; set; }
        public DateTime OTQPTYP_CreatedDate { get; set; }
        public DateTime OTQPTYP_UpdatedDate { get; set; }
        public long OTQPTYP_CreatedBy { get; set; }
        public long OTQPTYP_UpdatedBy { get; set; }
        public bool retrval { get; set; }
        public Array positionlist { get; set; }
        public Array qnstypelist { get; set; }
        public Array editlist { get; set; }
    }

}