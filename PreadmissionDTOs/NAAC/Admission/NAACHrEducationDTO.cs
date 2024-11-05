using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACHrEducationDTO
    {
        public long NCAC522HRED_Id { get; set; }
        public long NCAC522HREDF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC522HRED_Year { get; set; }
        public string NCAC522HRED_HrEduEnrollStudentNo { get; set; }
        public long NCAC522HRED_GraduatedProgram { get; set; }
        public long NCAC522HRED_GraduatedDept { get; set; }
        public string NCAC522HRED_InstitutionName { get; set; }
        public string NCAC522HRED_AdmittedProgram { get; set; }
        public string NCAC522HRED_AdmittedDept { get; set; }
        public bool NCAC522HRED_ActiveFlg { get; set; }
        public long NCAC522HRED_CreatedBy { get; set; }
        public long NCAC522HRED_UpdatedBy { get; set; }
        public DateTime NCAC522HRED_CreatedDate { get; set; }
        public string NCAC522HRED_StatusFlg { get; set; }
        public DateTime NCAC522HRED_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array branchlist { get; set; }
        public Array courselist { get; set; }

        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }

    }
}
