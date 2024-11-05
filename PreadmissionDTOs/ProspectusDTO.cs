using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class ProspectusDTO : CommonParamDTO
    {
        public long PASP_Id { get; set; }
        public long MI_ID { get; set; }
        public string IVRMMCT_Id { get; set; }
       
        public long IVRMMC_Id { get; set; }
       
        public long IVRMMS_Id { get; set; }
       
        public long ASMAY_Id { get; set; }
        
        public long ASMCL_Id { get; set; }
        
        public long PAMR_Id { get; set; }
        
        public long PAMS_Id { get; set; }
        public string PASP_First_Name { get; set; }
        public string PASP_Middle_Name { get; set; }
        public string PASP_Last_Name { get; set; }
        public string PASP_Pincode { get; set; }
        public DateTime? PASP_Date { get; set; }
        public long PASP_MobileNo { get; set; }
        public long PASP_PhoneNo { get; set; }
        public string PASP_EmailId { get; set; }
        public string PASP_HouseNo { get; set; }
        public string PASP_Street { get; set; }
        public string PASP_Area { get; set; }
        public string PASP_Enquiry { get; set; }
        public string returnval { get; set; }

        public string PASP_ProspectusNo { get; set; }

        public Array countryDrpDown { get; set; }
        public Array stateDrpDown { get; set; }
        public Array cityDrpDown { get; set; }

        public Array yeardropDown { get; set; }

        public Array referencedropDown { get; set; }
        public Array sourcedropDown { get; set; }
        public Array classdropDown { get; set; }

        public Array Prospectuslist { get; set; }

        public long id { get; set; }

        public Array MasterConfiguration { get; set; }

        public int ISPAC_EnquiryApplFlag { get; set; }

        //added on 13 jan 2017
        public string GeneratedNumber { get; set; }
        public MasterConfigurationDTO configurationsettings { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public long roleId { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public int count { get; set; }
        public string message { get; set; }

        public string prospectusfilePath { get; set; }

        public  Array paydet { get; set; }

        public Array paymentdetails { get; set; }

        public Array feedata { get; set; }

        public long FMA_Id { get; set; }

        public decimal FMA_Amount { get; set; }

        public int payementcheck { get; set; }

        public Array prospectusPaymentlist { get; set; }

    }
}
