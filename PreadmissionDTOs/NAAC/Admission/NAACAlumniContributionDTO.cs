using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACAlumniContributionDTO
    {
        public long NCAC542ALMCON_Id { get; set; }
        public long NCAC542ALMCONF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC542ALMCON_ContributionYear { get; set; }
        public string NCAC542ALMCON_AlumnsName { get; set; }
        public long NCAC542ALMCON_GraduationYear { get; set; }
        public string NCAC542ALMCON_AadharPAN { get; set; }
        public string NCAC542ALMCON_StatusFlg { get; set; }
        public decimal NCAC542ALMCON_ContriAmount { get; set; }
        public bool NCAC542ALMCON_ActiveFlg { get; set; }
        public long NCAC542ALMCON_CreatedBy { get; set; }
        public long NCAC542ALMCON_UpdatedBy { get; set; }
        public DateTime NCAC542ALMCON_CreatedDate { get; set; }
        public DateTime NCAC542ALMCON_UpdatedDate { get; set; }
        public string NCAC542ALMCON_AreaOfContribution { get; set; }
        public long UserId { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMAY_Year1 { get; set; }
        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editfiles { get; set; }
        public bool NCAC531SPCAS_FinancialORKindFlag { get; set; }
        public bool NCAC531SPCAS_DonationOfBooksFlag { get; set; }
        public bool NCAC531SPCAS_StudentsplacementFlag { get; set; }
        public bool NCAC531SPCAS_StudentexchangesFlag { get; set; }
        public bool NCAC531SPCAS_InstendowmentsFlag { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
    }
}
