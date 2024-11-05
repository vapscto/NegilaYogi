using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_Complexities")]
    public class LP_Master_ComplexitiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPMCOMP_Id { get; set; }
        public string LPMCOMP_ComplexityName { get; set; }
        public string LPMCOMP_ComplexityDesc { get; set; }
        public bool LPMCOMP_ActiveFlg { get; set; }
        public bool? LPMCOMP_DefaultFlg { get; set; }
        public long LPMCOMP_CreatedBy { get; set; }
        public DateTime? LPMCOMP_CreatedDate { get; set; }
        public long LPMCOMP_UpdatedBy { get; set; }
        public DateTime? LPMCOMP_UpdatedDate { get; set; }
    }
}
