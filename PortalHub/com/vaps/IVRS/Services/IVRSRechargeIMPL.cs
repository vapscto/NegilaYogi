using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class IVRSRechargeIMPL : Interfaces.IVRSRechargeInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public IVRSRechargeIMPL(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public IVRS_Acc_RechargeDTO getdetails(IVRS_Acc_RechargeDTO data)
        {
            data.monthlist = _ivrs.IVRM_Month_DMO.Where(a => a.Is_Active == true).ToArray();
            data.yearlist = (from a in _ivrs.AcademicYearDMO
                             select new IVRS_Acc_RechargeDTO
                             {
                                 ASMAY_Year = a.ASMAY_Year

                             }).Distinct().ToArray();
            data.institute = _ivrs.IVRM_IVRS_ConfigurationDMO.ToArray();
            data.maindata = (from a in _ivrs.IVRS_Acc_RechargeDMO
                             from b in _ivrs.IVRM_IVRS_ConfigurationDMO
                             where (a.IACRE_VirtualNo == b.IIVRSC_VirtualNo)
                             select new IVRS_Acc_RechargeDTO
                             {
                                 IACRE_Id = a.IACRE_Id,
                                 IACRE_VirtualNo = a.IACRE_VirtualNo,
                                 MI_Id = a.MI_Id,
                                 IACRE_Year = a.IACRE_Year,
                                 IACRE_Month = a.IACRE_Month,
                                 IACRE_RechargeAmt = a.IACRE_RechargeAmt,
                                 IACRE_PaymentMode = a.IACRE_PaymentMode,
                                 IACRE_ReferneceNo = a.IACRE_ReferneceNo,
                                 IACRE_NoOfCalls = a.IACRE_NoOfCalls,
                                 IACRE_ActiveFlg = a.IACRE_ActiveFlg,
                                 aA_SchoolName = b.IIVRSC_SchoolName
                             }).ToArray();
            return data;
        }

        public IVRS_Acc_RechargeDTO savedetail(IVRS_Acc_RechargeDTO data)
        {
            if (data.IACRE_Id != 0)
            {
                var res = _ivrs.IVRS_Acc_RechargeDMO.Where(t => t.IACRE_Id != data.IACRE_Id && t.IACRE_VirtualNo == data.IACRE_VirtualNo && t.IACRE_Year == data.IACRE_Year && t.IACRE_Month == data.IACRE_Month && t.IACRE_RechargeAmt == data.IACRE_RechargeAmt && t.IACRE_ReferneceNo == data.IACRE_ReferneceNo && t.IACRE_NoOfCalls == data.IACRE_NoOfCalls).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var result = _ivrs.IVRS_Acc_RechargeDMO.Single(t => t.IACRE_Id == data.IACRE_Id);
                    result.IACRE_VirtualNo = data.IACRE_VirtualNo;
                    result.IACRE_Year = data.IACRE_Year;
                    result.IACRE_Month = data.IACRE_Month;
                    result.IACRE_RechargeAmt = data.IACRE_RechargeAmt;
                    result.IACRE_ReferneceNo = data.IACRE_ReferneceNo;
                    result.IACRE_NoOfCalls = data.IACRE_NoOfCalls;
                    result.IACRE_PaymentMode = data.IACRE_PaymentMode;
                    result.MI_Id = data.MI_Id;
                    result.IACRE_ActiveFlg = true;
                    result.CreatedDate = DateTime.Now;
                    result.UpdatedDate = DateTime.Now;
                    _ivrs.Update(result);
                    var contactExists = _ivrs.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            else
            {
                var res = _ivrs.IVRS_Acc_RechargeDMO.Where(t => t.IACRE_VirtualNo == data.IACRE_VirtualNo && t.IACRE_Year == data.IACRE_Year && t.IACRE_Month == data.IACRE_Month && t.IACRE_RechargeAmt == data.IACRE_RechargeAmt && t.IACRE_ReferneceNo == data.IACRE_ReferneceNo && t.IACRE_NoOfCalls == data.IACRE_NoOfCalls).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    IVRS_Acc_RechargeDMO master = new IVRS_Acc_RechargeDMO();
                    master.IACRE_VirtualNo = data.IACRE_VirtualNo;
                    master.IACRE_Year = data.IACRE_Year;
                    master.IACRE_Month = data.IACRE_Month;
                    master.IACRE_RechargeAmt = data.IACRE_RechargeAmt;
                    master.IACRE_ReferneceNo = data.IACRE_ReferneceNo;
                    master.IACRE_NoOfCalls = data.IACRE_NoOfCalls;
                    master.IACRE_PaymentMode = data.IACRE_PaymentMode;
                    master.MI_Id = data.MI_Id;
                    master.IACRE_ActiveFlg = true;
                    master.CreatedDate = DateTime.Now;
                    master.UpdatedDate = DateTime.Now;
                    _ivrs.Add(master);
                    var contactExists = _ivrs.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            return data;
        }
        public IVRS_Acc_RechargeDTO getdetails_page(int id)
        {
            IVRS_Acc_RechargeDTO TTMC = new IVRS_Acc_RechargeDTO();
            try
            {
                TTMC.maindata_grid = _ivrs.IVRS_Acc_RechargeDMO.Where(t => t.IACRE_Id == id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public IVRS_Acc_RechargeDTO deactivate(IVRS_Acc_RechargeDTO acd)
        {
            try
            {
                if (acd.IACRE_Id > 0)
                {
                    var result = _ivrs.IVRS_Acc_RechargeDMO.Single(t => t.IACRE_Id.Equals(acd.IACRE_Id));
                    if (result.IACRE_ActiveFlg.Equals(true))
                    {
                        result.IACRE_ActiveFlg = false;
                    }
                    else
                    {
                        result.IACRE_ActiveFlg = true;
                    }
                    _ivrs.Update(result);
                    var flag = _ivrs.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
