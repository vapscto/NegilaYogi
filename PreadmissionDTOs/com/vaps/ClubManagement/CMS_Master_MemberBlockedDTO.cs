using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_Master_MemberBlockedDTO
    {
        public long CMSMMEMBLK_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public DateTime? CMSMMEMBLK_BlockedDate { get; set; }
        public string CMSMMEMBLK_ReasonForBlock { get; set; }
        public DateTime? CMSMMEMBLK_RenewalDate { get; set; }
        public bool CMSMMEMBLK_ActiveFlg { get; set; }
        public DateTime? CMSMMEMBLK_CreatedDate { get; set; }
        public string MemberName { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array getreport { get; set; }
        public Array getname { get; set; }
    }
}
