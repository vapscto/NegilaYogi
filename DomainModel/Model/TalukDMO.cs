using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Taluk")]
    public class TalukDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMT_Id { get; set; }
        public string IVRMMT_Name { get; set; }
        public DateTime? IVRMMT_CreatedDate { get; set; }
        public DateTime? IVRMMT_UpdatedDate { get; set; }
        public long IVRMMT_CreatedBy { get; set; }
        public long IVRMMT_UpdatedBy { get; set; }
        public bool? IVRMMT_ActiveFlag { get; set; }
        public long IVRMMD_Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
