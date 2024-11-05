using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Form16DTO
    {
        public long MI_Id { get; set; }

        public long roleId { get; set; }

        public Array employeedropdown { get; set; }

        public Array masterloandropdown { get; set; }
        public Array groupTypedropdown { get; set; }
        public Array monthdropdown { get; set; }
        public long HRME_Id { get; set; }
        public InstitutionDTO institutionDetails { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public string hrmE_EmployeeFirstName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public long LogInUserId { get; set; }
        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public long IMFY_Id { get; set; }
        public Array leaveyeardropdown { get; set; }

        public Array quarterheads { get; set; }

        public Array tdsheads { get; set; }

        public string HRMAL_AllowanceName { get; set; }

        public decimal? HREAL_Allowance { get; set; }

        public Array allowancelist { get; set; }

        public Array masterallowance { get; set; }

        public Array otherincomelist { get; set; }

        public Array chapterlist { get; set; }

        public Array change { get; set; }
        public decimal? HRECVIA_Amount { get;set; }
        public string HRMCVIA_SectionName { get; set; }
        public string HRMOI_OtherIncomeName { get; set; }
        public decimal? HREOI_OtherIncome { get; set; }

        public decimal? empGrossSal { get; set; }

        public decimal? HRESD_Amount { get; set; }


        public decimal? professionaltaxamount { get; set; }
        public string HRMQ_QuarterName { get; set; }
        public Array chapterlist80E { get; set; }
        public Array chapterlist80EE { get; set; }
        public Array chapterlist80G { get; set; }
        public Array chapterlist80GG { get; set; }

        public Array chapterlist80DD { get; set; }
        public Array chapterlist80D { get; set; }
        public Array chapterlist80DDB { get; set; }

        public Array chapterlist80U { get; set; }

        public DateTime IMFY_FromDate { get; set; }

        public DateTime IMFY_ToDate { get; set; }

        public string HRETDSR_ReceiptNo { get; set; }

        public Array receit { get; set; }
        public DateTime? HRMQ_FromDay { get; set; }

        public DateTime? HRMQ_ToDay { get; set; }

        public decimal? pfvalue { get; set; }

        public decimal? licvalue { get; set; }
        public string HRMCIA_ROMANORDER { get; set; }
        public decimal? HRC_EducationCess { get; set; }
        public long HRMCVIA_ORDER { get; set; }
        public Array masterch { get; set; }

        public Array calculation { get; set; }

        public string incomeTaxList { get; set; }
        public string HRMIT_AgeFlag { get; set; }

        public long HRMIT_GenderFlag { get; set; }
        public long HRMIT_Id { get; set; }
       
        public Int32? HRMIT_FromAge { get; set; }
        public Int32? HRMIT_ToAge { get; set; }

        public string IVRMMG_GenderName { get; set; }

        public decimal? HRMITD_AmountFrom { get; set; }
        public decimal? HRMITD_AmountTo { get; set; }
        public decimal? HRMITD_IncomeTax { get; set; }

        public long IVRMMG_Id { get; set; }

        public int HRME_DOB { get; set; }
        public int birthyear { get; set; }

    }
}
