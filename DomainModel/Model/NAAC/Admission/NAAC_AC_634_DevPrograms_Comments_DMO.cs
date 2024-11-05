using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_634_DevPrograms_Comments")]
    public class NAAC_AC_634_DevPrograms_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC634DEVPRGC_Id { get; set; }
        public string NCAC634DEVPRGC_Remarks { get; set; }
        public long? NCAC634DEVPRGC_RemarksBy { get; set; }
        public string NCAC634DEVPRGC_StatusFlg { get; set; }
        public bool? NCAC634DEVPRGC_ActiveFlag { get; set; }
        public long? NCAC634DEVPRGC_CreatedBy { get; set; }
        public DateTime? NCAC634DEVPRGC_CreatedDate { get; set; }
        public long? NCAC634DEVPRGC_UpdatedBy { get; set; }
        public DateTime? NCAC634DEVPRGC_UpdatedDate { get; set; }
        public long NCAC634DEVPRG_Id { get; set; }
    }
}
