

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class PAYCAREEmployeeDetailsImpl : Interfaces.PAYCAREEmployeeDetailsInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public PAYCAREEmployeeDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public PAYCAREEmployeeDetailsDTO Getdetails(PAYCAREEmployeeDetailsDTO data)//int IVRMM_Id
        {

            try
            {


                List<PAYCAREEmployeeDetailsDTO> result = new List<PAYCAREEmployeeDetailsDTO>();
                List<PAYCAREEmployeeDetailsDTO> result1 = new List<PAYCAREEmployeeDetailsDTO>();



                //List<MasterAcademic> list = new List<MasterAcademic>();
                //list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).ToList();
                //data.yearlist = list.ToArray();

                //data.selectedyear= _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id).ToArray();
               // data.MI_Id = 5;
                var  departmentdropdown = _ChairmanDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Id).ToList();

                data.departmentdropdown = departmentdropdown.ToArray();
                if (data.hrmd_id==0)
                {
                    var HRMD_Id = departmentdropdown[0].HRMD_Id;

                    data.hrmd_id = HRMD_Id;

                    data.departmentgraph = (from a in _ChairmanDashboardContext.HR_Master_Department

                                            from b in _ChairmanDashboardContext.HR_Master_Employee_DMO
                                            from c in _ChairmanDashboardContext.HR_Master_Designation
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id &&
                                                                   b.HRMD_Id == a.HRMD_Id &&
                                                                  c.HRMDES_Id == b.HRMDES_Id && b.HRME_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRMD_ActiveFlag == true && b.HRME_LeftFlag==false
                                                               )

                                            select new
                                            {
                                                departmentid = a.HRMD_Id,
                                                departmentname = a.HRMD_DepartmentName,
                                                depttotalEmployees = b.HRME_Id
                                            })

                                     .Distinct().GroupBy(id => new { id.departmentid, id.departmentname }).Select(g => new PAYCAREEmployeeDetailsDTO { hrmd_id = g.Key.departmentid, departmentName = g.Key.departmentname, depttotalEmployees = g.Count() }).ToArray();
                }



                data.filldesiganation = (from a in _ChairmanDashboardContext.HR_Master_Department

                                         from b in _ChairmanDashboardContext.HR_Master_Employee_DMO
                                         from c in _ChairmanDashboardContext.HR_Master_Designation
                                         where (a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id==b.MI_Id &&
                                                                b.HRMD_Id == a.HRMD_Id && b.HRMD_Id == data.hrmd_id &&
                                                               c.HRMDES_Id == b.HRMDES_Id && b.HRME_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRMD_ActiveFlag == true && b.HRME_LeftFlag==false
                                                           )

                                         select new
                                         {
                                             designationid = c.HRMDES_Id,
                                             designationname = c.HRMDES_DesignationName,
                                             totalEmployees = b.HRME_Id
                                         })

                                       .Distinct().GroupBy(id => new { id.designationid, id.designationname }).Select(g => new PAYCAREEmployeeDetailsDTO { HRMDES_Id = g.Key.designationid, designationname = g.Key.designationname, totalEmployees = g.Count() }).ToArray();



              





            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public PAYCAREEmployeeDetailsDTO Getemppop(PAYCAREEmployeeDetailsDTO data)
        {
         
            try
            {
                List<PAYCAREEmployeeDetailsDTO> result1 = new List<PAYCAREEmployeeDetailsDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_EMP_POPUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMDES_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.HRMDES_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrmd_id",
                      SqlDbType.BigInt)
                    {
                        Value = data.hrmd_id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.employeeDetails = retObject.ToArray();
                    }
                    //try
                    //{
                    //    using (var dataReader = cmd.ExecuteReader())
                    //    {
                    //        while (dataReader.Read())
                    //        {
                    //            result1.Add(new PAYCAREEmployeeDetailsDTO
                    //            {
                    //                HRME_Id = Convert.ToInt64(dataReader["HRME_Id"].ToString()),
                    //                empname = (dataReader["empname"].ToString()),
                    //                //doj = Convert.ToDateTime(dataReader["doj"].ToString()),
                    //                mstatus = dataReader["IVRMMMS_MaritalStatus"].ToString(),
                    //                gender = (dataReader["IVRMMG_GenderName"].ToString()),
                    //                mobileno = Convert.ToInt64(dataReader["HRME_MobileNo"].ToString()),
                    //                email = dataReader["HRME_EmailId"].ToString(),
                    //                doj = Convert.ToDateTime(dataReader["doj"].ToString() == null || dataReader["doj"].ToString() == "" ? "" : dataReader["doj"].ToString()),


                    //            });
                    //            data.employeeDetails = result1.ToArray();
                    //        }
                    //    }
                    //}
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }



                //data.employeeDetails = (from a in _ChairmanDashboardContext.HR_Master_Department
                //                        from b in _ChairmanDashboardContext.HR_Master_Employee_DMO
                //                        from c in _ChairmanDashboardContext.HR_Master_Designation
                //                        where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && 
                //                                               b.HRMD_Id == a.HRMD_Id && b.HRMD_Id == data.hrmd_id &&
                //                                              c.HRMDES_Id == b.HRMDES_Id && c.HRMDES_Id == data.HRMDES_Id 
                //                          )

                //                        select new PAYCAREEmployeeDetailsDTO
                //                        {
                //                            HRME_Id = b.HRME_Id,
                //                            empname = b.HRME_EmployeeFirstName
                //                        })

                //                    .Distinct().ToArray();

            }

            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }




        


       

    }
}
