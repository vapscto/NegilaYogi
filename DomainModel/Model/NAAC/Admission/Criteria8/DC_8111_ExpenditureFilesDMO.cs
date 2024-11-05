using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_DC_8111_Expenditure_Files")]
    public class DC_8111_ExpenditureFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDC8111EF_Id { get; set; }
        public long NCDC8111E_Id { get; set; }
        public string NCDC8111EF_StatusFlg { get; set; }
        public string NCMC811NEETF_FileDesc { get; set; }
        public string NCDC8111EF_FileName { get; set; }
        public string NCDC8111EF_FilePath { get; set; }
        public bool NCDC8111EF_ActiveFlg { get; set; }
        public DateTime? NCDC8111EF_CreatedDate { get; set; }
        public DateTime? NCDC8111EF_UpdatedDate { get; set; }
        public long NCDC8111EF_CreatedBy { get; set; }
        public long NCDC8111EF_UpdatedBy { get; set; }
    }
}
