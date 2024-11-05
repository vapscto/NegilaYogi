using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_653_IQAC_Comments")]
    public class NAAC_AC_653_IQAC_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC653IQACC_Id { get; set; }
        public string NCAC653IQACC_Remarks { get; set; }
        public long? NCAC653IQACC_RemarksBy { get; set; }
        public string NCAC653IQACC_StatusFlg { get; set; }
        public bool? NCAC653IQACC_ActiveFlag { get; set; }
        public long? NCAC653IQACC_CreatedBy { get; set; }
        public DateTime? NCAC653IQACC_CreatedDate { get; set; }
        public long? NCAC653IQACC_UpdatedBy { get; set; }
        public DateTime? NCAC653IQACC_UpdatedDate { get; set; }
        public long NCAC653IQAC_Id { get; set; }
    }
}
