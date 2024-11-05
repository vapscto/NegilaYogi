using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Room_Files")]
    public class HR_Master_Room_FilesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRFI_Id { get; set; }
        public long HRMR_Id { get; set; }
        public string HRMRFI_FileName { get; set; }
        public string HRMRFI_FilePath { get; set; }
        public bool HRMRFI_ActiveFlag { get; set; }
        public DateTime HRMRFI_CreatedDate { get; set; }
        public DateTime HRMRFI_UpdatedDate { get; set; }
        public long HRMRFI_CreatedBy { get; set; }
        public long HRMRFI_UpdatedBy { get; set; }
        public bool HRMRFI_DefFlg { get; set; }
        
    }
}
