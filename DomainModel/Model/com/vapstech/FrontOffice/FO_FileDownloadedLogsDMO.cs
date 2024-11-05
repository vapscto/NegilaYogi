using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_FileDownloadedLogs", Schema = "FO")]
    public class FO_FileDownloadedLogsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FODLL_Id { get; set; }
        public long MI_Id { get; set; }
        public string FODLL_Date { get; set; }
        public string FODLL_time { get; set; }
        public string FODLL_JSONData { get; set; }
        
    }
}
