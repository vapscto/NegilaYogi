using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.HRMS
{

    [Table("HR_Employee_TDS_Quarter")]
    public class HR_Employee_TDS_Quarter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRETDSQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMQ_Id { get; set; }
        public string HRETDSR_ReceiptNo { get; set; }
        public decimal? HRETDS_AmountPaid { get; set; }
        public bool HRETDSQ_ActiveFlg { get; set; }
        public long HRETDSQ_CreatedBy { get; set; }
        public long HRETDSQ_UpdatedBy { get; set; }
        public decimal? HRETDSQ_TaxDeposited { get; set; }

    }
}

