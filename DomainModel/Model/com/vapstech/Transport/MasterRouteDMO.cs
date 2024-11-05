using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Route", Schema = "TRN")]
    public class MasterRouteDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMR_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMA_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }
        public int TRMR_order { get; set; }
    }
}
