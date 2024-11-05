using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_423_Memberships")]
    public class NAAC_AC_423_Memberships_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC423MEM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC423MEM_Membership { get; set; }
        public string NCAC423MEM_Subscription { get; set; }
        public Nullable<long> NCAC423MEM_NoOfEResources { get; set; }
        public Nullable<long> NCAC423MEM_ValidityPeriod { get; set; }
        public string NCAC423MEM_UsageReport { get; set; }
        public Nullable<bool> NCAC423MEM_RemoteAccessFlg { get; set; }
        public Nullable<bool> NCAC423MEM_ActiveFlg { get; set; }
        public Nullable<long> NCAC423MEM_CreatedBy { get; set; }
        public Nullable<long> NCAC423MEM_UpdatedBy { get; set; }
        public long NCAC423MEM_Year { get; set; }
        public string NCAC423MEM_Fulltextaccess { get; set; }
        public string NCAC423MEM_WeblinkOfRemoteAccess { get; set; }
        public string NCAC423MEM_StatusFlg { get; set; }

        public List<NAAC_AC_423_Memberships_Files_DMO> NAAC_AC_423_Memberships_Files_DMO { get; set; }
    }
}
