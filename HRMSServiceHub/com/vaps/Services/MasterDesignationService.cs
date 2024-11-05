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
    public class MasterDesignationService : Interfaces.MasterDesignationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterDesignationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_DesignationDTO getBasicData(HR_Master_DesignationDTO dto)
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
        //Onchange:
        public HR_Master_DesignationDTO changeorderData(HR_Master_DesignationDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.DesignationDTO.Count() > 0)
                {
                    foreach (HR_Master_DesignationDTO mob in dto.DesignationDTO)
                    {
                        if (mob.HRMDES_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_Designation.Single(t => t.HRMDES_Id == mob.HRMDES_Id);

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

        public HR_Master_DesignationDTO SaveUpdate(HR_Master_DesignationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_Designation dmoObj = Mapper.Map<HR_Master_Designation>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_Designation.Where(t => t.HRMDES_DesignationName == dto.HRMDES_DesignationName  && t.HRMDES_BasicAmount == dto.HRMDES_BasicAmount && t.HRMDES_SanctionedSeats == dto.HRMDES_SanctionedSeats && t.HRMDES_DisplaySanctionedSeatsFlag == dto.HRMDES_DisplaySanctionedSeatsFlag && t.MI_Id == dto.MI_Id).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMDES_Id > 0)
                    {

                        var duplicatecountresultorder = _HRMSContext.HR_Master_Designation.Where(t => t.HRMDES_DesignationName == dto.HRMDES_DesignationName && t.MI_Id == dto.MI_Id && t.HRMDES_Id != dto.HRMDES_Id).Count();
                        if (duplicatecountresultorder == 0)
                        {
                            var result = _HRMSContext.HR_Master_Designation.Single(t => t.HRMDES_Id == dmoObj.HRMDES_Id);

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
                        else if (duplicatecountresultorder > 0)
                        {
                            dto.retrunMsg = "Order";
                            return dto;
                        }
                    }
                    else
                    {
                        var duplicatecountresultorder = _HRMSContext.HR_Master_Designation.Where(t => t.HRMDES_DesignationName == dto.HRMDES_DesignationName && t.MI_Id == dto.MI_Id).Count();
                        if (duplicatecountresultorder == 0)
                        {
                            dmoObj.HRMDES_ActiveFlag = true;
                            dmoObj.HRMDES_DesignationName = dto.HRMDES_DesignationName;
                            dmoObj.HRMDES_BasicAmount = dto.HRMDES_BasicAmount;
                            dmoObj.HRMDES_SanctionedSeats = dto.HRMDES_SanctionedSeats;
                            dmoObj.HRMDES_DisplaySanctionedSeatsFlag = dto.HRMDES_DisplaySanctionedSeatsFlag;
                            dmoObj.HRMDES_Order = dto.HRMDES_Order;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                HR_Master_DesignationDTO DTO = Mapper.Map<HR_Master_DesignationDTO>(dmoObj);
                                DTO.HRMDES_Order = Convert.ToInt32(DTO.HRMDES_Id);
                                var result = _HRMSContext.HR_Master_Designation.Single(t => t.HRMDES_Id == DTO.HRMDES_Id);
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
                        else if (duplicatecountresultorder > 0)
                        {
                            dto.retrunMsg = "Order";
                            return dto;
                        }
                    }

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
        public HR_Master_DesignationDTO editData(int id)
        {

            HR_Master_DesignationDTO dto = new HR_Master_DesignationDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_Designation> lorg = new List<HR_Master_Designation>();
                lorg = _HRMSContext.HR_Master_Designation.AsNoTracking().Where(t => t.HRMDES_Id.Equals(id)).ToList();
                dto.designationlList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_DesignationDTO deactivate(HR_Master_DesignationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMDES_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_Designation.Single(t => t.HRMDES_Id == dto.HRMDES_Id);

                    if (result.HRMDES_ActiveFlag == true)
                    {
                        var mappingresult = _HRMSContext.HRGroupDeptDessgDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMDES_Id == dto.HRMDES_Id).ToList();
                        if (mappingresult.Count() > 0)
                        {
                            dto.retrunMsg = "Mapped";
                            return dto;
                        }
                        result.HRMDES_ActiveFlag = false;
                    }
                    else if (result.HRMDES_ActiveFlag == false)
                    {
                        result.HRMDES_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMDES_ActiveFlag == true)
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

        public HR_Master_DesignationDTO GetAllDropdownAndDatatableDetails(HR_Master_DesignationDTO dto)
        {
            List<HR_Master_Designation> datalist = new List<HR_Master_Designation>();
            try
            {
                
                    datalist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.designationlList = datalist.OrderBy(t=>t.HRMDES_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
