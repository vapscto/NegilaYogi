using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class CLGTransportApproveDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FASMAY_Id { get; set; }

        public long CASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASTACO_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public long userId { get; set; }
        public long TRMRL_Id { get; set; }
        public long TRML_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public int? AMST_PerPincode { get; set; }
        public string Flag { get; set; }
        public string studentname { get; set; }
        public string areaname { get; set; }
        public string pickuproute { get; set; }

        public string pickuprouteno { get; set; }

        public string droprouteno { get; set; }
        public string pickuplocation { get; set; }
        public string drouproute { get; set; }
        public string drouplocation { get; set; }
        public string message { get; set; }
        public string applicationno { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }

        public string AMST_BloodGroup { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_Photoname { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMS_Name { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public string AMST_FatherOfficeAdd { get; set; }
        public string AMST_emailId { get; set; }
        public string ASMC_SectionName { get; set; }

        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public long? AMST_ConState { get; set; }
        public long? AMST_ConCountry { get; set; }
        public int? AMST_ConPincode { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMST_AdmNo { get; set; }
        public bool returnval { get; set; }
        public Array getaccyear { get; set; }
        public Array getcourse { get; set; }
        public Array getalldetails { get; set; }
        public Array studentdetails { get; set; }       
        public DateTime AMST_DOB { get; set; }
        public Temp_Save_List1[] Temp_Save_List { get; set; }
  

        public string ASTA_ApplStatus { get; set; }


        public string logopath { get; set; }

        public Array logoheader { get; set; }
        public Array getdetails { get; set; }

        public string ASTA_Regnew { get; set; }


        public long TRMR_Id { get; set; }

        public Array routename { get; set; }

        public string neworreguular { get; set; }

        public Array feeopeningbalance { get; set; }

        public long FMG_Id { get; set; }

        public Array openingbalance { get; set; }

        public Array transportcharges { get; set; }

        public long payableamount { get; set; }
        public long paidamount { get; set; }

        public string receiptno { get; set; }

        public DateTime paiddate { get; set; }

        public string termname { get; set; }
        public string headname { get; set; }
        public int ASMAY_Order { get; set; }
        public Array transportchargespaid { get; set; }

        public long FMT_Id { get; set; }
        public int? FMT_Order { get; set; }
        public int? year_Order { get; set; }
        public long year_Id { get; set; }

        public bool FMH_RefundFlag { get; set; }

        public string RegularNew { get; set; }
        public Array totalstudopebal { get; set; }

        public string remarks { get; set; }

        public Array smsemailarry { get; set; }

        public string studentremark { get; set; }

        public string studentremarkemail { get; set; }

        public bool smscheck { get; set; }
        public bool emailcheck { get; set; }

        public string htmldata { get; set; }

        public CLGTransportApproveDTO[] data_array { get; set; }
        public semisterlistdata[] AMSE_Idss { get; set; }

        public string astA_Landmark { get; set; }

        public long studentaccyear { get; set; }

        public Array axcess_op_bal { get; set; }


        //added Praveen
        public Array picsesslist { get; set; }
        public Array drpsesslist { get; set; }
        public long? PickUp_Session { get; set; }
        public long? Drop_Session { get; set; }
        //end Praveen


    }

    public class Temp_Save_List1
    {
        public long ASTACO_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long FASMAY_Id { get; set; }
        public string Applicationno { get; set; }
        public long PickUp_Session { get; set; }
        public long Drop_Session { get; set; }


    }
    public class semisterlistdata
    {

        public long AMSE_Id { get; set; }

    }

}
