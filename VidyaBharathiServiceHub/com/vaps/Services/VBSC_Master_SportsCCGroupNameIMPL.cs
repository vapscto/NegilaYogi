using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBSC_Master_SportsCCGroupNameIMPL : Interfaces.VBSC_Master_SportsCCGroupNameInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
        //ILogger<VBSC_Master_SportsCCGroupNameIMPL> _logInv;
        public VBSC_Master_SportsCCGroupNameIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;

        }

        public VBSC_Master_SportsCCGroupNameDTO getloaddata(VBSC_Master_SportsCCGroupNameDTO data)
        {
            try
            {
                var Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == data.MI_Id && a.MO_ActiveFlag == 1)
                                    select new VBSC_Master_SportsCCGroupNameDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                              ).FirstOrDefault();
                data.MT_Id = Master_trust.MT_Id;

                data.Master_trust = (from a in _VidyaBharathiContext.Organisation
                                    from b in _VidyaBharathiContext.Institute
                                    where (a.MO_Id == b.MO_Id && b.MI_Id == data.MI_Id && a.MO_ActiveFlag == 1)
                                    select new VBSC_Master_SportsCCGroupNameDTO
                                    {
                                        MT_Id = a.MO_Id,
                                        MO_Name = a.MO_Name
                                    }
                                ).Distinct().OrderByDescending(R => R.MT_Id).ToArray();

                data.get_customer = (from a in _VidyaBharathiContext.Organisation
                                     from b in _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO
                                     where (a.MO_Id == b.MT_Id && a.MO_ActiveFlag == 1)
                                     select new VBSC_Master_SportsCCGroupNameDTO
                                     {
                                         MT_Id = a.MO_Id,
                                         MO_Name = a.MO_Name,
                                         VBSCMSCCG_SportsCCGroupName = b.VBSCMSCCG_SportsCCGroupName,
                                         VBSCMSCCG_SportsCCGroupDesc = b.VBSCMSCCG_SportsCCGroupDesc,
                                         VBSCMSCCG_SCCFlag = b.VBSCMSCCG_SCCFlag,
                                         VBSCMSCCG_ActiveFlag = b.VBSCMSCCG_ActiveFlag,
                                         VBSCMSCCG_Id = b.VBSCMSCCG_Id
                                     }
                               ).Distinct().OrderByDescending(R => R.MT_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        public VBSC_Master_SportsCCGroupNameDTO savedetails(VBSC_Master_SportsCCGroupNameDTO data)
        {
            try
            {

                if (data.VBSCMSCCG_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO.Where(t => t.MT_Id == data.MT_Id && t.VBSCMSCCG_SportsCCGroupName == data.VBSCMSCCG_SportsCCGroupName && t.VBSCMSCCG_SCCFlag == data.VBSCMSCCG_SCCFlag && t.VBSCMSCCG_Id != data.VBSCMSCCG_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO.Single(t => t.VBSCMSCCG_Id == data.VBSCMSCCG_Id );
                        result.MT_Id = data.MT_Id;
                        result.VBSCMSCCG_SportsCCGroupName = data.VBSCMSCCG_SportsCCGroupName;
                        result.VBSCMSCCG_SportsCCGroupDesc = data.VBSCMSCCG_SportsCCGroupDesc;
                        result.VBSCMSCCG_SCCFlag = data.VBSCMSCCG_SCCFlag;
                        result.VBSCMSCCG_ActiveFlag = true;
                        result.VBSCMSCCG_UpdatedDate = DateTime.Now;
                        result.VBSCMSCCG_CreatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
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
                    var res = _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO.Where(t => t.MT_Id == data.MT_Id && t.VBSCMSCCG_SportsCCGroupName == data.VBSCMSCCG_SportsCCGroupName && t.VBSCMSCCG_SCCFlag == data.VBSCMSCCG_SCCFlag ).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Master_SportsCCGroupNameDMO customer = new VBSC_Master_SportsCCGroupNameDMO();
                        customer.MT_Id = data.MT_Id;
                        customer.VBSCMSCCG_SportsCCGroupName = data.VBSCMSCCG_SportsCCGroupName;
                        customer.VBSCMSCCG_SportsCCGroupDesc = data.VBSCMSCCG_SportsCCGroupDesc;
                        customer.VBSCMSCCG_SCCFlag = data.VBSCMSCCG_SCCFlag;
                        customer.VBSCMSCCG_ActiveFlag = true;
                        customer.VBSCMSCCG_UpdatedDate = DateTime.Now;
                        customer.VBSCMSCCG_CreatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(customer);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
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
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        public VBSC_Master_SportsCCGroupNameDTO deactive(VBSC_Master_SportsCCGroupNameDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Master_SportsCCGroupNameDMO.Single(t => t.VBSCMSCCG_Id == data.VBSCMSCCG_Id);

                if (result.VBSCMSCCG_ActiveFlag == true)
                {
                    result.VBSCMSCCG_ActiveFlag = false;
                }
                else if (result.VBSCMSCCG_ActiveFlag == false)
                {
                    result.VBSCMSCCG_ActiveFlag = true;
                }
                result.VBSCMSCCG_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(result);
                int returnval = _VidyaBharathiContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }
    }

}
