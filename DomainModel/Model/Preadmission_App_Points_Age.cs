using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_App_Points_Age")]
    public class Preadmission_App_Points_AgeDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAAPA_Id  { get; set; }
        public long MI_Id  { get; set; }
        public long ASMCL_Id  { get; set; }

        public int PAAPA_MaxAge  { get; set; }
        public int PAAPA_MaxMonth  { get; set; }
        public int PAAPA_Maxdays  { get; set; }
        public int PAAPA_MinAge  { get; set; }
        public int PAAPA_MinMonth  { get; set; }
        public int PAAPA_Mindays  { get; set; }
        public int PAAPA_Points { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
