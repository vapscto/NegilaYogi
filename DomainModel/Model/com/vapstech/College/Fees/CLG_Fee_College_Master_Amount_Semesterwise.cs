using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Master_Amount_Semesterwise", Schema = "CLG")]
    public class CLG_Fee_College_Master_Amount_Semesterwise :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FCMAS_Id { get; set; }
        public long MI_Id { get; set; }
        public long FCMA_Id { get; set; }
        public long AMSE_Id { get; set; }
        public decimal FCMAS_Amount { get; set; }
        public string FCMAS_Currency { get; set; }
        public bool FCMAS_ActiveFlg { get; set; }

        public DateTime? FCMAS_DueDate { get; set; }
        public List<CLG_Fee_College_T_Fine_Slabs> CLG_Fee_College_T_Fine_Slabs { get; set; }
        public List<CLG_Fee_College_T_Due_DateDMO> CLG_Fee_College_T_Due_DateDMO { get; set; }


    }
}
