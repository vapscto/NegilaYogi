using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_ServiceStation", Schema = "TRN")]
    public class TR_Master_ServStationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMSST_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMSST_ServiceStationName { get; set; }
        public long TRMSST_ContactNo { get; set; }
        public string TRMSST_EmailId { get; set; }
        public string TRMSST_Address { get; set; }
        public bool TRMSST_ActiveFlag { get; set; }
        public long TRMSST_CreatedBy { get; set; }
        public long TRMSST_UpdatedBy { get; set; }
    }
}
