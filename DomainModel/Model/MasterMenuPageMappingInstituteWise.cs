using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Menu_Page_Mapping_Institutionwise")]
    public class MasterMenuPageMappingInstituteWise : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMMPMI_Id { get; set; }
        public long IVRMMMI_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMMPMI_PageDisplayName { get; set; }
    }
}
