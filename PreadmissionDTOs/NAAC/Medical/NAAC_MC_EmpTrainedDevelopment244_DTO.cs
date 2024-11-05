using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
    public class NAAC_MC_EmpTrainedDevelopment244_DTO
    {

        public long NCMCETD244_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCETD244_Year { get; set; }
        public long NCMCETD244_NoOfTechersTrainedForDevOfEcontents { get; set; }
        public long NCMCETD244_TotalNoOfTeachers { get; set; }
        public bool NCMCETD244_ActiveFlag { get; set; }
        public long NCMCETD244_CreatedBy { get; set; }
        public long NCMCETD244_UpdatedBy { get; set; }
        public DateTime NCMCETD244_CreatedDate { get; set; }
        public DateTime NCMCETD244_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public long ASMAY_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editlist { get; set; }
        public string msg { get; set; }
        public long NCMCETD244F_Id { get; set; }
        public NAAC_MC_EmpTrainedDevelopment244_DTO[] filelist { get; set; }

        public string Remarks { get; set; }
        public string UserName { get; set; }
        public long filefkid { get; set; }


        public long NCMCETD244C_Id { get; set; }
        public string NCMCETD244C_Remarks { get; set; }
        public long NCMCETD244C_RemarksBy { get; set; }
        public string NCMCETD244C_StatusFlg { get; set; }
        public bool? NCMCETD244C_ActiveFlag { get; set; }
        public long? NCMCETD244C_CreatedBy { get; set; }
        public DateTime? NCMCETD244C_CreatedDate { get; set; }
        public long? NCMCETD244C_UpdatedBy { get; set; }
        public DateTime? NCMCETD244C_UpdatedDate { get; set; }


        public long NCMCETD244FC_Id { get; set; }
        public string NCMCETD244FC_Remarks { get; set; }
        public long? NCMCETD244FC_RemarksBy { get; set; }
        public bool? NCMCETD244FC_ActiveFlag { get; set; }
        public long? NCMCETD244FC_CreatedBy { get; set; }
        public DateTime? NCMCETD244FC_CreatedDate { get; set; }
        public long? NCMCETD244FC_UpdatedBy { get; set; }
        public DateTime? NCMCETD244FC_UpdatedDate { get; set; }
        public string NCMCETD244FC_StatusFlg { get; set; }
        public string NCMCETD244_StatusFlg { get; set; }
        public string NCMCETD244F_StatusFlg { get; set; }
        public bool? NCMCETD244F_ApprovedFlg { get; set; }
        public string NCMCETD244_Remarks { get; set; }
        public bool? NCMCETD244_ApprovedFlg { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public long cfileid { get; set; }
        public bool? cfileactive { get; set; }

    }
}
