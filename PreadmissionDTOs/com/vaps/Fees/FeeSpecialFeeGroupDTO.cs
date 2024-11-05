using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeSpecialFeeGroupDTO:CommonParamDTO
    {
        public long FMSFH_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMH_ID { get; set; }
        public string FMSFH_Name { get; set; }
        public bool FMSFH_ActiceFlag { get; set; }
        public long IVRMSTAUL_Id { get; set; }


        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }
        public TempSpecialDTo[] TempararyArrayList { get; set; }

        public Array RetrivefeeHeadData { get; set; }
        public List<FeeSpecialFeeGroupsGroupDTO> fggdto { get; set; }
        public string  hdname { get; set; }
        public Array newarydatah { set; get; }
        public long editidh { set; get; }

        public string FMH_Name { get; set; }
        public long FMSFHFH_Id { set; get; }
        public Array Feegroupgrping { get; set; }
        public bool FMSFHFH_ActiceFlag { get; set; }
    }
}
