using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_346_EmpApprovedJournalLists_Files")]
   public class HSU_346_EmpApprovedJournalLists_Files_DMO
    {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU346EAJLF_Id { get; set; }
        public long NCHSU346EAJL_Id { get; set; }
        public string NCHSU346EAJLF_FileDesc { get; set; }
        public string NCHSU346EAJLF_FileName { get; set; }
        public string NCHSU346EAJLF_FilePath { get; set; }
        public bool NCHSU346EAJLF_ActiveFlg { get; set; }
        public DateTime? NCHSU346EAJLF_CreatedDate { get; set; }
        public DateTime? NCHSU346EAJLF_UpdatedDate { get; set; }
        public long NCHSU346EAJLF_CreatedBy { get; set; }
        public long NCHSU346EAJLF_UpdatedBy { get; set; }
    }
}
