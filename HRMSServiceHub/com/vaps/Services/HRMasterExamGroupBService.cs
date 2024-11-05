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
    public class HRMasterExamGroupBService : Interfaces.HRMasterExamGroupBInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRMasterExamGroupBService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_MasterExam_GroupBDTO getBasicData(HR_MasterExam_GroupBDTO dto)
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

        public HR_MasterExam_GroupBDTO SaveUpdate(HR_MasterExam_GroupBDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_MasterExam_GroupBDMO dmoObj = Mapper.Map<HR_MasterExam_GroupBDMO>(dto);

                var alldata = _HRMSContext.HR_MasterExam_GroupBDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGB_GroupBExamName.Equals(dto.HRMEGB_GroupBExamName)).Count();
                if (alldata == 0)
                {
                    if (dmoObj.HRMEGB_Id > 0)
                    {
                            var duplicateExamName = _HRMSContext.HR_MasterExam_GroupBDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGB_GroupBExamName.Equals(dto.HRMEGB_GroupBExamName) && t.HRMEGB_Id != dmoObj.HRMEGB_Id).Count();

                        if (duplicateExamName == 0)
                            {
                                var result = _HRMSContext.HR_MasterExam_GroupBDMO.Single(t => t.HRMEGB_Id == dmoObj.HRMEGB_Id);
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
                        var duplicateExamName = _HRMSContext.HR_MasterExam_GroupBDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGB_GroupBExamName.Equals(dto.HRMEGB_GroupBExamName)).Count();

                        if (duplicateExamName == 0)
                        {
                            dmoObj.HRMEGB_ActiveFlg = true;
                            dmoObj.HRMEGB_GroupBExamName = dto.HRMEGB_GroupBExamName;
                            dmoObj.HRMEGB_CreatedBy = dto.HRMEGB_CreatedBy;
                            dmoObj.HRMEGB_UpdatedBy = dto.HRMEGB_UpdatedBy;
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

                        else if (duplicateExamName > 0)
                        {
                            dto.retrunMsg = "bank";
                            return dto;
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

        public HR_MasterExam_GroupBDTO editData(int id)
        {

            HR_MasterExam_GroupBDTO dto = new HR_MasterExam_GroupBDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_MasterExam_GroupBDMO> lorg = new List<HR_MasterExam_GroupBDMO>();
                lorg = _HRMSContext.HR_MasterExam_GroupBDMO.AsNoTracking().Where(t => t.HRMEGB_Id.Equals(id)).ToList();
                dto.examdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MasterExam_GroupBDTO deactivate(HR_MasterExam_GroupBDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMEGB_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterExam_GroupBDMO.Single(t => t.HRMEGB_Id == dto.HRMEGB_Id);

                    if (result.HRMEGB_ActiveFlg == true)
                    {
                        result.HRMEGB_ActiveFlg = false;
                    }
                    else if (result.HRMEGB_ActiveFlg == false)
                    {
                        result.HRMEGB_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMEGB_ActiveFlg == true)
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

        public HR_MasterExam_GroupBDTO GetAllDropdownAndDatatableDetails(HR_MasterExam_GroupBDTO dto)
        {
            List<HR_MasterExam_GroupBDMO> datalist = new List<HR_MasterExam_GroupBDMO>();
            try
            {
               
                    datalist = _HRMSContext.HR_MasterExam_GroupBDMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.examdetailList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
