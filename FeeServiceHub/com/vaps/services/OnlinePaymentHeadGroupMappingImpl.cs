using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class OnlinePaymentHeadGroupMappingImpl:interfaces.OnlinePaymentHeadGroupMappingInterface
    {
        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<OnlinePaymentHeadGroupMappingImpl> _logger;
        public OnlinePaymentHeadGroupMappingImpl(FeeGroupContext frgContext, ILogger<OnlinePaymentHeadGroupMappingImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
        public Fee_OnlinePayment_MappingDTO getDetails(int mi_id)
        {
            Fee_OnlinePayment_MappingDTO obj = new Fee_OnlinePayment_MappingDTO();
            try
            {
                obj.fillyear = _FeeGroupContext.AcademicYear.Where(d => d.MI_Id == mi_id && d.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToArray();

                obj.institutionList = _FeeGroupContext.master_institution.Where(d => d.MI_Id == mi_id && d.MI_ActiveFlag == 1).ToArray();
                obj.classList = _FeeGroupContext.School_M_Class.Where(d => d.MI_Id == mi_id && d.ASMCL_ActiveFlag == true).OrderBy(d=>d.ASMCL_Order).ToArray();
                obj.groupNameList = _FeeGroupContext.FeeGroupDMO.Where(d => d.MI_Id == mi_id && d.FMG_ActiceFlag == true).ToArray();
                obj.headNameList = _FeeGroupContext.FeeHeadDMO.Where(d => d.MI_Id == mi_id && d.FMH_ActiveFlag == true).ToArray();
                obj.installmentList = _FeeGroupContext.FeeInstallmentsyearlyDMO.Where(d => d.MI_ID == mi_id).ToArray();
                obj.termList = _FeeGroupContext.feeTr.Where(d => d.MI_Id == mi_id && d.FMT_ActiveFlag == true).ToArray();
                obj.subMerchantIdList = _FeeGroupContext.Fee_PaymentGateway_Details.Where(d => d.MI_Id == mi_id).ToArray();
                obj.feeonlinepaymentmappingList = (from m in _FeeGroupContext.Fee_OnlinePayment_Mapping
                                                   from n in _FeeGroupContext.master_institution
                                                   from o in _FeeGroupContext.School_M_Class
                                                   from p in _FeeGroupContext.FeeGroupDMO
                                                   from q in _FeeGroupContext.FeeHeadDMO
                                                   from r in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                                   from s in _FeeGroupContext.feeTr
                                                   from t in _FeeGroupContext.Fee_PaymentGateway_Details
                                                   where m.MI_Id == n.MI_Id && m.ASMCL_Id==o.ASMCL_Id && m.fmg_id==p.FMG_Id && m.FMH_Id==q.FMH_Id && m.fti_id==r.FTI_Id && m.fmt_id==s.FMT_Id && m.fpgd_id==t.FPGD_Id && m.MI_Id == mi_id
                                                   select new Fee_OnlinePayment_MappingDTO
                                                   {
                                                       installmentName=r.FTI_Name,
                                                       fpgd_id=m.fpgd_id,
                                                       groupName=p.FMG_GroupName,
                                                       headName=q.FMH_FeeName,
                                                       institutionName=n.MI_Name,
                                                       termName=s.FMT_Name,
                                                       className=o.ASMCL_ClassName,
                                                       FOPM_Id=m.FOPM_Id,
                                                       submerchandId=t.FPGD_SubMerchantId
                                                   }).ToArray();
                if(obj.feeonlinepaymentmappingList.Length > 0)
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
        public Fee_OnlinePayment_MappingDTO saveDetails(Fee_OnlinePayment_MappingDTO data)
        {
            try
            {
                if (data.FOPM_Id > 0)
                {
                    var checkduplicate = _FeeGroupContext.Fee_OnlinePayment_Mapping.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_Id == data.ASMCL_ID && d.fmg_id == data.fmg_id && d.FMH_Id == data.selectedheadList[0].FMH_Id && d.fmt_id == data.fmt_id && d.fpgd_id == data.fpgd_id && d.fti_id == data.fti_id && d.FOPM_Id != data.FOPM_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        var result = _FeeGroupContext.Fee_OnlinePayment_Mapping.Single(d => d.FOPM_Id == data.FOPM_Id);
                        result.ASMCL_Id = data.ASMCL_ID;
                        result.fmg_id = data.fmg_id;
                        result.FMH_Id = data.selectedheadList[0].FMH_Id;
                        result.fmt_id = data.fmt_id;
                        result.fpgd_id = data.fpgd_id;
                        result.fti_id = data.fti_id;
                        result.UpdatedDate = DateTime.Now;
                        result.FOPM_UpdatedBy = data.user_id;
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
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
                        var checkduplicate = _FeeGroupContext.Fee_OnlinePayment_Mapping.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_Id == data.ASMCL_ID && d.fmg_id == data.fmg_id && d.FMH_Id == data.selectedheadList[i].FMH_Id && d.fmt_id == data.fmt_id && d.fpgd_id == data.fpgd_id && d.fti_id == data.fti_id).ToList();
                        if (checkduplicate.Count == 0)
                        {
                            Fee_OnlinePayment_MappingDMO obj = Mapper.Map<Fee_OnlinePayment_MappingDMO>(data);
                            obj.FMH_Id = data.selectedheadList[i].FMH_Id;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            obj.FOPM_CreatedBy = data.user_id;
                            obj.FOPM_UpdatedBy = data.user_id;
                            _FeeGroupContext.Add(obj);
                        }
                        else
                        {
                            data.Isduplicate = "duplicate";
                        }
                    }
                    var flag = _FeeGroupContext.SaveChanges();
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
        public Fee_OnlinePayment_MappingDTO editDetails(int id)
        {
            Fee_OnlinePayment_MappingDTO obj = new Fee_OnlinePayment_MappingDTO();
            try
            {
                obj.editdata = _FeeGroupContext.Fee_OnlinePayment_Mapping.Where(d => d.FOPM_Id == id).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }

            return obj;
        }
        public Fee_OnlinePayment_MappingDTO deleteDetails(int id)
        {
            Fee_OnlinePayment_MappingDTO obj = new Fee_OnlinePayment_MappingDTO();
            try
            {
                var result = _FeeGroupContext.Fee_OnlinePayment_Mapping.Where(d => d.FOPM_Id == id).ToArray();
                if(result.Length > 0)
                {
                   if(result.Any())
                    {
                       _FeeGroupContext.Remove(result.ElementAt(0));
                       var n = _FeeGroupContext.SaveChanges();
                        if(n > 0)
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

        public Fee_OnlinePayment_MappingDTO selecgroup(Fee_OnlinePayment_MappingDTO data)
        {
            try
            {
                data.headNameList = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                        from b in _FeeGroupContext.FeeHeadDMO
                                        where (a.FMH_Id == b.FMH_Id && a.FMG_Id==data.fmg_id  && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id)
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMH_Id = b.FMH_Id,
                                            FMH_FeeName = b.FMH_FeeName
                                        }).OrderBy(t => t.FMH_FeeName).Distinct().ToArray();
                if(data.headNameList.Length > 0)
                {
                    data.headlistcount = data.headNameList.Length;
                }
                else
                {
                    data.headlistcount = 0;
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Fee_OnlinePayment_MappingDTO selechead(Fee_OnlinePayment_MappingDTO data)
        {
            try
            {
                var selectedHead = data.selectedheadList.Select(d=>d.FMH_Id).ToList();
                data.installmentList = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                     from b in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                     where (a.FTI_Id == b.FTI_Id && a.FMG_Id == data.fmg_id && selectedHead.Contains(a.FMH_Id) && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
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

        public Fee_OnlinePayment_MappingDTO acde(Fee_OnlinePayment_MappingDTO data)
        {
            try
            {

                data.classList = (from a in _FeeGroupContext.School_M_Class
                                     from b in _FeeGroupContext.Masterclasscategory
                                     where (a.ASMCL_Id==b.ASMCL_Id && b.ASMAY_Id==data.ASMAY_Id && a.MI_Id==data.MI_Id)
                                     select new Fee_OnlinePayment_MappingDTO
                                     {
                                         ASMCL_ID=a.ASMCL_Id,
                                         ASMCL_ClassName = a.ASMCL_ClassName,
                                         ASMCL_Order = a.ASMCL_Order

                                     }).OrderBy(t => t.ASMCL_Order).Distinct().ToArray();

                data.groupNameList = (from a in _FeeGroupContext.FeeGroupDMO
                                  from b in _FeeGroupContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                  select new Fee_OnlinePayment_MappingDTO
                                  {
                                      fmg_id = a.FMG_Id,
                                      groupName = a.FMG_GroupName,

                                  }).Distinct().ToArray();
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
