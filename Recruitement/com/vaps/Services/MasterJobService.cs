using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Recruitment.com.vaps.Services
{
    public class MasterJobService : Interfaces.MasterJobInterface
    {

        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterJobService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _Context = Context;
        }

        public HR_Master_JobsDTO getBasicData(HR_Master_JobsDTO dto)
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

        public HR_Master_JobsDTO SaveUpdate(HR_Master_JobsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_JobsDMO dmoObj = Mapper.Map<HR_Master_JobsDMO>(dto);


                var duplicatecountresult = _VMSContext.HR_Master_JobsDMO.Where(t => t.HRMJ_JobCode == dto.HRMJ_JobCode && t.HRMJ_JobTiTle == dto.HRMJ_JobTiTle && t.HRMLO_Id == dto.HRMLO_Id).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRMJ_Id > 0)
                    {

                        var result = _VMSContext.HR_Master_JobsDMO.Single(t => t.HRMJ_Id == dmoObj.HRMJ_Id);

                        dto.UpdatedDate = DateTime.Now;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
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
                            dmoObj.HRMJ_ActiveFlg = true;
                            dmoObj.HRMJ_JobCode = dto.HRMJ_JobCode;
                            dmoObj.HRMJ_JobTiTle = dto.HRMJ_JobTiTle;
                            dmoObj.HRMLO_Id = dto.HRMLO_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _VMSContext.Add(dmoObj);
                            var flag = _VMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                    }

                }
                if (duplicatecountresult > 0)
                {
                    dto.retrunMsg = "Duplicate";
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

        public HR_Master_JobsDTO editData(int id)
        {

            HR_Master_JobsDTO dto = new HR_Master_JobsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_JobsDMO> lorg = new List<HR_Master_JobsDMO>();
                lorg = _VMSContext.HR_Master_JobsDMO.AsNoTracking().Where(t => t.HRMJ_Id.Equals(id)).ToList();
                dto.JobList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_JobsDTO deactivate(HR_Master_JobsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMJ_Id > 0)
                {
                    var result = _VMSContext.HR_Master_JobsDMO.Single(t => t.HRMJ_Id == dto.HRMJ_Id);

                    if (result.HRMJ_ActiveFlg == true)
                    {
                        result.HRMJ_ActiveFlg = false;
                    }
                    else if (result.HRMJ_ActiveFlg == false)
                    {
                        result.HRMJ_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMJ_ActiveFlg == true)
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

        public HR_Master_JobsDTO GetAllDropdownAndDatatableDetails(HR_Master_JobsDTO dto)
        {
            //List<HR_Master_JobsDMO> datalist = new List<HR_Master_JobsDMO>();
            try
            {
                //datalist = _VMSContext.HR_Master_JobsDMO.ToList();
                var datalist = (from a in _VMSContext.HR_Master_JobsDMO
                                from b in _VMSContext.HR_Master_LocationDMO
                                where (a.HRMLO_Id == b.HRMLO_Id && b.MI_Id == dto.MI_Id)
                                select new HR_Master_JobsDTO
                                {
                                    HRMJ_Id = a.HRMJ_Id,
                                    HRMJ_JobCode = a.HRMJ_JobCode,
                                    HRMJ_JobTiTle = a.HRMJ_JobTiTle,
                                    HRMLO_Id = a.HRMLO_Id,
                                    HRMLO_LocationName = b.HRMLO_LocationName,
                                    HRMJ_Posted = a.HRMJ_Posted,
                                    HRC_Id = a.HRC_Id,
                                    HRMJ_ActiveFlg = a.HRMJ_ActiveFlg
                                }).ToList();
                dto.JobList = datalist.ToArray();

                dto.locationlist = _VMSContext.HR_Master_LocationDMO.Where(t=>t.MI_Id == dto.MI_Id && t.HRMLO_ActiveFlg == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
