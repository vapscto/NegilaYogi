using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeePrevilegeDTO:CommonParamDTO
    {
       
        public long FGL_Id { get; set; }
        public long FMG_ID { get; set; }
        public long User_Id { get; set; }
        public long FMH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array adcyear { get; set; }
        public Array prelist { get; set; }
        public string NormalizedUserName { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMH_FeeName { get; set; }
        public string IVRMRT_Role { get; set; }
        public long IVRMRT_Id { get; set; }
        public int UserId { get; set; }
        public Array group { get; set; }
        public Array role { get; set; }
        public Array head { get; set; }
        public Array usnam { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array roleid { get; set; }
        public string returnduplicatestatus { get; set; }
        public FeePrevilegeDTO[] grouphead { get; set; }
        public FeePrevilegeDTO[] username { get; set; }

        public Array fillheads { get; set; }

    }
}
