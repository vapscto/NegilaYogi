using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthManagement.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.HealthManagement;

namespace HealthManagement.Controllers
{

    [Route("api/[controller]")]
    public class HM_MasterFacadeController : Controller
    {
        HM_MasterInterfaces _inter;
        public HM_MasterFacadeController(HM_MasterInterfaces inter)
        {
            _inter = inter;
        }

        // Master Behaviour
        [Route("load_MB")]
        public Master_HealthManagementDTO load_MB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.load_MB(dto);
        }

        [Route("Save_MB")]
        public Master_HealthManagementDTO Save_MB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_MB(dto);
        }

        [Route("Edit_MB")]
        public Master_HealthManagementDTO Edit_MB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_MB(dto);
        }

        [Route("ActiveDeactive_MB")]
        public Master_HealthManagementDTO ActiveDeactive_MB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_MB(dto);
        }

        // Master Cleanness
        [Route("load_CL")]
        public Master_HealthManagementDTO load_CL([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.load_CL(dto);
        }

        [Route("Save_CL")]
        public Master_HealthManagementDTO Save_CL([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_CL(dto);
        }

        [Route("Edit_CL")]
        public Master_HealthManagementDTO Edit_CL([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_CL(dto);
        }

        [Route("ActiveDeactive_CL")]
        public Master_HealthManagementDTO ActiveDeactive_CL([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_CL(dto);
        }

        // Master Doctor
        [Route("load_DC")]
        public Master_HealthManagementDTO load_DC([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.load_DC(dto);
        }

        [Route("Save_DC")]
        public Master_HealthManagementDTO Save_DC([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_DC(dto);
        }

        [Route("Edit_DC")]
        public Master_HealthManagementDTO Edit_DC([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_DC(dto);
        }

        [Route("ActiveDeactive_DC")]
        public Master_HealthManagementDTO ActiveDeactive_DC([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_DC(dto);
        }

        // Master Examination
        [Route("load_EX")]
        public Master_HealthManagementDTO load_EX([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.load_EX(dto);
        }

        [Route("Save_EX")]
        public Master_HealthManagementDTO Save_EX([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_EX(dto);
        }

        [Route("Edit_EX")]
        public Master_HealthManagementDTO Edit_EX([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_EX(dto);
        }

        [Route("ActiveDeactive_EX")]
        public Master_HealthManagementDTO ActiveDeactive_EX([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_EX(dto);
        }

        // Master Observation
        [Route("load_OB")]
        public Master_HealthManagementDTO load_OB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.load_OB(dto);
        }

        [Route("Save_OB")]
        public Master_HealthManagementDTO Save_OB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_OB(dto);
        }

        [Route("Edit_OB")]
        public Master_HealthManagementDTO Edit_OB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_OB(dto);
        }

        [Route("ActiveDeactive_OB")]
        public Master_HealthManagementDTO ActiveDeactive_OB([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_OB(dto);
        }

        // Master Illness
        [Route("Load_illness")]
        public Master_HealthManagementDTO Load_illness([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Load_illness(dto);
        }

        [Route("Save_illness")]
        public Master_HealthManagementDTO Save_illness([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Save_illness(dto);
        }

        [Route("Edit_illness")]
        public Master_HealthManagementDTO Edit_illness([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.Edit_illness(dto);
        }

        [Route("ActiveDeactive_illness")]
        public Master_HealthManagementDTO ActiveDeactive_illness([FromBody] Master_HealthManagementDTO dto)
        {
            return _inter.ActiveDeactive_illness(dto);
        }


    }
}