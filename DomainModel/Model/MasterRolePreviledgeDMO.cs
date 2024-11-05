using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Role_Privileges")]
    public class MasterRolePreviledgeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMRP_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long IVRMMP_Id { get; set; }
        public bool IVRMRP_AddFlag { get; set; }
        public bool IVRMRP_UpdateFlag { get; set; }
        public bool IVRMRP_DeleteFlag { get; set; }
        public bool IVRMRP_ProcessFlag { get; set; }
        public bool IVRMRP_ReportFlag { get; set; }
      
       // public MasterPageModuleMapping masterPageMapping { get; set; }

        //public MasterRoleType masterRoleType { get; set; }



        //  public MasterPageModuleMapping[] masterPageModuleMapping { get; set; }
        //  public MasterPage masterPage { get; set; }


        //public MasterModule mastermodule { get; set; }

        //public int IVRMP_Id { get; set; }
        //public virtual MasterPage masterpage { get; set; }

        //public int IVRMM_Id { get; set; }
        //public virtual MasterModule mastermodule { get; set; }

    }
}
