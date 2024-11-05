using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("TSAT_Registration")]
    public class SatRegistrationDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRE_Id { get; set; }
        public string PASRE_FullName { get; set; }
        public string PASRE_EmailId { get; set; }
        public string PASRE_FatherName { get; set; }
        public string PASRE_SchoolName { get; set; }
        public string PASRE_Gender { get; set; }
        public string PASRE_Address { get; set; }
        public long PASRE_MobileNo { get; set; }
        public long PASRE_WhatsappNo { get; set; }
        public long MI_Id { get; set; }
    }
}
