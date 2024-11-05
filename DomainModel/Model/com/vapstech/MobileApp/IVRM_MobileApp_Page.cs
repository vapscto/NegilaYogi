
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.MobileApp 
{
    [Table("IVRM_MobileApp_Page")]
    public class IVRM_MobileApp_Page :CommonParamDMO
    {
        [Key]
        public long IVRMMAP_Id { get; set; }
        public string IVRMMAP_AppPageName  { get; set; } 

        public string IVRMMAP_Displayname { get; set; }
        public string IVRMMAP_AppPageDesc  { get; set; }
        public string IVRMMAP_AppPageURL  { get; set; }
        public bool IVRMMAP_AnalyticalFlag { get; set; }
        public bool IVRMMAP_ActiveFlg { get; set; }
    }
}
