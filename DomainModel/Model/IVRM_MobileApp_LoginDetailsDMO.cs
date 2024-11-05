using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model
{
    [Table("IVRM_MobileApp_LoginDetails")]
    public class IVRM_MobileApp_LoginDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMMALD_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public DateTime? IVRMMALD_DateTime { get; set; }
        public string IVRMMALD_logintype { get; set; }
        public string IVRMMALD_MobileModel { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? IVRMMALD_CreatedBy { get; set; }
        public long? IVRMMALD_UpdatedBy { get; set; }
    }
}
