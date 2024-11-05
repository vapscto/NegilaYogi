using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRM_Configuration_URL")]
    public class IVRM_Configuration_URLDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IIVRSCURL_Id  { get; set; }
        public long IIVRSC_Id { get; set; }
        public string IIVRSCURL_APIName { get; set; }
        public string IIVRSCURL_APIURL { get; set; }
    }
}
