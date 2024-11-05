using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC
{
    [Table("NAAC_User_Privilege_Institution")]
    public class NAAC_User_Privilege_InstitutionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACUPRIIN_Id { get; set; }
        public long NAACUPRI_Id { get; set; }
        public long MI_Id { get; set; }
        public bool NAACUPRIIN_ActiveFlag { get; set; }
        public long NAACUPRIIN_CreatedBy { get; set; }
        public DateTime? NAACUPRIIN_CreatedDate { get; set; }
        public long NAACUPRIIN_UpdatedBy { get; set; }
        public DateTime? NAACUPRIIN_UpdatedDate { get; set; }
    }
}
