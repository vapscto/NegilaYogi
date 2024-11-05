using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class Naac_ICT_DTO
    {
        public long NCAC413ICT_Id { get; set; }
        public long NCAC413ICTF_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string NCAC413ICT_RoomNo { get; set; }
        public string NCAC413ICT_ICTFacility { get; set; }
        public string NCAC413ICT_FileName { get; set; }
        public string NCAC413ICT_FilePath { get; set; }
        public Nullable<bool> NCAC413ICT_ActiveFlg { get; set; }
        public Nullable<long> NCAC413ICT_CreatedBy { get; set; }
        public Nullable<long> NCAC413ICT_UpdatedBy { get; set; }
        public Naac_ICT_DTO[] filelist { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public long cfileid { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array allgridlist { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
       
        public Array viewuploadflies { get; set; }
        public Array institutionlist { get; set; }



        public Array commentlist { get; set; }
        public long NCAC413ICTC_Id { get; set; }
        public string NCAC413ICTC_Remarks { get; set; }
        public long? NCAC413ICTC_RemarksBy { get; set; }
        public string NCAC413ICTC_StatusFlg { get; set; }
        public bool? NCAC413ICTC_ActiveFlag { get; set; }
        public DateTime? NCAC413ICTC_CreatedDate { get; set; }
        public long? NCAC413ICTC_CreatedBy { get; set; }
        public long? NCAC413ICTC_UpdatedBy { get; set; }
        public DateTime? NCAC413ICTC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public long NCAC413ICTFC_Id { get; set; }
        public string NCAC413ICTFC_Remarks { get; set; }
        public long? NCAC413ICTFC_RemarksBy { get; set; }
        public bool? NCAC413ICTFC_ActiveFlag { get; set; }
        public long? NCAC413ICTFC_CreatedBy { get; set; }
        public DateTime? NCAC413ICTFC_CreatedDate { get; set; }
        public long? NCAC413ICTFC_UpdatedBy { get; set; }
        public DateTime? NCAC413ICTFC_UpdatedDate { get; set; }
        public string NCAC413ICTFC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
       public string NCAC413ICTF_StatusFlg { get; set; }



    }
}
