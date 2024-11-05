using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Master_Documents")]
    public class StudentUploadImage : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASMD_Id { get; set; }
        public string PASMD_DocumentName { get; set; }
        public string PASMD_Path { get; set; }
    }
}
