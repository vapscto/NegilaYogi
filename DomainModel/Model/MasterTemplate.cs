using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Templates")]
    public class MasterTemplate : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMT_Id { get; set; }
        [ForeignKey("IVRMP_Id")]
        public long IVRMP_Id { get; set; }
        public string IVRMT_Name { get; set; }
        public string IVRMT_Description { get; set; }
        public bool Is_Active { get; set; }
    }
}
