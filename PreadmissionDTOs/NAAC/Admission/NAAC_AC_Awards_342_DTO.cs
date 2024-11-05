using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_Awards_342_DTO
    {
        public long NCACAW342_Id { get; set; }
        public long MI_Id { get; set; }
        public long pkid { get; set; }
        public string ids { get; set; }
        public string NCACAW342_ActivityName { get; set; }
        public string NCACAW342_AwardName { get; set; }
        public string NCACAW342_AwardingBody { get; set; }
        public string NCACAW342_AgencyName { get; set; }
        public string NCACAW342_CategoryName { get; set; }
        public long NCACAW342_AwardYear { get; set; }
        public string NCACAW342_FileName { get; set; }
        public string NCACAW342_FilePath { get; set; }
        public bool NCACAW342_ActiveFlg { get; set; }
        public long NCACAW342_CreatedBy { get; set; }
        public long NCACAW342_UpdatedBy { get; set; }
        public DateTime NCACAW342_CreatedDate { get; set; }
        public DateTime NCACAW342_UpdatedDate { get; set; }
        public Array list { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public long UserId {get;set;}
        public long ASMAY_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public NAAC_AC_Awards_342_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCACAW342F_Id { get; set; }
        public Array institutionlist { get; set; }
        public long NCACAW342C_Id { get; set; }
        public string NCACAW342C_Remarks { get; set; }
        public long? NCACAW342C_RemarksBy { get; set; }
        public string NCACAW342C_StatusFlg { get; set; }
        public bool? NCACAW342C_ActiveFlag { get; set; }
        public long? NCACAW342C_CreatedBy { get; set; }
        public DateTime? NCACAW342C_CreatedDate { get; set; }
        public long? NCACAW342C_UpdatedBy { get; set; }
        public DateTime? NCACAW342C_UpdatedDate { get; set; }
        public long NCACAW342FC_Id { get; set; }
        public string NCACAW342FC_Remarks { get; set; }
        public long? NCACAW342FC_RemarksBy { get; set; }
        public bool? NCACAW342FC_ActiveFlag { get; set; }
        public long? NCACAW342FC_CreatedBy { get; set; }
        public DateTime? NCACAW342FC_CreatedDate { get; set; }
        public long? NCACAW342FC_UpdatedBy { get; set; }
        public DateTime? NCACAW342FC_UpdatedDate { get; set; }
        public string NCACAW342FC_StatusFlg { get; set; }
        public string NCACAW342_StatusFlg { get; set; }
        public string NCACAW342F_StatusFlg { get; set; }
        public bool? NCACAW342F_ActiveFlg { get; set; }

        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public Array commentlist { get; set; }
    }
}
