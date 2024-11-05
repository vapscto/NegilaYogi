using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Visitor_Appointment", Schema = "VM")]
    public class Visitor_Management_Visitor_Appointment_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMVAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long? VMMV_Id { get; set; }
        public long? VMAP_Id { get; set; }
    }
}
