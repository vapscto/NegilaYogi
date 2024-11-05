using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Biometric_Device", Schema = "FO")]
    public class FO_Biometric_DeviceDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FOBD_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOBD_DeviceName { get; set; }
        public string FOBD_IPAddress { get; set; }
        public string FOBD_DeviceType { get; set; }
        public long FOBD_DevicePortNo { get; set; }
        public string FOBD_DevicePassword { get; set; }
        public bool FOBD_ActiveFlg { get; set; }
        public string FOBD_StudStaffFlg { get; set; }
    }
}     
     
















       
