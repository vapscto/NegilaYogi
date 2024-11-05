using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Demo")]
    public class ISM_Sales_Lead_Demo_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLEDM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMSLEDM_DemoType { get; set; }
        public DateTime ISMSLEDM_DemoDate { get; set; }
        public string ISMSLEDM_ContactPerson { get; set; }
        public string ISMSLEDM_DemoAddress { get; set; }
        public string ISMSLEDM_Remarks { get; set; }
        public bool ISMSLEDM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLEDM_CreatedBy { get; set; }
        public long ISMSLEDM_UpdatedBy { get; set; }
        public long? ISMSLEDM_Status_Flg { get; set; }

        public List<ISM_Sales_Lead_Demo_Products_DMO> ISM_Sales_Lead_Demo_Products_DMO { get; set; }
    }
}
