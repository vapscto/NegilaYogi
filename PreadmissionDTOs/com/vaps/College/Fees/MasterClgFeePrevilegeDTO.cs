using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class MasterClgFeePrevilegeDTO : CommonParamDTO
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
        public string ASMAY_Year { get; set; }
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
        public MasterClgFeePrevilegeDTO[] grouphead { get; set; }
        public MasterClgFeePrevilegeDTO[] username { get; set; }
    }
}
