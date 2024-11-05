using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;
using AutoMapper;

namespace FeeServiceHub.com.vaps.services
{
    public class FeePaymentGatewayDetailsImpl : interfaces.FeePaymentGatewayDetailsInterface
    {
        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeePaymentGatewayDetailsImpl> _logger;
        public FeePaymentGatewayDetailsImpl(FeeGroupContext frgContext, ILogger<FeePaymentGatewayDetailsImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
        public Fee_PaymentGateway_DetailsDTO getPaymentGatewayDetails(int mi_id)
        {
            Fee_PaymentGateway_DetailsDTO obj = new Fee_PaymentGateway_DetailsDTO();
            try
            {
                obj.institutionlist = _FeeGroupContext.master_institution.Where(d => d.MI_Id == mi_id && d.MI_ActiveFlag == 1).ToArray();

                obj.gateway_list = (from g in _FeeGroupContext.PAYUDETAILS
                                    where g.IMPG_ActiveFlg==true
                                    select new Fee_PaymentGateway_DetailsDTO
                                    {
                                        IMPG_PGName = g.IMPG_PGName,                                       
                                        IMPG_Id = g.IMPG_Id,
                                    }).ToArray();

                obj.paymentGatewayDetailList = (from m in _FeeGroupContext.Fee_PaymentGateway_Details
                                                from n in _FeeGroupContext.master_institution
                                                from o in _FeeGroupContext.PAYUDETAILS
                                                where m.MI_Id == n.MI_Id && m.MI_Id == mi_id && m.IMPG_Id == o.IMPG_Id
                                                select new Fee_PaymentGateway_DetailsDTO
                                                {
                                                    institutionName = n.MI_Name,
                                                    FPGD_AuthorisationKey = m.FPGD_AuthorisationKey,
                                                    FPGD_Id = m.FPGD_Id,
                                                    FPGD_MerchantId = m.FPGD_MerchantId,
                                                    FPGD_SaltKey = m.FPGD_SaltKey,
                                                    FPGD_SubMerchantId = m.FPGD_SubMerchantId,
                                                    FPGD_URL = m.FPGD_URL,
                                                    IMPG_Id = o.IMPG_Id,                                                                        IMPG_PGName=o.IMPG_PGName,
                                                    FPGD_Image = m.FPGD_Image,
                                                    FPGD_MobileActiveFlag = true,
                                                    FPGD_AccNo = m.FPGD_AccNo,
                                                    MI_Id = m.MI_Id,

                                                }).ToArray();
                if (obj.paymentGatewayDetailList.Length > 0)
                {
                    obj.count = obj.paymentGatewayDetailList.Length;
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
        public Fee_PaymentGateway_DetailsDTO savePaymentGatewayDetails(Fee_PaymentGateway_DetailsDTO data)
        {
            var result1 = _FeeGroupContext.PAYUDETAILS.Where(a => a.IMPG_Id == data.IMPG_Id).ToList();
            data.FPGD_PGName = result1[0].IMPG_PGName;
            try
            {
                if (data.FPGD_Id > 0)
                {
                    var checkDuplicate = _FeeGroupContext.Fee_PaymentGateway_Details.Where(d => d.MI_Id == data.MI_Id && d.FPGD_AuthorisationKey == data.FPGD_AuthorisationKey && d.FPGD_MerchantId == data.FPGD_MerchantId && d.FPGD_SaltKey == data.FPGD_SaltKey && d.FPGD_SubMerchantId == data.SubmerchantIds[0].FPGD_SubMerchantId && d.FPGD_URL == data.FPGD_URL && d.FPGD_Id != data.FPGD_Id && d.FPGD_MobileActiveFlag == data.FPGD_MobileActiveFlag && d.IMPG_Id == data.IMPG_Id && d.FPGD_Image == data.FPGD_Image && d.FPGD_AccNo == data.FPGD_AccNo).ToList();
                    if (checkDuplicate.Count == 0)
                    {
                    var result = _FeeGroupContext.Fee_PaymentGateway_Details.Single(d => d.FPGD_Id == data.FPGD_Id);
                    result.FPGD_AuthorisationKey = data.FPGD_AuthorisationKey;
                    result.FPGD_MerchantId = data.FPGD_MerchantId;
                    result.FPGD_SaltKey = data.FPGD_SaltKey;
                    result.FPGD_SubMerchantId = data.SubmerchantIds[0].FPGD_SubMerchantId;
                    result.FPGD_URL = data.FPGD_URL;
                    result.IMPG_Id = data.IMPG_Id;
                    result.User_id = data.user_id;
                    result.FPGD_PGName = data.FPGD_PGName;
                    result.FPGD_Image = data.FPGD_Image;
                    result.FPGD_AccNo = data.FPGD_AccNo;
                    result.FPGD_MobileActiveFlag = data.FPGD_MobileActiveFlag;
                    result.UpdatedDate = DateTime.Now;
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
                    for (int i = 0; i < data.SubmerchantIds.Length; i++)
                    {
                        var checkDuplicate = _FeeGroupContext.Fee_PaymentGateway_Details.Where(d => d.MI_Id == data.MI_Id && d.FPGD_AuthorisationKey == data.FPGD_AuthorisationKey && d.FPGD_MerchantId == data.FPGD_MerchantId && d.FPGD_SaltKey == data.FPGD_SaltKey && d.FPGD_SubMerchantId == data.SubmerchantIds[i].FPGD_SubMerchantId && d.FPGD_URL == data.FPGD_URL && d.FPGD_MobileActiveFlag == data.FPGD_MobileActiveFlag && d.IMPG_Id == data.IMPG_Id && d.FPGD_Image == data.FPGD_Image && d.FPGD_AccNo == data.FPGD_AccNo).ToList();
                        if(checkDuplicate.Count==0)
                        {
                        Fee_PaymentGateway_DetailsDMO obj = Mapper.Map<Fee_PaymentGateway_DetailsDMO>(data);
                        obj.FPGD_AuthorisationKey = data.FPGD_AuthorisationKey;
                        obj.FPGD_MerchantId = data.FPGD_MerchantId;
                        obj.FPGD_SaltKey = data.FPGD_SaltKey;
                        obj.FPGD_SubMerchantId = data.SubmerchantIds[0].FPGD_SubMerchantId;
                        if(data.FPGD_URL==null)
                        {
                            obj.FPGD_URL = "";
                        }
                        else if (data.FPGD_URL != null)
                        {
                            obj.FPGD_URL = data.FPGD_URL;
                        }
                        obj.IMPG_Id = data.IMPG_Id;
                        obj.FPGD_PGName = data.FPGD_PGName;
                        obj.FPGD_Image = data.FPGD_Image;
                        obj.FPGD_AccNo = data.FPGD_AccNo;
                        obj.User_id = data.user_id;
                        obj.FPGD_MobileActiveFlag = data.FPGD_MobileActiveFlag;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.FPGD_PGActiveFlag = "1";
                        _FeeGroupContext.Add(obj);
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
                        else
                        {
                            data.Isduplicate = "duplicate";
                        }
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
        public Fee_PaymentGateway_DetailsDTO editPaymentGatewayDetails(int id)
        {
            Fee_PaymentGateway_DetailsDTO obj = new Fee_PaymentGateway_DetailsDTO();
            try
            {
                obj.paymentGatewayDetailList = (from m in _FeeGroupContext.Fee_PaymentGateway_Details
                                                from n in _FeeGroupContext.master_institution
                                                from o in _FeeGroupContext.PAYUDETAILS
                                                where m.MI_Id == n.MI_Id && m.FPGD_Id == id && o.IMPG_Id == m.IMPG_Id
                                                select new Fee_PaymentGateway_DetailsDTO
                                                {
                                                    institutionName = n.MI_Name,
                                                    FPGD_AuthorisationKey = m.FPGD_AuthorisationKey,
                                                    FPGD_Id = m.FPGD_Id,
                                                    FPGD_MerchantId = m.FPGD_MerchantId,
                                                    FPGD_SaltKey = m.FPGD_SaltKey,
                                                    FPGD_SubMerchantId = m.FPGD_SubMerchantId,
                                                    FPGD_URL = m.FPGD_URL,
                                                    FPGD_MobileActiveFlag = m.FPGD_MobileActiveFlag,
                                                    IMPG_Id=m.IMPG_Id,                                                   
                                                    FPGD_Image = m.FPGD_Image,
                                                    FPGD_AccNo = m.FPGD_AccNo,
                                                    MI_Id = m.MI_Id
                                                }).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return obj;
        }
        public Fee_PaymentGateway_DetailsDTO deletePaymentGatewayDetails(int id)
        {
            Fee_PaymentGateway_DetailsDTO obj = new Fee_PaymentGateway_DetailsDTO();
            try
            {
                var res = _FeeGroupContext.Fee_PaymentGateway_Details.Single(d => d.FPGD_Id == id);
                var res2 = _FeeGroupContext.Fee_OnlinePayment_Mapping.Where(e => e.fpgd_id == id).ToList();
                if (res2.Count==0 )
                {                    
                var result = _FeeGroupContext.Fee_PaymentGateway_Details.Single(d =>d.FPGD_Id==id);                
                    _FeeGroupContext.Remove(result);
                    var count = _FeeGroupContext.SaveChanges();
                    if (count>0)
                    {
                        obj.returnval = "deleted";
                    }               
                else
                {
                    obj.returnval = "deletefailed";
                }
                }
                else
                {
                    obj.returnval = "Mapped";
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                obj.returnval = "failed";
            }
            return obj;
        }
    }
}
