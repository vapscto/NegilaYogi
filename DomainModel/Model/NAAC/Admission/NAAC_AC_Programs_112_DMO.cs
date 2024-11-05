using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Programs_112")]
    public class NAAC_AC_Programs_112_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACPR112_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool NCACPR112_ActiveFlg { get; set; }
        public long NCACPR112_CreatedBy { get; set; }
        public long NCACPR112_UpdatedBy { get; set; }
        public DateTime NCACPR112_CreatedDate { get; set; }
        public DateTime NCACPR112_UpdatedDate { get; set; }
        public long NCACMPR112_Id { get; set; }
        public long NCACPR112_Year { get; set; }
        public DateTime NCACPR112_Date { get; set; }
        public string NCACPR112_DeptName { get; set; }
        public long NCACPR112_RevisionYear { get; set; }
        public long NCACPR112_RevcarriedSyllabusYerars { get; set; }
        public bool? NCACPR112_ApprovedFlg { get; set; }
        public string NCACPR112_Remarks { get; set; }
        public string NCACPR112_StatusFlg { get; set; }
        public List<NAAC_AC_Programs_112_FilesDMO> NAAC_AC_Programs_112_FilesDMO { get; set; }

    }
}
