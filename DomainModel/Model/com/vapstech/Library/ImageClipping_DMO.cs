using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NewsPaper_Clippings", Schema = "LIB")]
    public class ImageClipping_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNPCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string LNPCL_ClipName { get;set; }
        public string LNPCL_ClipImage { get; set; }
        public string LNPCL_FilePath { get; set; }
        public string LNPCL_ClipDetails { get; set; }
        public bool LNPCL_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
       
    }
}
