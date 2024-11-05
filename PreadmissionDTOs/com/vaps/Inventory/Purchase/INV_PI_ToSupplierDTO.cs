using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_PI_ToSupplierDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string sms { get; set; }
        public string email { get; set; }
        public string atchtempl { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public long INVPITS_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVPITS_SupplierName { get; set; }
        public long INVPITS_ContactNo { get; set; }
        public string INVPITS_EmailId { get; set; }
        public DateTime INVPITS_SMSSentDate { get; set; }
        public DateTime INVPITS_MailSentDate { get; set; }
        public bool INVPITS_ActiveFlg { get; set; }
        public long INVPITS_CreatedBy { get; set; }
        public long INVPITS_UpdatedBy { get; set; }

        public Array get_supplier { get; set; }
        public Array get_piNo { get; set; }
        public Array get_pidetails { get; set; }

        public supplierArrayDTO[] supplierArray { get; set; }
    }
    public class supplierArrayDTO
    {
        public long INVMPI_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string INVPITS_SupplierName { get; set; }
        public long INVPITS_ContactNo { get; set; }
        public string INVPITS_EmailId { get; set; }

    }

}
