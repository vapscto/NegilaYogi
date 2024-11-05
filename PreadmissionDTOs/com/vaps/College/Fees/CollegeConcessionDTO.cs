using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CollegeConcessionDTO : CommonParamDTO
    {
        public long AMSE_Id { get; set; }
        public long AMSC_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }
        public string FMH_FeeName { get; set; }
        public Array savedcondatalist { get; set; }
        public Array fillheaddata { get; set; }
        public string multiplegroups { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long userid { get; set; }
        public Array yearlst { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array semisterlistnew { get; set; }
        public Array studentlist { get; set; }
        public Array grouplist { get; set; }
        public Array smsemailsettings { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long[] AMS_Id { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long[] FMG_Ids { get; set; }
        public long FMG_Id { get; set; }
        public CollegeConcessionDTO[] savetmpdata { get; set; }
        public CollegeConcessionDTO[] savetmpdata1 { get; set; }
        public long FMH_Id { get; set; }
        public long FCSC_Id { get; set; }
        public string FCSC_ConcessionReason { get; set; }
        public string FCSC_ConcessionType { get; set; }
        public FeeHeadDTO[] TempararyArrayheadList { get; set; }
        public long FTI_Id { get; set; }
        public long FSCI_ID { get; set; }
        public long FSCI_ConcessionAmount { get; set; }
        public string returnval { get; set; }
        public string FTI_Name { get; set; }
        public long FMA_Amount { get; set; }
        public long FMA_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public Array savedrecord { get; set; }
        public Array studentrecord { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string FMG_GroupName { get; set; }
        public Array result { get; set; }
        public Array alldatahead { get; set; }
        public Array alldata { get; set; }
        public DailyCollectionReportDTO[] All_List { get; set; }
        public FeeGroupDTO[] TempararyArrayList { get; set; }

        public string classflag { get; set; }
        public long classid { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string allorindivflag { get; set; }
        public string allorstdorothersflag { get; set; }

        public string allorcorchoronlineflag { get; set; }

        public long cheque { get; set; }

        public Array fillfeehead { get; set; }
        public Array fillsection { get; set; }
        public string reporttype { get; set; }
    
        public string radioval { get; set; }
        public string studenttype { get; set; }

        public long active { get; set; }
        public long deactive { get; set; }
        public long left { get; set; }
        public decimal FCSA_Amount { get; set; }
        public long FCSA_Id { get; set; }
        public Array quota { get; set; }
        public Array category { get; set; }
        public long AMCOC_Id { get; set; }

        public Array userdetails { get; set; }
        public long user_id { get; set; }

        public Array fetchmodeofpayment { get; set; }

        public Array feeconfig { get; set; }

        public string rpttypmkckset { get; set; }
        public long AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }

        public string type { get; set; }
        public string radiobtnvalue { get; set; }
        public Array fillyear { get; set; }
        public string configset { get; set; }

        public Array fillgroup { get; set; }

        public Array EditfeeDetails { get; set; }

        public Array stafflist { get; set; }
        public Array fillfeecategory { get; set; }

        public Array configsetting { get; set; }
        public Array studentdata { get; set; }

        public string HRME_EmployeeFirstName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }

        public Array staffdata { get; set; }
          public string studentname { get; set; }

        public long HRME_Id { get; set; }

        public long FEC_Id { get; set; }
        public long FECI_Id { get; set; }

        public long FOC_Id { get; set; }

        public long FOCI_Id { get; set; }

        public Array instalspecial { get; set; }

        public Array otherlist { get; set; }

        public Array othersdata { get; set; }

        public string FMOST_StudentName { get; set; }

        public long FMOST_StudentMobileNo { get; set; }

        public string FMOST_StudentEmailId { get; set; }

        public long FMOST_Id { get; set; }

        public string FSC_ConcessionType { get; set; }

        public string FSC_ConcessionReason { get; set; }

        public Array terms_groups { get; set; }

        public DateTime asondate { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public long count { get; set; }

        public string admNo { get; set; }
      //  public string studentName { get; set; }
        public string charges { get; set; }
        public string concession { get; set; }
        public string rebate { get; set; }
        public string waiveOff { get; set; }
        public string fine { get; set; }
        public long collection { get; set; }
        public string debitBalance { get; set; }
        public string lastYearDue { get; set; }

        public string PFY_EndDate_DebitBalance { get; set; }
        public long CFY_PaidAmount { get; set; }
        public string CFY_BalanceAmount { get; set; }
        public string ExcessAmount { get; set; }

        public Array feeaccountsPositionReport { get; set; }

        public string FeeName { get; set; }
        public string className { get; set; }
        public long User_Id { get; set; }
        public string RouteName { get; set; }

        public Array feeconfiguration { get; set; }

        public Array termsList { get; set; }

        public string FMT_Name { get; set; }
        public long FMT_Id { get; set; }

        public Array customgrpList { get; set; }

        public long FMGG_Id { get; set; }

     //   public Array groupList { get; set; }

        public CollegeConcessionDTO[] selectedCGList { get;set;}
        public CollegeConcessionDTO[] selectedGroup { get;set;}
        public CollegeConcessionDTO[] FMT_Ids { get;set;}

    }

}
