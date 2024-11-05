using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider /*DomainModel.Model *//*DataAccessMsSqlServerProvider*/
{
    [Table("ApplicationUserRole")]
    public class ApplicationUserRole : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public long RoleTypeId { get; set; }
    }
}
