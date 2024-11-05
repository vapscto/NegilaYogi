using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBSC_EventsIMPL : Interfaces.VBSC_EventsInterface
    {
        public VidyaBharathiContext _VidyaBharathiContext;
        public VBSC_EventsIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_EventsDTO getloaddata(VBSC_EventsDTO data)
        {
            try
            {
                var mtid = _VidyaBharathiContext.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.MT_Id = mtid[0].MO_Id;
                data.academicYear = _VidyaBharathiContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.get_Competitionlevel = _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO.Where(m => m.MT_Id == data.MT_Id && m.VBSCMCL_ActiveFlag == true ).OrderBy(m => m.VBSCMCL_Id).ToArray();
                data.get_eventlist = _VidyaBharathiContext.VBSC_Master_EventsDMO.Where(m => m.MT_Id == data.MT_Id && m.VBSCME_ActiveFlag == true).ToArray();
                data.get_VBSCeventlist = (from a in _VidyaBharathiContext.VBSC_EventsDMO
                                          from b in _VidyaBharathiContext.VBSC_Master_Competition_LevelDMO
                                          from c in _VidyaBharathiContext.VBSC_Master_EventsDMO
                                          from d in _VidyaBharathiContext.AcademicYear
                                          where (a.ASMAY_Id == d.ASMAY_Id && a.VBSCMCL_Id == b.VBSCMCL_Id && a.VBSCME_Id == c.VBSCME_Id && a.MT_Id == data.MT_Id)
                                          select new VBSC_EventsDTO
                                          {
                                              VBSCE_Id = a.VBSCE_Id,
                                              VBSCMCL_Id = a.VBSCMCL_Id,
                                              VBSCMCL_CompetitionLevel = b.VBSCMCL_CompetitionLevel,
                                              ASMAY_Id = a.ASMAY_Id,
                                              ASMAY_Year = d.ASMAY_Year,
                                              VBSCME_Id = a.VBSCME_Id,
                                              VBSCME_EventName = c.VBSCME_EventName,
                                              VBSCE_VenueName = a.VBSCE_VenueName,
                                              VBSCE_StartDate = a.VBSCE_StartDate,
                                              VBSCE_EndDate = a.VBSCE_EndDate,
                                              VBSCE_StartTime = a.VBSCE_StartTime,
                                              VBSCE_EndTime = a.VBSCE_EndTime,
                                              VBSCE_Remarks = a.VBSCE_Remarks,
                                              VBSCE_ActiveFlag = a.VBSCE_ActiveFlag
                                          }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "admin";
            }
            return data;
        }

        public VBSC_EventsDTO savedetails(VBSC_EventsDTO data)
        {
            try
            {
                var mtid =_VidyaBharathiContext.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.MT_Id = mtid[0].MO_Id;

                if (data.VBSCE_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_EventsDMO.Where(t => t.MT_Id == data.MT_Id && t.VBSCMCL_Id == data.VBSCMCL_Id && t.VBSCME_Id != data.VBSCME_Id && t.VBSCE_VenueName == data.VBSCE_VenueName).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_EventsDMO.Single(t => t.VBSCE_Id == data.VBSCE_Id);
                        result.MT_Id = data.MT_Id;
                        result.VBSCMCL_Id = data.VBSCMCL_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.IVRMMS_ID = null;
                        result.IVRMMD_ID = null;
                        result.VBSCME_Id = data.VBSCME_Id;
                        result.VBSCE_VenueName = data.VBSCE_VenueName;
                        result.VBSCE_StartDate = data.VBSCE_StartDate;
                        result.VBSCE_EndDate = data.VBSCE_EndDate;
                        result.VBSCE_StartTime = data.VBSCE_StartTime;
                        result.VBSCE_EndTime = data.VBSCE_EndTime;
                        result.VBSCE_Remarks = data.VBSCE_Remarks;
                        result.VBSCE_ActiveFlag = true;
                        result.VBSCE_UpdatedDate = DateTime.Now;
                        result.VBSCE_CreatedDate = DateTime.Now;
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

                    var res = _VidyaBharathiContext.VBSC_EventsDMO.Where(t => t.MT_Id==data.MT_Id && t.VBSCMCL_Id==data.VBSCMCL_Id &&t.VBSCME_Id==data.VBSCME_Id&& t.VBSCE_VenueName == data.VBSCE_VenueName).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_EventsDMO Events = new VBSC_EventsDMO();
                        Events.MT_Id = data.MT_Id;
                        Events.VBSCMCL_Id = data.VBSCMCL_Id;
                        Events.ASMAY_Id = data.ASMAY_Id;
                        Events.IVRMMS_ID = null;
                        Events.IVRMMD_ID = null;
                        Events.VBSCME_Id = data.VBSCME_Id;
                        Events.VBSCE_VenueName = data.VBSCE_VenueName;
                        Events.VBSCE_StartDate = data.VBSCE_StartDate;
                        Events.VBSCE_EndDate = data.VBSCE_EndDate;
                        Events.VBSCE_StartTime = data.VBSCE_StartTime;
                        Events.VBSCE_EndTime = data.VBSCE_EndTime;
                        Events.VBSCE_Remarks = data.VBSCE_Remarks;
                        Events.VBSCE_ActiveFlag = true;
                        Events.VBSCE_UpdatedDate = DateTime.Now;
                        Events.VBSCE_CreatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(Events);

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

        public VBSC_EventsDTO deactive(VBSC_EventsDTO data)
        {
            try
            {

                var result = _VidyaBharathiContext.VBSC_EventsDMO.Single(t => t.VBSCE_Id == data.VBSCE_Id);

                if (result.VBSCE_ActiveFlag == true)
                {
                    result.VBSCE_ActiveFlag = false;
                }
                else if (result.VBSCE_ActiveFlag == false)
                {
                    result.VBSCE_ActiveFlag = true;
                }
                result.VBSCE_UpdatedDate = DateTime.Now;
                result.VBSCE_CreatedDate = DateTime.Now;
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


