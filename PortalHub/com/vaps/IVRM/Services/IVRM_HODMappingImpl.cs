using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.HOD;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRM.Services
{
    public class IVRM_HODMappingImpl : Interfaces.IVRM_HODMappingInterface
    {
        private static ConcurrentDictionary<string, IVRM_HodMappingDTO> _login =
        new ConcurrentDictionary<string, IVRM_HodMappingDTO>();
        private PortalContext _PortalContext;


        public IVRM_HODMappingImpl(PortalContext para)
        {
            _PortalContext = para;

        }

        public async Task<IVRM_HodMappingDTO> GetdetailsAsync(IVRM_HodMappingDTO data)
        {
            try
            {
                var hoddatalist = _PortalContext.HOD_DMO.Where(a => a.MI_Id == data.MI_Id && a.IHOD_ActiveFlag == true).Distinct().ToList();
                if (hoddatalist.Count > 0)
                {
                    var hrmeid = hoddatalist.Select(f => f.HRME_Id);
                    data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      from b in _PortalContext.HR_Master_Designation
                                      from c in _PortalContext.Staff_User_Login
                                      from d in _PortalContext.Exm_Login_PrivilegeDMO
                                      from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                      from g in _PortalContext.School_M_Class
                                      where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && !hrmeid.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                      select new IVRM_HodMappingDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();
                    //data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                    //                  from b in _PortalContext.HR_Master_Designation
                    //                  from c in _PortalContext.Staff_User_Login
                    //                  from d in _PortalContext.Exm_Login_PrivilegeDMO
                    //                  from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                    //                  from g in _PortalContext.School_M_Class
                    //                  where (a.MI_Id==b.MI_Id && a.MI_Id== g.MI_Id && a.HRMDES_Id==b.HRMDES_Id && a.HRME_Id==c.Emp_Code && d.Login_Id==c.IVRMSTAUL_Id && d.ELP_Id==f.ELP_Id && g.ASMCL_Id==f.ASMCL_Id && !hrmeid.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id  && b.HRMDES_DesignationName== "TEACHER" && d.ELP_Flg=="st" | d.ELP_Flg=="ct" &&  a.HRME_ActiveFlag == true)
                    //                  select new IVRM_HodMappingDTO
                    //                  {
                    //                      HRME_Id = a.HRME_Id,
                    //                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                  }).Distinct().ToArray();
                }
                else
                {
                    data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      from b in _PortalContext.HR_Master_Designation
                                      from c in _PortalContext.Staff_User_Login
                                      from d in _PortalContext.Exm_Login_PrivilegeDMO
                                      from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                      from g in _PortalContext.School_M_Class
                                      where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                      select new IVRM_HodMappingDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();

                    //data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                    //                  where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                    //                  select new IVRM_HodMappingDTO
                    //                  {
                    //                      HRME_Id = a.HRME_Id,
                    //                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                  }).Distinct().ToArray();
                }

                var hodclasslist = _PortalContext.IVRM_HOD_Class_DMO.Distinct().ToList();
                if (hodclasslist.Count > 0)
                {
                    var cls = hodclasslist.Select(t => t.ASMCL_Id);
                    data.clsslist = (from a in _PortalContext.School_M_Class
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && !cls.Contains(a.ASMCL_Id) && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1)
                                     select new IVRM_HodMappingDTO
                                     {
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMCL_ClassName = a.ASMCL_ClassName,
                                     }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();


                }
                else
                {
                    data.clsslist = (from a in _PortalContext.School_M_Class
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1)
                                     select new IVRM_HodMappingDTO
                                     {
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMCL_ClassName = a.ASMCL_ClassName,
                                     }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }
                #region alldata
                data.alldata = (from a in _PortalContext.HOD_DMO
                                from b in _PortalContext.IVRM_HOD_Class_DMO
                                from e in _PortalContext.HR_Master_Employee_DMO
                                where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == e.HRME_Id && a.MI_Id == data.MI_Id && e.HRME_ActiveFlag == true)
                                select new IVRM_HodMappingDTO
                                {
                                    IHOD_Id = a.IHOD_Id,
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeCode = e.HRME_EmployeeCode,
                                    HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                    IHOD_ActiveFlag = a.IHOD_ActiveFlag
                                }).Distinct().ToArray();
                #endregion

                #region form2 data
                data.hodlist = (from a in _PortalContext.HOD_DMO
                                from b in _PortalContext.HR_Master_Employee_DMO
                                where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.IHOD_ActiveFlag == true && b.HRME_ActiveFlag == true)
                                select new IVRM_HodMappingDTO
                                {
                                    IHOD_Id = a.IHOD_Id,
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                }).Distinct().ToArray();

                var hoddatalst = (from a in _PortalContext.HOD_DMO
                                  where (a.MI_Id == data.MI_Id && a.IHOD_ActiveFlag==true)
                                  select new IVRM_HodMappingDTO
                                  {
                                      HRME_Id = a.HRME_Id
                                  }).Distinct().ToList();
                var hodstfdatalst = (from a in _PortalContext.HOD_DMO
                                     from b in _PortalContext.IVRM_HOD_Staff_DMO
                                     where (a.IHOD_Id == b.IHOD_Id && a.MI_Id == data.MI_Id && a.IHOD_ActiveFlag == true && b.IHODS_ActiveFlag==true)
                                     select new IVRM_HodMappingDTO
                                     {
                                         HRME_Id = b.HRME_Id
                                     }).Distinct().ToList();


                if (hoddatalst.Count > 0 || hodstfdatalst.Count > 0)
                {
                    var countid = hoddatalist.Select(f => f.HRME_Id);
                    var countid2 = hodstfdatalst.Select(t => t.HRME_Id);

                    data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                                       from b in _PortalContext.HR_Master_Designation
                                       from c in _PortalContext.Staff_User_Login
                                       from d in _PortalContext.Exm_Login_PrivilegeDMO
                                       from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                       from g in _PortalContext.School_M_Class
                                       where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && !countid.Contains(a.HRME_Id) && !countid2.Contains(a.HRME_Id) && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                       select new IVRM_HodMappingDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       }).Distinct().ToArray();


                    //data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                    //                   from b in _PortalContext.HR_Master_Designation
                    //                   from c in _PortalContext.Staff_User_Login
                    //                   from d in _PortalContext.Exm_Login_PrivilegeDMO
                    //                   from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                    //                   from g in _PortalContext.School_M_Class
                    //                   where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && !countid.Contains(a.HRME_Id) && !countid2.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true)
                    //                   select new IVRM_HodMappingDTO
                    //                   {
                    //                       HRME_Id = a.HRME_Id,
                    //                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                   }).Distinct().ToArray();
                }
                else
                {
                    data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                                       from b in _PortalContext.HR_Master_Designation
                                       from c in _PortalContext.Staff_User_Login
                                       from d in _PortalContext.Exm_Login_PrivilegeDMO
                                       from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                       from g in _PortalContext.School_M_Class
                                       where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                       select new IVRM_HodMappingDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       }).Distinct().ToArray();
                }
                #endregion

                try
                {
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_HodStaffDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.gethodstafdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_HodMappingDTO saveclsdata(IVRM_HodMappingDTO data)
        {
            try
            {
                if (data.IHOD_Id > 0)
                {
                    var Duplicate = _PortalContext.HOD_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHOD_Id != data.IHOD_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update1 = _PortalContext.HOD_DMO.Where(t => t.MI_Id == data.MI_Id && t.IHOD_Id == data.IHOD_Id).SingleOrDefault();

                        update1.IHOD_Id = data.IHOD_Id;
                        update1.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(update1);

                        var resultclass = _PortalContext.IVRM_HOD_Class_DMO.Where(t => t.IHOD_Id == data.IHOD_Id).ToList();
                        if (resultclass.Count > 0)
                        {
                            foreach (var item in resultclass)
                            {
                                _PortalContext.Remove(item);
                            }
                        }

                        foreach (var ss in data.classlst)
                        {
                            IVRM_HOD_Class_DMO obj2 = new IVRM_HOD_Class_DMO();
                            obj2.IHOD_Id = data.IHOD_Id;
                            obj2.ASMCL_Id = ss.ASMCL_Id;
                            obj2.UpdatedDate = DateTime.Now;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.IHODC_ActiveFlag = true;

                            _PortalContext.Add(obj2);
                        }

                        _PortalContext.Update(update1);
                        int returnval = _PortalContext.SaveChanges();
                        if (returnval > 0)
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
                    var Duplicate = _PortalContext.HOD_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HOD_DMO obj = new HOD_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.IHOD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj);

                        foreach (var clssid in data.classlst)
                        {
                            IVRM_HOD_Class_DMO obj2 = new IVRM_HOD_Class_DMO();

                            obj2.IHOD_Id = obj.IHOD_Id;
                            obj2.ASMCL_Id = clssid.ASMCL_Id;
                            obj2.IHODC_ActiveFlag = true;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _PortalContext.Add(obj2);

                        }

                        int rowAffected = _PortalContext.SaveChanges();
                        if (rowAffected > 0)
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

        public IVRM_HodMappingDTO savehodstaf(IVRM_HodMappingDTO data)
        {
            try
            {
                if (data.IHODS_Id > 0)
                {
                    var Duplicate = _PortalContext.IVRM_HOD_Staff_DMO.Where(t => t.IHODS_Id != data.IHODS_Id && t.IHOD_Id == data.IHOD_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _PortalContext.IVRM_HOD_Staff_DMO.Where(t => t.IHODS_Id == data.IHODS_Id).SingleOrDefault();

                        update.IHOD_Id = data.IHOD_Id;
                        update.HRME_Id = data.HRME_Id;

                        update.UpdatedDate = DateTime.Now;

                        _PortalContext.Update(update);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
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
                    var Duplicate = _PortalContext.IVRM_HOD_Staff_DMO.Where(t => t.IHOD_Id == data.IHOD_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_HOD_Staff_DMO obj2 = new IVRM_HOD_Staff_DMO();
                        obj2.IHOD_Id = data.IHOD_Id;
                        obj2.HRME_Id = data.HRME_Id;
                        obj2.IHODS_ActiveFlag = true;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj2);

                        int rowAffected = _PortalContext.SaveChanges();

                        if (rowAffected > 0)
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

        public IVRM_HodMappingDTO deactivehod(IVRM_HodMappingDTO data)
        {
            try
            {
                var query = _PortalContext.HOD_DMO.Single(s => s.IHOD_Id == data.IHOD_Id);

                if (query.IHOD_ActiveFlag == true)
                {
                    query.IHOD_ActiveFlag = false;

                }
                else
                {
                    query.IHOD_ActiveFlag = true;

                }
                query.UpdatedDate = DateTime.Now;

                _PortalContext.Update(query);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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

        public IVRM_HodMappingDTO Deactivatestaf(IVRM_HodMappingDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_HOD_Staff_DMO.Single(s => s.IHODS_Id == data.IHODS_Id);

                if (query.IHODS_ActiveFlag == true)
                {
                    query.IHODS_ActiveFlag = false;

                }
                else
                {
                    query.IHODS_ActiveFlag = true;

                }
                query.UpdatedDate = DateTime.Now;

                _PortalContext.Update(query);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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

        public IVRM_HodMappingDTO editHoddata(IVRM_HodMappingDTO data)
        {
            try
            {
                data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                                   from b in _PortalContext.HR_Master_Designation
                                   from c in _PortalContext.Staff_User_Login
                                   from d in _PortalContext.Exm_Login_PrivilegeDMO
                                   from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                   from g in _PortalContext.School_M_Class
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                   select new IVRM_HodMappingDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.clsslist = (from a in _PortalContext.School_M_Class
                                 from b in _PortalContext.School_Adm_Y_StudentDMO
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1)
                                 select new IVRM_HodMappingDTO
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     ASMCL_ClassName = a.ASMCL_ClassName,
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                data.edithodlist = (from a in _PortalContext.HOD_DMO
                                    from b in _PortalContext.IVRM_HOD_Class_DMO
                                    from c in _PortalContext.HR_Master_Employee_DMO
                                    from d in _PortalContext.School_M_Class
                                    where (a.MI_Id == c.MI_Id && a.IHOD_Id == b.IHOD_Id && a.HRME_Id == c.HRME_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id && a.IHOD_Id == data.IHOD_Id)
                                    select new IVRM_HodMappingDTO
                                    {
                                        IHOD_Id = a.IHOD_Id,
                                        IHODC_Id = b.IHODC_Id,
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                        ASMCL_Id = b.ASMCL_Id,
                                        ASMCL_ClassName = d.ASMCL_ClassName,

                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_HodMappingDTO editHodStaffdata(IVRM_HodMappingDTO data)
        {
            try
            {
                data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                                  from b in _PortalContext.HR_Master_Designation
                                  from c in _PortalContext.Staff_User_Login
                                  from d in _PortalContext.Exm_Login_PrivilegeDMO
                                  from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                  from g in _PortalContext.School_M_Class
                                  where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                  select new IVRM_HodMappingDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToArray();

                //data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                //                   from b in _PortalContext.HR_Master_Designation
                //                   from c in _PortalContext.Staff_User_Login
                //                   from d in _PortalContext.Exm_Login_PrivilegeDMO
                //                   from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                //                   from g in _PortalContext.School_M_Class
                //                   where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                //                   select new IVRM_HodMappingDTO
                //                   {
                //                       HRME_Id = a.HRME_Id,
                //                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                   }).Distinct().ToArray();


                data.edithodstaflist = (from a in _PortalContext.HOD_DMO
                                        from b in _PortalContext.IVRM_HOD_Staff_DMO
                                        from c in _PortalContext.HR_Master_Employee_DMO
                                        where (a.MI_Id == c.MI_Id && a.IHOD_Id == b.IHOD_Id && b.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && b.IHODS_Id == data.IHODS_Id)
                                        select new IVRM_HodMappingDTO
                                        {
                                            IHOD_Id = a.IHOD_Id,
                                            IHODS_Id = b.IHODS_Id,
                                            HRME_Id = a.HRME_Id,
                                            HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                            staff_id = b.HRME_Id,

                                        }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_HodMappingDTO onmodelclick(IVRM_HodMappingDTO data)
        {
            try
            {
                data.modalclaslist = (from a in _PortalContext.HOD_DMO
                                      from b in _PortalContext.IVRM_HOD_Class_DMO
                                      from c in _PortalContext.HR_Master_Employee_DMO
                                      from d in _PortalContext.School_M_Class
                                      where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == c.HRME_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id && a.IHOD_Id == data.IHOD_Id)
                                      select new IVRM_HodMappingDTO
                                      {
                                          IHOD_Id = a.IHOD_Id,
                                          IHODC_Id = b.IHODC_Id,
                                          HRME_Id = a.HRME_Id,
                                          HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                          HRME_EmployeeCode = c.HRME_EmployeeCode,
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = d.ASMCL_ClassName,
                                          IHODC_ActiveFlag = b.IHODC_ActiveFlag,

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_HodMappingDTO Deactivateclass(IVRM_HodMappingDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_HOD_Class_DMO.Single(s => s.IHODC_Id == data.IHODC_Id);

                if (query.IHODC_ActiveFlag == true)
                {
                    query.IHODC_ActiveFlag = false;

                }
                else
                {
                    query.IHODC_ActiveFlag = true;

                }
                query.UpdatedDate = DateTime.Now;

                _PortalContext.Update(query);

                var contactExists = _PortalContext.SaveChanges();
                if (contactExists > 0)
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
