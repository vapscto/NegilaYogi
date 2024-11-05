using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    public class Adm_Master_ActivitiesDTO
    {
        public Adm_Master_ActivitiesDTO[] listdata23 { get; set; }
        public long AMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMA_ActivityName { get; set; }
        public string AMA_ActivityDesc { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public bool AMA_ActiveFlg { get; set; }
        public long AMA_CreatedBy { get; set; }
        public DateTime AMA_CreatedDate { get; set; }
        public long AMA_UpdatedBy { get; set; }
        public DateTime AMA_UpdatedDate { get; set; }
        public long ASA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool ASA_ApprovedFlg { get; set; }
        public long ASA_ApprovedBy { get; set; }
        public bool ASA_ActiveFlg { get; set; }
        public long ASA_CreatedBy { get; set; }
        public DateTime ASA_CreatedDate { get; set; }
        public long ASA_UpdatedBy { get; set; }
        public DateTime ASA_UpdatedDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long loaddata { get; set; }
        public long UserId { get; set; }
        public Array gethead { get; set; }
        public Array group_list { get; set; }
        public Array get_masterlist { get; set; }
        public string FMG_GroupName_new { get; set; }
        public string FMG_GroupName{ get; set; }
        public string FMH_FeeName_new { get; set; }
        public long savedata { get; set; }
        public Adm_Master_ActivitiesDTO[] headid { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public long count { get; set; }
        public long count1 { get; set; }
     
       // public long masterDecative { get; set; }
      
        public long ASMS_Id { get; set; }
        public long userid { get; set; }
        public Array getstudata { get; set; }
        public string AMST_FirstName { get; set; }

        public long AMAY_RollNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public  Array fetheaddata { get; set; }
        public string FMH_FeeName { get; set; }
       
        public decimal FMA_Amount { get; set; }
        public string FTI_Name { get; set; }
        public Array getstusaveddata { get; set; }
        public bool returnval { get; set; }
        public string return_value { get; set; }
        public Adm_Master_ActivitiesDTO[] headnames { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }

        public Array fetheadstudata { get; set; }

        public DateTime FYP_Date_From { get; set; }

        public DateTime FYP_Date_To  { get; set; }

        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public Array getstusaveddataheadview { get; set; }

        public string searchType { get; set; }
        public string searchtext { get; set; }
        public Array searcharray { get; set; }

        public DateTime searchdate { get; set; }
    }
}
