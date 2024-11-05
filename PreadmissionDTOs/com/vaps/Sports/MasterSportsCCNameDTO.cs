using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterSportsCCNameDTO:CommonParamDTO
    {
        public long SPCCMSCC_Id { get; set; }
        public long SPCCMSCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }
        public string SPCCMSCC_SportsCCDesc { get; set; }
        public string SPCCMSCC_SGFlag { get; set; }
        public int SPCCMSCC_NoOfMembers { get; set; }
        public string SPCCMSCC_RecHighLowFlag { get; set; }
        public string SPCCMSCC_RecInfo { get; set; }
        public bool SPCCMSCC_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array sportsCCNameList { get; set; }
        public Array editDetails { get; set; }
        public Array gropList { get; set; }
        public string groupName { get; set; }
        public bool retval { get; set; }
        public SPCCMSCCG_Underd[] tempDatas { get; set; }
        public string SPCCMSCCG_Under { get; set; }
        public Array editsubevent { get; set; }
        public bool? SPCCMSCC_MultiAttemptFlg { get; set; }
        public int? SPCCMSCC_NoOfAttempts { get; set; }
    }
    public class SPCCMSCCG_Underd
    {
        public long? SPCCMSCCG_Under { get; set; }
        public int? SPCCMSCCG_Level { get; set; }
        public long? SPCCMSCCG_Id { get; set; }
        public string SPCCMSCCG_UnderEvent { get; set; }
    }
}
