using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Client_Project_Docs")]
    public class ISM_Client_Project_Docs_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTPRDOC_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long ISMCLTPRMDOC_Id { get; set; }
        public bool ISMCLTPRDOC_ActiveFlag { get; set; }
        public long ISMCLTPRDOC_CreatedBy { get; set; }
        public long ISMCLTPRDOC_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime ISMCLTPRDOC_Date { get; set; }
        public string ISMCLTPRDOC_FileName { get; set; }
        public string ISMCLTPRDOC_FilePath { get; set; }
    }
}
