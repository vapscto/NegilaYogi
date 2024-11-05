using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Programs_112_File_Comments")]
    public class NAAC_AC_Programs_112_File_Comments_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACPR112FC_Id { get; set; }
        public long NCACPR112F_Id { get; set; }
        public string NCACPR112FC_Remarks { get; set; }
        public long NCACPR112FC_RemarksBy { get; set; }
        public bool NCACPR112FC_ActiveFlag { get; set; }
        public long? NCACPR112FC_CreatedBy { get; set; }
        public DateTime? NCACPR112FC_CreatedDate { get; set; }
        public long? NCACPR112FC_UpdatedBy { get; set; }
        public DateTime? NCACPR112FC_UpdatedDate { get; set; }
        public string NCACPR112FC_StatusFlg { get; set; }

    }
}
