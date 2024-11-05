using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class MastersProject_DTO
    {
        public long ISMMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public string ISMMPR_Desc { get; set; }
        public bool ISMMPR_InternalProjectFlg { get; set; }
        public bool ISMMPR_ActiveFlg { get; set; }
        public long? ISMMPR_CreatedBy { get; set; }
        public long? ISMMPR_UpdatedBy { get; set; }

        public long UserId { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }
        public Array getinstitution { get; set; }
    }
}
