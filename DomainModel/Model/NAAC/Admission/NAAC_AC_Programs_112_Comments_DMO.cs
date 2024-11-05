using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Programs_112_Comments")]
    public class NAAC_AC_Programs_112_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACPR112C_Id { get; set; }
        public long NCACPR112_Id { get; set; }
        public string NCACPR112C_Remarks { get; set; }
        public long NCACPR112C_RemarksBy { get; set; }
        public string NCACPR112C_StatusFlg { get; set; }
        public bool NCACPR112C_ActiveFlag { get; set; }        
        public long? NCACPR112C_CreatedBy { get; set; }
        public DateTime? NCACPR112C_CreatedDate { get; set; }
        public DateTime? NCACPR112C_UpdatedDate { get; set; }
        public long? NCACPR112C_UpdatedBy { get; set; }


    }
}
