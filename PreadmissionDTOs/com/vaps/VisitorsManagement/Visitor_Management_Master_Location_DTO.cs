using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class Visitor_Management_Master_Location_DTO : CommonParamDTO
    {
        public long VMML_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMML_Location { get; set; }
        public string VMML_LocationDescription { get; set; }
        public string VMML_LocationFacilities { get; set; }
        public bool VMML_ActiveFlg { get; set; }
        public long? VMML_CreatedBy { get; set; }
        public long? VMML_UpdatedBy { get; set; }

        public long UserId { get; set; }
        public Array getdata { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array editlist { get; set; }

    }
}
