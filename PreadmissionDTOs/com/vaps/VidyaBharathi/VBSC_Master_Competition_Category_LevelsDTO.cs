using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class VBSC_Master_Competition_Category_LevelsDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public string returnval { get; set; }
        public long VBSCMCCCLE_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long VBSCMCL_Id { get; set; }
        public bool VBSCMCCCLE_ActiveFlag { get; set; }
        public Category_Levels[] Category_Level { get; set; }
        public int savecount { get; set; }
        public int duplicatecount { get; set; }
        public Array Competetioncategory { get; set; }
        public long MT_Id { get; set; }
        public string MO_Name { get; set; }
        public Array Competelevelc { get; set; }
        public Array getReport { get; set; }
    }
    public class Category_Levels
    {
        public long VBSCMCL_Id { get; set; }
    }
}
