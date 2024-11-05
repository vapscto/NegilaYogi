using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Master_SL_File")]
    public class NAAC_Master_SL_FileDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACMSLF_Id { get; set; }
        public long NAACMSL_Id { get; set; }
        public string NAACMSLF_FileName { get; set; }
        public string NAACMSLF_FilePath { get; set; }
        public DateTime? NAACMSLF_UploadDate { get; set; }
        public string NAACMSLF_FileRemarks { get; set; }
        public string NAACMSLF_FileStatus { get; set; }
        public bool NAACMSLF_ActiveFlag { get; set; }
        public long NAACMSLF_CreatedBy { get; set; }
        public DateTime NAACMSLF_CreatedDate { get; set; }
        public long NAACMSLF_UpdatedBy { get; set; }
        public DateTime NAACMSLF_UpdatedDate { get; set; }
        public List<NAAC_Master_SL_File_CommentsDMO> NAAC_Master_SL_File_CommentsDMO { get; set; }
    }
}
