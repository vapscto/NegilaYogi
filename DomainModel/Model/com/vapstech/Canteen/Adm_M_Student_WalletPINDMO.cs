using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("Adm_M_Student_WalletPIN")]
    public  class Adm_M_Student_WalletPINDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMCSTW_Id { get; set; }
        public long MI_Id { get; set; }
        public long Amst_Id { get; set; }
        public string AMCSTW_WalletPIN { get; set; }
        public DateTime? AMCSTW_CreatedDate { get; set; }
        public DateTime? AMCSTW_UpdatedDate { get; set; }
        public long? AMCSTW_CreatedBy { get; set; }
        public long? AMCSTW_UpdatedBy { get; set; }
    }
}
