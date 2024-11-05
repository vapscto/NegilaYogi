using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
  public  class CMS_Master_Member_EmailDTO
    {

        public long CMSMMEMEID_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMEID_EmailId { get; set; }
        public bool CMSMMEMEID_DeFaultFlag { get; set; }
        public bool CMSMMEMEID_ActiveFlg { get; set; }
        public string returnval { get; set; }
        public long UserId { get; set; }
    }
}
