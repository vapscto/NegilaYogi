using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_Y_Payment_School_StudentDTO
    {
        public long FYPS_Id { get; set; }
        public long FYP_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASAMY_Id { get; set; }
        public decimal FTP_TotalPaidAmount { get; set; }
        public decimal FTP_TotalWaivedAmount { get; set; }
        public decimal FTP_TotalConcessionAmount { get; set; }
        public decimal FTP_TotalFineAmount { get; set; }
    }
}
