
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.MobileApp
{
    [Table("IVRM_User_MobileApp_Login_Privileges")]
    public class IVRM_User_MobileApp_Login_Privileges : CommonParamDMO
    {
        [Key]
        public long IVRMUMALP_Id { get; set; }
        public long MI_Id { get; set; } 
        public long IVRMUL_Id { get; set; }
        public long IVRMMAP_Id { get; set; }
        public bool IVRMUMALP_ActiveFlg { get; set; }
        public bool? IVRMUMALP_AddFlg { get; set; }
        public bool? IVRMUMALP_UpdateFlg { get; set; }
        public bool? IVRMUMALP_DeleteFlg { get; set; }

    }
}
