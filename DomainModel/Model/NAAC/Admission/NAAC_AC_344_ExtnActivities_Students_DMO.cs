using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_Students")]
    public class NAAC_AC_344_ExtnActivities_Students_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET343S_Id { get; set; }
        public long NCACET343_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string NCACET343S_Role { get; set; }
        public bool NCACET343S_ActiveFlg { get; set; }
        public long NCACET343S_CreatedBy { get; set; }
        public long NCACET343S_UpdatedBy { get; set; }
        public DateTime NCACET343S_CreatedDate { get; set; }
        public DateTime NCACET343S_UpdatedDate { get; set; }
        public string NCACET343S_StatusFlg { get; set; }
        public List<NAAC_AC_344_ExtnActivities_Students_Files_DMO> NAAC_AC_344_ExtnActivities_Students_Files_DMO { get; set; }
    }
}
