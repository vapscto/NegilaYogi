using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class Exm_Col_CourseBranchDTO
    {
        public long ECYS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public bool ECYS_ActiveFlag { get; set; }
        public long ECYSG_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool ECYSG_ActiveFlag { get; set; }
        public long ECYSGS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECYSGS_ActiveFlg { get; set; }



        public Array courseslist { get; set; }
        public Array subjectshemalist { get; set; }
        public Array subjectgrplist { get; set; }
        public Array branchlist { get; set; }
        public Array schmetypelist { get; set; }
        public Array semisters { get; set; }
        public Exm_Col_CourseBranchDTO[] selected_semis { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string ISMS_SubjectName { get; set; }
        public Array subjectgroups { get; set; }
        public Array examlist { get; set; }
        public Array editlist { get; set; }
        public Array editlist2 { get; set; }
        public Exm_Col_CourseBranchDTO[] selected_subgrps { get; set; }
        public Array alllist { get; set; }
        public Array viewrecords { get; set; }
        public string ACSS_SchmeName { get; set; }
        public string ACST_SchmeType { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array edit_branchcourse { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public string subgroupname { get; set; }
    }
}
