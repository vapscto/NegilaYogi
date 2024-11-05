using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee")]
    public class NAAC_AC_Committee_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACCOMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACCOMM_CommitteeName { get; set; }
        public string NCACCOMM_Flg { get; set; }
        public long NCACCOMM_Year { get; set; }
        public bool NCACCOMM_ActiveFlg { get; set; }
        public long NCACCOMM_CreatedBy { get; set; }
        public long NCACCOMM_UpdatedBy { get; set; }
        public DateTime NCACCOMM_CreatedDate { get; set; }
        public DateTime NCACCOMM_UpdatedDate { get; set; }
        public string NCACCOMM_StaffFlg { get; set; }
        public string NCACCOMM_StatusFlg { get; set; }
        public List<NAAC_AC_Committee_Files_DMO> NAAC_AC_Committee_Files_DMO { get; set; }
        public List<NAAC_AC__Committee_Members_DMO> NAAC_AC__Committee_Members_DMO { get; set; }
    }
}
