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
    public class masterpointsService : Interfaces.masterpointInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public masterpointsService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Employee_AssesmentpointsDTO getBasicData(HR_Employee_AssesmentpointsDTO dto)
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

        public HR_Employee_AssesmentpointsDTO SaveUpdate(HR_Employee_AssesmentpointsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Employee_Assesment_Points dmoObj = Mapper.Map<HR_Employee_Assesment_Points>(dto);

                var duplicatecountresult = _HRMSContext.HR_Employee_Assementpoint.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_Option.Equals(dto.HR_Emp_As_Option) && t.HR_Emp_As_Points.Equals(dto.HR_Emp_As_Points)).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HR_Emp_As_Opid > 0)
                    {
                        var QulaificationName = _HRMSContext.HR_Employee_Assementpoint.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_Option.Equals(dto.HR_Emp_As_Option) && t.HR_Emp_As_Points == dto.HR_Emp_As_Points).Count();
                        if (QulaificationName == 0)
                        {
                            var result = _HRMSContext.HR_Employee_Assementpoint.Single(t => t.HR_Emp_As_Opid == dmoObj.HR_Emp_As_Opid);

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
                            dto.retrunMsg = "Duplicate";
                        }


                    }
                    else
                    {
                        var QulaificationName = _HRMSContext.HR_Employee_Assementpoint.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_Option.Equals(dto.HR_Emp_As_Option)).Count();
                        if (QulaificationName == 0)
                        {
                            dmoObj.HR_Emp_Asspoint_ActiveFlag = true;
                            dmoObj.HR_Emp_As_Option = dto.HR_Emp_As_Option;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HR_Emp_As_Points = dto.HR_Emp_As_Points;
                         
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {

                                HR_Employee_AssesmentpointsDTO DTO = Mapper.Map<HR_Employee_AssesmentpointsDTO>(dmoObj);
                              
                                var result = _HRMSContext.HR_Employee_Assementpoint.Single(t => t.HR_Emp_As_Opid == DTO.HR_Emp_As_Opid);
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
                            dto.retrunMsg = "Duplicate";
                        }
                    }


                }
                else
                {
                    dto.retrunMsg = "AllDuplicate";

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

        public HR_Employee_AssesmentpointsDTO editData(int id)
        {

            HR_Employee_AssesmentpointsDTO dto = new HR_Employee_AssesmentpointsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_Assesment_Points> lorg = new List<HR_Employee_Assesment_Points>();
                lorg = _HRMSContext.HR_Employee_Assementpoint.AsNoTracking().Where(t => t.HR_Emp_As_Opid.Equals(id)).ToList();
                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Employee_AssesmentpointsDTO deactivate(HR_Employee_AssesmentpointsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HR_Emp_As_Opid > 0)
                {
                    var result = _HRMSContext.HR_Employee_Assementpoint.Single(t => t.HR_Emp_As_Opid == dto.HR_Emp_As_Opid);

                    if (result.HR_Emp_Asspoint_ActiveFlag == true)
                    {
                        result.HR_Emp_Asspoint_ActiveFlag = false;
                    }
                    else if (result.HR_Emp_Asspoint_ActiveFlag == false)
                    {
                        result.HR_Emp_Asspoint_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HR_Emp_Asspoint_ActiveFlag == true)
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

        public HR_Employee_AssesmentpointsDTO GetAllDropdownAndDatatableDetails(HR_Employee_AssesmentpointsDTO dto)
        {
            List<HR_Employee_Assesment_Points> datalist = new List<HR_Employee_Assesment_Points>();
            try
            {

                datalist = _HRMSContext.HR_Employee_Assementpoint.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
             dto.bankdetailList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
