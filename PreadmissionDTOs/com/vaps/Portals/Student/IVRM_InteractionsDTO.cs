using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class IVRM_InteractionsDTO
    {

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public string interactionid { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public bool IVRMGC_EnableSTIntFlg { get; set; }
        public bool IVRMGC_EnableCTIntFlg { get; set; }
        public bool IVRMGC_EnableHODIntFlg { get; set; }
        public bool IVRMGC_EnablePrincipalIntFlg { get; set; }
        public bool IVRMGC_EnableASIntFlg { get; set; }

        public long asmclid { get; set; }
        public long asmsid { get; set; }

        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long IVRMRT_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public Array studentlist { get; set; }

        public string Role_flag { get; set; }
        public Array roletype { get; set; }
        public Array configflag { get; set; }

        public Array staffinboxmsg { get; set; }
        public Array get_replies { get; set; }
        public Array studentinboxmsg { get; set; }
        //  public Array studentmsgdetails { get; set; }

        public Array staffwiseclasslist { get; set; }
        public Array sectionlist { get; set; }
        public Array get_student { get; set; }


        public string employeeName { get; set; }
        public string studentName { get; set; }



        public long IINTS_Id { get; set; }
        public long MI_Id { get; set; }
        public string IINTS_InteractionId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string IINTS_Subject { get; set; }
        public DateTime IINTS_Date { get; set; }
        public string IINTS_Interaction { get; set; }
        public long HRME_Id { get; set; }
        public string userflag { get; set; }
        public bool IINTS_ActiveFlag { get; set; }

        public long IINTSS_Id { get; set; }
        public string IINTSS_Interaction { get; set; }
        public DateTime IINTSS_Date { get; set; }
        public string IINTSS_ByFlg { get; set; }
        public bool IINTSS_ActiveFlag { get; set; }

        public long IINTST_Id { get; set; }
        public string IINTST_InteractionId { get; set; }
        public string IINTST_Subject { get; set; }
        public DateTime IINTST_Date { get; set; }
        public string IINTST_Interaction { get; set; }
        public bool IINTST_ActiveFlag { get; set; }

        public long IINTSTS_Id { get; set; }
        public string IINTSTS_Interaction { get; set; }
        public DateTime IINTSTS_Date { get; set; }
        public string IINTSTS_ByFlg { get; set; }
        public bool IINTSTS_ActiveFlag { get; set; }
        public string IINTS_ComposeFlag { get; set; }

        public string roleflg { get; set; }

        public Array getdetails { get; set; }

    }

    public class numberingdto
    {
        public long IMN_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMN_AutoManualFlag { get; set; }
        public string IMN_DuplicatesFlag { get; set; }
        public string IMN_StartingNo { get; set; }
        public string IMN_WidthNumeric { get; set; }
        public string IMN_ZeroPrefixFlag { get; set; }
        public bool IMN_PrefixAcadYearCode { get; set; }
        public string IMN_PrefixParticular { get; set; }
        public bool IMN_SuffixAcadYearCode { get; set; }
        public string IMN_SuffixParticular { get; set; }
        public string IMN_RestartNumFlag { get; set; }
        public string IMN_Flag { get; set; }
        public bool? IMN_PrefixFinYearCode { get; set; }
        public bool? IMN_PrefixCalYearCode { get; set; }
        public bool? IMN_SuffixFinYearCode { get; set; }
        public bool? IMN_SuffixCalYearCode { get; set; }
    }
}