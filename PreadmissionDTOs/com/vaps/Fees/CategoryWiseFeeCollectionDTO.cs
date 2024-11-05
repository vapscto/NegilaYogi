using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
   public  class CategoryWiseFeeCollectionDTO
    {
        public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public long userid { get; set; }
       
        public string typeflag { get; set; }
        public string FTI_Name { get; set; }
        public string ASMC_SectionName { get; set; }

        public long AMC_id { get; set; }

    

        public string FMH_FeeName { get; set; }
        public decimal ftp_tobepaid_amt { get; set; }

        public Array adcyear { get; set; }
        public Array terms { get; set; }
       
        public Array fillcategory { get; set; }



        public Array studentalldata { get; set; }


        public Array studentname { get; set; }
     
        public long amstid { get; set; }

        public string firstname { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public decimal balance { get; set; }
        public long mobileno { get; set; }

        public string groupname { get; set; }

        public string headname { get; set; }

        public bool category { get; set; }

        public string AMST_Firstname { get; set; }
        public long[] ASMAY_Ids { get; set; }

        public string type { get; set; }
      
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public long FMT_Id { get; set; }
        public string FMT_Name { get; set; }

        public Array classlist { get; set; }
        public Array sectionlist { get; set; }

        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }

    }
}
