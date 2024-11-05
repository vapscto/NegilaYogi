using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumini_details", Schema = "ALU")]
    public class Alumini_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALMST_DETAILS_ID { get; set; }
        public long ALMST_ID { get; set; }
        public string ALMST_PUC_QS_DETAILS { get; set; }
        public string ALMST_PUC_INS_NAME { get; set; }
        public string ALMST_PUC_PASSED_OUT { get; set; }
        public string ALMST_PUC_PERCENTAGE { get; set; }
        public string ALMST_PUC_PLACE { get; set; }
        public string ALMST_PUC_STATE { get; set; }
        public string ALMST_UG_QS_DETAILS { get; set; }
        public string ALMST_UG_INS_NAME { get; set; }
        public string ALMST_UG_PASSED_OUT { get; set; }
        public string ALMST_UG_PERCENTAGE { get; set; }
        public string ALMST_UG_PLACE { get; set; }
        public string ALMST_UG_STATE { get; set; }
        public string ALMST_PG_QS_DETAILS { get; set; }

        public string ALMST_PG_INS_NAME { get; set; }
        public string ALMST_PG_PASSED_OUT { get; set; }
        public string ALMST_PG_PERCENTAGE { get; set; }
        public string ALMST_PG_PLACE { get; set; }
        public string ALMST_PG_STATE { get; set; }
        public string ALMST_ACH_DET { get; set; }
        public string ALMST_ACH_REMARKS { get; set; }
        public string ALMST_PRO_COMPANY_NAME { get; set; }

        public string ALMST_PRO_DESIGNATION { get; set; }
        public string ALMST_PRO_OFFICE_NO { get; set; }
        public string ALMST_PRO_ADDRESS { get; set; }
        public string ALMST_PRO_REMARKS { get; set; }

    }
}


