using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class TotalSeatAllotmentDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSCD_SeatNos { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public long ACQ_Id { get; set; }
        public TotalSeatAllotmentDTO[] TempararyArrayListcoloumn { get; set; }


        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array semlist { get; set; }
        public Array intake_list { get; set; }
        public Array admitted_list { get; set; }
        public long stud_count { get; set; }
        public Array datareport { get; set; }
        public Array datareport1 { get; set; }
        public Array datareport2 { get; set; }
        public int count { get; set; }
        public int totalseats { get; set; }
        public int admitted { get; set; }
        public int vac { get; set; }
        public int acqid { get; set; }
        public int amb { get; set; }
    }
}
