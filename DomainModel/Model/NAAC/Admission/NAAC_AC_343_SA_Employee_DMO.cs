using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_SA_Employee")]
    public class NAAC_AC_343_SA_Employee_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343E_Id { get; set; }
        public long NCACSA343_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACSA343E_Role { get; set; }
        public bool NCACSA343E_ActiveFlg { get; set; }
        public long NCACSA343E_CreatedBy { get; set; }
        public long NCACSA343E_UpdatedBy { get; set; }
        public DateTime NCACSA343E_CreatedDate { get; set; }
        public DateTime NCACSA343E_UpdatedDate { get; set; }

        public List<NAAC_AC_343_SA_Employee_Files_DMO> NAAC_AC_343_SA_Employee_Files_DMO { get; set; }
    }
}
