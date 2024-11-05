using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class Naac_MOU_DTO
    {
        public long NCAC352MOU_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string NCAC352MOU_OrganisationName { get; set; }
        public string NCAC352MOU_Name { get; set; }
        public long NCAC352MOU_SigningYear { get; set; }
        public string NCAC352MOU_Duration { get; set; }
        public string NCAC352MOU_ActivitiesList { get; set; }
        public long NCAC352MOU_NoOfStudents { get; set; }
        public long NCAC352MOU_NoOfStaff { get; set; }
        public string NCAC352MOU_LinkOfDocument { get; set; }
        public string NCAC352MOUF_FileName { get; set; }
        public string NCAC352MOUF_Filedesc { get; set; }
        public string NCAC352MOUF_FilePath { get; set; }
        public bool NCAC352MOU_ActiveFlg { get; set; }
        public long NCAC352MOU_CreatedBy { get; set; }
        public long NCAC352MOU_UpdatedBy { get; set; }
        public DateTime? NCAC352MOU_CreatedDate { get; set; }
        public DateTime? NCAC352MOU_UpdatedDate { get; set; }
        public string ASMAY_Year { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCAC352MOUF_Id { get; set; }
        public Array editFileslist { get; set; }
        public Naac_MOU_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public Array institutionlist { get; set; }
        public long NCAC352MOUC_Id { get; set; }
        public string NCAC352MOUC_Remarks { get; set; }
        public long? NCAC352MOUC_RemarksBy { get; set; }
        public string NCAC352MOUC_StatusFlg { get; set; }
        public bool? NCAC352MOUC_ActiveFlag { get; set; }
        public long? NCAC352MOUC_CreatedBy { get; set; }
        public DateTime? NCAC352MOUC_CreatedDate { get; set; }
        public long? NCAC352MOUC_UpdatedBy { get; set; }
        public DateTime? NCAC352MOUC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public Array commentlist { get; set; }
        public long NCAC352MOUFC_Id { get; set; }
        public string NCAC352MOUFC_Remarks { get; set; }
        public long? NCAC352MOUFC_RemarksBy { get; set; }
        public bool? NCAC352MOUFC_ActiveFlag { get; set; }
        public long? NCAC352MOUFC_CreatedBy { get; set; }
        public DateTime? NCAC352MOUFC_CreatedDate { get; set; }
        public long? NCAC352MOUFC_UpdatedBy { get; set; }
        public DateTime? NCAC352MOUFC_UpdatedDate { get; set; }
        public string NCAC352MOUFC_StatusFlg { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC352MOU_StatusFlg { get; set; }
        public string NCAC352MOUF_StatusFlg { get; set; }
        public bool NCAC352MOUF_ActiveFlg { get; set; }
    }
}
