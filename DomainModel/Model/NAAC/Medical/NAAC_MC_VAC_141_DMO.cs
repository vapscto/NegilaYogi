using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_VAC_141")]
    public class NAAC_MC_VAC_141_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCVAC141_Id { get; set; }
        public long MI_Id { get; set; }
        public bool NCMCVAC141_FKFromStudents { get; set; }
        public bool NCMCVAC141_FKFromteachers { get; set; }
        public bool NCMCVAC141_FKFromemployers { get; set; }
        public bool NCMCVAC141_FKFromalumni { get; set; }
        public bool FkCollFromOtherProfs { get; set; }
        public DateTime NCMCVAC141_CreatedDate { get; set; }
        public DateTime NCMCVAC141_UpdatedDate { get; set; }
        public long NCMCVAC141_CreatedBy { get; set; }
        public long NCMCVAC141_UpdatedBy { get; set; }
        public long NCMCVAC141_year { get; set; }

    }
}
