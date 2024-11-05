using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Principal;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Principal;
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
    public class IVRM_PrincipalMappingImpl : Interfaces.IVRM_PrincipalMappingInterface
    {

        private static ConcurrentDictionary<string, IVRM_PrincipalMappingDTO> _login =
        new ConcurrentDictionary<string, IVRM_PrincipalMappingDTO>();
        private PortalContext _PortalContext;


        public IVRM_PrincipalMappingImpl(PortalContext para)
        {
            _PortalContext = para;

        }

        public async Task<IVRM_PrincipalMappingDTO> GetdetailsAsync(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                var principledatalist = _PortalContext.IVRM_PrincipalDMO.Where(a => a.MI_Id == data.MI_Id && a.IPR_ActiveFlag == true).Distinct().ToList();
                if (principledatalist.Count > 0)
                {
                    var hrmeid = principledatalist.Select(f => f.IVRMUL_Id);
                    data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                                      from b in _PortalContext.HR_Master_Designation
                                      from c in _PortalContext.Staff_User_Login
                                      from d in _PortalContext.Exm_Login_PrivilegeDMO
                                      from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                      from g in _PortalContext.School_M_Class
                                      where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && !hrmeid.Contains(a.HRME_Id) && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                      select new IVRM_PrincipalMappingDTO
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
                    //                  where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && !hrmeid.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true)
                    //                  select new IVRM_PrincipalMappingDTO
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
                                      select new IVRM_PrincipalMappingDTO
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
                    //                  where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                    //                  select new IVRM_PrincipalMappingDTO
                    //                  {
                    //                      HRME_Id = a.HRME_Id,
                    //                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                  }).Distinct().ToArray();
                }

                var principledatalist1 = _PortalContext.IVRM_Principal_ClassDMO.Distinct().ToList();
                if (principledatalist1.Count > 0)
                {
                    var cls = principledatalist1.Select(t => t.ASMCL_Id);
                    data.clsslist = (from a in _PortalContext.School_M_Class
                                     from b in _PortalContext.School_Adm_Y_StudentDMO
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && !cls.Contains(a.ASMCL_Id) && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1)
                                     select new IVRM_PrincipalMappingDTO
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
                                     select new IVRM_PrincipalMappingDTO
                                     {
                                         ASMCL_Id = a.ASMCL_Id,
                                         ASMCL_ClassName = a.ASMCL_ClassName,
                                     }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }
                #region alldata
                data.alldata = (from a in _PortalContext.IVRM_PrincipalDMO
                                from b in _PortalContext.IVRM_Principal_ClassDMO
                                from e in _PortalContext.HR_Master_Employee_DMO
                                where (a.IPR_Id == b.IPR_Id && a.IVRMUL_Id == e.HRME_Id && a.MI_Id == data.MI_Id && e.HRME_ActiveFlag == true)
                                select new IVRM_PrincipalMappingDTO
                                {
                                    IPR_Id = a.IPR_Id,
                                    IVRMUL_Id = a.IVRMUL_Id,
                                    HRME_EmployeeCode = e.HRME_EmployeeCode,
                                    HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                    IPR_ActiveFlag = a.IPR_ActiveFlag
                                }).Distinct().ToArray();
                #endregion

                #region form2 data
                data.principlelist = (from a in _PortalContext.IVRM_PrincipalDMO
                                      from b in _PortalContext.HR_Master_Employee_DMO
                                      where (a.IVRMUL_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.IPR_ActiveFlag == true && b.HRME_ActiveFlag == true)
                                      select new IVRM_PrincipalMappingDTO
                                      {
                                          IPR_Id = a.IPR_Id,
                                          IVRMUL_Id = a.IVRMUL_Id,
                                          HRME_Id = b.HRME_Id,
                                          HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToArray();

                var princpldatalst = (from a in _PortalContext.IVRM_PrincipalDMO
                                      where (a.MI_Id == data.MI_Id && a.IPR_ActiveFlag == true)
                                      select new IVRM_PrincipalMappingDTO
                                      {
                                          IVRMUL_Id = a.IVRMUL_Id
                                      }).Distinct().ToList();
                var princplstfdatalst = (from a in _PortalContext.IVRM_PrincipalDMO
                                         from b in _PortalContext.IVRM_Principal_StaffDMO
                                         where (a.IPR_Id == b.IPR_Id && a.MI_Id == data.MI_Id && a.IPR_ActiveFlag == true && b.IRPS_ActiveFlag == true)
                                         select new IVRM_PrincipalMappingDTO
                                         {
                                             HRME_Id = b.HRME_Id
                                         }).Distinct().ToList();


                if (princpldatalst.Count > 0 || princplstfdatalst.Count > 0)
                {
                    var countid = princpldatalst.Select(f => f.IVRMUL_Id);
                    var countid2 = princplstfdatalst.Select(t => t.HRME_Id);

                    data.stafflist2 = (from a in _PortalContext.HR_Master_Employee_DMO
                                       from b in _PortalContext.HR_Master_Designation
                                       from c in _PortalContext.Staff_User_Login
                                       from d in _PortalContext.Exm_Login_PrivilegeDMO
                                       from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                       from g in _PortalContext.School_M_Class
                                       where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && !countid.Contains(a.HRME_Id) && !countid2.Contains(a.HRME_Id) && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                       select new IVRM_PrincipalMappingDTO
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
                    //                   select new IVRM_PrincipalMappingDTO
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
                                       where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                       select new IVRM_PrincipalMappingDTO
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
                    //                   select new IVRM_PrincipalMappingDTO
                    //                   {
                    //                       HRME_Id = a.HRME_Id,
                    //                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                   }).Distinct().ToArray();
                }
                #endregion

                try
                {
                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_PrincipleStaffDetails";
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
                            data.getprincplstafdata = retObject.ToArray();
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
                //data.getprincplstafdata = (from a in _PortalContext.IVRM_PrincipalDMO
                //                       from b in _PortalContext.IVRM_Principal_StaffDMO
                //                       from c in _PortalContext.HR_Master_Employee_DMO
                //                       where (a.IPR_Id == b.IPR_Id && a.IVRMUL_Id == c.HRME_Id && a.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true)
                //                       select new IVRM_PrincipalMappingDTO
                //                       {
                //                           IPR_Id = a.IPR_Id,
                //                           IVRMUL_Id = a.IVRMUL_Id,
                //                           IPR_ActiveFlag = a.IPR_ActiveFlag,
                //                           IRPS_ActiveFlag = b.IRPS_ActiveFlag,
                //                           HRME_EmployeeCode = c.HRME_EmployeeCode,
                //                           HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                //                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_PrincipalMappingDTO saveclsdata(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                if (data.IPR_Id > 0)
                {
                    var Duplicate = _PortalContext.IVRM_PrincipalDMO.Where(t => t.MI_Id == data.MI_Id && t.IPR_Id != data.IPR_Id && t.IVRMUL_Id == data.IVRMUL_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update1 = _PortalContext.IVRM_PrincipalDMO.Where(t => t.MI_Id == data.MI_Id && t.IPR_Id == data.IPR_Id).SingleOrDefault();

                        update1.IVRMUL_Id = data.IVRMUL_Id;
                        update1.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(update1);

                        var resultclass = _PortalContext.IVRM_Principal_ClassDMO.Where(t => t.IPR_Id == data.IPR_Id).ToList();
                        if (resultclass.Count > 0)
                        {
                            foreach (var item in resultclass)
                            {
                                _PortalContext.Remove(item);
                            }
                        }

                        foreach (var ss in data.classlst)
                        {
                            IVRM_Principal_ClassDMO obj2 = new IVRM_Principal_ClassDMO();
                            obj2.IPR_Id = data.IPR_Id;
                            obj2.ASMCL_Id = ss.ASMCL_Id;
                            obj2.UpdatedDate = DateTime.Now;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.IRPC_ActiveFlag = true;

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
                    var Duplicate = _PortalContext.IVRM_PrincipalDMO.Where(t => t.MI_Id == data.MI_Id && t.IVRMUL_Id == data.IVRMUL_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_PrincipalDMO obj = new IVRM_PrincipalDMO();

                        obj.MI_Id = data.MI_Id;
                        obj.IVRMUL_Id = data.IVRMUL_Id;
                        obj.IPR_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj);

                        foreach (var clssid in data.classlst)
                        {
                            IVRM_Principal_ClassDMO obj2 = new IVRM_Principal_ClassDMO();

                            obj2.IPR_Id = obj.IPR_Id;
                            obj2.ASMCL_Id = clssid.ASMCL_Id;
                            obj2.IRPC_ActiveFlag = true;
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

        public IVRM_PrincipalMappingDTO saveprncplstaf(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                if (data.IPRS_Id > 0)
                {
                    var Duplicate = _PortalContext.IVRM_Principal_StaffDMO.Where(t => t.IPRS_Id != data.IPRS_Id && t.IPR_Id == data.IPR_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _PortalContext.IVRM_Principal_StaffDMO.Where(t => t.IPRS_Id == data.IPRS_Id).SingleOrDefault();
                        update.IPR_Id = data.IPR_Id;
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
                    var Duplicate = _PortalContext.IVRM_Principal_StaffDMO.Where(t => t.IPR_Id == data.IPR_Id && t.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_Principal_StaffDMO obj2 = new IVRM_Principal_StaffDMO();
                        obj2.IPR_Id = data.IPR_Id;
                        obj2.HRME_Id = data.HRME_Id;
                        obj2.IRPS_ActiveFlag = true;
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

        public IVRM_PrincipalMappingDTO deactivehod(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_PrincipalDMO.Single(s => s.IPR_Id == data.IPR_Id);

                if (query.IPR_ActiveFlag == true)
                {
                    query.IPR_ActiveFlag = false;

                }
                else
                {
                    query.IPR_ActiveFlag = true;

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

        public IVRM_PrincipalMappingDTO Deactivatestaf(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_Principal_StaffDMO.Single(s => s.IPRS_Id == data.IPRS_Id);

                if (query.IRPS_ActiveFlag == true)
                {
                    query.IRPS_ActiveFlag = false;

                }
                else
                {
                    query.IRPS_ActiveFlag = true;

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


        public IVRM_PrincipalMappingDTO editprincipledata(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                                  from b in _PortalContext.HR_Master_Designation
                                  from c in _PortalContext.Staff_User_Login
                                  from d in _PortalContext.Exm_Login_PrivilegeDMO
                                  from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                  from g in _PortalContext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                  select new IVRM_PrincipalMappingDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().ToArray();

                data.clsslist = (from a in _PortalContext.School_M_Class
                                 from b in _PortalContext.School_Adm_Y_StudentDMO
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1)
                                 select new IVRM_PrincipalMappingDTO
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     ASMCL_ClassName = a.ASMCL_ClassName,
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                data.editprincclaslist = (from a in _PortalContext.IVRM_PrincipalDMO
                                          from b in _PortalContext.IVRM_Principal_ClassDMO
                                          from c in _PortalContext.HR_Master_Employee_DMO
                                          from d in _PortalContext.School_M_Class
                                          where (a.MI_Id == c.MI_Id && a.IPR_Id == b.IPR_Id && a.IVRMUL_Id == c.HRME_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id && a.IPR_Id == data.IPR_Id)
                                          select new IVRM_PrincipalMappingDTO
                                          {
                                              IPR_Id = a.IPR_Id,
                                              IPRC_Id = b.IPRC_Id,
                                              IVRMUL_Id = a.IVRMUL_Id,
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

        public IVRM_PrincipalMappingDTO editprinciplestaffdata(IVRM_PrincipalMappingDTO data)
        {
            try
            {

                data.stafflist = (from a in _PortalContext.HR_Master_Employee_DMO
                                   from b in _PortalContext.HR_Master_Designation
                                   from c in _PortalContext.Staff_User_Login
                                   from d in _PortalContext.Exm_Login_PrivilegeDMO
                                   from f in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                   from g in _PortalContext.School_M_Class
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && b.HRMDES_DesignationName.Contains("TEACHER") && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                                   select new IVRM_PrincipalMappingDTO
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
                //                  where (a.MI_Id == b.MI_Id && a.MI_Id == g.MI_Id && a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == c.Emp_Code && d.Login_Id == c.IVRMSTAUL_Id && d.ELP_Id == f.ELP_Id && g.ASMCL_Id == f.ASMCL_Id && a.MI_Id == data.MI_Id && b.HRMDES_DesignationName == "TEACHER" && d.ELP_Flg == "st" | d.ELP_Flg == "ct" && a.HRME_ActiveFlag == true && b.HRMDES_ActiveFlag == true && g.ASMCL_ActiveFlag == true && f.ELPs_ActiveFlg == true && c.IVRMSTAUL_ActiveFlag == 1 && d.ELP_ActiveFlg == true)
                //                  select new IVRM_PrincipalMappingDTO
                //                  {
                //                      HRME_Id = a.HRME_Id,
                //                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                  }).Distinct().ToArray();

                data.editprincstafflist = (from a in _PortalContext.IVRM_PrincipalDMO
                                           from b in _PortalContext.IVRM_Principal_StaffDMO
                                           from c in _PortalContext.HR_Master_Employee_DMO
                                           where (a.MI_Id == c.MI_Id && a.IPR_Id == b.IPR_Id && b.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && b.IPRS_Id == data.IPRS_Id)
                                           select new IVRM_PrincipalMappingDTO
                                           {
                                               IPR_Id = a.IPR_Id,
                                               IPRS_Id = b.IPRS_Id,
                                               IVRMUL_Id = a.IVRMUL_Id,
                                               HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                               HRME_Id = b.HRME_Id,
                                           }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_PrincipalMappingDTO onmodelclick(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                data.modalclaslist = (from a in _PortalContext.IVRM_PrincipalDMO
                                      from b in _PortalContext.IVRM_Principal_ClassDMO
                                      from c in _PortalContext.HR_Master_Employee_DMO
                                      from d in _PortalContext.School_M_Class
                                      where (a.IPR_Id == b.IPR_Id && a.IVRMUL_Id == c.HRME_Id && b.ASMCL_Id == d.ASMCL_Id && a.MI_Id == data.MI_Id && a.IPR_Id == data.IPR_Id)
                                      select new IVRM_PrincipalMappingDTO
                                      {
                                          IPR_Id = a.IPR_Id,
                                          IPRC_Id = b.IPRC_Id,
                                          IVRMUL_Id = a.IVRMUL_Id,
                                          HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                          HRME_EmployeeCode = c.HRME_EmployeeCode,
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMCL_ClassName = d.ASMCL_ClassName,
                                          IRPC_ActiveFlag = b.IRPC_ActiveFlag,

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public IVRM_PrincipalMappingDTO Deactivateclass(IVRM_PrincipalMappingDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_Principal_ClassDMO.Single(s => s.IPRC_Id == data.IPRC_Id);

                if (query.IRPC_ActiveFlag == true)
                {
                    query.IRPC_ActiveFlag = false;

                }
                else
                {
                    query.IRPC_ActiveFlag = true;

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
