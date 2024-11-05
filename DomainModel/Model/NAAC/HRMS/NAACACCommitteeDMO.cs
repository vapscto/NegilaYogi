using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("NAAC_AC_Committee")]
    public class NAACACCommitteeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACCOMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACCOMM_CommitteeName { get; set; }
        public string NCACCOMM_Flg { get; set; }
        public long NCACCOMM_Year { get; set; }
        public string NCACCOMM_FileName { get; set; }
        public string NCACCOMM_FilePath { get; set; }
        public bool NCACCOMM_ActiveFlg { get; set; }
        public long NCACCOMM_CreatedBy { get; set; }
        public long NCACCOMM_UpdatedBy { get; set; }
        public DateTime? NCACCOMM_CreatedDate { get; set; }
        public DateTime? NCACCOMM_UpdatedDate { get; set; }
    }
}
