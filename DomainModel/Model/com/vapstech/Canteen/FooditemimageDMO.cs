using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("IVRM_Canteen_Attatchment_Item")]
    public  class FooditemimageDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ICAI_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public string ICAI_FileName { get; set; }
        public string ICAI_Attachment { get; set; }
        public bool ICAI_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ICAI_CreatedBy { get; set; }
        public long ICAI_UpdatedBy { get; set; }


    }
}
