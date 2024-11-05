
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.MobileApp
{
    [Table("IVRM_Role_MobileApp_Privileges")]
    public class IVRM_Role_MobileApp_Privileges : CommonParamDMO
    {
        [Key]
        public long IVRMRMAP_Id  { get; set; }
        public long IVRMRT_Id { get; set; } 
        public long IVRMMAP_Id { get; set; }
        public bool IVRMRMAP_ActiveFlg  { get; set; }
        public long MI_ID { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
    }
}
