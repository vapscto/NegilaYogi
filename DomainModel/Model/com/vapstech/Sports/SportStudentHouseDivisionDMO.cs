using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{

    [Table("SPCC_Student_House", Schema = "SPC")]
    public class SportStudentHouseDivisionDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCSH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        //public DateTime? SPCCSH_AsOnDate { get; set; }
        public long ASMS_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long AMST_Id { get; set; }
        public string SPCCSH_Remarks { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }
        public DateTime? SPCCSH_Date { get; set; }
        public string SPCCSH_Age { get; set; }
        public string SPCCSH_Age_Format { get; set; }

        //public string SPCCSH_Age { get; set; }
        //public decimal? SPCCSH_Height { get; set; }
        //public decimal? SPCCSH_Weight { get; set; }
        //public decimal? SPCCSH_BMI { get; set; }
        //public string SPCCSH_BMIRemarks { get; set; }



    }
}
