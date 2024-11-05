using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
  public  class PreadmissionNoticeRegistrationReportDTO
    {
        public long MI_Id { get; set; }
     
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long pasr_id { get; set; }
        public long pkid { get; set; }
        public long idsss { get; set; }
        public long PASRANS_ID { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long? IVRMMS_Id { get; set; }
        public long PASRAN_ID { get; set; }
        public decimal PASRAN_FEEAMOUNT { get; set; }
        public long? CasteCategory_Id { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public string PASRAN_NAME { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string ASMAY_Year { get; set; }
        public string PASR_FirstName { get; set; }
        public  string  PASR_PerState { get; set; }
        public string PASR_PerCity { get; set; }
        public string PASR_PerCountry { get; set; }
        public string PASRANS_REMARKS { get; set; }
        public string PASRAN_PAYTIME { get; set; }
        public string PASRANS_TIME { get; set; }
        public DateTime PASR_DOB { get; set; }
        public DateTime? PASRANS_ADMDATE { get; set; }
        public DateTime? PASRAN_PAYDATE { get; set; }
        public int PASR_Age { get; set; }
        public Array yearlist { get; set; }
        public Array prospectuslist { get; set; }
 
        public Array studentDetails { get; set; }
        public Array countryDrpDown { get; set; }
        public Array statedropdown { get; set; }
        public Array saveddata { get; set; }
        public Array reportdetails { get; set; }
        public Array alldata1 { get; set; }
        public Array uploadstuddetails { get; set; }
        public Array programlist { get; set; }
        public Array reportgrid { get; set; }
        public Array searchstudentDetails2 { get; set; }
        public string returnvaledit { get; set; }
        public string message { get; set; }
        public bool returnresult { get; set; }
        public bool duplicate { get; set; }
        public int PASR_ActiveFlag { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public  list[] list { get; set; }
       

        //==============================
        //public long asmaY_Year { get; set; }
        //public long pasR_RegistrationNo { get; set; }
        //public string pasR_FirstName { get; set; }
        //public DateTime? pasR_DOB { get; set; }
        //public DateTime? pasranS_ADMDATE { get; set; }
        //public int pasR_Age { get; set; }
        //public string pasR_PerCity { get; set; }
        //public string imcC_CategoryName { get; set; }
        //public string pasranS_REMARKS { get; set; }
        //public string pasranS_TIME { get; set; }

    }
    public class list
    {
        //public long ASMAY_Id { get; set; }
        public long pasr_id { get; set; }
       // public long IVRMMC_Id { get; set; }
      //  public long IVRMMS_Id { get; set; }
        public long PASRAN_ID { get; set; }
        public int PASR_Age { get; set; }
        public decimal PASRAN_FEEAMOUNT { get; set; }
        public long? CasteCategory_Id { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public string PASRAN_NAME { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string ASMAY_Year { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_PerState { get; set; }
        public string PASR_PerCity { get; set; }
        public string PASR_PerCountry { get; set; }
        public string PASRANS_REMARKS { get; set; }
        public string PASRAN_PAYTIME { get; set; }
        public string PASRANS_TIME { get; set; }
        public DateTime? PASR_DOB { get; set; }
        public DateTime? PASRANS_ADMDATE { get; set; }
    }

}
