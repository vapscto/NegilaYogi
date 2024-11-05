using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("VM_Master_Visitor_File", Schema = "VM")]
    public class VM_Master_Visitor_FileDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMMVFL_Id { get; set; }
        public long VMMV_Id { get; set; }
        public string VMMVFL_FileName { get; set; }
        public string VMMVFL_FilePath { get; set; }
        public string VMMVFL_FileRemarks { get; set; }
        public bool? VMMVFL_ActiveFlg { get; set; }
        public long? VMMVFL_CreatedBy { get; set; }
        public DateTime? VMMVFL_CreatedDate { get; set; }
        public long? VMMVFL_UpdatedBy { get; set; }
        public DateTime? VMMVFL_Updateddate { get; set; }
    }
}
