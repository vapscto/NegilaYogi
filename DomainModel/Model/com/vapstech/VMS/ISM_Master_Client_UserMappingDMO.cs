using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VMS
{
    [Table("ISM_Master_Client_UserMapping")]
    public class ISM_Master_Client_UserMappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMMCLTUS_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMCIM_IEList { get; set; }
        public bool ISMMCLTUS_ActiveFlag { get; set; }
        public long ISMMCLTUS_CreatedBy { get; set; }
        public long ISMMCLTUS_UpdatedBy { get; set; }
    }
}
