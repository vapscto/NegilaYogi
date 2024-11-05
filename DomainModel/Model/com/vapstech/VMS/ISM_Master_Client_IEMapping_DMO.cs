using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VMS
{
    [Table("ISM_Master_Client_IEMapping")]
    public class ISM_Master_Client_IEMapping_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMCLTIE_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMCIM_IEList { get; set; }
        public bool ISMMCLTIE_ActiveFlag { get; set; }
        public long ISMMCLTIE_CreatedBy { get; set; }
        public long ISMMCLTIE_UpdatedBy { get; set; }

    }
}
