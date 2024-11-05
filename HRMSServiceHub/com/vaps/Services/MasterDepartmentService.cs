using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterDepartmentService : Interfaces.MasterDepartmentInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterDepartmentService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_DepartmentDTO getBasicData(HR_Master_DepartmentDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_DepartmentDTO changeorderData(HR_Master_DepartmentDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.DeraptmentDTO.Count() > 0)
                {
                    foreach (HR_Master_DepartmentDTO mob in dto.DeraptmentDTO)
                    {
                        if (mob.HRMD_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_Department.Single(t => t.HRMD_Id == mob.HRMD_Id);

                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }

                    }

                    dto.retrunMsg = "Order updated successfully";
                }
                else
                {
                    dto.retrunMsg = "Someting is wrong please Check it !!!";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_DepartmentDTO SaveUpdate(HR_Master_DepartmentDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_Department dmoObj = Mapper.Map<HR_Master_Department>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_DepartmentName.Equals(dto.HRMD_DepartmentName) &&  t.HRMD_InternalTrainingMinimumHrs.Equals(dto.HRMD_InternalTrainingMinimumHrs) && t.HRMD_Order.Equals(dto.HRMD_Order)).Count();
                var duplicatedept = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_DepartmentName.Equals(dto.HRMD_DepartmentName)).Count();
                if (duplicatecountresult > 0 && duplicatedept > 0)
                {
                    if (dmoObj.HRMD_Id > 0)
                    {
                        var result = _HRMSContext.HR_Master_Department.Single(t => t.HRMD_Id == dmoObj.HRMD_Id);

                        dto.UpdatedDate = DateTime.Now;
                        Mapper.Map(dto, result);
                        _HRMSContext.Update(result);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRMD_ActiveFlag = true;
                        dmoObj.HRMD_DepartmentName = dto.HRMD_DepartmentName;
                        dmoObj.HRMD_InternalTrainingMinimumHrs = dto.HRMD_InternalTrainingMinimumHrs;
                        dmoObj.HRMD_Order = dto.HRMD_Order;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            HR_Master_DepartmentDTO DTO = Mapper.Map<HR_Master_DepartmentDTO>(dmoObj);
                            DTO.HRMD_Order = Convert.ToInt32(DTO.HRMD_Id);
                            var result = _HRMSContext.HR_Master_Department.Single(t => t.HRMD_Id == DTO.HRMD_Id);
                            Mapper.Map(DTO, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }


                    }

                }
                else if (duplicatedept > 0)
                {
                    dto.retrunMsg = "Order";
                    return dto;
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }

                

                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_DepartmentDTO editData(int id)
        {

            HR_Master_DepartmentDTO dto = new HR_Master_DepartmentDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_Department> lorg = new List<HR_Master_Department>();
                lorg = _HRMSContext.HR_Master_Department.AsNoTracking().Where(t => t.HRMD_Id.Equals(id)).ToList();
                dto.departmentList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_DepartmentDTO deactivate(HR_Master_DepartmentDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMD_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_Department.Single(t => t.HRMD_Id == dto.HRMD_Id);

                    if (result.HRMD_ActiveFlag == true)
                    {

                        var mappingresult = _HRMSContext.HRGroupDeptDessgDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMD_Id == dto.HRMD_Id).ToList();
                        if (mappingresult.Count() > 0)
                        {
                            dto.retrunMsg = "Mapped";
                            return dto;
                        }
                        result.HRMD_ActiveFlag = false;
                    }
                    else if (result.HRMD_ActiveFlag == false)
                    {
                        result.HRMD_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMD_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_DepartmentDTO GetAllDropdownAndDatatableDetails(HR_Master_DepartmentDTO dto)
        {
            
            try
            {
                List<HR_Master_Department> datalist = new List<HR_Master_Department>();
                //datalist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id==dto.MI_Id).ToList();
                    dto.departmentList = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id == dto.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
