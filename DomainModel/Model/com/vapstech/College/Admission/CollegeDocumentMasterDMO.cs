using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_m_School_Master_Documents")]
    public class CollegeDocumentMasterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMSMD_DocumentName { get; set; }
        public string AMSMD_Description { get; set; }
        public bool AMSMD_FLAG { get; set; }
        public bool? AMSMD_ToUploadFlg { get; set; }
        public string AMSMD_FileName { get; set; }
        public string AMSMD_FilePath { get; set; }
       

    }
}
