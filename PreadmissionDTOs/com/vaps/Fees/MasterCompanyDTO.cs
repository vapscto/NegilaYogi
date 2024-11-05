using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class MasterCompanyDTO
    {
        public int Cmp_Code { get; set; }
        public string CMP_Name { get; set; }
        public string CMP_Address { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string E_MailAddress { get; set; }
        public string IncomeTaxNo { get; set; }
    }
}
