using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Student_SiblingsDetails")]
    public class StudentSiblingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTS_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTS_SiblingsName { get; set; }
        public string AMSTS_SiblingsRelation { get; set; }
        public long AMCL_Id { get; set; }
        public long AMSTS_Siblings_AMST_ID { get; set; }
        public int AMSTS_SiblingsOrder { get; set; }
        public string AMSTS_TCIssuesFlag { get; set; }
        public long? AMSTS_CreatedBy { get; set; }
        public long? AMSTS_UpdatedBy { get; set; }
    }
}
