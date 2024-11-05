using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
  [Table("ISM_Resignation_ChecKLists")]
    public class ISM_Resignation_ChecKLists_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public long ISMRESGMCL_Id { get; set; }
        public string ISMRESGCL_FileName { get; set; }
        public string ISMRESGCL_FilePath { get; set; }
        public bool ISMRESGCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGCL_CreatedBy { get; set; }
        public long? ISMRESGCL_UpdatedBy { get; set; }
    }
}
