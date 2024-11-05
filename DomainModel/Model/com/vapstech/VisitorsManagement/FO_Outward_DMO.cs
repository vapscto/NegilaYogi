using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("FO_Outward", Schema = "VM")]
    public class FO_Outward_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOOUT_Id { get; set; }
        public long MI_Id { get; set; }
        public string  FOOUT_OutwardNo { get; set; }
        public DateTime FOOUT_DateTime { get; set; }
        public string FOOUT_Discription { get; set; }
        public string FOOUT_From { get; set; }
        public string FOOUT_To { get; set; }
        public string FOOUT_Address { get; set; }
        public long? FOOUT_PhoneNo { get; set; }
        public string FOOUT_EmailId { get; set; }
        public long FOOUT_DispatachedBy { get; set; }
        public string FOOUT_DispatchedThrough { get; set; }
        public string FOOUT_DispatchedDeatils { get; set; }
        public string FOOUT_DispatchedPhNo { get; set; }
        public bool FOOUT_ActiveFlag { get; set; }
        public long FOOUT_CreatedBy { get; set; }
        public long FOOUT_UpdatedBy { get; set; }

    }
}
