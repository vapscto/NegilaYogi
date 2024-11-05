using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_345_TeacherResearchPapers_Files")]
   public class NAAC_HSU_345_TeacherResearchPapers_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU345TRPF_Id { get; set; }
        public long NCHSU345TRP_Id { get; set; }
        public string NCHSU345TRPF_FileDesc { get; set; }
        public string NCHSU345TRPF_FileName { get; set; }
        public string NCHSU345TRPF_FilePath { get; set; }
        public bool NCHSU345TRPF_ActiveFlg { get; set; }
        public DateTime NCHSU345TRPF_CreatedDate { get; set; }
        public DateTime NCHSU345TRPF_UpdatedDate { get; set; }
        public long NCHSU345TRPF_CreatedBy { get; set; }
        public long NCHSU345TRPF_UpdatedBy { get; set; }
    }
}
