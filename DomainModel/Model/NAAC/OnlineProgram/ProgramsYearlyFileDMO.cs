using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Yearly_File")]
    public class ProgramsYearlyFileDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PRYRF_Id { get; set; }
        [ForeignKey("PRYR_Id")]
        public long PRYR_Id { get; set; }
        public string PRYRF_FileName { get; set; }
        public string PRYRF_FileType { get; set; }
        public string PRYRF_FilePath { get; set; }
        public bool PRYRF_ActiveFlag { get; set; }
        public long PRYRF_CreatedBy { get; set; }
        public long PRYRF_UpdatedBy { get; set; }


    }
}
