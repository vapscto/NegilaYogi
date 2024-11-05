
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
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Microsoft.AspNetCore.Hosting;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.HRMS;

namespace PortalHub.com.vaps.Principal.Services
{
    public class AttendanceStaffWiseImpl : Interfaces.AttendanceStaffWiseInterface
    {
        int MI_ID = 0;
        private readonly IHostingEnvironment _hostingEnvironment;

        private static ConcurrentDictionary<string, AttendanceStaffWiseDTO> _login =
         new ConcurrentDictionary<string, AttendanceStaffWiseDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<AttendanceStaffWiseImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public AttendanceStaffWiseImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, IHostingEnvironment hostingEnvironment)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public AttendanceStaffWiseDTO Getdetails(AttendanceStaffWiseDTO data)//int IVRMM_Id
        {
            try
            {
                List<AttendanceStaffWiseDTO> result = new List<AttendanceStaffWiseDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Principal_StaffAttandance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                     SqlDbType.Date)
                    {
                        Value = data.Fromdate.Value.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                     SqlDbType.Date)
                    {
                        Value = data.Todate.Value.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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
                                result.Add(new AttendanceStaffWiseDTO
                                {
                                    empFname = dataReader["ename"].ToString(),
                                    HRME_EmployeeCode= dataReader["ecode"].ToString(),
                                    FOEST_IHalfLoginTime = dataReader["actualtime"].ToString(),
                                    FOEP_PunchDate = Convert.ToDateTime(dataReader["punchdate"].ToString()),
                                    ts = dataReader["lateby"].ToString(),
                                    FOEPD_PunchTime = dataReader["intime"].ToString()

                                });
                                data.StaffName = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }
   


        public AttendanceStaffWiseDTO Getdepartment(AttendanceStaffWiseDTO data)
        {
            var departmentdropdown =  _PrincipalDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();

            var GroupTypelist = _PrincipalDashboardContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
            data.groupTypedropdown = GroupTypelist.ToArray();
            return data;
        }

        public AttendanceStaffWiseDTO get_department(AttendanceStaffWiseDTO dto)
        {
            //dto.departmentdropdown = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
            //                           from b in _PrincipalDashboardContext.HR_Master_GroupType
            //                           from c in _PrincipalDashboardContext.HR_Master_Department
            //                           where (a.HRMGT_Id == b.HRMGT_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
            //                           && b.HRMGT_ActiveFlag == true && a.HRME_ActiveFlag == true
            //                           && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == dto.MI_Id && dto.multipledep.ToString().Contains(Convert.ToString(b.HRMGT_Id)))
            //                           select new SalaryDetailsDTO
            //                           {
            //                               HRMGT_Id = b.HRMGT_Id,
            //                                HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,
            //                           }
            //         ).Distinct().ToArray();

            dto.departmentdropdown = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                                      from b in _PrincipalDashboardContext.HR_Master_Department
                                      where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && dto.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(dto.MI_Id) && b.HRMD_ActiveFlag == true)
                                      select b).Distinct().ToArray();

            return dto;
        }
        public AttendanceStaffWiseDTO get_designation(AttendanceStaffWiseDTO data)
        {
            data.designationdropdown = ( from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                                         from b in _PrincipalDashboardContext.HR_Master_Designation
                                         from c in _PrincipalDashboardContext.HR_Master_Department
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                    select new AttendanceStaffWiseDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
       
        public AttendanceStaffWiseDTO get_employee(AttendanceStaffWiseDTO data)
        {
            data.stafflist = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                              where (a.MI_Id == data.MI_Id && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))  && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)) && a.HRME_ActiveFlag==true && a.HRME_LeftFlag==false)
                                 select new AttendanceStaffWiseDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode
                                 }
                     ).Distinct().OrderBy(t=>t.HRME_EmployeeFirstName).ToArray();
            return data;
        }
    }
}
