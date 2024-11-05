using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeInstallmentsDetailsDTO
    {
        public string FMI_Installment_Type { get; set; }
        public long FMI_No_Of_Installments { get; set; }
        public string FMI_Name { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public DateTime? Applicable_Date { get; set; }
        public DateTime? Due_Date { get; set; }
        public Array InstallmentDatalist { get; set; }
        public long MI_ID { get; set; }
        public long userid { get; set; }
        public long ASMAY_Id { get; set; }

    }
}
