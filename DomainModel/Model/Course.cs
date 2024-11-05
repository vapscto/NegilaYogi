using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("adm_M_course")]
    public class Course : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AMCO_Id { get; set; }
        public string AMCO_Name { get; set; }
        public string AMCO_Details { get; set; }
        public string AMCo_Flag { get; set; }
        public string AMCO_Code { get; set; }
        public int AMCO_Min_Atten_Per { get; set; }
        public int AMCO_No_Year { get; set; }
        public int AMCO_Fee_App_Type { get; set; }
        public int AMC_ID { get; set; }
        public string AMC_MOS { get; set; }
        public int AMCO_Order { get; set; }

    }
}
