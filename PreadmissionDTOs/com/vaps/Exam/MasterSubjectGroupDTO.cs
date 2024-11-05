using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MasterSubjectGroupDTO:CommonParamDTO
    {
        public bool already_cnt { get; set; }
        //master group
        public int EMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMG_TotSubjects { get; set; }
        public int EMG_MaxAplSubjects { get; set; }
        public int EMG_MinAplSubjects { get; set; }
        public int EMG_BestOff { get; set; }
        public bool EMG_ActiveFlag { get; set; }
        public bool EMG_ElectiveFlg { get; set; }

        //master group subjects
        public int EMGS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool EMGS_ActiveFlag { get; set; }
        public Array subjectlist { get; set; }
        public IVRM_School_Master_SubjectsDTO[] subj_list { get; set; }
        public Array Group_list { get; set; }
        public Array Grade_Details_list { get; set; }
        public Array view_group_subjects { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public Array edit_m_group { get; set; }
        public Array edit_m_group_subjects { get; set; }
        public Exm_Master_Grade_DetailsDTO[] sub_list { get; set; }
      
      
    
        public string returnMsg { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }



    }
}
