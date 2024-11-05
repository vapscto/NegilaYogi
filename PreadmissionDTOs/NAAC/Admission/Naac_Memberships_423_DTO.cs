using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class Naac_Memberships_423_DTO
    {
        public long NCAC423MEM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC423MEM_Membership { get; set; }
        public string NCAC423MEM_Subscription { get; set; }
        public Nullable<long> NCAC423MEM_NoOfEResources { get; set; }
        public Nullable<long> NCAC423MEM_ValidityPeriod { get; set; }
        public string NCAC423MEM_UsageReport { get; set; }
        public Nullable<bool> NCAC423MEM_RemoteAccessFlg { get; set; }
        public string NCAC423MEM_FileName { get; set; }
        public string NCAC423MEM_FilePath { get; set; }
        public Nullable<bool> NCAC423MEM_ActiveFlg { get; set; }
        public Nullable<long> NCAC423MEM_CreatedBy { get; set; }
        public Nullable<long> NCAC423MEM_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long UserId { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public long NCAC423MEM_Year { get; set; }
        public string NCAC423MEM_Fulltextaccess { get; set; }
        public string NCAC423MEM_WeblinkOfRemoteAccess { get; set; }
        public long NCAC423MEMF_Id { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Naac_Memberships_423_DTO[] filelist { get; set; }
        public string NCAC423MEM_StatusFlg { get; set; }
        public long NCAC423MEMC_Id { get; set; }
        public string NCAC423MEMC_Remarks { get; set; }
        public long? NCAC423MEMC_RemarksBy { get; set; }
        public string NCAC423MEMC_StatusFlg { get; set; }
        public bool? NCAC423MEMC_ActiveFlag { get; set; }
        public long? NCAC423MEMC_CreatedBy { get; set; }
        public DateTime? NCAC423MEMC_CreatedDate { get; set; }
        public long? NCAC423MEMC_UpdatedBy { get; set; }
        public DateTime? NCAC423MEMC_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCAC423MEMF_StatusFlg { get; set; }

        public long NCAC423MEMFC_Id { get; set; }
        public string NCAC423MEMFC_Remarks { get; set; }
        public long? NCAC423MEMFC_RemarksBy { get; set; }
        public bool? NCAC423MEMFC_ActiveFlag { get; set; }
        public long? NCAC423MEMFC_CreatedBy { get; set; }
        public DateTime? NCAC423MEMFC_CreatedDate { get; set; }
        public long? NCAC423MEMFC_UpdatedBy { get; set; }
        public DateTime? NCAC423MEMFC_UpdatedDate { get; set; }
        public string NCAC423MEMFC_StatusFlg { get; set; }
      

    }
}
