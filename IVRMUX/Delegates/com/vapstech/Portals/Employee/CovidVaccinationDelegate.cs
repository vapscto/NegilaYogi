﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;
namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class CovidVaccinationDelegate
    {
        CommonDelegate<CovidVaccineDTO, CovidVaccineDTO> _com = new CommonDelegate<CovidVaccineDTO, CovidVaccineDTO>();

        public CovidVaccineDTO onloaddata(CovidVaccineDTO data)
        {
            return _com.POSTPORTALData(data, "CovidVaccinationFacade/onloaddata");
        }
        public CovidVaccineDTO saverecord(CovidVaccineDTO data)
        {
            return _com.POSTPORTALData(data, "CovidVaccinationFacade/saverecord");
        }
        public CovidVaccineDTO deactiveY(CovidVaccineDTO data)
        {
            return _com.POSTPORTALData(data, "CovidVaccinationFacade/deactiveY");
        }

    }
}