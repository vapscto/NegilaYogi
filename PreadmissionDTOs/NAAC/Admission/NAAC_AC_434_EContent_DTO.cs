using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_434_EContent_DTO
    {

        public long NCAC434ECT_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string NCAC434ECT_DevFacilityName { get; set; }
        public string NCAC434ECT_LinkName { get; set; }
        public string NCAC434ECT_FileName { get; set; }
        public string NCAC434ECT_FilePath { get; set; }
        public Nullable<bool> NCAC434ECT_ActiveFlg { get; set; }
        public Nullable<long> NCAC434ECT_CreatedBy { get; set; }
        public Nullable<long> NCAC434ECT_UpdatedBy { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array allgridlist { get; set; }
        public Array editlist { get; set; }

        public long NCAC434ECTF_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public NAAC_AC_434_EContent_DTO[] filelist { get; set; }
        public Array institutionlist { get; set; }

        public long NCAC434ECTC_Id { get; set; }
        public string NCAC434ECTC_Remarks { get; set; }
        public long? NCAC434ECTC_RemarksBy { get; set; }
        public string NCAC434ECTC_StatusFlg { get; set; }
        public bool? NCAC434ECTC_ActiveFlag { get; set; }
        public long? NCAC434ECTC_CreatedBy { get; set; }
        public DateTime? NCAC434ECTC_CreatedDate { get; set; }
        public long? NCAC434ECTC_UpdatedBy { get; set; }
        public DateTime? NCAC434ECTC_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public long NCAC434ECTFC_Id { get; set; }
        public string NCAC434ECTFC_Remarks { get; set; }
        public long? NCAC434ECTFC_RemarksBy { get; set; }
        public bool? NCAC434ECTFC_ActiveFlag { get; set; }
        public long? NCAC434ECTFC_CreatedBy { get; set; }
        public DateTime? NCAC434ECTFC_CreatedDate { get; set; }
        public long? NCAC434ECTFC_UpdatedBy { get; set; }
        public DateTime? NCAC434ECTFC_UpdatedDate { get; set; }
        public string NCAC434ECTFC_StatusFlg { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }

        public string NCAC434ECT_StatusFlg { get; set; }
        public string NCAC434ECTF_StatusFlg { get; set; }
        public bool? NCAC434ECTF_ActiveFlg { get; set; }

    }
}
