using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
   public class CategoryConcessionGroupMappingDTO
    {

        public long FMCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public bool FMCCG_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public long user_Id { get; set; }
        public Array acayear { get; set; }
        public Array concession { get; set; }
        public Array group { get; set; }
        public Array head { get; set; }
        public string FMH_FeeName { get; set; }
        public Array conce { get; set; }
        public string FTI_Name { get; set; }
        public CategoryConcessionGroupMappingDTO[] headlistdata { get; set; }
        public long [] fmhss { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }
        public Array alldata { get; set; }
        public string ASMAY_Year { get; set; }
        public string FMCC_ConcessionName { get; set; }
        public string FMG_GroupName { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
    }
}
