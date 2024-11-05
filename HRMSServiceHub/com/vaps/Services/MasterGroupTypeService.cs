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
    public class MasterGroupTypeService : Interfaces.MasterGroupTypeInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterGroupTypeService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public HR_Master_GroupTypeDTO getBasicData(HR_Master_GroupTypeDTO dto)
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



        public HR_Master_GroupTypeDTO changeorderData(HR_Master_GroupTypeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.GroupTypeDTO.Count() > 0)
                {
                    foreach (HR_Master_GroupTypeDTO mob in dto.GroupTypeDTO)
                    {
                        if (mob.HRMGT_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_GroupType.Single(t => t.HRMGT_Id == mob.HRMGT_Id);

                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }

                    }

                    dto.retrunMsg = "Order updated successfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_Master_GroupTypeDTO SaveUpdate(HR_Master_GroupTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_GroupType dmoObj = Mapper.Map<HR_Master_GroupType>(dto);


                
               
                var duplicatecountresultorder = _HRMSContext.HR_Master_GroupType.Where(t => t.HRMGT_EmployeeGroupType == dto.HRMGT_EmployeeGroupType && t.MI_Id == dto.MI_Id && t.HRMGT_Code == dto.HRMGT_Code).Count();
                if (duplicatecountresultorder == 0)
                {

                    if (dmoObj.HRMGT_Id > 0)
                    {
                        var duplicateGroupType = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_EmployeeGroupType == dto.HRMGT_EmployeeGroupType && t.HRMGT_Id !=dto.HRMGT_Id).Count();
                        var duplicatecode = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_Code == dto.HRMGT_Code && t.HRMGT_Id != dto.HRMGT_Id).Count();

                        if (duplicateGroupType == 0 && duplicatecode == 0)
                        {
                            var result = _HRMSContext.HR_Master_GroupType.Single(t => t.HRMGT_Id == dmoObj.HRMGT_Id);

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
                            if (duplicateGroupType > 0)
                            {
                                dto.retrunMsg = "Duplicate";
                                return dto;
                            }
                            else if (duplicatecode > 0)
                            {
                                dto.retrunMsg = "Code";
                                return dto;
                            }
                        }
                    }
                    else
                    {

                        var duplicateGroupType = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_EmployeeGroupType == dto.HRMGT_EmployeeGroupType).Count();
                        var duplicatecode = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_Code == dto.HRMGT_Code).Count();

                        if (duplicateGroupType == 0 && duplicatecode == 0)
                        {
                            dmoObj.HRMGT_ActiveFlag = true;
                            dmoObj.HRMGT_EmployeeGroupType = dto.HRMGT_EmployeeGroupType;
                            dmoObj.HRMGT_Code = dto.HRMGT_Code;
                            dmoObj.HRMGT_Order = dto.HRMGT_Order;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                //set Order
                                HR_Master_GroupTypeDTO DTO = Mapper.Map<HR_Master_GroupTypeDTO>(dmoObj);
                                DTO.HRMGT_Order = Convert.ToInt32(DTO.HRMGT_Id);
                                var result = _HRMSContext.HR_Master_GroupType.Single(t => t.HRMGT_Id == DTO.HRMGT_Id);
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
                        else
                        {
                            if (duplicateGroupType > 0)
                            {
                                dto.retrunMsg = "Duplicate";
                                return dto;
                            }
                            else if (duplicatecode > 0)
                            {
                                dto.retrunMsg = "Code";
                                return dto;
                            }
                        }
                    }
                }
                
                else if (duplicatecountresultorder > 0)
                {
                    dto.retrunMsg = "AllDuplicate";
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

        public HR_Master_GroupTypeDTO editData(int id)
        {

            HR_Master_GroupTypeDTO dto = new HR_Master_GroupTypeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_GroupType> lorg = new List<HR_Master_GroupType>();
                lorg = _HRMSContext.HR_Master_GroupType.AsNoTracking().Where(t => t.HRMGT_Id.Equals(id)).ToList();
                dto.grouptypeList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_GroupTypeDTO deactivate(HR_Master_GroupTypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMGT_Id > 0)
                {

                    

                    var result = _HRMSContext.HR_Master_GroupType.Single(t => t.HRMGT_Id == dto.HRMGT_Id);

                    if (result.HRMGT_ActiveFlag == true)
                    {
                        var mappingresult = _HRMSContext.HRGroupDeptDessgDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMGT_Id == dto.HRMGT_Id).ToList();
                        if (mappingresult.Count() > 0)
                        {
                            dto.retrunMsg = "Mapped";
                            return dto;
                        }
                        result.HRMGT_ActiveFlag = false;
                    }
                    else if (result.HRMGT_ActiveFlag == false)
                    {
                        result.HRMGT_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMGT_ActiveFlag == true)
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
        public HR_Master_GroupTypeDTO GetAllDropdownAndDatatableDetails(HR_Master_GroupTypeDTO dto)
        {
            List<HR_Master_GroupType> datalist = new List<HR_Master_GroupType>();
            try
            {
                    datalist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.grouptypeList = datalist.OrderBy(t=>t.HRMGT_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
