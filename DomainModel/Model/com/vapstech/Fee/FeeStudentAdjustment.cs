using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Adjustment")]
    public class FeeStudentAdjustment 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FSA_Date { get; set; }
        public long FSA_From_FMH_Id { get; set; }
        public long FSA_From_FMG_Id { get; set; }
        public long FSA_AdjustedAmount { get; set; }
        public long FSA_To_FMH_Id { get; set; }
        public long FSA_To_FMG_Id { get; set; }
      //  public string FSA_Trasanction_No   { get; set; }
        public bool FSA_ActiveFlag { get; set; }

        public long FSA_From_FMA_Id { get; set; }
        public long FSA_From_FTI_Id { get; set; }
        public long FSA_To_FMA_Id { get; set; }
        public long FSA_TO_FTI_Id { get; set; }
        public long User_Id { get; set; }


    }
}
