using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class Master_Competition_Category_ClassesDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public string returnval { get; set; }
        public long VBSCMCCCL_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long ASMCL_ID { get; set; }
        public bool VBSCMCC_ActiveFlag { get; set; }
        public clalist[] clalists { get; set; }
        public int savecount { get; set; }
        public int duplicatecount { get; set; }
    }
    public class clalist
    {
        public long ASMCL_ID { get; set; }
    }
}
