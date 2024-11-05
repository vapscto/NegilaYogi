using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class Master_CovidVaccineTypeIMPL : Interfaces.Master_CovidVaccineTypeInterface
    {
        public PortalContext _context;
        public Master_CovidVaccineTypeIMPL(PortalContext _con)
        {
            _context = _con;
        }


        public CovidVaccineDTO onloaddata(CovidVaccineDTO data)
        {
            try
            {

                 data.getloaddetails = _context.Master_CovidVaccineTypeDMO.Where(a => a.IMCOVVAC_ActiveFlag == true).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CovidVaccineDTO saverecord(CovidVaccineDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                List<long> employeeids = new List<long>();
                if (data.IMCOVVAC_Id > 0)
                {
                    var checkduplicate = _context.Master_CovidVaccineTypeDMO.Where(a => a.IMCOVVAC_VaccinationName == data.IMCOVVAC_VaccinationName).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                        var checkresult = _context.Master_CovidVaccineTypeDMO.Single(a => a.IMCOVVAC_Id == data.IMCOVVAC_Id);
                        checkresult.IMCOVVAC_Id = data.IMCOVVAC_Id;
                        checkresult.AMST_Id = 169;
                        checkresult.IMCOVVAC_VaccinationName = data.IMCOVVAC_VaccinationName;
                        checkresult.IMCOVVAC_UpdatedBy = data.Userid;
                        checkresult.IMCOVVAC_UpdatedDate = indiantime0;
                        var kk = _context.SaveChanges();
                        if (kk > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Master_CovidVaccineTypeDMO.Where(a => a.IMCOVVAC_Id == data.IMCOVVAC_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Master_CovidVaccineTypeDMO obj = new Master_CovidVaccineTypeDMO();
                       // obj.IMCOVVAC_Id = data.IMCOVVAC_Id;
                        obj.IMCOVVAC_VaccinationName = data.IMCOVVAC_VaccinationName;
                        obj.AMST_Id = 169;
                        obj.IMCOVVAC_ActiveFlag = true;
                        obj.IMCOVVAC_UpdatedBy = data.Userid;
                        obj.IMCOVVAC_UpdatedDate = indiantime0;
                        obj.IMCOVVAC_CreatedBy = data.Userid;
                        obj.IMCOVVAC_CreatedDate = indiantime0;
                        _context.Add(obj);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CovidVaccineDTO deactiveY(CovidVaccineDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                var checkactivestatus = _context.Master_CovidVaccineTypeDMO.Single(a =>  a.IMCOVVAC_Id == data.IMCOVVAC_Id);

                if (checkactivestatus.IMCOVVAC_ActiveFlag == true)
                {
                    var resultdeactive = _context.Master_CovidVaccineTypeDMO.Single(a => a.IMCOVVAC_Id == data.IMCOVVAC_Id);

                    if (resultdeactive.IMCOVVAC_ActiveFlag == true)
                    {
                        resultdeactive.IMCOVVAC_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.IMCOVVAC_ActiveFlag = true;
                    }

                    resultdeactive.IMCOVVAC_UpdatedDate = indiantime0;
                    resultdeactive.IMCOVVAC_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                else
                {
                    var resultdeactive = _context.Master_CovidVaccineTypeDMO.Single(a => a.IMCOVVAC_Id == data.IMCOVVAC_Id);

                    if (resultdeactive.IMCOVVAC_ActiveFlag == true)
                    {
                        resultdeactive.IMCOVVAC_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.IMCOVVAC_ActiveFlag = true;
                    }

                    resultdeactive.IMCOVVAC_UpdatedDate = indiantime0;
                    resultdeactive.IMCOVVAC_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}
