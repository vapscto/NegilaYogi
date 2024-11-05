using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRS_Master_Languages")]
    public class IVRS_Master_LanguagesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMLA_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMLA_VirtualNo { get; set; }
        public string IMLA_SchoolURL { get; set; }
        public string IMLA_SchoolName { get; set; }
        public string IMLA_Language { get; set; }
        public int IMLA_LanguageOrder { get; set; }
        public bool IMLA_ActiveFlg { get; set; }
        public DateTime? IMLA_CreatedDate { get; set; }
        public DateTime? IMLA_UpdatedDate { get; set; }
        public string IMLA_CreatedBy { get; set; }
        public string IMLA_UpdatedBy { get; set; }
        
    }
}
