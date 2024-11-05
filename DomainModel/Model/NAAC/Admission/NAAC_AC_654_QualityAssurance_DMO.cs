using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_654_QualityAssurance")]
    public class NAAC_AC_654_QualityAssurance_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC654QUAS_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC654QUAS_Year { get; set; }
        public bool NCAC654QUAS_AQARFlg { get; set; }
        public bool NCAC654QUAS_AAAFlg { get; set; }
        public bool NCAC654QUAS_NIRFFlg { get; set; }
        public bool NCAC654QUAS_NBAFlg { get; set; }
        public bool NCAC654QUAS_ISOFlg { get; set; }
        public bool NCAC654QUAS_FkStsCollectedAnlreportFlag { get; set; }
        public bool NCAC654QUAS_OrgWsSsPrgAdmStaffFlag { get; set; }
        public bool NCAC654QUAS_ActiveFlg { get; set; }
        public long NCAC654QUAS_CreatedBy { get; set; }
        public long NCAC654QUAS_UpdatedBy { get; set; }
        public DateTime NCAC654QUAS_CreatedDate { get; set; }
        public DateTime NCAC654QUAS_UpdatedDate { get; set; }
        public string NCAC654QUAS_StatusFlg { get; set; }
        public bool? NCAC654QUAS_ApprovedFlg { get; set; }
        public string NCAC654QUAS_Remarks { get; set; }       
        public List<NAAC_AC_654_QualityAssurance_files_DMO> NAAC_AC_654_QualityAssurance_files_DMO { get; set; }

    }
}
