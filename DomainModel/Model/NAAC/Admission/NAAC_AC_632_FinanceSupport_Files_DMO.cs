using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{


    [Table("NAAC_AC_632_FinanceSupport_Files")]
    public class NAAC_AC_632_FinanceSupport_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC632FINSUPF_Id { get; set; }
        public long NCAC632FINSUP_Id { get; set; }
        public string NCAC632FINSUPF_FileName { get; set; }
        public string NCAC632FINSUPF_Filedesc { get; set; }
        public string NCAC632FINSUPF_FilePath { get; set; }

        public string NCAC632FINSUPF_StatusFlg { get; set; }
        public bool? NCAC632FINSUPF_ActiveFlg { get; set; }
        public bool? NCAC632FINSUPF_ApprovedFlg { get; set; }
        public string NCAC632FINSUPF_Remarks { get; set; }
    }
}
