using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_SA_Students")]
    public class NAAC_AC_343_SA_Students_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343S_Id { get; set; }
        public long NCACSA343_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string NCACSA343S_Role { get; set; }
        public bool NCACSA343S_ActiveFlg { get; set; }
        public long NCACSA343S_CreatedBy { get; set; }
        public long NCACSA343S_UpdatedBy { get; set; }
        public DateTime NCACSA343S_CreatedDate { get; set; }
        public DateTime NCACSA343S_UpdatedDate { get; set; }

        public List<NAAC_AC_343_SA_Students_Files_DMO> NAAC_AC_343_SA_Students_Files_DMO { get; set; }

    }
}
