using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.Portals.HOD;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HOD_PRINCImpl : Interfaces.HOD_PRINCInterface
    {
        public DomainModelMsSqlServerContext _db;
        PortalContext _PrincipalDashboardContext;
        public FOContext _FOContext;
        public HOD_PRINCImpl(FOContext context, DomainModelMsSqlServerContext db, PortalContext dashboard)
        {
            _db = db;
            _FOContext = context;
            _PrincipalDashboardContext = dashboard;

        }
        public HOD_DTO getdata(HOD_DTO data)
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                var hod_stf_ids = _PrincipalDashboardContext.HOD_DMO.Where(t => t.MI_Id == data.MI_Id).Select(t => t.HRME_Id).Distinct().ToList();

                data.query01 = (from a in _db.HR_Master_Employee_DMO
                                where (a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_LeftFlag == false && !hod_stf_ids.Contains(a.HRME_Id))
                                select new HOD_DTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                         + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    hrme_employeeCode = a.HRME_EmployeeCode
                                }).Distinct().ToArray();

                data.classlist = (from c in _db.School_M_Class
                                  where (c.ASMCL_ActiveFlag == true && c.MI_Id == data.MI_Id)
                                  select new HOD_DTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      classname = c.ASMCL_ClassName
                                  }).Distinct().ToArray();

                data.hodlist = (from h in _PrincipalDashboardContext.HOD_DMO
                                from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                where (a.HRME_Id == h.HRME_Id && h.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new HOD_DTO
                                {
                                    IHOD_Id = h.IHOD_Id,
                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg=h.IHOD_Flg,
                                    hrme_employeeCode = a.HRME_EmployeeCode
                                }).Distinct().ToArray();

                //var saved_hods = _PrincipalDashboardContext.HOD_DMO.Where(t=>t.MI_Id==data.MI_Id).Distinct().ToList();
                var saved_hods = (from a in _PrincipalDashboardContext.HOD_DMO
                                  from b in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                  from c in _PrincipalDashboardContext.IVRM_HOD_Class_DMO
                                  where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.IHOD_Id == c.IHOD_Id && b.HRME_ActiveFlag == true && b.HRME_Id == a.HRME_Id)
                                  select new HOD_DTO
                                  {
                                      IHOD_Id = a.IHOD_Id,
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName)
                                       + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                      IHOD_ActiveFlag = a.IHOD_ActiveFlag,
                                      IHOD_Flg=a.IHOD_Flg,
                                  }).Distinct().ToList();

                data.saved_hods = saved_hods.ToArray();

                var saved_hods_cls = (from a in _PrincipalDashboardContext.IVRM_HOD_Class_DMO
                                      from b in _PrincipalDashboardContext.HOD_DMO
                                      from d in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                      from c in _PrincipalDashboardContext.School_M_Class
                                      where (a.IHOD_Id == b.IHOD_Id && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true && c.ASMCL_Id == a.ASMCL_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && d.MI_Id == data.MI_Id)
                                      select new HOD_DTO
                                      {
                                          IHOD_Id = a.IHOD_Id,
                                          ASMCL_Id = a.ASMCL_Id,
                                          classname = c.ASMCL_ClassName,
                                          IHODC_ActiveFlag = a.IHODC_ActiveFlag,
                                          IHOD_Flg=b.IHOD_Flg,
                                      }).Distinct().ToList();

                data.saved_hods_cls = saved_hods_cls.ToArray();

                var saved_hods_stf = (from a in _PrincipalDashboardContext.IVRM_HOD_Staff_DMO
                                      from b in _PrincipalDashboardContext.HOD_DMO
                                      from c in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                      where (a.IHOD_Id == b.IHOD_Id && b.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_Id == a.HRME_Id)
                                      select new HOD_DTO
                                      {
                                          IHOD_Id = a.IHOD_Id,
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName)
                                       + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),
                                          IHODS_ActiveFlag = a.IHODS_ActiveFlag,
                                          IHOD_Flg=b.IHOD_Flg,
                                      }).Distinct().ToList();

                data.saved_hods_stf = saved_hods_stf.ToArray();

                data.hodclass = (from p in _PrincipalDashboardContext.HOD_DMO
                                 from q in _PrincipalDashboardContext.IVRM_HOD_Class_DMO
                                 from r in _PrincipalDashboardContext.School_M_Class
                                 from s in _PrincipalDashboardContext.IVRM_HOD_Staff_DMO
                                 from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                 where (p.IHOD_Id == q.IHOD_Id && q.ASMCL_Id == r.ASMCL_Id && p.IHOD_Id == s.IHOD_Id && s.HRME_Id == a.HRME_Id && p.MI_Id == data.MI_Id)
                                 select new HOD_DTO
                                 {
                                     IHOD_Id = p.IHOD_Id,
                                     HRME_Id = a.HRME_Id,
                                     classname = r.ASMCL_ClassName,
                                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                       + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                     IHODC_ActiveFlag = q.IHODC_ActiveFlag,
                                     hrme_employeeCode = a.HRME_EmployeeCode,
                                     IHODS_ActiveFlag = s.IHODS_ActiveFlag
                                 }).Distinct().ToArray();

                //data.hodstaff = (from p in _PrincipalDashboardContext.HOD_DMO
                //                 //from q in _PrincipalDashboardContext.IVRM_HOD_Class_DMO
                //                 //from r in _PrincipalDashboardContext.School_M_Class
                //                 from s in _PrincipalDashboardContext.IVRM_HOD_Staff_DMO
                //                 from a in _PrincipalDashboardContext.HR_Master_Employee_DMO

                //                 where (p.IHOD_Id == s.IHOD_Id && s.HRME_Id == a.HRME_Id && p.MI_Id == data.MI_Id && a.HRME_Id == p.HRME_Id)
                //                 select new HOD_DTO
                //                 {
                //                     IHOD_Id = p.IHOD_Id,
                //                     HRME_Id = a.HRME_Id,
                //                     HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                //                       + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                //                     IHODS_ActiveFlag = s.IHODS_ActiveFlag,
                //                     hrme_employeeCode = a.HRME_EmployeeCode

                //                 }
                //).Distinct().ToArray();

            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public HOD_DTO savedata(HOD_DTO data)
        {
            try
            {
                var empId = data.employee.Select(d => d.HRME_Id).ToList();
                var query = _PrincipalDashboardContext.HOD_DMO.Where(q => empId.Contains(q.HRME_Id)).Select(d => d.HRME_Id).ToList();
                if (query.Count > 0)
                {

                }
                else
                {
                    for (int i = 0; i < data.employee.Count(); i++)
                    {

                        HOD_DMO objpge = new HOD_DMO();

                        objpge.HRME_Id = data.employee[i].HRME_Id;
                        objpge.IHOD_ActiveFlag = true;
                        objpge.MI_Id = data.MI_Id;
                        objpge.IHOD_Flg = data.IHOD_Flg;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _PrincipalDashboardContext.Add(objpge);

                    }
                    var contactExists = _PrincipalDashboardContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.returnsavestatus = "saved";
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                data.hodlist = (from h in _PrincipalDashboardContext.HOD_DMO
                                from a in _PrincipalDashboardContext.HR_Master_Employee_DMO

                                where (a.HRME_Id == h.HRME_Id && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id && h.IHOD_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                select new HOD_DTO
                                {

                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg=h.IHOD_Flg,
                                   
                                }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public HOD_DTO mappHODdata(HOD_DTO data)
        {
            try
            {
                var empId = data.employee.Select(d => d.HRME_Id).ToList();
                if (empId.Count > 0)
                {
                    //var query = _PrincipalDashboardContext.IVRM_HOD_Staff_DMO.Where(q => empId.Contains(q.HRME_Id)).Select(d => d.HRME_Id).ToList();

                    //if (query.Count > 0)
                    //{

                    //}
                    //else
                    //{
                        for (int i = 0; i < data.employee.Count(); i++)
                        {

                            IVRM_HOD_Staff_DMO objpge = new IVRM_HOD_Staff_DMO();

                            objpge.HRME_Id = data.employee[i].HRME_Id;
                            objpge.IHODS_ActiveFlag = true;
                            objpge.IHOD_Id = data.IHOD_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _PrincipalDashboardContext.Add(objpge);

                        }
                        var contactExists = _PrincipalDashboardContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.returnsavestatus = "saved";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                   // }
                }

                var clss_id = data.class_lst.Select(d => d.ASMCL_Id).ToList();
                if (clss_id.Count > 0)
                {
                    //var query2 = _PrincipalDashboardContext.IVRM_HOD_Class_DMO.Where(p => clss_id.Contains(p.IHODC_Id) && p.IHODC_ActiveFlag == true).Select(t => t.IHODC_Id).ToList();
                    //if (query2.Count > 0)
                    //{

                    //}
                    //else
                    //{

                        for (int i = 0; i < data.class_lst.Count(); i++)
                        {

                            IVRM_HOD_Class_DMO objpge = new IVRM_HOD_Class_DMO();

                            objpge.ASMCL_Id = data.class_lst[i].ASMCL_Id;
                            objpge.IHODC_ActiveFlag = true;
                            objpge.IHOD_Id = data.IHOD_Id;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _PrincipalDashboardContext.Add(objpge);

                        }
                        var contactExists = _PrincipalDashboardContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.returnsavestatus = "saved";
                        }
                        else
                        {
                            data.returnval = false;
                        }
                   // }
                }
                data.hodlist = (from h in _PrincipalDashboardContext.HOD_DMO
                                from a in _PrincipalDashboardContext.HR_Master_Employee_DMO

                                where (a.HRME_Id == h.HRME_Id && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id)
                                select new HOD_DTO
                                {

                                    HRME_Id = h.HRME_Id,
                                    HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName)
                                      + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = h.IHOD_ActiveFlag,
                                    IHOD_Flg=h.IHOD_Flg,

                                }
                   ).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public HOD_DTO updateHOD(HOD_DTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.HOD_DMO.Where(a => a.IHOD_Id == data.IHOD_Id).ToList();
                if (query.Count > 0)
                {
                    var update = _PrincipalDashboardContext.HOD_DMO.Single(s => s.IHOD_Id == data.IHOD_Id);
                    if (update.IHOD_ActiveFlag == true)
                    {
                        update.IHOD_ActiveFlag = false;
                    }
                    else
                    {
                        update.IHOD_ActiveFlag = true;
                    }
                    update.UpdatedDate = DateTime.Now;
                    _PrincipalDashboardContext.Update(update);
                    var contactExists = _PrincipalDashboardContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.returnsavestatus = "updated";
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return data;
        }

        public HOD_DTO deactiveY(HOD_DTO data)
        {
            try
            {
                var result = _PrincipalDashboardContext.HOD_DMO.Single(a => a.MI_Id == data.MI_Id && a.IHOD_Id == data.IHOD_Id);
                if (result.IHOD_ActiveFlag == true)
                {
                    result.IHOD_ActiveFlag = false;
                }
                else if (result.IHOD_ActiveFlag == false)
                {
                    result.IHOD_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PrincipalDashboardContext.Update(result);
                int rowAffected = _PrincipalDashboardContext.SaveChanges();
                if (rowAffected > 0)
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
            }
            return data;
        }
    }
}
