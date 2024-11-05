using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Student_Documents")]
    public class StudentDocumentDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTD_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTD_DOC_Path { get; set; }
        public string AMSTD_DOC_Name { get; set; }
        public long AMSMD_Id { get; set; }
        public long? AMSTD_CreatedBy { get; set; }
        public long? AMSTD_UpdatedBy { get; set; }        
    }
}