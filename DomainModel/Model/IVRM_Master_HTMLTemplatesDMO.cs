using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_HTMLTemplates")]
    public class IVRM_Master_HTMLTemplatesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  ISMHTML_Id { get; set; }
        public long MI_Id { get; set; }
        public string  ISMHTML_HTMLName { get; set; }
        public string ISMHTML_HTMLTemplate { get; set; }
        public bool  ISMHTML_ActiveFlg { get; set; }
        public long  ISMHTML_CreatedBy { get; set; }
        public long  ISMHTML_UpdatedBy { get; set; }
        public DateTime  ISMHTML_CreatedDate { get; set; }
        public DateTime ISMHTML_UpdatedDate { get; set; }
    }
}
