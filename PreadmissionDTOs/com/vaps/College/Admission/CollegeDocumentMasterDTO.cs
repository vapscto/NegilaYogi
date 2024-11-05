using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeDocumentMasterDTO
    {
        public long AMSMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMSMD_DocumentName { get; set; }
        public string AMSMD_Description { get; set; }
        public bool AMSMD_FLAG { get; set; }
        public string message { get; set; }
        public Array getdetails { get; set; }
        public Array getmappeddetails { get; set; }
        public Array selectedRowDetails { get; set; }
        public bool returnval { get; set; }
        public string[] images_paths { get; set; }
        public string images_name { get; set; }
        public bool? AMSMD_ToUploadFlg { get; set; }
        public string AMSMD_FileName { get; set; }
        public string AMSMD_FilePath { get; set; }
        public FilePath_Array1[] FilePath_Array { get; set; }

        // Second Tab
        public long ACQCD_Id { get; set; }      
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACQ_Id { get; set; }      
        public bool ACQCD_CompulsoryFlg { get; set; }
        public bool ACQCD_ActiveFlg { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }        
        public string ACQ_QuotaName { get; set; }
        public temp_branchDTO[] temp_branchDTO { get; set; }
        public temp_documentDTO[] temp_documentDTO { get; set; }
        public Array documentlist { get; set; }
        public Array quotalist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array doclist { get; set; }
        public Array getsavedbranchlist { get; set; }
    }
       public class FilePath_Array1
    {
        public string FileName { get; set; }
        public string AMSMD_FilePath { get; set; }
    }
    public class temp_branchDTO
    {
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class temp_documentDTO
    {
        public long AMSMD_Id { get; set; }
        public string AMSMD_DocumentName { get; set; }
    }
}
