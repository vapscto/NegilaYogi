using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PreadmissionDTOs
{
    [Table("IVRM_Custom_UserName_Password")]
    public class IVRM_Custom_UserName_PasswordDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMCUNP_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMCUNP_Field { get; set; }
        public long IVRMCUNP_Length { get; set; }
        public string IVRMCUNP_FromOrderFlg { get; set; }
        public long IVRMCUNP_Order { get; set; }
        public bool IVRMCUNP_ActiveFlag { get; set; }

        public DateTime? IVRMCUNP_CreatedDate { get; set; }
        public DateTime? IVRMCUNP_UpdatedDate { get; set; }
        public long IVRMCUNP_CreatedBy { get; set; }
        public long IVRMCUNP_UpdatedBy { get; set; }


    }
}
