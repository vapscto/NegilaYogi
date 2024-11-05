using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeGroupMappingDTO:CommonParamDTO
    {
        public long FMGG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMGG_GroupName { get; set; }
        public string FMGG_GroupCode { get; set; }
        public long FMG_Id { get; set; }
        public bool FMGG_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupGroupingData { set; get; }
        public Array GroupData { set; get; }   
        public TempDTOO[] TempararyArrayList { get; set; }
        public List<FeegroupgroupingDTO> fggdto { get; set; }
        public string grpname { get; set; }
        public Array newarydata { set; get; }
        public long editid { set; get; }


        //new add 
        public string groupgrpnamebind { get; set; }
        public string groupgroupcode { get; set; }
        public string grpnamebind { get; set; }
        public bool actflag { get; set; }
        public long fmggidbind { get; set; }
        public bool returnvalforeign { get; set; }
        public long FMGGG_Id { get; set; }

    }
}
