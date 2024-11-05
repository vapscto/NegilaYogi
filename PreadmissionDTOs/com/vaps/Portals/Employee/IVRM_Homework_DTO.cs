using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class IVRM_Homework_DTO : CommonParamDTO
    {
        public long IHW_Id { get; set; }
        public long IHWUPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public decimal Marks { get; set; }
        public long IHW_AssignmentNo { get; set; }
        public long ISMS_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? IHW_Date { get; set; }
        public string IHW_Topic { get; set; }
        public string IHWATT_Attachment { get; set; }
        public string IHWUPL_FileName { get; set; }
        public string IHW_Assignment { get; set; }
        public string IHWUPL_Attachment { get; set; }
        public string IHW_Attachment { get; set; }
        public string IHWATT_FileName { get; set; }
        public string IHW_FilePath { get; set; }
        public bool IHW_ActiveFlag { get; set; }
        public long Login_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool returnval { get; set; }
        public string returnsavestatus { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public string Role_flag { get; set; }
        public string roleflg { get; set; }
        public long AMST_MobileNo { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array roletype { get; set; }
        public Array academicyear { get; set; }
        public Array configflag { get; set; }
        public Array home_class_work_reports { get; set; }
        public Array getsubject_list { get; set; }
        public Array yearlist { get; set; }
        public Array attachementlist { get; set; }
        public Array classlist { get; set; }
        public Array class_gridlist { get; set; }
        public Array marksupdate_list { get; set; }
        public Array sectionlist { get; set; }
        public Array classwork { get; set; }
        public Array subjectlist { get; set; }
        public Array get_studentlist { get; set; }
        public Array viewhomework { get; set; }
        public Array viewstudentupload { get; set; }     
        public Array gethomework_list { get; set; }
        public Array edit_mark_list { get; set; }
        public Array editstudent { get; set; }
        public bool dulicate { get; set; }
        public Array studentlist1 { get; set; }
        public string ASMC_SectionName { get; set; }
        public string upload_flg { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string sound { get; set; }
        public string icon { get; set; }
        public string iconColor { get; set; }
        public Array editlist { get; set; }
        public Array editlist_section { get; set; }
        public Array deviceArray { get; set; }
        // public Array registration_ids { get; set; }
        public clsarrayDTO[] clsarray { get; set; }
        public secarrayDTO[] secarray { get; set; }
        public notificationDTO notification { get; set; }
        public string[] registration_ids { get; set; }
        public  hm_section_list1[] hm_section_list { get; set; }
        public hm_subject_list1[] hm_subject_list { get; set; }

        
        public IHW_FilePath_Array1[] IHW_FilePath_Array { get; set; }
        public studentarray1[] studentarray { get; set; }
        public gethomework_list_array1[] gethomework_list_array { get; set; }
        public doclist1[] doclist { get; set; }
        public string IHWUPLATT_StaffUpload { get; set; }
        public string IHWUPLATT_StaffRemarks { get; set; }
        public long IHWUPLATT_Id { get; set; }
        public Array TopicList { get; set; }

        //added by roopa//
        public Array devicelist { get; set; }
        //
        public class IHW_FilePath_Array1
        {
            public string IHW_FilePath { get; set; }
            public string FileName { get; set; }
        }
        public class clsarrayDTO
        {
            public long ASMCL_Id { get; set; }
        }
        public class secarrayDTO
        {
           
            public long ASMS_Id { get; set; }
        }
        public class notificationDTO
        {
            public string title { get; set; }
            public string message { get; set; }
            public string sound { get; set; }
            public string icon { get; set; }
            public string iconColor { get; set; }

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
        public Array classwork12 { get; set; }
        public List<IVRM_Homework_DTO> devicelist12 { get; set; }
        public List<IVRM_Homework_DTO> classwork1 { get; set; }

        public class studentarray1
        {
            public long AMST_Id1 { get; set; }
        }
          public class gethomework_list_array1
        {
            public long AMST_Id { get; set; }
            public long IHW_Id { get; set; }
            public decimal Marks { get; set; }
            public long IHWUPL_Id { get; set; }
            public string IHWUPL_StaffUpload { get; set; }
            public string IHWUPL_FileName { get; set; }
            public string IHWUPL_StaffRemarks { get; set; }
            public doclist_temp[] doclist_temp { get; set; }
        }
        public class doclist1
        {
            public string FilePath1 { get; set; }
            public string FileName1 { get; set; }
            public long IHWUPL_Id { get; set; }
            public string Remarks { get; set; }
        }
        public class doclist_temp
        {
            public string FilePath1 { get; set; }
            public string FileName1 { get; set; }
            public long IHWUPL_Id { get; set; }
            public long IHWUPLATT_Id { get; set; }
            public string Remarks { get; set; }
        }

    }
}
