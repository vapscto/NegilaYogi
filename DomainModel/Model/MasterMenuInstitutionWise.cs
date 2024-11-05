using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Menu_Institutionwise")]
    public class MasterMenuInstitutionWise : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMMI_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMMI_MenuName { get; set; }
        public long IVRMM_Id { get; set; }
        public long IVRMMMI_ParentId { get; set; }
        public bool IVRMMMI_PageNonPageFlag { get; set; }
        public int IVRMMMI_MenuOrder { get; set; }
        public long IVRMMM_Id { get; set; }

        public string IVRMMMI_Icon { get; set; }
        public string IVRMMMI_Color { get; set; }
    }
}
