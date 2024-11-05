using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
  public  class NAAC_AC_331_DTO
    {
        public long NCAC331_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC331_EthicsURL { get; set; }
        public bool NCAC331_PDSFlg { get; set; }
        public string NCAC331_PDMecanism { get; set; }
        public string smsflag { get; set; }
        public string NCAC331F_FileName { get; set; }
        public string NCAC331F_FilePath { get; set; }
        public string NCAC331F_Filedesc { get; set; }
        public bool pds_flag { get; set; }
        public string msg { get; set; }
        public bool dat { get; set; }
        public string pds_chk { get; set; }
        public bool NCAC331_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public long NCAC331_CreatedBy { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array alldata1 { get; set; }
        public NAAC_AC_331_DTO[] alldata2 { get; set; }
        public long UserId { get; set; }
        public long NCAC331_UpdatedBy { get; set; }
        public DateTime NCAC331_CreatedDate { get; set; }
        public DateTime NCAC331_UpdatedDate { get; set; }
        public NAAC_AC_331_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCAC331F_Id { get; set; }
        public Array institutionlist { get; set; }

        public long NCAC331C_Id { get; set; }
        public string NCAC331C_Remarks { get; set; }
        public long? NCAC331C_RemarksBy { get; set; }
        public string NCAC331C_StatusFlg { get; set; }
        public bool? NCAC331C_ActiveFlag { get; set; }
        public long? NCAC331C_CreatedBy { get; set; }
        public DateTime? NCAC331C_CreatedDate { get; set; }
        public long? NCAC331C_UpdatedBy { get; set; }
        public DateTime? NCAC331C_UpdatedDate { get; set; }
        public long NCAC331FC_Id { get; set; }
        public string NCAC331FC_Remarks { get; set; }
        public long? NCAC331FC_RemarksBy { get; set; }
        public bool? NCAC331FC_ActiveFlag { get; set; }
        public long? NCAC331FC_CreatedBy { get; set; }
        public DateTime? NCAC331FC_CreatedDate { get; set; }
        public long? NCAC331FC_UpdatedBy { get; set; }
        public DateTime? NCAC331FC_UpdatedDate { get; set; }
        public string NCAC331FC_StatusFlg { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC331F_StatusFlg { get; set; }
        public bool? NCAC331F_ActiveFlg { get; set; }
        public string NCAC331_StatusFlg { get; set; }

    }
}
