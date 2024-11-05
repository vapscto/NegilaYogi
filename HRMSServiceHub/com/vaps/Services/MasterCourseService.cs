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
    public class MasterCourseService : Interfaces.MasterCourseInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterCourseService(HRMSContext HRMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _HRMSContext = HRMSContext;
            _Context = OrganisationContext;
        }

        public HR_Master_CourseDTO getBasicData(HR_Master_CourseDTO dto)
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
        //Onchange 
        //Added by Ramesh
        public HR_Master_CourseDTO changeorderData(HR_Master_CourseDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.CourseDTO.Count() > 0)
                {
                    foreach (HR_Master_CourseDTO mob in dto.CourseDTO)
                    {
                        if (mob.HRMC_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_Course.Single(t => t.HRMC_Id==mob.HRMC_Id);
                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated sucessfully";
                }
                else
                {
                    dto.retrunMsg = "Someting is wrong please Check it !!!";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }

        public HR_Master_CourseDTO SaveUpdate(HR_Master_CourseDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_CourseDMO dmoObj = Mapper.Map<HR_Master_CourseDMO>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_QulaificationName.Equals(dto.HRMC_QulaificationName) && t.HRMC_QualificationDesc.Equals(dto.HRMC_QualificationDesc) && t.HRMC_DefaultQualFag==dto.HRMC_DefaultQualFag && t.HRMC_SpecialisationFlag == dto.HRMC_SpecialisationFlag).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HRMC_Id > 0)
                    {
                        var QulaificationName = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_QulaificationName.Equals(dto.HRMC_QulaificationName) && t.HRMC_Id != dto.HRMC_Id).Count();
                        if (QulaificationName == 0)
                        {
                            var result = _HRMSContext.HR_Master_Course.Single(t => t.HRMC_Id == dmoObj.HRMC_Id);

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

                        }else
                        {
                            dto.retrunMsg = "Duplicate";
                        }

                            
                    }
                    else
                    {
                        var QulaificationName = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id == dto.MI_Id && t.HRMC_QulaificationName.Equals(dto.HRMC_QulaificationName)).Count();
                        if (QulaificationName == 0)
                        {
                            dmoObj.HRMC_ActiveFlag = true;
                            dmoObj.HRMC_QulaificationName = dto.HRMC_QulaificationName;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRMC_QualificationDesc = dto.HRMC_QualificationDesc;
                            dmoObj.HRMC_Order = dto.HRMC_Order;
                            dmoObj.HRMC_DefaultQualFag = dto.HRMC_DefaultQualFag;
                            dmoObj.HRMC_SpecialisationFlag = dto.HRMC_SpecialisationFlag;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {

                                HR_Master_CourseDTO DTO = Mapper.Map<HR_Master_CourseDTO>(dmoObj);
                                DTO.HRMC_Order = Convert.ToInt32(DTO.HRMC_Id);
                                var result = _HRMSContext.HR_Master_Course.Single(t => t.HRMC_Id == DTO.HRMC_Id);
                                Mapper.Map(DTO, result);
                                _HRMSContext.Update(result);
                                _HRMSContext.SaveChanges();

                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }else
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

        public HR_Master_CourseDTO editData(int id)
        {

            HR_Master_CourseDTO dto = new HR_Master_CourseDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_CourseDMO> lorg = new List<HR_Master_CourseDMO>();
                lorg = _HRMSContext.HR_Master_Course.AsNoTracking().Where(t => t.HRMC_Id.Equals(id)).ToList();
                dto.courseList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_CourseDTO deactivate(HR_Master_CourseDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMC_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_Course.Single(t => t.HRMC_Id == dto.HRMC_Id);

                    if (result.HRMC_ActiveFlag == true)
                    {
                        result.HRMC_ActiveFlag = false;
                    }
                    else if (result.HRMC_ActiveFlag == false)
                    {
                        result.HRMC_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMC_ActiveFlag == true)
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

        public HR_Master_CourseDTO GetAllDropdownAndDatatableDetails(HR_Master_CourseDTO dto)
        {
            List<HR_Master_CourseDMO> datalist = new List<HR_Master_CourseDMO>();
            try
            {
               
                    datalist = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.courseList = datalist.OrderBy(t => t.HRMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
