                                               using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Age_Calculation", Schema = "SPC")]
    public class StudentAgeCalcDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long SPCCAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int Age_Years { get; set; }
        public int Age_Months { get; set; }
        public int Age_Days { get; set; }
        public DateTime Till_Date { get; set; }

    }
}
