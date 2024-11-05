using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("FO_Inward", Schema = "VM")]
    public class FO_Inward_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOIN_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOIN_InwardNo { get; set; }
        public DateTime? FOIN_DateTime { get; set; }
        public string FOIN_From { get; set; }
        public string FOIN_Adddress { get; set; }
        public string FOIN_ContactPerson { get; set; }
        public string FOIN_PhoneNo { get; set; }
        public string FOIN_EmailId { get; set; }
        public string FOIN_Discription { get; set; }
        public long FOIN_To { get; set; }
        public long FOIN_ReceivedBy { get; set; }
        public long FOIN_HandedOverTo { get; set; }
        public bool FOIN_ActiveFlag { get; set; }
        public long FOIN_CreatedBy { get; set; }
        public long FOIN_UpdatedBy { get; set; }

    }
}
