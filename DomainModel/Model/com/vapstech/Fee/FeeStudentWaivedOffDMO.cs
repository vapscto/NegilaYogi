using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Waived_Off")]
    public class FeeStudentWaivedOffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FSWO_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FSWO_Date { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSWO_WaivedOffAmount { get; set; }
        public bool FSWO_ActiveFlag { get; set; }
        public long User_Id { get; set; }
        public int FSWO_FineFlg { get; set; }

        public string FSWO_WaivedOffRemarks { get; set; }
        public int FSWO_FullFineWaiveOffFlg { get; set; }
        public string FSWO_WaivedOfffilepath { get; set; }
        public string FSWO_WaivedOfffilename { get; set; }

    }
}
