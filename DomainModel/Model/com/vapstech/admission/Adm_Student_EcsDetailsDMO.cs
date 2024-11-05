using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_ECS")]
    public class Adm_Student_EcsDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASECS_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string ASECS_AccountHolderName { get; set; }
        public string ASECS_AccountNo { get; set; }
        public string ASECS_AccountType { get; set; }
        public string ASECS_BankName { get; set; }
        public string ASECS_Branch { get; set; }
        public string ASECS_MICRNo { get; set; }
        public bool ASECS_ActiveFlg { get; set; }
        public long? ASECS_CreatedBy { get; set; }
        public long? ASECS_UpdatedBy { get; set; }   
    }
}
