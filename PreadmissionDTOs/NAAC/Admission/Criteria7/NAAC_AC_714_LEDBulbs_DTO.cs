using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_714_LEDBulbs_DTO : CommonParamDTO
    {
        public long NCAC714LEDBU_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC714LEDBU_Year { get; set; }
        public string NCAC714LEDBU_LightingsRequirements { get; set; }
        public string NCAC714LEDBU_LughtingLED { get; set; }
        public string NCAC714LEDBU_OtherSource { get; set; }
        public string NCAC714LEDBUF_Filedesc { get; set; }
        public string NCAC714LEDBUF_FileName { get; set; }
        public string NCAC714LEDBUF_FilePath { get; set; }
        public bool NCAC714LEDBU_ActiveFlg { get; set; }
        public long NCAC714LEDBU_CreatedBy { get; set; }
        public long NCAC714LEDBU_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBU_CreatedDate { get; set; }
        public DateTime? NCAC714LEDBU_UpdatedDate { get; set; }
        public long NCAC714LEDBUF_Id { get; set; }


        public long NCAC714LEDBUC_Id { get; set; }
        public string NCAC714LEDBUC_Remarks { get; set; }
        public long? NCAC714LEDBUC_RemarksBy { get; set; }
        public string NCAC714LEDBUC_StatusFlg { get; set; }
        public bool? NCAC714LEDBUC_ActiveFlag { get; set; }
        public long? NCAC714LEDBUC_CreatedBy { get; set; }
        public DateTime? NCAC714LEDBUC_CreatedDate { get; set; }
        public long? NCAC714LEDBUC_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBUC_UpdatedDate { get; set; }
        public long NCAC714LEDBUFC_Id { get; set; }
        public string NCAC714LEDBUFC_Remarks { get; set; }
        public long? NCAC714LEDBUFC_RemarksBy { get; set; }
        public bool? NCAC714LEDBUFC_ActiveFlag { get; set; }
        public long? NCAC714LEDBUFC_CreatedBy { get; set; }
        public DateTime? NCAC714LEDBUFC_CreatedDate { get; set; }
        public long? NCAC714LEDBUFC_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBUFC_UpdatedDate { get; set; }
        public string NCAC714LEDBUFC_StatusFlg { get; set; }
        public Array commentlist1 { get; set; }
        public string NCAC714LEDBU_StatusFlg { get; set; }
        public string NCAC714LEDBUF_StatusFlg { get; set; }
        public bool? NCAC714LEDBUF_ActiveFlg { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_714_LEDBulbs_DTO[] NAACAC7DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }

        //MC
        public long NCMC717DFE_Id { get; set; }
        public long NCMC715WCFC_Id { get; set; }
        public string NCMC715WCFC_Remarks { get; set; }
        public long? NCMC715WCFC_RemarksBy { get; set; }
        public string NCMC715WCFC_StatusFlg { get; set; }
        public bool? NCMC715WCFC_ActiveFlag { get; set; }
        public long? NCMC715WCFC_CreatedBy { get; set; }
        public DateTime? NCMC715WCFC_CreatedDate { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
        public string UserName { get; set; }
        public long? NCMC715WCFC_UpdatedBy { get; set; }
        public DateTime? NCMC715WCFC_UpdatedDate { get; set; }
        public string NCMC717DFE_BuiltEnvwithRampsORLiftsFlag { get; set; }
        public string NCMC717DFE_DisabledFriendlyWashroomsFlag { get; set; }
        public string NCMC717DFE_SignageIncTactilePathssignpostsFlag { get; set; }
        public string NCMC717DFE_AssistiveTechnologyFacfacMEFlag { get; set; }
        public string NCMC717DFE_ProvisionForEnquiryScreenReadingFlag { get; set; }
        public long NCMC717DFE_CreatedBy { get; set; }
        public long NCMC717DFE_UpdatedBy { get; set; }
        public DateTime NCMC717DFE_CreatedDate { get; set; }
        public DateTime NCMC717DFE_UpdatedDate { get; set; }
        public long NCMC717DFE_Year { get; set; }
        public string NCMC715WCF_StatusFlg { get; set; }
        public bool NCMC717DFE_ActiveFlg { get; set; }

        public long NCMC716GCI_Id { get; set; }
        public string NCMC716GCI_RestrictedentryOfAutomobilesFlag { get; set; }
        public string NCMC716GCI_BatterypoweredvehiclesFlag { get; set; }
        public string NCMC716GCI_PedestrianFriendlyPathwaysFlag { get; set; }
        public string NCMC716GCI_BanOnTheuseOfPlasticsFlag { get; set; }
        public string NCMC716GCI_LandscapingwithtreesplantsFlag { get; set; }
        public long NCMC716GCI_CreatedBy { get; set; }
        public long NCMC716GCI_UpdatedBy { get; set; }
        public DateTime NCMC716GCI_CreatedDate { get; set; }
        public DateTime NCMC716GCI_UpdatedDate { get; set; }
        public long NCMC716GCI_Year { get; set; }
        public bool NCMC716GCI_ActiveFlg { get; set; }

        public long NCMC715WCF_Id { get; set; }
        public string NCMC715WCF_RainWaterHarvestingFlag { get; set; }
        public string NCMC715WCF_BorewellOpenwellRecFlag { get; set; }
        public string NCMC715WCF_ConstructionOftanksbundsFlag { get; set; }
        public string NCMC715WCF_MaintenanceOfWaterbodiesDSFlag { get; set; }
        public string NCMC715WCF_WastewaterrecyclingFlag { get; set; }
        public long NCMC715WCF_CreatedBy { get; set; }
        public long NCMC715WCF_UpdatedBy { get; set; }
        public DateTime NCMC715WCF_CreatedDate { get; set; }
        public DateTime NCMC715WCF_UpdatedDate { get; set; }
        public long NCMC715WCF_Year { get; set; }
        public bool NCMC715WCF_ActiveFlg { get; set; }
        //MC
    }
}
