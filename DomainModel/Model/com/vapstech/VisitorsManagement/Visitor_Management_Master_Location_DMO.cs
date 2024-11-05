using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    
    [Table("Visitor_Management_Master_Location", Schema = "VM")]
    public class Visitor_Management_Master_Location_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMML_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMML_Location { get; set; }
        public string VMML_LocationDescription { get; set; }
        public string VMML_LocationFacilities { get; set; }
        public bool VMML_ActiveFlg { get; set; }
        public long? VMML_CreatedBy { get; set; }
        public long? VMML_UpdatedBy { get; set; }


    }
}
