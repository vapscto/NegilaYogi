using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
   public class ClientWise_Module_Feature_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_Flag { get; set; }
        public string Flag { get; set; }
        public string Type { get; set; }
        public string moduleflage { get; set; }
        public Array clientlist { get; set; }
        public Array projectlist { get; set; }
        public Array modulelist { get; set; }
        public Array getreportdetails { get; set; }
        public Array getmodulename { get; set; }
    }
}
