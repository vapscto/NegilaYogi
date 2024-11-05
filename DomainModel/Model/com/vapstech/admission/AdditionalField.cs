using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Page_Additional_Fields")]
    public class AdditionalField:CommonParamDMO
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IPAF_Id { get; set; }
        public long Page_Id { get; set; }
        public int IPAF_Flag { get; set; }
        public string IPAF_Name { get; set; }
        public string IPAF_Type { get; set; }
        public decimal IPAF_Size { get; set; }
        public decimal IPAF_Scale { get; set; }
        public int IPAF_Apl_Report { get; set; }
        public string IPAF_Display_Name { get; set; }
        public int IPAF_Active_Flag { get; set; }
        public long MI_Id { get; set; }
    }

}

