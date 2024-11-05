using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Student_Bonds")]
    public class MasterStudentBondDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTB_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public long IMGB_Id { get; set; }
        public string AMSTB_BondName { get; set; }
        public int AMSTB_BandNo { get; set; }
        public long? AMSTB_CreatedBy { get; set; }
        public long? AMSTB_UpdatedBy { get; set; }         
    }
}
