using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_351_CollaborationActivities")]
    public class NAAC_MC_351_CollaborationActivitiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCMC351CA_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC351CA_Year { get; set; }
        public string NCMC351CA_AgencyName { get; set; }
        public string NCMC351CA_ActivityTitle { get; set; }
        public string NCMC351CA_AgencyContactDetails { get; set; }
        public string NCMC351CA_ParticipantsNames { get; set; }
        public string NCMC351CA_SourceOfFinacialSupport { get; set; }
        public string NCMC351CA_Duration { get; set; }
        public string NCMC351CA_NatureOfActivity { get; set; }
        public string NCMC351CA_LinkDocument { get; set; }
        public bool NCMC351CA_ActiveFlag { get; set; }
        public long NCMC351CA_CreatedBy { get; set; }
        public long NCMC351CA_UpdatedBy { get; set; }
        public DateTime? NCMC351CA_CreatedDate { get; set; }
        public DateTime? NCMC351CA_UpdatedDate { get; set; }
        public List<NAAC_MC_351_CollaborationActivities_FilesDMO> NAAC_MC_351_CollaborationActivities_FilesDMO { get; set; }

    }
}
