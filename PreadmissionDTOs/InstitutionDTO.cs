using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class InstitutionDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long MO_Id { get; set; }
        public long MVIDCON_Id { get; set; }
        public string MI_Name { get; set; }
        public string MVIDCON_VCNames { get; set; }
        public string MVIDCON_VCCode { get; set; }
        public string MI_VCStudentFlag { get; set; }
        public string MI_VCOthersFlag { get; set; }
        public string MI_Type { get; set; }
        public string MI_Address1 { get; set; }
        public string MI_Address2 { get; set; }
        public string MI_Address3 { get; set; }
        // public string MI_AddressArea { get; set; }
        public string IVRMMCT_Name { get; set; }
        public long IVRMMS_Id { get; set; }
        public string MI_MSTeamsAdminUsername { get; set; }
        public string MI_MSTeamsAdminPassword { get; set; }
        public long IVRMMC_Id { get; set; }
        public int MI_Pincode { get; set; }
        public int? MI_FaxNo { get; set; }
        public string MI_BackgroundImage { get; set; }
        public string MI_FormColor { get; set; }
        public string MI_FontColor { get; set; }
        public int? MI_FontSize { get; set; }
        public string MI_WeekStartDay { get; set; }
        public string MI_DateFormat { get; set; }
        public string MI_DateSeparator { get; set; }
        public string MI_Logo { get; set; }
        public string MI_Precision { get; set; }
        public string MI_GradingSystem { get; set; }
        public string MI_PasswordFlag { get; set; }
        public int MI_ActiveFlag { get; set; }
        public string MI_NAAC_InstitutionTypeFlg { get; set; }
        public string MI_Affiliation { get; set; }
        //public string MI_SMSAlertToemailids { get; set; }
        public Array TrustDropdown { get; set; }
        public Array vcdetails { get; set; }
        public Array emailsalert { get; set; }
        public Array Institutionname { get; set; }
        public Array countryDrpDown { get; set; }
        public Array stateDrpDown { get; set; }
        public Array cityDrpDown { get; set; }
        public Institution_Phone_NoDTO[] phones { get; set; }
        public Institution_MobileDTO[] mobiles { get; set; }
        public Institution_EmailIdDTO[] emails { get; set; }
        public Smscreditalertemails[] alertemails { get; set; }
        public vcidsss[] selectedvc { get; set; }
        public Array PhonearrayList { get; set; }
        public Array MobilearrayList { get; set; }
        public Array EmailarrayList { get; set; }
        public Array mandatoryList { get; set; }
        public string returnval { get; set; }
        public Array subscriptionlist { get; set; }
        public SortingPagingInfoDTO instutePagination { get; set; }
        public SortingPagingInfoDTO subscriptionPagination { get; set; }
        public long IVRMP_Id { get; set; }
        public long sessionMI_Id { get; set; }
        public long sessionMO_Id { get; set; }
        public Array instutedropdown { get; set; }
        public string MI_Subdomain { get; set; }
        public int UserId { get; set; }
        public long roleId { get; set; }
        public string MI_ContactDetails { get; set; }
        public string MI_AboutInstitute { get; set; }
        public Array fillinstitution { get; set; }
        public int? MI_FranchiseFlag { get; set; }
        public string MI_PAN { get; set; }
        public string MI_TAN { get; set; }
        public string MI_GPSUserName { get; set; }
        public string MI_IVRSVirtualNo { get; set; }
        public string MI_IVRSOutboundNo { get; set; }
        public string MI_SchoolCollegeFlag { get; set; }
        public string MI_NAAC_SubInstitutionTypeFlg { get; set; }
        public long? MI_SMSCountAlert { get; set; }
        public string MI_MSTeamsClientId { get; set; } 
        public string  MI_MSTeamsTenentId { get; set; }
        public string  MI_MSTemasClinetSecretCode { get; set; }
        public string  MI_MSTeamsAppAccessTockenURL { get; set; }
        public string  MI_MSTeamsUserAceessTockenURL { get; set; }
        public string  MI_MSTeamsMeetingScheduleURL { get; set; }
        public string  MI_MSTeamsGrantType { get; set; }
        public string  MI_MSTeamsScope { get; set; }
        public string  MI_80GRegNo { get; set; }
        public string MVIDCONINT_HostedURL { get; set; }
        public string MI_EntityId { get; set; }
        public string User_Name { get; set; }
        public long Pervious_MI_Id { get; set; }
        public long Current_MI_Id { get; set; }
        public string IVRM_OTP_ADMNO { get; set; }
    }


    public class Smscreditalertemails {
        public string MI_SMSAlertToemailids { get; set; }

    }

    public class vcidsss {

        public long MVIDCON_Id { get; set; }
        public string  MVIDCON_VCNames { get; set; }
        public string MVIDCON_VCCode { get; set; }
        public string MVIDCONINT_HostedURL { get; set; }

    }
}
