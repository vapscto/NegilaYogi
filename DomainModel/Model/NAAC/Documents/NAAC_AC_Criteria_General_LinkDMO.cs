using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_AC_Criteria_General_Link")]
    public class NAAC_AC_Criteria_General_LinkDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACCRGENLI_Id { get; set; }

    public long NCACCRGEN_Id { get; set; }
    public string NCACCRGENLI_LinkName { get; set; }
    public string NCACCRGENLI_LinkDescription { get; set; }
    public bool NCACCRGENLI_ActiveFlg { get; set; }
    public long NCACCRGENLI_CreatedBy { get; set; }
    public long NCACCRGENLI_UpdatedBy { get; set; }
    public DateTime NCACCRGENLI_CreatedDate { get; set; }
    public DateTime NCACCRGENLI_UpdatedDate { get; set; }
}
}
