using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeAutopromotionService : Interfaces.EmployeeAutopromotionInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public EmployeeAutopromotionService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public HR_Emp_AutopromotionDTO getBasicData(HR_Emp_AutopromotionDTO dto)
        {

            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;
        }

        public HR_Emp_AutopromotionDTO SaveUpdate(HR_Emp_AutopromotionDTO dto)
        {




            return dto;
        }

        public HR_Emp_AutopromotionDTO editData(int id)
        {

            HR_Emp_AutopromotionDTO dto = new HR_Emp_AutopromotionDTO();


            return dto;
        }

        public HR_Emp_AutopromotionDTO getDetailsByEmployee(HR_Emp_AutopromotionDTO dto)
        {

            var employees = (from dep in _HRMSContext.HR_Master_Department
                             from mm in _HRMSContext.MasterEmployee
                             from des in _HRMSContext.HR_Master_Designation
                             from grad in _HRMSContext.HR_Master_Grade
                             from grou in _HRMSContext.HR_Master_GroupType
                             from emptypr in _HRMSContext.HR_Master_EmployeeType
                             where mm.MI_Id.Equals(dto.MI_Id)
                             && mm.HRMDES_Id == des.HRMDES_Id && mm.HRME_Id == dto.HRME_Id
                             && mm.HRME_ActiveFlag == true
                             && dep.HRMD_ActiveFlag == true && des.HRMDES_ActiveFlag == true && grad.HRMG_ActiveFlag == true && grou.HRMGT_ActiveFlag == true && emptypr.HRMET_ActiveFlag == true && grad.HRMG_Id == mm.HRMG_Id && grou.HRMGT_Id == mm.HRMGT_Id && dep.HRMD_Id == mm.HRMD_Id && emptypr.HRMET_Id == mm.HRMET_Id

                             select new HR_Emp_AutopromotionDTO
                             {
                                 HRME_Id = mm.HRME_Id,
                                 hrmE_EmployeeFirstName = ((mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                 HRME_EmployeeCode = mm.HRME_EmployeeCode,
                                 HRMG_GradeName = grad.HRMG_GradeName,
                                 HRMG_Id = grad.HRMG_Id,
                                 HRMDES_DesignationName = des.HRMDES_DesignationName,
                                 HRMDES_Id = des.HRMDES_Id,
                                 HRMD_DepartmentName = dep.HRMD_DepartmentName,
                                 HRMD_Id = dep.HRMD_Id,
                                 HRMET_EmployeeType = emptypr.HRMET_EmployeeType,
                                 HRMET_Id = emptypr.HRMET_Id,
                                 HRMGT_EmployeeGroupType = grou.HRMGT_EmployeeGroupType,
                                 HRMGT_Id = grou.HRMGT_Id,
                                 HRME_PHOTO = mm.HRME_Photo,
                                 HRME_DOB = mm.HRME_DOB,
                                 HRME_DOJ = mm.HRME_DOJ,
                                 HRME_DOC = mm.HRME_DOC,
                                 HRMG_PayScaleRange = grad.HRMG_PayScaleRange,

                             }).ToList();



            dto.dropdownvalus = employees.ToArray();


            dto.employeegrade = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id == dto.MI_Id && t.HRMG_ActiveFlag == true).ToArray();
            dto.employeedesig = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_ActiveFlag == true).ToArray();
            dto.employeedept = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_ActiveFlag == true).ToArray();
            dto.employeeemptype = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id == dto.MI_Id && t.HRMET_ActiveFlag == true).ToArray();
            dto.employeeempgrouptype = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_ActiveFlag == true).ToArray();







            return dto;
        }


        public HR_Emp_AutopromotionDTO deactivate(HR_Emp_AutopromotionDTO dto)
        {

            return dto;
        }


        public HR_Emp_AutopromotionDTO GetAllDropdownAndDatatableDetails(HR_Emp_AutopromotionDTO dto)

        {
            var employees = (from dep in _HRMSContext.HR_Master_Department
                             from mm in _HRMSContext.MasterEmployee
                             from des in _HRMSContext.HR_Master_Designation
                             from grad in _HRMSContext.HR_Master_Grade
                             from grou in _HRMSContext.HR_Master_GroupType
                             from emptypr in _HRMSContext.HR_Master_EmployeeType
                             where mm.MI_Id.Equals(dto.MI_Id)
                             && mm.HRMDES_Id == des.HRMDES_Id
                             && mm.HRME_ActiveFlag == true
                             && dep.HRMD_ActiveFlag == true && des.HRMDES_ActiveFlag == true && grad.HRMG_ActiveFlag == true && grou.HRMGT_ActiveFlag == true && emptypr.HRMET_ActiveFlag == true && grad.HRMG_Id == mm.HRMG_Id && grou.HRMGT_Id == mm.HRMGT_Id && dep.HRMD_Id == mm.HRMD_Id && des.HRMDES_Id == mm.HRMDES_Id

                             select new HR_Emp_AutopromotionDTO
                             {
                                 HRME_Id = mm.HRME_Id,
                                 hrmE_EmployeeFirstName = ((mm.HRME_EmployeeCode == null ? " " : mm.HRME_EmployeeCode) + (mm.HRME_EmployeeFirstName == null ? " " : mm.HRME_EmployeeFirstName) + (mm.HRME_EmployeeMiddleName == null ? " " : mm.HRME_EmployeeMiddleName) + (mm.HRME_EmployeeLastName == null ? " " : mm.HRME_EmployeeLastName)).Trim(),
                                 HRME_PHOTO = mm.HRME_Photo,
                                 HRME_DOJ = mm.HRME_DOJ,
                                 HRME_DOC = mm.HRME_DOC,
                             }).Distinct().ToList();



            dto.employeedropdown = employees.ToArray();


            
            return dto;
        }


    }
}
