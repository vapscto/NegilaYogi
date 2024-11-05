using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.HRMS
{
  public  class HRMasterPANDTO : CommonParamDTO
    {
        public long HRMPN_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRME_PAN { get; set; }
        public string HRME_TPAN { get; set; }
        public bool HRMPN_ActiveFlag { get; set; }
        public Array gmasterloanList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}

   