using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Biometric_VAPS_IEMapping", Schema = "FO")]
    public class FO_Biometric_VAPS_IEMapping_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOBVIEM_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOBVIEM_BiometricId { get; set; }
        public long FOBVIEM_HRMEId { get; set; }
        public string FOBVIEM_InsertURL { get; set; }
        public bool FOBVIEM_ActiveFlg { get; set; }
        public long FOBVIEM_Insert_MI_Id { get; set; }
        public string FOBVIEM_CreatedBy { get; set; }
        public string FOBVIEM_UpdatedBy { get; set; }

    }
}
