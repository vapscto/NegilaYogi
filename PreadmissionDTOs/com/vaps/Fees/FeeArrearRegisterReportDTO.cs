using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeArrearRegisterReportDTO
    {
        public TempDTO[] TempararyArrayListnew { get; set; }
        public FeeArrearRegisterReportDTO[] TempararyArrayhEADListnew { get; set; }
        public long MI_ID { get; set; }
        public Array admsudentslist { get; set; }
        public Array admsudentslist1 { get; set; }
        public Array acayear { get; set; }
        public Array busroutelist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array grouplist { get; set; }
        public Array headlist { get; set; }
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long asmay_id { get; set; }
        public string filterinitialdata { get; set; }
        public Array reportdatelist { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public string fmg_groupname { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }


        public FeeArrearRegisterReportDTO[] savegrplst { get; set; }
        public FeeArrearRegisterReportDTO[] saveheadlst { get; set; }

        public long asmayidpss { get; set; }
        public string typeofrpt { get; set; }
        public DateTime? datepass { get; set; }
        public string fillbusroutestudents { get; set; }
        public long fillclasflg { get; set; }
        public long fillseccls { get; set; }

        //on 02 May 2017 by mahaboob
        public int userid { get; set; }
        public Array fillterms { get; set; }
        public long FMT_ID { get; set; }
        public Array duration { get; set; }
        public string enddate { get; set; }
        public string columnName { get; set; }
        public string columnID { get; set; }
        public string acadamicyear { get; set; }


        public long paid { get; set; }
        public long balance { get; set; }
        public long fti_id { get; set; }
        public Array studentdetails { get; set; }
        public Array studentlist { get; set; }
        public string fti_name { get; set; }
        public long concession { get; set; }
        public long excess_amount { get; set; }
        public decimal fine { get; set; }

        public long ASMCL_Id { get; set; }
        public long AMSC_Id { get; set; }
        public string asmc_sectionname { get; set; }
        public string month { get; set; }

        //added by kiran
        public string groupname { get; set; }
        public string groupid { get; set; }
        public long FMGG_Id { get; set; }
        public FeeArrearRegisterReportDTO[] TempararyArrayGroupList { get; set; }

        public string year { get; set; }
        public string date { get; set; }

        public string period { get; set; }
        public string reporttype { get; set; }
        public Array fillmastergroup { get; set; }
        public Array customlist { get; set; }

        public string customflag { get; set; }

        public string groupflag { get; set; }
        public string termflag { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }
        public string term_group { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public string FMG_GroupName {get; set;}
        public Array fillroute { get; set; }

        public long trmR_Id { get; set; }
        public int? ASMC_Order { get; set; }
        public string Admno { get; set; }
        public string ASMCL_Classname { get; set; }
    }
}
