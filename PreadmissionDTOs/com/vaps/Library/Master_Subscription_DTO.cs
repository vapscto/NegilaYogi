using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class Master_Subscription_DTO:CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public long LMSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMPE_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LMD_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long LML_Id { get; set; }
        public string LMSU_PeriodicalTitle { get; set; }
        public string LMSU_SubscriptionNo { get; set; }
        public string LMSU_PeriodicalTypeFlg { get; set; }
        public DateTime? LMSU_SubscriptionDate { get; set; }
        public DateTime? LMSU_ExpiryDate { get; set; }
        public DateTime? LMSU_PreTerminationDate { get; set; }
        public decimal LMSU_Price { get; set; }
        public decimal LMSU_Discount { get; set; }
        public string LMSU_DoscountTypeFlg { get; set; }
        public decimal LMSU_NetPrice { get; set; }
        public string LMSU_OrderNo { get; set; }
        public DateTime? LMSU_OrderDate { get; set; }
        public int LMSU_NoOfCopies { get; set; }
        public string LMSU_StartVolumeNo { get; set; }
        public string LMSU_StartIssueNo { get; set; }
        public DateTime? LMSU_ExpectedDate { get; set; }
        public bool LMSU_AutoAccnNoFlg { get; set; }
        public DateTime? LMSU_EntryDate { get; set; }
        public int LMSU_NoOfIssues { get; set; }
        public string LMSU_CurrencyType { get; set; }
        public bool LMSU_PreTerminateFlg { get; set; }
        public bool LMSU_ActiveFlg { get; set; }
       


        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array periodicitylist { get; set; }
        public Array publisherlist { get; set; }
        public Array deptlist { get; set; }
        public Array vendorlist { get; set; }
        public Array categorylist { get; set; }
        public Array languagelist { get; set; }
        public Array alldata { get; set; }
        public Array editdata { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string trans_id { get; set; }
        public string LMP_PublisherName { get; set; }
        public string LMPE_PeriodicityName { get; set; }
        public string LMD_DepartmentName { get; set; }
        public string LMV_VendorName { get; set; }
        public string LML_LanguageName { get; set; }

    }
}
