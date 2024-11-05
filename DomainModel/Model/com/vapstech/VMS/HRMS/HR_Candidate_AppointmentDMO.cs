using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Appointment")]
    public class HR_Candidate_AppointmentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRCA_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCA_FirstDocName { get; set; }
        public string HRCA_SecDocName { get; set; }
        public DateTime HRCA_FirstDocDate { get; set; }
        public DateTime HRCA_SecDocDate { get; set; }
        public decimal HRCA_AnnualCTC { get; set; }
        public decimal HRCA_MonthlyCTC { get; set; }
        public string HRCA_AppointmentRefNo { get; set; }
        public string HRCA_AcknowledgementRefNo { get; set; }
    }

}