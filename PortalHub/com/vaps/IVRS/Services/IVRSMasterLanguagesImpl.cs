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
    public class IVRSMasterLanguagesImpl : Interfaces.IVRSMasterLanguagesInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public IVRSMasterLanguagesImpl(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public IVRS_Master_LanguagesDTO getdetails(IVRS_Master_LanguagesDTO data)
        {
            data.institute = _ivrs.IVRM_IVRS_ConfigurationDMO.ToArray();
            data.maindata = _ivrs.IVRS_Master_LanguagesDMO.ToArray();
            return data;
        }
        public IVRS_Master_LanguagesDTO savedetail(IVRS_Master_LanguagesDTO data)
        {
            if (data.IMLA_Id != 0)
            {
                var res = _ivrs.IVRS_Master_LanguagesDMO.Where(t => t.IMLA_Id != data.IMLA_Id && t.IMLA_LanguageOrder== data.IMLA_LanguageOrder && t.IMLA_SchoolName==data.IMLA_SchoolName).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Order Duplicate";
                }
                else
                {
                    var result = _ivrs.IVRS_Master_LanguagesDMO.Single(t => t.IMLA_Id == data.IMLA_Id);
                    result.IMLA_Language = data.IMLA_Language;
                    result.IMLA_VirtualNo = data.IMLA_VirtualNo;
                    result.IMLA_SchoolName = data.IMLA_SchoolName;
                    result.IMLA_SchoolURL = data.IMLA_SchoolURL;
                    result.IMLA_LanguageOrder = data.IMLA_LanguageOrder;
                    result.IMLA_CreatedBy = data.IMLA_CreatedBy;
                    result.IMLA_UpdatedBy = data.IMLA_CreatedBy;
                    result.MI_Id = data.MI_Id;
                    result.IMLA_ActiveFlg = true;
                    result.IMLA_UpdatedDate = DateTime.Now;
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
                var res = _ivrs.IVRS_Master_LanguagesDMO.Where(t => t.IMLA_SchoolName == data.IMLA_SchoolName && t.IMLA_Language==data.IMLA_Language).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
                    var res1 = _ivrs.IVRS_Master_LanguagesDMO.Where(t =>t.MI_Id==data.MI_Id && t.IMLA_LanguageOrder == data.IMLA_LanguageOrder).ToList();
                    if (res1.Count > 0)
                    {
                        data.returnduplicatestatus = "Order Duplicate";
                    }
                    else
                    {
                    IVRS_Master_LanguagesDMO master = new IVRS_Master_LanguagesDMO();
                    master.IMLA_Language = data.IMLA_Language;
                    master.IMLA_VirtualNo = data.IMLA_VirtualNo;
                    master.IMLA_SchoolName = data.IMLA_SchoolName;
                    master.IMLA_SchoolURL = data.IMLA_SchoolURL;
                    master.IMLA_LanguageOrder = data.IMLA_LanguageOrder;
                    master.IMLA_CreatedBy = data.IMLA_CreatedBy;
                    master.IMLA_UpdatedBy = data.IMLA_CreatedBy;
                    master.IMLA_ActiveFlg = true;
                        master.MI_Id = data.MI_Id;
                        master.IMLA_CreatedDate = DateTime.Now;
                    master.IMLA_UpdatedDate = DateTime.Now;
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
            }
            return data;
        }
        public IVRS_Master_LanguagesDTO getdetails_page(int id)
        {
            IVRS_Master_LanguagesDTO TTMC = new IVRS_Master_LanguagesDTO();
            try
            {
                TTMC.maindata_grid = _ivrs.IVRS_Master_LanguagesDMO.Where(t => t.IMLA_Id == id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public IVRS_Master_LanguagesDTO deactivate(IVRS_Master_LanguagesDTO acd)
        {
            try
            {
                IVRS_Master_LanguagesDMO pge = Mapper.Map<IVRS_Master_LanguagesDMO>(acd);
                if (pge.IMLA_Id > 0)
                {
                    var result = _ivrs.IVRS_Master_LanguagesDMO.Single(t => t.IMLA_Id.Equals(pge.IMLA_Id));
                    if (result.IMLA_ActiveFlg.Equals(true))
                    {
                        result.IMLA_ActiveFlg = false;
                    }
                    else
                    {
                        result.IMLA_ActiveFlg = true;
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
