using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PortalHub.com.vaps.Employee.Services
{
    public class CovidVaccinationIMPL : Interfaces.CovidVaccinationInterface
    {
        public PortalContext _context;
        public CovidVaccinationIMPL(PortalContext _con)
        {
            _context = _con;
        }

        public CovidVaccineDTO onloaddata(CovidVaccineDTO data)
        {
            try
            {
                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                               from b in _context.StudentUserLoginDMO
                               where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                               select b).Select(t => t.AMST_Id).FirstOrDefault();

                data.vaccinationtype = _context.Master_CovidVaccineTypeDMO.Where(a => a.IMCOVVAC_ActiveFlag == true).ToArray();
               
                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                if (data.RoleType == "Staff")
                {
                    data.getloaddetails = (from a in _context.Staff_CovidVaccinationDMO
                                           from b in _context.Master_CovidVaccineTypeDMO
                                           where (a.IMCOVVAC_Id == b.IMCOVVAC_Id &&  a.MI_Id == data.MI_Id && a.HRME_Id == data.hrmeid)
                                           select new CovidVaccineDTO
                                           {
                                               ISTCOVVAC_Id=a.ISTCOVVAC_Id,
                                               IMCOVVAC_Id=b.IMCOVVAC_Id,
                                               IMCOVVAC_VaccinationName = b.IMCOVVAC_VaccinationName,
                                               ISTCOVVAC_Dose=a.ISTCOVVAC_Dose,
                                               ISTCOVVAC_VaccinationDate = a.ISTCOVVAC_VaccinationDate,
                                               ISTUCOVVAC_ActiveFlag=a.ISTCOVVAC_ActiveFlag,
                                               ISTCOVVAC_FilePath=a.ISTCOVVAC_FilePath,
                                           }).Distinct().ToArray();
                }
                else if (data.RoleType == "STUDENT")
                {
                    data.getloaddetails = (from a in _context.Student_CovidVaccinationDMO
                                           from b in _context.Master_CovidVaccineTypeDMO
                                           where (a.IMCOVVAC_Id==b.IMCOVVAC_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.stdid)
                                           select new CovidVaccineDTO
                                           {
                                               ISTUCOVVAC_Id = a.ISTUCOVVAC_Id,
                                               IMCOVVAC_Id = b.IMCOVVAC_Id,
                                               IMCOVVAC_VaccinationName = b.IMCOVVAC_VaccinationName,
                                               ISTCOVVAC_Dose = a.ISTUCOVVAC_Dose,
                                               ISTCOVVAC_VaccinationDate = a.ISTUCOVVAC_VaccinationDate,
                                               ISTUCOVVAC_ActiveFlag=a.ISTUCOVVAC_ActiveFlag,
                                               ISTCOVVAC_FilePath=a.ISTUCOVVAC_FilePath,
                                           }).Distinct().ToArray();
                }
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

                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                              from b in _context.StudentUserLoginDMO
                              where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                              select b).Select(t => t.AMST_Id).FirstOrDefault();

                if (data.RoleType == "Staff")
                {
                    if (data.ISTCOVVAC_Id > 0)
                    {
                        var checkduplicate = _context.Staff_CovidVaccinationDMO.Where(a => a.MI_Id == data.MI_Id && a.ISTCOVVAC_Id != data.ISTCOVVAC_Id && a.ISTCOVVAC_Dose==data.ISTCOVVAC_Dose).ToList();

                        if (checkduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {

                            var checkresult = _context.Staff_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVVAC_Id == data.ISTCOVVAC_Id);
                            checkresult.ISTCOVVAC_VaccinationDate = data.ISTCOVVAC_VaccinationDate;
                            checkresult.IMCOVVAC_Id = data.IMCOVVAC_Id;
                            checkresult.HRME_Id = data.hrmeid;
                            checkresult.ISTCOVVAC_Dose = data.ISTCOVVAC_Dose;
                            checkresult.ISTCOVVAC_FileName = data.ISTCOVVAC_FileName;
                            checkresult.ISTCOVVAC_FilePath = data.ISTCOVVAC_FilePath;
                            checkresult.ISTCOVVAC_UpdatedBy = data.Userid;
                            checkresult.ISTCOVVAC_UpdatedDate = indiantime0;
                            _context.Update(checkresult);
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
                        var checkduplicate = _context.Staff_CovidVaccinationDMO.Where(a => a.MI_Id == data.MI_Id &&  a.ISTCOVVAC_Dose == data.ISTCOVVAC_Dose).ToList();
                        if (checkduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {
                            Staff_CovidVaccinationDMO obj = new Staff_CovidVaccinationDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.ISTCOVVAC_Id = data.ISTCOVVAC_Id;
                            obj.ISTCOVVAC_VaccinationDate = data.ISTCOVVAC_VaccinationDate;
                            obj.IMCOVVAC_Id = data.IMCOVVAC_Id;
                            obj.HRME_Id = data.hrmeid;
                            obj.ISTCOVVAC_Dose = data.ISTCOVVAC_Dose;
                            obj.ISTCOVVAC_FileName = data.ISTCOVVAC_FileName;
                            obj.ISTCOVVAC_FilePath = data.ISTCOVVAC_FilePath;
                            obj.ISTCOVVAC_ActiveFlag = true;
                            obj.ISTCOVVAC_CreatedBy = data.Userid;
                            obj.ISTCOVVAC_UpdatedBy = data.Userid;
                            obj.ISTCOVVAC_CreatedDate = indiantime0;
                            obj.ISTCOVVAC_UpdatedDate = indiantime0;
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
                else if (data.RoleType == "STUDENT")
                {
                    if (data.ISTUCOVVAC_Id > 0)
                    {
                        var checkduplicate = _context.Student_CovidVaccinationDMO.Where(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Id != data.ISTUCOVVAC_Id && a.ISTUCOVVAC_Dose == data.ISTCOVVAC_Dose).ToList();

                        if (checkduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {
                            var checkresult = _context.Student_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Id == data.ISTUCOVVAC_Id);
                            checkresult.ISTUCOVVAC_Id = data.ISTUCOVVAC_Id;
                            checkresult.ISTUCOVVAC_VaccinationDate = data.ISTCOVVAC_VaccinationDate;
                            checkresult.IMCOVVAC_Id = data.IMCOVVAC_Id;
                            checkresult.AMST_Id = data.stdid;
                            checkresult.ISTUCOVVAC_Dose = data.ISTCOVVAC_Dose;
                            checkresult.ISTUCOVVAC_FileName = data.ISTCOVVAC_FileName;
                            checkresult.ISTUCOVVAC_FilePath = data.ISTCOVVAC_FilePath;
                            checkresult.ISTUCOVVAC_UpdatedBy = data.Userid;
                            checkresult.ISTUCOVVAC_UpdatedDate = indiantime0;
                            _context.Update(checkresult);
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
                        var checkduplicate = _context.Student_CovidVaccinationDMO.Where(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Dose == data.ISTCOVVAC_Dose).ToList();
                        if (checkduplicate.Count > 0)
                        {
                            data.message = "Duplicate";
                        }
                        else
                        {
                            Student_CovidVaccinationDMO obj = new Student_CovidVaccinationDMO();
                            obj.ISTUCOVVAC_Id = data.ISTUCOVVAC_Id;
                            obj.MI_Id = data.MI_Id;
                            obj.ISTUCOVVAC_VaccinationDate = data.ISTCOVVAC_VaccinationDate;
                            obj.IMCOVVAC_Id = data.IMCOVVAC_Id;
                            obj.AMST_Id = data.stdid;
                            obj.ISTUCOVVAC_Dose = data.ISTCOVVAC_Dose;
                            obj.ISTUCOVVAC_FileName = data.ISTCOVVAC_FileName;
                            obj.ISTUCOVVAC_FilePath = data.ISTCOVVAC_FilePath;
                            obj.ISTUCOVVAC_ActiveFlag = true;
                            obj.ISTUCOVVAC_CreatedBy = data.Userid;
                            obj.ISTUCOVVAC_UpdatedBy = data.Userid;
                            obj.ISTUCOVVAC_CreatedDate = indiantime0;
                            obj.ISTUCOVVAC_UpdatedDate = indiantime0;
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

                data.RoleType = (from a in _context.ApplicationRole_con
                                 where (a.Id == data.roleid)
                                 select a).Select(t => t.roleType).FirstOrDefault();

                data.hrmeid = (from a in _context.ApplicationUser
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.stdid = (from a in _context.ApplicationUser
                              from b in _context.StudentUserLoginDMO
                              where (a.Id == b.IVRMSTUUL_Id && a.Id == data.Userid)
                              select b).Select(t => t.AMST_Id).FirstOrDefault();

                if (data.RoleType == "Staff")
                {
                    var checkactivestatus = _context.Staff_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVVAC_Id == data.ISTCOVVAC_Id);

                    if (checkactivestatus.ISTCOVVAC_ActiveFlag == true)
                    {
                        var resultdeactive = _context.Staff_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVVAC_Id == data.ISTCOVVAC_Id);

                        if (resultdeactive.ISTCOVVAC_ActiveFlag == true)
                        {
                            resultdeactive.ISTCOVVAC_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTCOVVAC_ActiveFlag = true;
                        }

                        resultdeactive.ISTCOVVAC_UpdatedDate = indiantime0;
                        resultdeactive.ISTCOVVAC_UpdatedBy = data.Userid;
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
                        var resultdeactive = _context.Staff_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTCOVVAC_Id == data.ISTCOVVAC_Id);

                        if (resultdeactive.ISTCOVVAC_ActiveFlag == true)
                        {
                            resultdeactive.ISTCOVVAC_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTCOVVAC_ActiveFlag = true;
                        }

                        resultdeactive.ISTCOVVAC_UpdatedDate = indiantime0;
                        resultdeactive.ISTCOVVAC_UpdatedBy = data.Userid;
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
                else if (data.RoleType == "STUDENT")
                {
                    var checkactivestatus = _context.Student_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Id == data.ISTUCOVVAC_Id);

                    if (checkactivestatus.ISTUCOVVAC_ActiveFlag == true)
                    {
                        var resultdeactive = _context.Student_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Id == data.ISTUCOVVAC_Id);

                        if (resultdeactive.ISTUCOVVAC_ActiveFlag == true)
                        {
                            resultdeactive.ISTUCOVVAC_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTUCOVVAC_ActiveFlag = true;
                        }

                        resultdeactive.ISTUCOVVAC_UpdatedDate = indiantime0;
                        resultdeactive.ISTUCOVVAC_UpdatedBy = data.Userid;
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
                        var resultdeactive = _context.Student_CovidVaccinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ISTUCOVVAC_Id == data.ISTUCOVVAC_Id);

                        if (resultdeactive.ISTUCOVVAC_ActiveFlag == true)
                        {
                            resultdeactive.ISTUCOVVAC_ActiveFlag = false;
                        }
                        else
                        {
                            resultdeactive.ISTUCOVVAC_ActiveFlag = true;
                        }

                        resultdeactive.ISTUCOVVAC_UpdatedDate = indiantime0;
                        resultdeactive.ISTUCOVVAC_UpdatedBy = data.Userid;
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
                }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}
