using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using PreadmissionDTOs.com.vaps.College.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGOnlinePaymentHeadGroupMappingImpl : CLGOnlinePaymentHeadGroupMappingInterface
    {
       
        public CollFeeGroupContext _clgfee;
        readonly ILogger<CLGOnlinePaymentHeadGroupMappingImpl> _logger;

        public CLGOnlinePaymentHeadGroupMappingImpl(CollFeeGroupContext _clgfeecon,ILogger<CLGOnlinePaymentHeadGroupMappingImpl> log)
        {
            _logger = log;
            _clgfee = _clgfeecon;
        }


        public Clg_StudentFeeGroupMapping_DTO onlineMappingDetails(int mi_id)
        {

            Clg_StudentFeeGroupMapping_DTO obj = new Clg_StudentFeeGroupMapping_DTO();
            try
            {
                obj.fillyear = _clgfee.AcademicYear.Where(d => d.MI_Id == mi_id && d.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();

                obj.institutionList = _clgfee.master_institution.Where(d => d.MI_Id == mi_id && d.MI_ActiveFlag == 1).ToArray();

              //  obj.courselist = _clgfee.MasterCourseDMO.Where(d => d.MI_Id == mi_id && d.AMCO_ActiveFlag == true).OrderBy(d => d.AMCO_Order).ToArray();

                obj.groupNameList = _clgfee.FeeGroupClgDMO.Where(d => d.MI_Id == mi_id && d.FMG_ActiceFlag == true).ToArray();
                obj.headNameList = _clgfee.FeeHeadClgDMO.Where(d => d.MI_Id == mi_id && d.FMH_ActiveFlag == true).ToArray();
                obj.installmentList = _clgfee.Clg_Fee_Installments_Yearly_DMO.Where(d => d.MI_ID == mi_id).ToArray();
                obj.termList = _clgfee.feeTr.Where(d => d.MI_Id == mi_id && d.FMT_ActiveFlag == true).ToArray();
                obj.subMerchantIdList = _clgfee.Fee_PaymentGateway_Details.Where(d => d.MI_Id == mi_id).ToArray();

                obj.feeonlinepaymentmappingList = (from m in _clgfee.CLG_Fee_OnlinePayment_MappingDMO
                                                   from n in _clgfee.master_institution
                                                   from o in _clgfee.MasterCourseDMO
                                                   from p in _clgfee.FeeGroupClgDMO
                                                   from q in _clgfee.FeeHeadClgDMO
                                                   from r in _clgfee.Clg_Fee_Installments_Yearly_DMO
                                                   from t in _clgfee.Fee_PaymentGateway_Details
                                                   where m.MI_Id == n.MI_Id && m.AMCO_Id == o.AMCO_Id && m.fmg_id == p.FMG_Id && m.FMH_Id == q.FMH_Id && m.fti_id == r.FTI_Id && m.fpgd_id == t.FPGD_Id && m.MI_Id == mi_id
                                                   select new Clg_StudentFeeGroupMapping_DTO
                                                   {
                                                       FTI_Name = r.FTI_Name,
                                                       fpgd_id = m.fpgd_id,
                                                       FMG_GroupName = p.FMG_GroupName,
                                                       FMH_FeeName = q.FMH_FeeName,
                                                       InstitutionName = n.MI_Name,
                                                       AMCO_CourseName = o.AMCO_CourseName,
                                                       CFOPM_Id = m.CFOPM_Id,
                                                       FPGD_SubMerchantId = t.FPGD_SubMerchantId
                                                   }).ToArray();
                if (obj.feeonlinepaymentmappingList.Length > 0)
                {
                    obj.count = obj.feeonlinepaymentmappingList.Length;
                }
                else
                {
                    obj.count = 0;
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return obj;
        }

        public Clg_StudentFeeGroupMapping_DTO saveDetails(Clg_StudentFeeGroupMapping_DTO data)
        {

            try
            {
                if (data.CFOPM_Id > 0)
                {
                    var checkduplicate = _clgfee.CLG_Fee_OnlinePayment_MappingDMO.Where(d => d.MI_Id == data.MI_Id && d.AMCO_Id == data.AMCO_Id && d.fmg_id == data.FMG_Id && d.FMH_Id == data.selectedheadList[0].FMH_Id && d.fpgd_id == data.fpgd_id && d.fti_id == data.fti_id && d.CFOPM_Id != data.CFOPM_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        var result = _clgfee.CLG_Fee_OnlinePayment_MappingDMO.Single(d => d.CFOPM_Id == data.CFOPM_Id);
                        result.AMCO_Id = data.AMCO_Id;
                        result.fmg_id = data.FMG_Id;
                        result.FMH_Id = data.selectedheadList[0].FMH_Id;
                        result.fpgd_id = data.fpgd_id;
                        result.fti_id = data.fti_id;
                        result.UpdatedDate = DateTime.Now;
                        _clgfee.Update(result);
                        var flag = _clgfee.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = "updated";
                        }
                        else
                        {
                            data.returnval = "updatefailed";
                        }
                    }
                    else
                    {
                        data.Isduplicate = "duplicate";
                    }

                }
                else
                {
                    for (int i = 0; i < data.selectedheadList.Length; i++)
                    {
                        var checkduplicate = _clgfee.CLG_Fee_OnlinePayment_MappingDMO.Where(d => d.MI_Id == data.MI_Id && d.AMCO_Id == data.AMCO_Id && d.fmg_id == data.FMG_Id && d.FMH_Id == data.selectedheadList[i].FMH_Id && d.fpgd_id == data.fpgd_id && d.fti_id == data.fti_id).ToList();
                        if (checkduplicate.Count == 0)
                        {
                            CLG_Fee_OnlinePayment_MappingDMO obj = new CLG_Fee_OnlinePayment_MappingDMO();
                            obj.AMCO_Id = data.AMCO_Id;
                            obj.MI_Id = data.MI_Id;
                            obj.fmg_id = data.FMG_Id;
                            obj.FMH_Id = data.selectedheadList[i].FMH_Id;
                            obj.fpgd_id = data.fpgd_id;
                            obj.fti_id = data.fti_id;
                            obj.PreAdmFlag = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _clgfee.Add(obj);
                        }
                        else
                        {
                            data.Isduplicate = "duplicate";
                        }
                    }
                    var flag = _clgfee.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = "saved";
                    }
                    else
                    {
                        data.returnval = "savingfailed";
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "failed";
            }
            return data;
        }
        public Clg_StudentFeeGroupMapping_DTO editDetails(int id)
        {
            Clg_StudentFeeGroupMapping_DTO obj = new Clg_StudentFeeGroupMapping_DTO();
            try
            {
                obj.editdata = _clgfee.CLG_Fee_OnlinePayment_MappingDMO.Where(d => d.CFOPM_Id == id).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }

            return obj;
        }


        public Clg_StudentFeeGroupMapping_DTO deleteDetails(int id)
        {
            Clg_StudentFeeGroupMapping_DTO obj = new Clg_StudentFeeGroupMapping_DTO();
            try
            {
                var result = _clgfee.CLG_Fee_OnlinePayment_MappingDMO.Where(d => d.CFOPM_Id == id).ToArray();
                if (result.Length > 0)
                {
                    if (result.Any())
                    {
                        _clgfee.Remove(result.ElementAt(0));
                        var n = _clgfee.SaveChanges();
                        if (n > 0)
                        {
                            obj.returnval = "deleted";
                        }
                        else
                        {
                            obj.returnval = "deletefailed";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                obj.returnval = "failed";
            }
            return obj;
        }

        public Clg_StudentFeeGroupMapping_DTO groupselection(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                data.headNameList = (from a in _clgfee.CLG_Fee_Yearly_Group_Head_Mapping
                                     from b in _clgfee.FeeHeadClgDMO
                                     where (a.FMH_Id == b.FMH_Id && a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMH_Id = b.FMH_Id,
                                         FMH_FeeName = b.FMH_FeeName
                                     }).OrderBy(t => t.FMH_FeeName).Distinct().ToArray();
                if (data.headNameList.Length > 0)
                {
                    data.headlistcount = data.headNameList.Length;
                }
                else
                {
                    data.headlistcount = 0;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO headsel(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                var selectedHead = data.selectedheadList.Select(d => d.FMH_Id).ToList();
                data.installmentList = (from a in _clgfee.Clg_Fee_AmountEntry_DMO
                                        from b in _clgfee.Clg_Fee_Installments_Yearly_DMO
                                        where (a.FTI_Id == b.FTI_Id && a.FMG_Id == data.FMG_Id && selectedHead.Contains(a.FMH_Id) && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            FTI_Id = b.FTI_Id,
                                            FTI_Name = b.FTI_Name
                                        }
    ).Distinct().ToArray();
                if (data.installmentList.Length > 0)
                {
                    data.installmentlistcount = data.installmentList.Length;
                }
                else
                {
                    data.installmentlistcount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO academicsel(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {

                data.courselist = (from a in _clgfee.MasterCourseDMO
                                   from b in _clgfee.CLG_Adm_College_AY_CourseDMO
                                   where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                   select new Clg_StudentFeeGroupMapping_DTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName,
                                       AMCO_Order = a.AMCO_Order
                                   }).OrderBy(t => t.AMCO_Order).Distinct().ToArray();

                data.groupNameList = (from a in _clgfee.FeeGroupClgDMO
                                      from b in _clgfee.FeeYearGroupDMO
                                      where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                      select new Clg_StudentFeeGroupMapping_DTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName,

                                      }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
