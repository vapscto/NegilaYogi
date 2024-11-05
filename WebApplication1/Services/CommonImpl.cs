using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainModel;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class CommonImpl : Interfaces.CommonInterface
    {
        private readonly DomainModelMsSqlServerContext _db;

        public CommonImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        public async Task<CommonDTO> getPagePreviledges(CommonDTO ct)
        {
            CommonDTO cdto = new CommonDTO();
            var a1 = await (from rolePlvg in _db.Role_Privileges
                            from mod_rol in _db.masterPageModuleMapping
                            where (rolePlvg.IVRMMP_Id == mod_rol.IVRMMP_Id && mod_rol.IVRMP_Id == ct.pageId && rolePlvg.IVRMRT_Id == ct.roleId)
                            select new CommonDTO {
                                 pageid = mod_rol.IVRMP_Id,
                                 IVRMRP_AddFlag = rolePlvg.IVRMRP_AddFlag, IVRMRP_UpdateFlag = rolePlvg.IVRMRP_UpdateFlag,
                                 IVRMRP_DeleteFlag = rolePlvg.IVRMRP_DeleteFlag, IVRMRP_ProcessFlag = rolePlvg.IVRMRP_ProcessFlag,
                                 IVRMRP_ReportFlag = rolePlvg.IVRMRP_ReportFlag
                            }
                            ).Distinct().ToListAsync();
            cdto.pagePreviledgs = a1.ToArray();
            return cdto;
        }
    }
}
