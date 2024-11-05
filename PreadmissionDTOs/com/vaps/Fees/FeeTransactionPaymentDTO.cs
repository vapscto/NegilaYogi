using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeTransactionPaymentDTO
    {
        public long FTP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long FMA_Id { get; set; }
        public decimal FTP_Paid_Amt { get; set; }
        public decimal FTP_Fine_Amt { get; set; }
        public decimal FTP_Concession_Amt { get; set; }
        public decimal FTP_Waived_Amt { get; set; }
        public string ftp_remarks { get; set; }
        public long Balance_1 { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string con { get; set; }

        //kiran

        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMSC_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string typeflag { get; set; }
        public string FTI_Name { get; set; }
        public string ASMC_SectionName { get; set; }
        public long AMC_id { get; set; }

        public long FTI_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public decimal ftp_tobepaid_amt { get; set; }

        public Array adcyear { get; set; }
        public Array institutionDetails { get; set; }
        public Array feeheads { get; set; }
        public Array get_studentlist { get; set; }
        public Array feestatusreportlist { get; set; }
        public Array getfeedheadlist { get; set; }

        public Array fillmastergroup { get; set; }
        public Array feedefaulterreport { get; set; }

        public Array fillmasterhead { get; set; }

        public Array fillinstallment { get; set; }

        public Array fillclass { get; set; }

        public Array fillsection { get; set; }

        public Array fillcategory { get; set; }
        public Array getfeedefaultertemplate { get; set; }

        public FeeGroupDTO[] TempararyArrayList { get; set; }

        public Array studentalldata { get; set; }

        public Array classwisedata { get; set; }

        public Array groupalldata { get; set; }

        public Array headalldata { get; set; }

        public Array classalldata { get; set; }
        public Array class_list { get; set; }
        public Array section_list { get; set; }
        public Array termslist { get; set; }
        public Array fillgroup { get; set; }
        public classarray1[] classarray { get; set; }
        public long amstid { get; set; }

        public string TemplateName { get; set; }
        public string firstname { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public decimal balance { get; set; }
        public long mobileno { get; set; }

        public string groupname { get; set; }

        public string headname { get; set; }

        public bool category { get; set; }

        public Array rebate { get; set; }

        public long[] FTI_ids { get; set; }

        //added{
        public Array installment { get; set; }
        public Array installmentwisedata { get; set; }
        public FeeTransactionPaymentDTO[] SmsMailStudentDetails { get; set; }

        public string radioval { get; set; }

        public string studenttype { get; set; }

        public string type { get; set; }

        public DateTime? From_Date { get; set; }

        public DateTime? To_Date { get; set; }

        public string groupall { get; set; }

        public string reporttype { get; set; }

        public string duedate { get; set; }
        public string SmsMailText { get; set; }

        public string Select_Button { get; set; }

        public string columnID { get; set; }
        public string columnName { get; set; }
        public string FSDREM_Remarks { get; set; }
        public string message { get; set; }

        public FeeInstallmentyeralyDTO[] temparrayinst { get; set; }
        public FeeTransactionPaymentDTO[] temparrayinst1 { get; set; }
        public FeeTransactionPaymentDTO[] remarkarray { get; set; }

        //21/04/2017

        public Array fillstudent { get; set; }
        public int MI_ID { get; set; }
        public int userid { get; set; }


        public long Amst_Id { get; set; }
        public long IVRMRT_Id { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long HRME_Id { get; set; }
        public FeeStudentGroupMappingDTO[] studentdata { get; set; }
        public FeeStudentGroupMappingDTO[] savegrplst { get; set; }
        public FeeStudentGroupMappingDTO[] saveheadlst { get; set; }
        public FeeStudentGroupMappingDTO[] saveftilst { get; set; }
        public FeeTransactionPaymentDTO[] receiptlist { get; set; }

        public Array accountdetails { get; set; }
        public Array studentdetaillist { get; set; }
        public Array receiptdetails { get; set; }

        public long FBD_Id { get; set; }
        public string IFSC { get; set; }
        public string Acc_No { get; set; }
        public string Bank_Name { get; set; }
        public string MI_Name { get; set; }
        public Array fillterms { get; set; }
        public long FMT_ID { get; set; }
        public string FMT_Name { get; set; }

        public FeeTransactionPaymentDTO[] groupid { get; set; }

        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew { get; set; }
        public Array duration { get; set; }

        public int yearid { get; set; }

        public FeeTransactionPaymentDTO[] selectedStudentList { get; set; }
        public FeeTransactionPaymentDTO[] selectedStudentListforemail { get; set; }
        public string StudentName { get; set; }
        public string AMST_MobileNo { get; set; }

        public string totalbalance { get; set; }
        public string AMST_emailId { get; set; }
        //kiran

        public string msg { get; set; }
        public FeetransactionSMS[] FeetransactionSMS { get; set; }

        public string htmldata { get; set; }

        public Array customlist { get; set; }
        public Array grouplist { get; set; }
        public long FMGG_Id { get; set; }

       // public string FMH_Ids { get; set; }

        public string fmg_groupname { get; set; }

        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew1 { get; set; }
        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew2 { get; set; }
        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew3 { get; set; }
        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew4 { get; set; }
        public feeheadarray1[] feeheadarray { get; set; }
        public string customflag { get; set; }

        public string groupflag { get; set; }
        public string termflag { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }

        public long[] TRMR_Ids { get; set; }
        public string term_group { get; set; }

        public string AMST_AppDownloadedDeviceId { get; set; }
        public Array SWTPCount { get; set; }
        public Array CWTPCount { get; set; }

        public string year { get; set; }
        public string month { get; set; }
        public string date { get; set; }
        public string Due_Date { get; set; }
        public string amstemail { get; set; }
        public List<FeetransactionSMS> Due_Date_array { get; set; }
        public Array categorydata { get; set; }
        public long TRMR_Id { get; set; }
        public long FMCC_Id { get; set; }
        public string stype { get; set; }

        public string allindi { get; set; }

        public string FMG_Name { get; set; }
        public Array feesummlist { get; set; }
        public List<FeeTransactionPaymentDTO> FMT_Idss { get; set; }
        public List<FeeTransactionPaymentDTO> FMGG_Idss { get; set; }
        public List<FeeTransactionPaymentDTO> FMG_Idss { get; set; }

        public Array specialheadlist { get; set; }
        public Array specialheaddetails { get; set; }
        public Array instalspecial { get; set; }
        public Array alldata { get; set; }

        public long[] FMH_Ids { get; set; }
        public long active { get; set; }
        public long deactive { get; set; }
        public long left { get; set; }
        public long studying { get; set; }
        public long miid { get; set; }
        public long termswise { get; set; }

        public string AMST_AdmNo { get; set; }
        


        // ======================Staff fee Defaulter report
        public Array staff_flag { get; set; }
        public string ELP_Flg { get; set; }          
        public string asmcL_ClassName { get; set; }
        public string asmc_sectionname { get; set; }

        public Array smsemailsettings { get; set; }
        public Array student_balance_list { get; set; }
      
        public termslistarray1[] termslistarray { get; set; }
        public fillgrouparray1[] fillgrouparray { get; set; }
        public sectionarray1[] sectionarray { get; set; }

        public Array classalldatainscon { get; set; }
        public Array classalldatainsconN { get; set; }

        
        // public long HRME_Id { get; set; }

        //public long HRME_Id { get; set; }

        public class classarray1
        {
            public long ASMCL_Id { get; set; }
        }
        public class feeheadarray1
        {
            public long FMH_Id { get; set; }
        }

        public class termslistarray1
        {
            public long FMT_Id { get; set; }
        }

        public class fillgrouparray1
        {
            public long FMG_Id { get; set; }
        }
        public class sectionarray1
        {
            public long ASMS_Id { get; set; }
        }
    }
}
