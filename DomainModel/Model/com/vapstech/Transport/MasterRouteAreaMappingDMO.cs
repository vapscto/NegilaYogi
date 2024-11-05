using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Area_Route", Schema = "TRN")]
    public class MasterRouteAreaMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRAR_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRMA_Id { get; set; }
        public bool TRAR_ActiveFlg { get; set; }
        public DateTime? TRAR_CreatedDate { get; set; }
        public DateTime? TRAR_UpdatedDate { get; set; }
        public long? TRAR_CreatedBy { get; set; }
        public long? TRAR_UpdatedBy { get; set; }

    }
}
