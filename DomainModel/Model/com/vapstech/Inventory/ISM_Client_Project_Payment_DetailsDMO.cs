using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Client_Project_Payment_Details")]
    public class ISM_Client_Project_Payment_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCPPD_Id { get; set; }
        public long ISMCLTPRP_Id { get; set; }
        public long? HRMBD_Id { get; set; }
        public decimal? ISMCPPD_ReceivedAmount { get; set; }
        public DateTime? ISMCPPD_ReceivedDate { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string ISMCPPD_PaymentRefNo { get; set; }
        public string ISMCPPD_Remarks { get; set; }
        public DateTime? ISMCPPD_ChequeDate { get; set; }
        public long ISMCPPD_CreatedBy { get; set; }
        public long ISMCPPD_UpdatedBy { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
        public List<VMS_Receipt_VoucherDMO> VMS_Receipt_VoucherDMO { get; set; }
    }
}
