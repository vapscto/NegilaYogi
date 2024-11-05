using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Program_Master", Schema = "SPC")]
    public class ProgramMasterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCPM_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCPM_Name { get; set; }
        public string SPCCPM_Description { get; set; }
        public bool SPCCPM_ActiveFlag { get; set; }

    }
}
