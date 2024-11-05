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
    public class Master80CService : Interfaces.Master80CInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public Master80CService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_80CDTO getBasicData(HR_Master_80CDTO dto)
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

        public HR_Master_80CDTO SaveUpdate(HR_Master_80CDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_80CDTO dmoObj = Mapper.Map<HR_Master_80CDTO>(dto);

                var alldata = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id == dto.MI_Id && t.HRMMC_Name.Equals(dto.HRMMC_Name) && t.HRMMMC_Description.Equals(dto.HRMMMC_Description)).Count();
                if (alldata == 0)
                {

                    if (dmoObj.HRMMM_Id > 0)
                    {

                            var duplicateHRMBD_BankName = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id == dto.MI_Id && t.HRMMC_Name.Equals(dto.HRMMC_Name) && t.HRMMM_Id != dmoObj.HRMMM_Id).Count();
                            var duplicateaccno = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id == dto.MI_Id && t.HRMMMC_Description.Equals(dto.HRMMMC_Description) && t.HRMMM_Id != dmoObj.HRMMM_Id).Count();
                            

                        if (duplicateaccno == 0 )
                            {
                                var result = _HRMSContext.HR_Master_80C.Single(t => t.HRMMM_Id == dmoObj.HRMMM_Id);
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
                       
                                    

                    }
                    else
                        {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id == dto.MI_Id && t.HRMMC_Name.Equals(dto.HRMMC_Name)).Count();
                        var duplicateaccno = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id == dto.MI_Id && t.HRMMMC_Description.Equals(dto.HRMMMC_Description)).Count();
                        //var duplicatebranch = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BranchName.Equals(dto.HRMBD_BranchName)).Count();
                        //var duplicateifsc = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_IFSCCode.Equals(dto.HRMBD_IFSCCode)).Count();

                        if (duplicateaccno == 0 )
                        {
                            dmoObj.HRMMMC_ActiveFlag = true;
                            dmoObj.HRMMC_Name = dto.HRMMC_Name;
                            dmoObj.HRMMMC_Description = dto.HRMMMC_Description;
                           // dmoObj.HRMBD_BankAddress = dto.HRMBD_BankAddress;
                           // dmoObj.HRMBD_BranchName = dto.HRMBD_BranchName;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
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

                }else
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

        public HR_Master_80CDTO editData(int id)
        {

            HR_Master_80CDTO dto = new HR_Master_80CDTO();
            dto.retrunMsg = "";
            try
            {
                // List<HR_Master_QuarterDTO> lorg = new List<HR_Master_QuarterDTO>();
                //lorg = _HRMSContext.HR_Master_quarter.AsNoTracking().Where(t => t.HRMQ_Id.Equals(id)).ToList();
                //dto.bankdetailList = lorg.ToArray();
                var lorg = _HRMSContext.HR_Master_80C.Where(t => t.HRMMM_Id.Equals(id)).ToList();

                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_80CDTO deactivate(HR_Master_80CDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMMM_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_80C.Single(t => t.HRMMM_Id == dto.HRMMM_Id);

                    if (result.HRMMMC_ActiveFlag == true)
                    {
                        result.HRMMMC_ActiveFlag = false;
                    }
                    else if (result.HRMMMC_ActiveFlag == false)
                    {
                        result.HRMMMC_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMMMC_ActiveFlag == true)
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

        public HR_Master_80CDTO GetAllDropdownAndDatatableDetails(HR_Master_80CDTO dto)
        {
         
            try
            {
             var datalist = _HRMSContext.HR_Master_80C.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();

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
