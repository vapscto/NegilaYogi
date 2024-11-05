using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class ParentSmartCardDTO
    {
        public long MI_Id { get; set; }

        public long ASMAY_Id { get; set; }
        //public long AMST_Id { get; set; }
 



        public Array cardData { get; set; }

        public Array Griddata { get; set; }

        public Array updatestudetailslist { get; set; }
        public Array countryDrpDown { get; set; }

        public Array stateDrpDown { get; set; }
        public string returnval { get; set; }

        public long STP_ID { get; set; }
        public long AMST_ID { get; set; }
        public string STP_SNAME { get; set; }
        public string STP_SEMAIL { get; set; }
        public long? STP_SMOBILENO { get; set; }
        public string STP_SBLOOD { get; set; }
        public string STP_SPHOTO { get; set; }
        public string STP_FNAME { get; set; }
        public string STP_FEMAIL { get; set; }
        public long? STP_FMOBILENO { get; set; }
        public string STP_FBLOOD { get; set; }
        public string STP_FPHOTO { get; set; }
        public string STP_MNAME { get; set; }
        public string STP_MEMAIL { get; set; }
        public long? STP_MMOBILENO { get; set; }
        public string STP_MBLOOD { get; set; }
        public string STP_MPHOTO { get; set; }
        public string STP_PERSTREET { get; set; }
        public string STP_PERAREA { get; set; }
        public string STP_PERCITY { get; set; }
        public long STP_PERSTATE { get; set; }
        public long STP_PERCOUNTRY { get; set; }
        public long STP_PERPIN { get; set; }
        public string STP_CURSTREET { get; set; }
        public string STP_CURAREA { get; set; }
        public string STP_CURCITY { get; set; }
        public long STP_CURSTATE { get; set; }
        public long STP_CURCOUNTRY { get; set; }
        public long STP_CURPIN { get; set; }
        public string STP_STATUS { get; set; }

        public string STP_FLAG { get; set; }
        public string school { get; set; }

        public bool existsornot { get; set; }

        public string searchfilter { get; set; }

        public Array fillstudent { get; set; }

        public string AMST_FirstName { get; set; }

        public string RoleName { get; set; }

        public string htmldata { get; set; }
        public DateTime? STP_DOB { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string STP_DOBWORDS { get; set; }
    }
}