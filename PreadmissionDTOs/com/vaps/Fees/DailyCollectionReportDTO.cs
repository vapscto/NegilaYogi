using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class DailyCollectionReportDTO
    {
        public long FTP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long FMA_Id { get; set; }
        public decimal FTP_Paid_Amt { get; set; }
        public decimal FTP_Fine_Amt { get; set; }
        public decimal FTP_Concession_Amt { get; set; }
        public decimal FTP_Waived_Amt { get; set; }
        public string ftp_remarks { get; set; }


        // 07/12/2016  for daily coll

        public long ASMAY_Id { get; set; }
        public long AMAY_Id { get; set; }
        public string classname { get; set; }
        public long classid { get; set; }
        public string sectionname { get; set; }
        public long sectionid { get; set; }
        public Array fillyear { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public Array fillfeegroup { get; set; }
        public Array fillfeehead { get; set; }
        public Array alldata { get; set; }
        public Array alldatahead { get; set; }

        public Array alldatagridreport { get; set; }
        public Array alldatagridreportheads { get; set; }
        public FeeGroupDTO[] TempararyArrayList { get; set; }
        public string TempararyArrayListstring { get; set; }
        public Array allgroupheaddata { get; set; }
        public FeeHeadDTO[] TempararyArrayheadList { get; set; }

        public string FirstName { get; set; }

        public string FTIName { get; set; }
        public decimal FTIpaidamount { get; set; }
        public string Bankorcash { get; set; }
        public string Bankname { get; set; }
        public string chequenumber { get; set; }
        public DateTime chequedate { get; set; }
        public DateTime paiddate { get; set; }
        public string receiptnumber { get; set; }
        public string remarks { get; set; }
        public decimal concession { get; set; }

        public decimal fine { get; set; }

        public long FMG_Id { get; set; }

        public long FMH_Id { get; set; }

        public string FMH_FeeName { get; set; }

        // 07/12/2016  for daily coll

        //16/01/2016 for daily collection
        public string allorindivflag { get; set; }

        public string allorstdorothersflag { get; set; }
        public string allorcorchoronlineflag { get; set; }

        public string feehead { get; set; }

        public double cash { get; set; }
        public double checkdd { get; set; }
        public double total { get; set; }

        public string columnID { get; set; }
        public string columnName { get; set; }//mahaboob
     

        public FeeTransactionPaymentDTO[] TempararyArraygroupids { get; set; }
        public int mid { get; set; }
        public int? FMG_Order { get; set; }

        public string classflag { get; set; }
        public string groupflag { get; set; }

        public Array studentlist { get; set; }

        public string regornamedetails { get; set; }

        public long Amst_Id { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }

        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }

        //  public string fromdate { get; set; }
        //   public string todate { get; set; }
        public int Group_All { get; set; }
        //peera on 27/04/2017
        public Array alllist { get; set; }
        public DailyCollectionReportDTO[] All_List { get; set; }

        public int MI_ID { get; set; }
        public int userid { get; set; }
        public int yearid { get; set; }

        public long cheque { get; set; }

        public string fmG_GroupName { get; set; }

        public long FMSFHFH_Id { get; set; }
         public long FMSFH_Id { get; set; }

        public string TemplateString { get; set; }

        public string Mgt_Emailid { get; set; }

        public long MIMN_MobileNo { get; set; }

        public string MIE_EmailId { get; set; }

        public bool headwise { get; set; }
        public bool paymentwise { get; set; }
        public Array headwisecollection { get; set; }

        public Array studentalldata { get; set; }
        public Array totalcollection { get; set; }

        public Array dailycollreport { get; set; }
    }
}
