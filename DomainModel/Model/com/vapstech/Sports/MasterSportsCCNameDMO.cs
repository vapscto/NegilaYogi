using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_SportsCCName", Schema ="SPC")]
    public class MasterSportsCCNameDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMSCC_Id { get; set; }
        public long SPCCMSCCG_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }
        public string SPCCMSCC_SportsCCDesc { get; set; }
        public string SPCCMSCC_SGFlag { get; set; }
        public int SPCCMSCC_NoOfMembers { get; set; }
        public string SPCCMSCC_RecHighLowFlag { get; set; }
        public string SPCCMSCC_RecInfo { get; set; }
        public bool SPCCMSCC_ActiveFlag { get; set; }
        public bool? SPCCMSCC_MultiAttemptFlg { get; set; }
        public int? SPCCMSCC_NoOfAttempts { get; set; }
    }
}
