using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Awards_342")]
   public class NAAC_AC_Awards_342_DMO
    {
        [Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public long NCACAW342_Id { get; set; }
           public long MI_Id { get; set; } 
           public string NCACAW342_ActivityName { get; set; }
        public string NCACAW342_AwardName { get; set; }
        public string NCACAW342_AwardingBody { get; set; }
        public long NCACAW342_AwardYear { get; set; }
        public string NCACAW342_AgencyName { get; set; }
        public string NCACAW342_CategoryName { get; set; }
        public bool NCACAW342_ActiveFlg { get; set; }
        public long NCACAW342_CreatedBy { get; set; }
        public long NCACAW342_UpdatedBy { get; set; }
        public DateTime NCACAW342_CreatedDate { get; set; }
        public DateTime NCACAW342_UpdatedDate { get; set; }
        public string NCACAW342_StatusFlg { get; set; }

        public List<NAAC_AC_Awards_342_Files_DMO> NAAC_AC_Awards_342_Files_DMO { get; set; }
    }
}
