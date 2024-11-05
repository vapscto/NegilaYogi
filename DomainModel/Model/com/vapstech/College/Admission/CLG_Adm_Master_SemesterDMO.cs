using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_Semester", Schema = "CLG")]
    public class CLG_Adm_Master_SemesterDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMSE_Id { get; set; }
       // [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string AMSE_SEMName { get; set; } 
        public string AMSE_SEMInfo { get; set; }
        public string AMSE_SEMCode { get; set; }
        public string AMSE_SEMTypeFlag { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public int AMSE_Year { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public bool AMSE_ActiveFlg { get; set; }

    }
}
