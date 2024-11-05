using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Configuration", Schema = "LIB")]
    public class LIB_ConfigurationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string LC_NewArrivalPeriod { get; set; }
        public bool? LC_AccnNoAutoGenFlg { get; set; }
        public long? LC_CreatedBy { get; set; }
        public long? LC_UpdatedBy { get; set; }
        public bool? LC_CommonLibraryFlg { get; set; }
    }
}
