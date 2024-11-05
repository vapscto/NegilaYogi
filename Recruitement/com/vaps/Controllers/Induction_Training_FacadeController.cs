using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Induction_Training_FacadeController : Controller
    {
        public Induction_Training_Interface _ipi;


        public VMSContext _vmsconte;
        public DomainModelMsSqlServerContext _dsc;

        public Induction_Training_FacadeController( Induction_Training_Interface ip )
        {
           
            _ipi = ip;
        }

        [Route("getalldata")]
        public HR_Training_Create_DTO getalldata([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.getalldata(dto);
        }

        [Route("getEmpDD")]
        public Hr_Master_Employee_DTO getEmpDD([FromBody] Hr_Master_Employee_DTO dto)
        {
            return _ipi.getEmpDD(dto);
        }


        [Route("getFloorDD")]
        public HR_Master_Floor_DTO getFloorDD([FromBody] HR_Master_Floor_DTO dto)
        {
            return _ipi.getFloorDD(dto);
        }

        [Route("getRoomDD")]
        public HR_Master_Room_DTO getRoomDD([FromBody] HR_Master_Room_DTO dto)
        {
            return _ipi.getRoomDD(dto);
        }
        [Route("get_trainer")]
        public HR_Training_Create_DTO get_trainer([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.get_trainer(dto);
        }
        [Route("SaveEdit_Induction")]
        public HR_Training_Create_DTO SaveEdit_Induction([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.SaveEdit_Induction(dto);
        }
        [Route("SaveEvalution_trinee_rating")]
        public HR_Training_Create_DTO SaveEvalution_trinee_rating([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.SaveEvalution_trinee_rating(dto);
        }
        [Route("Training_Views")]
        public HR_Training_Create_DTO Training_Views([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.Training_Views(dto);
        }
        [Route("EveGet")]
        public HR_Training_Create_DTO EveGet([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.EveGet(dto);
        }
        [Route("update_status")]
        public HR_Training_Create_DTO update_status([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.update_status(dto);
        }
        [Route("edit_induction_create")]
        public HR_Training_Create_DTO edit_induction_create([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.edit_induction_create(dto);
        }
        [Route("edit_training_details")]
        public HR_Training_Create_DTO edit_training_details([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.edit_training_details(dto);
        }
        [Route("SaveEdit_training_details")]
        public HR_Training_Create_DTO SaveEdit_training_details([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.SaveEdit_training_details(dto);
        }
        [Route("deactivate_Induction_create")]
        public HR_Training_Create_DTO deactivate_Induction_create([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.deactivate_Induction_create(dto);

        }
        [Route("GetInductionReport")]
        public  HR_Training_Create_DTO GetInductionReport([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.GetInductionReport(dto);

        }
        [Route("print_trainer_list")]
        public HR_Training_Create_DTO print_trainer_list([FromBody] HR_Training_Create_DTO dto)
        {
            return _ipi.print_trainer_list(dto);

        }
    }
}