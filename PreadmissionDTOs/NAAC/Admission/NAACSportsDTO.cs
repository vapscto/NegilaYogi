using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACSportsDTO
    {
        public long NCAC531SPCA_Id { get; set; }
        public long NCAC531SPCAF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC531SPCA_Year { get; set; }
        public long NCAC531SPCA_NoOfStudents { get; set; }
        public bool NCAC531SPCA_ActiveFlg { get; set; }
        public long NCAC531SPCA_CreatedBy { get; set; }
        public long NCAC531SPCA_UpdatedBy { get; set; }
        public DateTime NCAC531SPCA_CreatedDate { get; set; }
        public DateTime NCAC531SPCA_UpdatedDate { get; set; }
        public long NCAC531SPCAS_Id { get; set; }
    
        public long AMCST_Id { get; set; }
        public string NCAC531SPCAS_AwardName { get; set; }
        public string NCAC531SPCAS_NatOrInterNatFlg { get; set; }
        public string NCAC531SPCAS_SportsCAIEEEFlg { get; set; }
        public bool NCAC531SPCAS_ActiveFlg { get; set; }
        public long NCAC531SPCAS_CreatedBy { get; set; }
        public long NCAC531SPCAS_UpdatedBy { get; set; }
        public DateTime NCAC531SPCAS_CreatedDate { get; set; }
        public DateTime NCAC531SPCAS_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string NCAC531SPCAS_StatusFlg { get; set; }
        public int ACMS_Order { get; set; }

        public string ASMAY_Year { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string ACMS_SectionName { get; set; }

        public Array alldatatab2 { get; set; }
        public Array institutionlist { get; set; }
        public Array examlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }

    }
}
