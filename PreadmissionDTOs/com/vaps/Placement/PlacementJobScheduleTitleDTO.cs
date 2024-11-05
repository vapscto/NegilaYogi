using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
    public class PlacementJobScheduleTitleDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        //table DTO
        public long PLCISCHCOMJTST_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMCST_Id { get; set; }
        public DateTime PLCISCHCOMJTST_Date { get; set; }
        public bool PLCISCHCOMJTST_ActiveFlag { get; set; } 
        public DateTime PLCISCHCOMJTST_CreatedDate { get; set; }
        public long PLCISCHCOMJTST_CreatedBy { get; set; }
        public DateTime PLCISCHCOMJTST_UpdatedDate { get; set; }
        public long PLCISCHCOMJTST_UpdatedBy { get; set; }

        //extra DTO

        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public Array scompany { get; set; }
        public Array scourse { get; set; }
        public Array sbranch { get; set; }
        public Array schedulestudentname { get; set; }
        public Array shiftrole { get; set; }
        public string flag { get; set; }
        public string companyschedulename { get; set; }
        public long primarycompanyid { get; set; }
        public string schedulecourse { get; set; }
        public string schedulebranch { get; set; }
        public string studentname { get; set; }
        public long idschedulecourse { get; set; }
        public long idschedulebranch { get; set; }
        public long idschedulestudentname { get; set; }
        public long UserId { get; set; }
        public DateTime fromdate { get; set; }
        public Array schedulestudentname1 { get; set; }
        public Array schedulestudentnames { get; set; }
        public long idschedulestudentnames { get; set; }
        public string studentnames { get; set; }


        public Array studentgridtable { get; set; }
        public Array admingridtable { get; set; }
        public string csname { get; set; }
        public string asname { get; set; }
        public DateTime screateddate { get; set; }
        public DateTime acreateddate { get; set; }
        public string smname { get; set; }
        public string amname { get; set; }
        public DateTime supdateddate { get; set; }
        public DateTime aupdateddate { get; set; }
        public string PLMCOMP_CompanyName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string PLCISCHCOMJT_JobTitle { get; set; }
        public string PLCISCHCOMJT_QulaificationCriteria { get; set; }
        public string PLCISCHCOMJT_OtherDetails { get; set; }
        public Array editdata { get; set; }
        public long registration { get; set; }


        public grid_arraydata[] grid_arraydatasamb { get; set; }
        public class grid_arraydata
        {
            public long ambid { get; set; }

        }

        public grid_arraydatas[] grid_arraydatassamco { get; set; }
        public class grid_arraydatas
        {
            public long amcoid { get; set; }

        }

        public grid_arraydatass[] grid_arraydatasss { get; set; }
        public class grid_arraydatass
        {
            public long sid { get; set; }

        }





    }
}
