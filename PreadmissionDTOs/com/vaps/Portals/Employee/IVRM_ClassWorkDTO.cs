using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee


{

    public class IVRM_ClassWorkDTO
    {
        public long ICW_Id { get; set; }
        public long ICWUPL_Id { get; set; }
        public long ICWUPLATT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ICW_Topic { get; set; }
        public string ICW_SubTopic { get; set; }
        public string ICW_Content { get; set; }
        public long ICW_TeachingAid { get; set; }
        public bool ICW_ActiveFlag { get; set; }
        public DateTime ICW_FromDate { get; set; }
        public DateTime ICW_ToDate { get; set; }
        public string ICW_Evaluation { get; set; }
        public string ICW_Attachment { get; set; }
        public string ICWUPL_Attachment { get; set; }
        public string ICWUPL_FileName { get; set; }
        public string ICWATT_Attachment { get; set; }
        public string ICWATT_FileName { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string Role_flag { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long Login_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public string ICW_Assignment { get; set; }
        public string Attachment { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
        public string finalDate { get; set; }
        public string devicenew { get; set; }
        public string roleflg { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

        public Array classlist { get; set; }
        public Array roletype { get; set; }
        public Array class_gridlist { get; set; }
        public Array marksupdate_list { get; set; }
        public Array viewstudentupload { get; set; }
        public Array academicyear { get; set; }
        public Array attachementlist { get; set; }
        public Array studentlist1 { get; set; }
        public Array viewhomework { get; set; }

        public Array result_check { get; set; }
        public Array sectionlist { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public Array subjectlist { get; set; }
        public string ISMS_SubjectName { get; set; }
        public IVRM_ClassWorkDTO[] images_list { get; set; }
        public studentarray1[] studentarray { get; set; }
        public string returnsavestatus { get; set; }
        public bool returnval { get; set; }
        public string imagePath { get; set; }

        public long AMST_Id { get; set; }
        public Array worklist { get; set; }
        public Array classSectionlist { get; set; }
        public Array yearlist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array classwork { get; set; }
        public Array getclasswork_list { get; set; }
        public Array getsubject_list { get; set; }
        public getclasswork_list_array1[] getclasswork_list_array { get; set; }

        public string ICW_FilePath { get; set; }
        public bool dulicate { get; set; }
        public Array editlist { get; set; }
        public Array editlist_section { get; set; }
        public Array deviceArray { get; set; }
        public Array edit_mark_list { get; set; }
        public Array editstudent { get; set; }
        public int ASMCL_Order { get; set; }
        public long ISMS_order { get; set; }
        public hm_section_list1[] hm_section_list { get; set; }
        public hm_subject_list1[] hm_subject_list { get; set; }
        public ICW_FilePath_Array1[] ICW_FilePath_Array { get; set; }
        public doclist1[] doclist { get; set; }
        public string ICWUPLATT_StaffUpload { get; set; }
        public string ICWUPLATT_StaffRemarks { get; set; }
        public classarray1[] classarray { get; set; }
        public string flag { get; set; }
       public Array reportlist { get; set; }
        public Array view_array { get; set; }
        public Array TopicList { get; set; }
        public class classarray1
        {
            public long ASMCL_Id { get; set; }
          
        }
        public class hm_section_list1
        {
            public long ASMS_Id { get; set; }
            public string ASMC_SectionName { get; set; }
        }
        public class hm_subject_list1
        {
            public long ISMS_Id { get; set; }
        }
        public class ICW_FilePath_Array1
        {
          
            public string ICW_FilePath { get; set; }
            public string FileName { get; set; }
        }
        public Array classwork12 { get; set; }
        public List<IVRM_ClassWorkDTO> devicelist12 { get; set; }
        public List<IVRM_ClassWorkDTO> classwork1 { get; set; }

        public class studentarray1
        {
            public long AMST_Id1 { get; set; }
        }
        public class getclasswork_list_array1
        {
            public long AMST_Id { get; set; }
            public long ICW_Id { get; set; }
            public long ICWUPL_Id { get; set; }
            public decimal Marks { get; set; }
            public string ICWUPL_FileName { get; set; }
            public string ICWUPL_StaffUplaod { get; set; }
            public string ICWUPL_StaffRemarks { get; set; }
            public doclist_temp[] doclist_temp { get; set; }
        }
        public class doclist1
        {
            public string FilePath1 { get; set; }
            public string FileName1 { get; set; }
            public long ICWUPL_Id { get; set; }
            public string Remarks { get; set; }
        }

        public class doclist_temp
        {
            public string FilePath1 { get; set; }
            public string FileName1 { get; set; }
            public long ICWUPL_Id { get; set; }
            public long ICWUPLATT_Id { get; set; }
            public string Remarks { get; set; }
        }
    }
}



