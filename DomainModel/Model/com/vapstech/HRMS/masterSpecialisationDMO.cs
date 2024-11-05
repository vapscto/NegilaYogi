using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Specialisation")]

    public class masterSpecialisationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMSPL_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMSPL_SpecialisationName { get; set; }
        public bool HRMSPL_ActiveFlg { get; set; }
        public long HRMSPL_CreatedBy { get; set; }
        public long HRMSPL_UpdatedBy { get; set; }
        public DateTime? HRMSPL_CreatedDate { get; set; }
        public DateTime? HRMSPL_UpdatedDate { get; set; }

    }
}
