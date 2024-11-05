using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Vehicle_Documents", Schema = "TRN")]
    public class TR_Master_Vehicle_DocumentsDMO : CommonParamDMO
    {     

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMVDO_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMV_Id { get; set; }
        public string TRMVDO_DocumentName { get; set; }
        public string TRMVDO_DocumentFileName { get; set; }
        public bool TRMVDO_IsActiveFlg { get; set; }
        public bool TRMVDO_ActiveFlg { get; set; }
        public long TRMVDO_CreatedBy { get; set; }
        public long TRMVDO_UpdatedBy { get; set; }
        public string TRMVDO_DocumentFilePath { get; set; }
        public string TRMVDO_DocumentFileDesc { get; set; }

    }
}
