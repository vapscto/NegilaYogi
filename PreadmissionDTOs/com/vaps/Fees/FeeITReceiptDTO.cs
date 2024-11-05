using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeITReceiptDTO
    {
        public Array admsudentslist { get; set; }
        public Array academicyr { get; set; }
        public Array reportdatelist { get; set; }
        public Array studentsnames { get; set; }
        
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long Amst_Id { get; set; }
        public DateTime datedisplay { get; set; }
        public long MI_ID { get; set; }
        public long asmyid { get; set; }
        public string filterinitialdata { get; set; }
        public string AMST_AdmNo { get; set; }
    }
}
