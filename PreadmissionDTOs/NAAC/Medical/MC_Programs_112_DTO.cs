using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
    public class MC_Programs_112_DTO
    {

        public long NCMCMPR112_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCMPR112_Year { get; set; }
        public long NCMCMPR112_NoOfTeachersPartBos { get; set; }
        public long NCMCMPR112_NoOfTeachersAcu { get; set; }
        public DateTime NCMCMPR112_CreatedDate { get; set; }
        public DateTime NCMCMPR112_UpdatedDate { get; set; }
        public long NCMCMPR112_CreatedBy { get; set; }
        public long NCMCMPR112_UpdatedBy { get; set; }
        public bool NCMCMPR112_ActiveFlag { get; set; }

        public long NCMCMPR112D_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCMCMPR112D_PrgType { get; set; }
        public long NCMCMPR112D_CreatedBy { get; set; }
        public long NCMCMPR112D_UpdatedBy { get; set; }
        public DateTime NCMCMPR112D_CreatedDate { get; set; }
        public DateTime NCMCMPR112D_UpdatedDate { get; set; }

        public long NCMCMPR112DF_Id { get; set; }
        public string NCMCMPR112DF_FileDesc { get; set; }
        public string NCMCMPR112DF_FileName { get; set; }
        public string NCMCMPR112DF_FilePath { get; set; }
        public bool NCMCMPR112DF_ActiveFlg { get; set; }
        public long NCMCMPR112DF_CreatedBy { get; set; }
        public long NCMCMPR112DF_UpdatedBy { get; set; }
        public DateTime NCMCMPR112DF_CreatedDate { get; set; }
        public DateTime NCMCMPR112DF_UpdatedDate { get; set; }

        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string empname { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public int? emporder { get; set; }
        public string empnamecouncil { get; set; }
        public string empcode { get; set; }
        public long empid { get; set; }
        public string flag_BOS { get; set; }
        public string flag_COUN { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }

        public string ASMAY_Year { get; set; }
        public int countOfBOS { get; set; }
        public int countOfCouncil { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array emplylist_1 { get; set; }
        public Array emplylist_2 { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array alldata { get; set; }
        public Array staflist_boos { get; set; }
        public Array staflist_council { get; set; }
        public Array editdata { get; set; }
        public Array editdataBOS { get; set; }
        public Array editdatacouncil { get; set; }    
        public staffBosList[] staffBosList { get; set; }
        public staffCouncilList[] staffCouncilList { get; set; }
        public Naac_CommonFiles_DTO[] filelist { get; set; }

    }
    public class staffBosList
    {
        public long HRME_Id { get; set; }
    }
    public class staffCouncilList
    {
        public long empid { get; set; }
    }
}
