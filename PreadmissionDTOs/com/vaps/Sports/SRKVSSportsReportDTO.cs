using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public  class SRKVSSportsReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array yearlist { get; set; }
        public Array categoryList { get; set; }
        public Array categoryListnew { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime?  ToDate { get; set; }
        public string Type { get; set; }
        public Array CompetetionLevel { get; set; }
        public Array sportsCCList { get; set; }
        public Array GetReport { get; set; }
        public Array gettsreport { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
        public Categorylist[] Categorylists { get; set; }
        public CompetetionLeveltemp[] CompetetionLevels { get; set; }
        public Sportleveltemp[] Sportleveltemps { get; set; }
        public Array MasterEvent { get; set; }
        public long SPCCME_Id { get; set; }        
         public Array GetMasterEvent { get; set; }
        public long SPCCMSCCG_Id { get; set; }
        public SubEventLis[] SubEventLists { get; set; }
        public Array CompetetionLevelRecord { get; set; }
        public Array categoryListRecord { get; set; }
        public Array GetReportfinish { get; set; }


    }
    public class SubEventLis
    {
        public long SPCCMSCCG_Id { get; set; }
    }
    public class Categorylist
    {
        public long SPCCMCC_Id { get;set;}
    }
    public class CompetetionLeveltemp
    {
        public long SPCCMCL_Id { get; set; }
    }
    public class Sportleveltemp
    {
        public long SPCCME_Id { get; set; }
    }
   
    //work_attendence
}


