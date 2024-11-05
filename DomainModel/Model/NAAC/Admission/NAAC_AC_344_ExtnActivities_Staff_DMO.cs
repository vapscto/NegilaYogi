using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_Staff")]
    public class NAAC_AC_344_ExtnActivities_Staff_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET344STF_Id { get; set; }
        public long NCACET343_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACET344STF_Role { get; set; }
        public bool NCACET344STF_ActiveFlg { get; set; }
        public long NCACET344STF_CreatedBy { get; set; }
        public long NCACET344STF_UpdatedBy { get; set; }
        public DateTime NCACET344STF_CreatedDate { get; set; }
        public DateTime NCACET344STF_UpdatedDate { get; set; }
        public string NCACET344STF_StatusFlg { get; set; }

        public List<NAAC_AC_344_ExtnActivities_Staff_Files_DMO> NAAC_AC_344_ExtnActivities_Staff_Files_DMO
        { get; set; }

    }
}
