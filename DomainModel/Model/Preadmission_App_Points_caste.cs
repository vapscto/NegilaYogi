using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_App_Points_caste")]
    public class Preadmission_App_Points_casteDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAAPC_Id  { get; set; }
        public long MI_Id  { get; set; }
        public long IMC_Id { get; set; }
        public int PAAPC_Points  { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate  { get; set; }
    }
}
