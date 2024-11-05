using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Adm_M_Student_SiblingsDetails")]
    public class Adm_M_SiblingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTS_SiblingsName { get; set; }
        public string AMSTG_SiblingsRelation { get; set; }
        public long AMCL_Id { get; set; }
        public long AMSTS_SiblingsAMST_Id { get; set; }
        public int AMSTS_SiblingsOrder { get; set; }
        public int AMSTS_Tc_IssuesFlag { get; set; }
        //public decimal AMSTS_Concessionpercentage { get; set; }
    }
}
