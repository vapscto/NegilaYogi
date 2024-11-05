using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_NC_818_EmpCommittees")]
    public class NC_818_EmpCommitteesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCNC8111EC_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCDC8111EC_Year { get; set; }
        public string NCDC8111EC_FacultyMemberName { get; set; }
        public string NCDC8111EC_CommitteesName { get; set; }
        public string NCDC8111EC_TenureOfService { get; set; }
        public string NCNC8111EC_StatusFlg { get; set; }
        public DateTime? NCDC8111EC_CreatedDate { get; set; }
        public DateTime? NCDC8111EC_UpdatedDate { get; set; }
        public long NCDC8111EC_CreatedBy { get; set; }
        public long NCDC8111EC_UpdatedBy { get; set; }
        public bool NCDC8111EC_ActiveFlag { get; set; }
       public List<NC_818_EmpCommitteesFilesDMO> NC_818_EmpCommitteesFilesDMO { get; set; }

    }
}
