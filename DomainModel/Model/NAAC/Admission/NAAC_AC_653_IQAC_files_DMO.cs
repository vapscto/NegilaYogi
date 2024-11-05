using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_653_IQAC_files")]
    public class NAAC_AC_653_IQAC_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC653IQACF_Id { get; set; }
        [ForeignKey("NCAC653IQAC_Id")]
        public long NCAC653IQAC_Id { get; set; }
        public string NCAC653IQACF_FileName { get; set; }
        public string NCAC653IQACF_Filedesc { get; set; }
        public string NCAC653IQACF_FilePath { get; set; }
        public string NCAC653IQACF_StatusFlg { get; set; }
        public bool? NCAC653IQACF_ActiveFlg { get; set; }
        public bool? NCAC653IQACF_ApprovedFlg { get; set; }
        public string NCAC653IQACF_Remarks { get; set; }


    }
}
