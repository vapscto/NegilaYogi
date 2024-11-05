using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class CISReportIMPL : Interfaces.CISReportInterface
    {
        public PlacementContext _PlacementContext;
        public CISReportIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        //load data
        public PL_CampusInterview_ScheduleDTO getdetails(PL_CampusInterview_ScheduleDTO data)
        {
            try
            {
              //  data.getdata = _PlacementContext.PL_CampusInterview_ScheduleDMO.Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PL_CampusInterview_ScheduleDTO report(PL_CampusInterview_ScheduleDTO data)
        {
            try
            {
                data.getdata = _PlacementContext.PL_CampusInterview_ScheduleDMO.Where(t=> t.PLCISCH_DriveFromDate >= data.PLCISCH_DriveFromDate && t.PLCISCH_DriveToDate <= data.PLCISCH_DriveToDate).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
