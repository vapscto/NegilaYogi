using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AT_MasterSiteImpl : Interface.AT_MasterSiteInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AT_MasterSiteImpl> _logAT;
        public AT_MasterSiteImpl(AssetTrackingContext ATContext, ILogger<AT_MasterSiteImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data)
        {
            try
            {
                data.get_sites = _ATContext.INV_Master_SiteDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMSI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Master Site load Page:" + ex.Message);
            }
            return data;
        }

        public AT_MasterSiteDTO savedetails(AT_MasterSiteDTO data)
        {
            try
            {
                if (data.INVMSI_Id != 0)
                {
                    var res = _ATContext.INV_Master_SiteDMO.Where(t => t.INVMSI_SiteBuildingName == data.INVMSI_SiteBuildingName  && t.MI_Id == data.MI_Id && t.INVMSI_Id != data.INVMSI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ATContext.INV_Master_SiteDMO.Single(t => t.INVMSI_Id == data.INVMSI_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMSI_SiteBuildingName = data.INVMSI_SiteBuildingName;
                        result.INVMSI_SiteRemarks = data.INVMSI_SiteRemarks;                       
                        result.INVMSI_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _ATContext.Update(result);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
                    var res = _ATContext.INV_Master_SiteDMO.Where(t => t.INVMSI_SiteBuildingName == data.INVMSI_SiteBuildingName && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_SiteDMO site = new INV_Master_SiteDMO();
                        site.MI_Id = data.MI_Id;
                        site.INVMSI_SiteBuildingName = data.INVMSI_SiteBuildingName;
                        site.INVMSI_SiteRemarks = data.INVMSI_SiteRemarks;                       
                        site.INVMSI_ActiveFlg = true;
                        site.CreatedDate = DateTime.Now;
                        site.UpdatedDate = DateTime.Now;
                        _ATContext.Add(site);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Master Site savedata :" + ex.Message);
            }
            return data;
        }

        public AT_MasterSiteDTO deactive(AT_MasterSiteDTO data)
        {
            try
            {
                var result = _ATContext.INV_Master_SiteDMO.Single(t => t.INVMSI_Id == data.INVMSI_Id);

                if (result.INVMSI_ActiveFlg == true)
                {
                    result.INVMSI_ActiveFlg = false;
                }
                else if (result.INVMSI_ActiveFlg == false)
                {
                    result.INVMSI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
