using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Biometric_DeviceType", Schema = "FO")]
    public class FO_Biometric_DeviceTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOBDT_Id { get; set; }
        public string FOBDT_DeviceType { get; set; }
        public bool FOBDT_ActiveFlg { get; set; }
        public string FOBDT_CodeRef { get; set; }
    }
}     
     
















       
