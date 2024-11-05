using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class MasterSportsCCGroupDTO:CommonParamDTO
    {
        public long SPCCMSCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSCCG_SportsCCGroupName { get; set; }
        public string SPCCMSCCG_SportsCCGroupDesc { get; set; }
        public string SPCCMSCCG_SCCFlag { get; set; }
        public bool SPCCMSCCG_ActiveFlag { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array groupNameList { get; set; }
        public Array editDetails { get; set; }
        public bool returnval { get; set; }
        public tempData[] tempDatas { get; set; }
        public long UserId { get; set; }

    }
    public class tempData
    {
        public string SPCCMSCCG_SportsCCGroupName { get; set; }
        public long? SPCCMSCCG_Level { get; set; }
        public long? SPCCMSCCG_Id { get; set; }
        public string SPCCMSCCG_SportsCCGroupDesc { get; set; }
    }
}
