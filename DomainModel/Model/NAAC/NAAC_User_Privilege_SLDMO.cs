using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC
{
    [Table("NAAC_User_Privilege_SL")]
    public class NAAC_User_Privilege_SLDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACUPRISL_Id { get; set; }
        public long NAACUPRI_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public bool NAACUPRISL_ActiveFlag { get; set; }
        public long NAACUPRISL_CreatedBy { get; set; }
        public DateTime? NAACUPRISL_CreatedDate { get; set; }
        public long NAACUPRISL_UpdatedBy { get; set; }
        public DateTime? NAACUPRISL_UpdatedDate { get; set; }
    }
}
