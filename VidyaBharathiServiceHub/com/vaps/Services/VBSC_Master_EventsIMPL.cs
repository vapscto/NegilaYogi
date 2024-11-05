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
    public class VBSC_Master_EventsIMPL : Interfaces.VBSC_Master_EventsInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
        public VBSC_Master_EventsIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_Master_EventsDTO getloaddata(VBSC_Master_EventsDTO data)
        {
            try
            {
                var mtid = _VidyaBharathiContext.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.MT_Id = mtid[0].MO_Id;

                data.get_customer = _VidyaBharathiContext.VBSC_Master_EventsDMO.Where(m => m.MT_Id == data.MT_Id).OrderBy(m => m.VBSCME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        public VBSC_Master_EventsDTO savedetails(VBSC_Master_EventsDTO data)
        {
            try
            {
                var mtid =_VidyaBharathiContext.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.MT_Id = mtid[0].MO_Id;
                
                if (data.VBSCME_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Master_EventsDMO.Where(t => t.VBSCME_EventName == data.VBSCME_EventName && t.VBSCME_EventNameDesc == data.VBSCME_EventNameDesc  && t.VBSCME_Id != data.VBSCME_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Master_EventsDMO.Single(t => t.VBSCME_Id == data.VBSCME_Id);
                        result.MT_Id = data.MT_Id;
                        result.VBSCME_EventName = data.VBSCME_EventName;
                        result.VBSCME_EventNameDesc = data.VBSCME_EventNameDesc;
                        result.VBSCME_ActiveFlag = true;
                        result.VBSCME_UpdatedDate = DateTime.Now;
                        result.VBSCME_CreatedDate = DateTime.Now;
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
                    var res = _VidyaBharathiContext.VBSC_Master_EventsDMO.Where(t => t.VBSCME_EventName == data.VBSCME_EventName && t.VBSCME_EventNameDesc == data.VBSCME_EventNameDesc).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Master_EventsDMO customer = new VBSC_Master_EventsDMO();
                        customer.MT_Id = data.MT_Id;
                        customer.VBSCME_EventName = data.VBSCME_EventName;
                        customer.VBSCME_EventNameDesc = data.VBSCME_EventNameDesc;
                        customer.VBSCME_ActiveFlag = true;
                        customer.VBSCME_UpdatedDate = DateTime.Now;
                        customer.VBSCME_CreatedDate = DateTime.Now;
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

        public VBSC_Master_EventsDTO deactive(VBSC_Master_EventsDTO data)
        {
            try
            {

                var result = _VidyaBharathiContext.VBSC_Master_EventsDMO.Single(t => t.VBSCME_Id == data.VBSCME_Id);

                if (result.VBSCME_ActiveFlag == true)
                {
                    result.VBSCME_ActiveFlag = false;
                }
                else if (result.VBSCME_ActiveFlag == false)
                {
                    result.VBSCME_ActiveFlag = true;
                }
                result.VBSCME_UpdatedDate = DateTime.Now;
                result.VBSCME_CreatedDate = DateTime.Now;
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        
    }

}


