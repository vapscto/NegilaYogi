using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.Exam
{
    public class CategorySubjectMappingDTO:CommonParamDTO
    {
        public bool already_cnt { get; set; }
        public string returnMsg { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array getdetails { get; set; }
        public Array grouplist { get; set; }

        //yearly_category
        public int EYC_Id { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public bool EYC_ActiveFlg { get; set; }

        public DateTime? EYC_ExamStartDate { get; set; }
        public DateTime? EYC_ExamEndDate { get; set; }
        public DateTime? EYC_MarksEntryLastDate { get; set; }
        public DateTime? EYC_MarksProcessLastDate { get; set; }
        public DateTime? EYC_MarksPublishDate { get; set; }

        //yearly_category_group
        public int EYCG_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool EYCG_ActiveFlg { get; set; }
        //yearly_category_group_subs
        public int EYCGS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool EYCGS_ActiveFlg { get; set; }
        //student mapping subjects
        public int ESTSU_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool ESTSU_ElecetiveFlag { get; set; }
        public bool ESTSU_ActiveFlg { get; set; }
        public string EMCA_CategoryName { get; set; }
        public bool EMG_ElectiveFlg { get; set; }
        public string EMG_GroupName { get; set; }
        public string ASMAY_Year { get; set; }
        public Array subjectlist { get; set; }
        public IVRM_School_Master_SubjectsDTO[] subj_list { get; set; }
        public Array Grid_Details_list { get; set; }
        public Array view_group_subjects { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public Array edit_m_group { get; set; }
        public Array edit_m_group_subjects { get; set; }       
        public Exm_Master_Grade_DetailsDTO[] sub_list { get; set; } 
        public int ASMAY_Order { get; set; }
        public bool? EYC_BasedOnPaperTypeFlg { get; set; }

    }
}
