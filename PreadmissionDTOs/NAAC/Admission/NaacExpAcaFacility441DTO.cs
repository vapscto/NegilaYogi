using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NaacExpAcaFacility441DTO
    {
        public long NCAC441ExAcFc_Id { get; set; }
        public long MI_Id { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpAccFacility { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpPhyFacility { get; set; }
        public Nullable<long> NCAC441EXACFC_Year { get; set; }
        public string NCAC441EXACFC_FileName { get; set; }
        public string NCAC441EXACFC_FilePath { get; set; }
        public Nullable<bool> NCAC441EXACFC_ActiveFlg { get; set; }
        public Nullable<long> NCAC441EXACFC_CreatedBy { get; set; }
        public Nullable<long> NCAC441EXACFC_UpdatedBy { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public DateTime NCAC441EXACFC_CreatedDate { get; set; }
        public DateTime NCAC441EXACFC_UpdatedDate { get; set; }

        public long NCAC441ExAcFcF_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public NaacExpAcaFacility441DTO[] filelist { get; set; }
        public Array institutionlist { get; set; }
        public string NCAC441ExAcFc_StatusFlg { get; set; }
        public string NCAC441ExAcFcF_StatusFlg { get; set; }
        public bool? NCAC441ExAcFcF_ActiveFlg { get; set; }
        public long NCAC441EXACFCC_Id { get; set; }
        public string NCAC441EXACFCC_Remarks { get; set; }
        public long? NCAC441EXACFCC_RemarksBy { get; set; }
        public string NCAC441EXACFCC_StatusFlg { get; set; }
        public bool? NCAC441EXACFCC_ActiveFlag { get; set; }
        public long? NCAC441EXACFCC_CreatedBy { get; set; }
        public DateTime? NCAC441EXACFCC_CreatedDate { get; set; }
        public long? NCAC441EXACFCC_UpdatedBy { get; set; }
        public DateTime? NCAC441EXACFCC_UpdatedDate { get; set; }
        public long NCAC441EXACFCFC_Id { get; set; }
        public string NCAC441EXACFCFC_Remarks { get; set; }
        public long? NCAC441EXACFCFC_RemarksBy { get; set; }
        public bool? NCAC441EXACFCFC_ActiveFlag { get; set; }
        public long? NCAC441EXACFCFC_CreatedBy { get; set; }
        public DateTime? NCAC441EXACFCFC_CreatedDate { get; set; }
        public long? NCAC441EXACFCFC_UpdatedBy { get; set; }
        public DateTime? NCAC441EXACFCFC_UpdatedDate { get; set; }
        public string NCAC441EXACFCFC_StatusFlg { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
    }
}
