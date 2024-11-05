using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_634_DevPrograms_Files")]
    public class NAAC_AC_634_DevPrograms_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC634DEVPRGF_Id { get; set; }
        [ForeignKey("NCAC634DEVPRG_Id")]
        public long NCAC634DEVPRG_Id { get; set; }
        public string NCAC634DEVPRGF_FileName { get; set; }
        public string NCAC634DEVPRGF_Filedesc { get; set; }
        public string NCAC634DEVPRGF_FilePath { get; set; }
        public string NCAC634DEVPRGF_StatusFlg { get; set; }
        public bool? NCAC634DEVPRGF_ActiveFlg { get; set; }
        public bool? NCAC634DEVPRGF_ApprovedFlg { get; set; }
        public string NCAC634DEVPRGF_Remarks { get; set; }


    }
}
