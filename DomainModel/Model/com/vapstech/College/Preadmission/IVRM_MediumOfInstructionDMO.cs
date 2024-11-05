using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("IVRM_MediumOfInstruction")]
    public class IVRM_MediumOfInstructionDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long IVRMOI_Id { get; set; }

        public string IVRMOI_MediumOfInstruction { get; set; }

        public bool IVRMOI_ActiveFlag { get; set; }



    }
}
