using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeOfferAndExperienceReportDTO
    {

    public long MI_Id { get; set; }
    public string retrunMsg { get; set; }
    public long roleId { get; set; }
    public long HRME_Id { get; set; }

    public long HRMDES_Id { get; set; }

    public long HRMD_Id { get; set; }

    public long HRMGT_Id { get; set; }

    public DateTime HRMER_DOI { get; set; }

    public DateTime HRMER_Current_Date { get; set; }

    //designation name
    public string DesignationName { get; set; }

    //list for employee
    public MasterEmployeeDTO currentemployeeDetails { get; set; }

    //employee name dropdown
    public Array employeedropdown { get; set; }

    //desigantion dropdown
    public Array designationdropdown { get; set; }
    //Employee type dropdown
    public Array emptypedropdown { get; set; }
    //department dropdown
    public Array departmentdropdown { get; set; }

    public InstitutionDTO institutionDetails { get; set; }
        public long LogInUserId { get; set; }


    }
}
