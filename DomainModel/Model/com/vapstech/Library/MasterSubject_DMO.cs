using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Subject", Schema = "LIB")]
    public class MasterSubject_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string LMS_SubjectNo { get; set; }
        public long LMS_ParentId { get; set; }
        public long? LMS_Level { get; set; }
        public bool LMS_ActiveFlg { get; set; }
        public string LMS_ClassNo { get; set; }
        public List<BookRegisterDMO> BookRegisterDMO { get; set; }
    }
}
