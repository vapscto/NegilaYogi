using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Appointment_Files", Schema = "VM")]
    public class Visitor_Management_Appointment_FilesDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMAPVF_Id { get; set; }
        public long VMAP_Id { get; set; }
        public string VMAPVF_FileName { get; set; }
        public string VMAPVF_FilePath { get; set; }
        public string VMAPVF_Filedesc { get; set; }
        public bool VMAPVF_ActiveFlg { get; set; }
        public long VMAPVF_CreatedBy { get; set; }
        public DateTime VMAPVF_CreatedDate { get; set; }
        public long VMAPVF_UpdatedBy { get; set; }
        public DateTime VMAPVF_Updateddate { get; set; }
        
    }
}
