using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Biometric_DeviceType_Institutoinwise", Schema = "FO")]
    public class FO_Biometric_DeviceType_InstitutoinwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOBDTI_Id { get; set; }
        public long MI_Id { get; set; }
        public long FOBDT_Id { get; set; }
        public bool FOBDTI_ActiveFlg { get; set; }
    }
}     
     
















       
