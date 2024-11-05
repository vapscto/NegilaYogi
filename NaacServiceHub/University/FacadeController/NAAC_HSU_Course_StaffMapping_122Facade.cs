using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class NAAC_HSU_Course_StaffMapping_122Facade : Controller
    {

        public NAAC_HSU_Course_StaffMapping_122Interface _inter;
        public NAAC_HSU_Course_StaffMapping_122Facade(NAAC_HSU_Course_StaffMapping_122Interface i)
        {
            _inter = i;
        }

        [HttpPost]
        [Route("loaddata")]
        public NAAC_HSU_Course_StaffMapping_122DTO loaddata([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_HSU_Course_StaffMapping_122DTO save([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public NAAC_HSU_Course_StaffMapping_122DTO deactive([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public NAAC_HSU_Course_StaffMapping_122DTO EditData([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_HSU_Course_StaffMapping_122DTO deleteuploadfile([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_HSU_Course_StaffMapping_122DTO viewuploadflies([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.viewuploadflies(data);
        }
        [Route("get_course")]
        public NAAC_HSU_Course_StaffMapping_122DTO get_course([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.get_course(data);
        }
        [Route("get_designation")]
        public NAAC_HSU_Course_StaffMapping_122DTO get_designation([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.get_designation(data);
        }
        [Route("get_employee")]
        public NAAC_HSU_Course_StaffMapping_122DTO get_employee([FromBody] NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return _inter.get_employee(data);
        }
    }
}
