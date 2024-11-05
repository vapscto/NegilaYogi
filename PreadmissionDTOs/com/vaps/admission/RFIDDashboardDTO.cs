using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class RFIDDashboardDTO
    {
        public long yearid { get; set; }
        public long asmcL_Id { get; set; }
        public long sectionid { get; set; }
        public Array yearfilllist { get; set; }
        public Array classfilllist { get; set; }
        public Array sectionfilllist { get; set; }
        public Array studentlist { get; set; }
        public Array inlist { get; set; }
        public Array outlist { get; set; }
        public Array presentstudentlist { get; set; }
        public Array totalstudentlist { get; set; }
        public RFIDDashboardDTO[] savetmpdata { get; set; }
        //public string asmaY_Year { get; set; }
        public string asmcL_ClassName { get; set; }
        public string name { get; set; }
        public string stuFN { get; set; }
        public string stuMN { get; set; }
        public string stuLN { get; set; }
        public string regno { get; set; }
        public string admno { get; set; }
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_SOL { get; set; }
        public string AMST_RFCardNo { get; set; }
        public string AMST_SOL_activate { get; set; }
        public string AMST_SOL_deactivate { get; set; }
        public bool checkedvalue { get; set; }
        public bool returnval { get; set; }
        public int count { get; set; }
        public long MI_Id { get; set; }
        public int ASMCL_order { get; set; }
        public string remarks { get; set; }
        public long userid { get; set; }
        public DateTime adate { get; set; }
        public DateTime IRFPU_DateTime { get; set; }
    }
}
