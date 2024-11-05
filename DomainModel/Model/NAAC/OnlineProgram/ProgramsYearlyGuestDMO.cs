using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Yearly_Guest")]
    public class ProgramsYearlyGuestDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PRYRG_Id { get; set; }
        [ForeignKey("PRYR_Id")]
        public long PRYR_Id { get; set; }
        public string PRYRG_GuestType { get; set; }
        public string PRYRG_GuestName { get; set; }
        public string PRYRG_GuestPhoneNo { get; set; }
        public string PRYRG_GuestEmailId { get; set; }
        public bool PRYRG_ActiveFlag { get; set; }
        public long PRYRG_CreatedBy { get; set; }
        public long PRYRG_UpdatedBy { get; set; }
        public string PRYRG_GuestAddress { get; set; }
        public string PRYRG_GuestProfileFileName { get; set; }
        public string PRYRG_GuestProfileFilePath { get; set; }
        public string PRYRG_GuestSpeechName { get; set; }
        public string PRYRG_GuestSpeechFilePath { get; set; }
        public string PRYRG_GuestPhotoVideoPath { get; set; }
        public string PRYRG_GuestPhotoVideo { get; set; }
        
    }
}
