using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeDemandRegisterInstallmentDTO
    {

        public long AMST_Id { get; set; }
        public string installmentname { get; set; }
        public string installmentfees { get; set; }
        public string feename { get; set; }
        public string studentname { get; set; }
        public string admno { get; set; }

        public FeeDemandRegisterInstallmentDTO[] FeeDemandRegisterInstallmenttemp {get; set;}
      
    }
  
}
