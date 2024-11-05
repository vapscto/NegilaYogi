using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.OnlineProgram
{
    [Table("Programs_Yearly_Activities")]
    public class ProgramsYearlyActivitiesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PRYRA_Id { get; set; }
        [ForeignKey("PRYR_Id")]
        public long PRYR_Id { get; set; }
        public string PRYRA_ActivityName { get; set; }
        public string PRYRA_StartTime { get; set; }
        public string PRYRA_Duration { get; set; }
        public string PRYRA_Description { get; set; }
        public bool PRYRA_ActiveFlag { get; set; }
        public long PRYRA_CreatedBy { get; set; }
        public long PRYRA_UpdatedBy { get; set; }


    }
}
