using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_634_DevPrograms_File_Comments")]
    public class NAAC_AC_634_DevPrograms_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC634DEVPRGFC_Id { get; set; }
        public string NCAC634DEVPRGFC_Remarks { get; set; }
        public long? NCAC634DEVPRGFC_RemarksBy { get; set; }
        public bool? NCAC634DEVPRGFC_ActiveFlag { get; set; }
        public long? NCAC634DEVPRGFC_CreatedBy { get; set; }
        public DateTime? NCAC634DEVPRGFC_CreatedDate { get; set; }
        public long? NCAC634DEVPRGFC_UpdatedBy { get; set; }
        public DateTime? NCAC634DEVPRGFC_UpdatedDate { get; set; }
        public string NCAC634DEVPRGFC_StatusFlg { get; set; }
        public long NCAC634DEVPRGF_Id { get; set; }
    }
}
