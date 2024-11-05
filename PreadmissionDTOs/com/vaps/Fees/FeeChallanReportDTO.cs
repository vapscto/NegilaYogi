using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeChallanReportDTO
    {
        public TempDTO[] TempararyArrayListnew { get; set; }
        public FeeChallanReportDTO[] TempararyArrayhEADListnew { get; set; }
        public long MI_ID { get; set; }
        public Array admsudentslist { get; set; }
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
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }


        public FeeChallanReportDTO[] savegrplst { get; set; }
        public FeeChallanReportDTO[] saveheadlst { get; set; }

        public long asmayidpss { get; set; }
        public string typeofrpt { get; set; }
        public DateTime? datepass { get; set; }
        public string fillbusroutestudents { get; set; }
        public long fillclasflg { get; set; }
        public long fillseccls { get; set; }

        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public Array installmentList { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int userId { get; set; }
        public string studentName { get; set; }
        public FeeChallanReportDTO[] selectedGroup { get; set; }
        public FeeChallanReportDTO[] selectedTerm { get; set; }
        public long FMT_Id { get; set; }
        public Array bankDetails { get; set; }
        public long ASMCL_Id { get; set; }
        public string FatherName { get; set; }
        public string mobileNo { get; set; }
        public string className { get; set; }
        public string admNo { get; set; }
        public string receiptNo { get; set; }
        public long ASMS_Id { get; set; }
        public string sectionName { get; set; }
        public string logo { get; set; }
        public Array institution_det { get; set; }
        public Array student_det { get; set; }
        public Array feeConfiguration { get; set; }
        public Array termList { get; set; }
        public Array particularsList { get; set; }
        public string ischallangenerated { get; set; }
        public string returnval { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public long[] FMGG_Ids { get; set; }

        public string multiplegrpids { get; set; }
        public string multipletrmids { get; set; }
        public Array receiparraydelete { get; set; }
        public long FYP_Id { get; set; }
        public string config { get; set; }
        public Array disableterms { get; set; }
        public Array challanterms { get; set; }

        public string AMST_Sex { get; set; }

        public Array fee_opening_bal { get; set; }
        public string duration { get; set; }
        public string date { get; set; }

    }
}
