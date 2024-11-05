using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAACAC7Report_DTO : CommonParamDTO
    {
        public long NCAC711GENEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC711GENEQ_Year { get; set; }
        public string NCAC711GENEQ_ProgramTitle { get; set; }
        public DateTime? NCAC711GENEQ_FromDate { get; set; }
        public DateTime? NCAC711GENEQ_ToDate { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsMale { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsFeMale { get; set; }
        public string NCAC711GENEQF_Filedesc { get; set; }
        public string NCAC711GENEQF_FileName { get; set; }
        public string NCAC711GENEQF_FilePath { get; set; }
        public bool NCAC711GENEQ_ActiveFlg { get; set; }
        public long NCAC711GENEQ_CreatedBy { get; set; }
        public long NCAC711GENEQ_UpdatedBy { get; set; }
        public long NCAC711GENEQ_CreatedDate { get; set; }
        public long NCAC711GENEQ_UpdatedDate { get; set; }
        public long NCAC711GENEQF_Id { get; set; }


        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_711_GenderEquity_DTO[] NAACAC711GenderEquityDTO { get; set; }
        public NAACAC7Report_DTO[] selected_Inst { get; set; }
        public List<long> selectedYear { get; set; }
        public string ASMAY_Year { get; set; }
        public Array alldata { get; set; }
        public Array alldatafile { get; set; }
        public string Type { get; set; }
        public long NCAC713ALTENE_Year { get; set; }
        public string NCAC713ALTENE_PowerRequirements { get; set; }
        public long NCAC713ALTENE_TotalPowerReq { get; set; }
        public string NCAC713ALTENE_EnergySource { get; set; }
        public string NCAC713ALTENE_EnergyUsed { get; set; }
        public string NCAC713ALTENE_EnergySupplied { get; set; }
        public long NCAC713ALTENE_Id { get; set; }
        public long NCAC714LEDBU_Id { get; set; }
        public long NCAC714LEDBU_Year { get; set; }
        public string NCAC714LEDBU_LightingsRequirements { get; set; }
        public string NCAC714LEDBU_LughtingLED { get; set; }
        public string NCAC714LEDBU_OtherSource { get; set; }
        public long NCAC718WAMAN_Id { get; set; }
        public long NCAC718WAMAN_Year { get; set; }
        public decimal NCAC718WAMAN_Expenditure { get; set; }
        public long NCAC719DIFFAB_Id { get; set; }
        public long NCAC719DIFFAB_Year { get; set; }
        public string NCAC719DIFFAB_LIFTFacilityFlg { get; set; }
        public string NCAC719DIFFAB_PhysicalFacilityFlg { get; set; }
        public string NCAC719DIFFAB_BrailleSaoftFlg { get; set; }
        public string NCAC719DIFFAB_RestRoomFlg { get; set; }
        public string NCAC719DIFFAB_ExamScribeFlg { get; set; }
        public string NCAC719DIFFAB_SPLSkilDevFlg { get; set; }
        public string NCAC719DIFFAB_RampFacilityFlg { get; set; }
        public string NCAC719DIFFAB_OtherFacility { get; set; }
        public long NCAC7110LOCADVTG_Id { get; set; }
        public long NCAC7110LOCADVTG_Year { get; set; }
        public long NCAC7110LOCADVTG_NoOfAddress { get; set; }
        public long NCAC7110LOCADVTG_NoOfEngage { get; set; }
        public DateTime? NCAC7110LOCADVTG_Date { get; set; }
        public long NCAC7110LOCADVTG_Duration { get; set; }
        public string NCAC7110LOCADVTG_InitiativeName { get; set; }
        public string NCAC7110LOCADVTG_IssuesAddressed { get; set; }
        public long NCAC7110LOCADVTG_NoOfParticipant { get; set; }
        public long NCAC7112CODCON_Id { get; set; }
        public long NCAC7112CODCON_Year { get; set; }
        public string NCAC7112CODCON_URL { get; set; }
        public long NCAC7113CORVAL_Id { get; set; }
        public long NCAC7113CORVAL_Year { get; set; }
        public string NCAC7113CORVAL_URL { get; set; }
        public long NCAC7114HUVAL_Id { get; set; }
        public long NCAC7114HUVAL_Year { get; set; }
        public string NCAC7114HUVAL_ProgramTitle { get; set; }
        public long NCAC7114HUVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7114HUVAL_FromDate { get; set; }
        public DateTime NCAC7114HUVAL_ToDate { get; set; }
        public long NCAC7115PROETH_Id { get; set; }
        public long NCAC7115PROETH_Year { get; set; }
        public string NCAC7115PROETH_URL { get; set; }
        public long NCAC7116STABOD_Id { get; set; }
        public long NCAC7116STABOD_Year { get; set; }
        public string NCAC7116STABOD_URL { get; set; }
        public long NCAC7117UNIVAL_Id { get; set; }
        public long NCAC7117UNIVAL_Year { get; set; }
        public string NCAC7117UNIVAL_ProgramTitle { get; set; }
        public long NCAC7117UNIVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7117UNIVAL_FromDate { get; set; }
        public DateTime NCAC7117UNIVAL_ToDate { get; set; }


        public long NCAC7111LOCCOM_Id { get; set; }
        public long NCAC7111LOCCOM_Year { get; set; }
        public string NCAC7111LOCCOM_NoOfAddress { get; set; }
        public long NCAC7111LOCCOM_NoOfEngage { get; set; }
        public DateTime? NCAC7111LOCCOM_Date { get; set; }
        public long NCAC7111LOCCOM_Duration { get; set; }
        public string NCAC7111LOCCOM_InitiativeName { get; set; }
        public string NCAC7111LOCCOM_IssuesAddressed { get; set; }
        public long NCAC7111LOCCOM_NoOfParticipant { get; set; }
        public Array alldata11 { get; set; }
        public Array alldata11file { get; set; }
        public long cycleid { get; set; }

        //MC
        public long NCMC715WCF_Id { get; set; }
        public long NCMC715WCF_Year { get; set; }
        public string NCMC715WCF_RainWaterHarvestingFlag { get; set; }
        public string NCMC715WCF_BorewellOpenwellRecFlag { get; set; }
        public string NCMC715WCF_ConstructionOftanksbundsFlag { get; set; }
        public string NCMC715WCF_MaintenanceOfWaterbodiesDSFlag { get; set; }

        public string NCMC716GCI_RestrictedentryOfAutomobilesFlag { get; set; }
        public string NCMC716GCI_BatterypoweredvehiclesFlag { get; set; }
        public string NCMC716GCI_PedestrianFriendlyPathwaysFlag { get; set; }
        public string NCMC716GCI_BanOnTheuseOfPlasticsFlag { get; set; }
        public string NCMC716GCI_LandscapingwithtreesplantsFlag { get; set; }
        public long NCMC716GCI_Id { get; set; }
        public long NCMC716GCI_Year { get; set; }

        public string NCMC717DFE_BuiltEnvwithRampsORLiftsFlag { get; set; }
        public string NCMC717DFE_DisabledFriendlyWashroomsFlag { get; set; }
        public string NCMC717DFE_SignageIncTactilePathssignpostsFlag { get; set; }
        public string NCMC717DFE_AssistiveTechnologyFacfacMEFlag { get; set; }
        public string NCMC717DFE_ProvisionForEnquiryScreenReadingFlag { get; set; }
        public long NCMC717DFE_Id { get; set; }
        public long NCMC717DFE_Year { get; set; }
        public string NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag { get; set; }
        public string NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag { get; set; }
        public string NCAC719DIFFAB_ProfProgOrgStuStaffFlag { get; set; }
        public string NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag { get; set; }
        //MC
    }
}
