using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Appointment_Visitors", Schema = "VM")]
    public class Visitor_Management_Appointment_VisitorsDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMAPVI_Id { get; set; }

        public long VMAP_Id { get; set; }
        public string VMAPVI_VisitorName { get; set; }

        public string VMAPVI_VisitorAddress { get; set; }

        public string VMAPVI_VisitorEmailId { get; set; }

        public string VMAPVI_VisitorContactNo { get; set; }

        public string VMAPVI_Remarks { get; set; }

        public long VMAPVI_CreatedBy { get; set; }

        public DateTime VMAPVI_CreatedDate { get; set; }
        public long VMAPVI_UpdatedBy { get; set; }
        public DateTime VMAPVI_Updateddate { get; set; }
    }
}
