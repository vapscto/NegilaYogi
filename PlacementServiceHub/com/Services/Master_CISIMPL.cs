using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class Master_CISIMPL : Interfaces.Master_CISInterface
    {
        public PlacementContext _PlacementContext;
        public Master_CISIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        //load data
        public PL_CampusInterview_ScheduleDTO loaddata(int id)
        {
           
            PL_CampusInterview_ScheduleDTO dto = new PL_CampusInterview_ScheduleDTO();
            try
            {
                dto.getsavedata = _PlacementContext.PL_CampusInterview_ScheduleDMO.Distinct().ToArray();

                      
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return dto;

        }

        //save data
        public PL_CampusInterview_ScheduleDTO savedata(PL_CampusInterview_ScheduleDTO data)
        {
            try
            {
                if (data.PLCISCH_Id != 0)
                {
                    var res = _PlacementContext.PL_CampusInterview_ScheduleDMO.Where(t => t.PLCISCH_Id != data.PLCISCH_Id && t.PLCISCH_InterviewVenue == data.PLCISCH_InterviewVenue && t.PLCISCH_JobDetails == data.PLCISCH_JobDetails && t.PLCISCH_DriveFromDate == data.PLCISCH_DriveFromDate && t.PLCISCH_DriveToDate == data.PLCISCH_DriveToDate).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_CampusInterview_ScheduleDMO.Single(t => t.PLCISCH_Id == data.PLCISCH_Id);
                        result.MI_Id = data.MI_Id;
                        result.PLCISCH_InterviewVenue = data.PLCISCH_InterviewVenue;
                        result.PLCISCH_JobDetails = data.PLCISCH_JobDetails;
                        result.PLCISCH_DriveFromDate = data.PLCISCH_DriveFromDate;
                        result.PLCISCH_DriveToDate = data.PLCISCH_DriveToDate;
                        result.PLCISCH_CreatedDate = DateTime.Now;
                        result.PLCISCH_UpdatedDate = DateTime.Now;
                        result.PLCISCH_ActiveFlag = true;
                        _PlacementContext.Update(result);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var res = _PlacementContext.PL_CampusInterview_ScheduleDMO.Where(t => t.PLCISCH_InterviewVenue == data.PLCISCH_InterviewVenue && t.PLCISCH_JobDetails == data.PLCISCH_JobDetails && t.PLCISCH_DriveFromDate == data.PLCISCH_DriveFromDate && t.PLCISCH_DriveToDate == data.PLCISCH_DriveToDate).ToList();

                    if (res.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        PL_CampusInterview_ScheduleDMO an = new PL_CampusInterview_ScheduleDMO();

                       
                        
                        an.PLCISCH_InterviewVenue = data.PLCISCH_InterviewVenue;
                        an.MI_Id = data.MI_Id;
                        an.PLCISCH_JobDetails = data.PLCISCH_JobDetails;
                        an.PLCISCH_DriveFromDate = data.PLCISCH_DriveFromDate;
                        an.PLCISCH_DriveToDate = data.PLCISCH_DriveToDate;
                        an.PLCISCH_CreatedDate = DateTime.Now;
                        an.PLCISCH_UpdatedDate = DateTime.Now;
                        an.PLCISCH_ActiveFlag = true;
                        _PlacementContext.Add(an);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "notsaved";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
     //edit
        public PL_CampusInterview_ScheduleDTO edit(PL_CampusInterview_ScheduleDTO dto)        {            try            {

                dto.editdata = _PlacementContext.PL_CampusInterview_ScheduleDMO.Where(t => t.PLCISCH_Id == dto.PLCISCH_Id).ToArray();
            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }

        //deactive
        public PL_CampusInterview_ScheduleDTO deactive(PL_CampusInterview_ScheduleDTO data)
        {
            try
            {
                var result = _PlacementContext.PL_CampusInterview_ScheduleDMO.Single(t => t.PLCISCH_Id == data.PLCISCH_Id);

                if (result.PLCISCH_ActiveFlag == true)
                {
                    result.PLCISCH_ActiveFlag = false;
                }
                else if (result.PLCISCH_ActiveFlag == false)
                {
                    result.PLCISCH_ActiveFlag = true;
                }
                
                _PlacementContext.Update(result);
                int returnval = _PlacementContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
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
