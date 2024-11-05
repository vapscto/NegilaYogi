using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterHirer_Group_RateDTO:CommonParamDTO
    {
       //Master_Hirer_Group
        public long TRHG_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRHG_HirerGroup { get; set; }
        public string TRHG_HirerDec { get; set; }
        public bool TRHG_ActiveFlg { get; set; }
        public int count { get; set; }
        public string returnVal { get; set; }
        public Array hirerGroupList { get; set; }
        public Array editDataList { get; set; }

        //Master_Hirer_Rate
        public long TRHR_Id { get; set; }
        public long TRMVT_Id { get; set; }
        public decimal? TRHR_RatePerKM { get; set; }
        public string TRMVT_VehicleType { get; set; }
        public Array vhcleTypeList { get; set; }
        public Array hirerRateList { get; set; }

         //Master_Hirer

        public long TRMH_Id { get; set; }
        public string TRMH_HirerName { get; set; }
        public string TRMH_ConatctPerName { get; set; }
        public string TRMH_ContactPersonDesg { get; set; }
        public long TRMH_ContactNo { get; set; }
        public long TRMH_MobileNo { get; set; }
        public string TRMH_EmailId { get; set; }
        public string TRMH_Address { get; set; }
        public bool TRMH_ActiveFlg { get; set; }
        public Array hirerList { get; set; }


    }
}
