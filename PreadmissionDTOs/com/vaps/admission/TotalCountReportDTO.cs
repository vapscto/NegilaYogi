using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class TotalCountReportDTO
    {
       
        public long Id { get; set; }

       public string PASR_FirstName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Entry_Date { get; set; }
    }
}
