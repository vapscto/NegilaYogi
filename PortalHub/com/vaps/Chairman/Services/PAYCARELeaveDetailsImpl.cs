

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
    public class PAYCARELeaveDetailsImpl : Interfaces.PAYCARELeaveDetailsInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public PAYCARELeaveDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public PAYCARELeaveDetailsDTO Getdetails(PAYCARELeaveDetailsDTO data)//int IVRMM_Id
        {

            try
            {


                List<PAYCARELeaveDetailsDTO> result = new List<PAYCARELeaveDetailsDTO>();
                List<PAYCARELeaveDetailsDTO> result1 = new List<PAYCARELeaveDetailsDTO>();

                data.fillyear = _ChairmanDashboardContext.HR_MasterLeaveYear.Where(t => t.MI_Id == data.MI_Id && t.HRMLY_ActiveFlag.Equals(true)).ToArray();
                data.department = _ChairmanDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToArray();

               
                //var  departmentdropdown = _ChairmanDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Id).ToList();

                //data.departmentdropdown = departmentdropdown.ToArray();
                //if (data.hrmd_id==0)
                //{
                //    var HRMD_Id = departmentdropdown[0].HRMD_Id;

                //    data.hrmd_id = HRMD_Id;

                //    data.departmentgraph = (from a in _ChairmanDashboardContext.HR_Master_Department

                //                            from b in _ChairmanDashboardContext.HR_Master_Employee_DMO
                //                            from c in _ChairmanDashboardContext.HR_Master_Designation
                //                            where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id &&
                //                                                   b.HRMD_Id == a.HRMD_Id &&
                //                                                  c.HRMDES_Id == b.HRMDES_Id && b.HRME_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRMD_ActiveFlag == true
                //                                               )

                //                            select new
                //                            {
                //                                departmentid = a.HRMD_Id,
                //                                departmentname = a.HRMD_DepartmentName,
                //                                depttotalEmployees = b.HRME_Id
                //                            })

                //                     .Distinct().GroupBy(id => new { id.departmentid, id.departmentname }).Select(g => new PAYCARELeaveDetailsDTO { hrmd_id = g.Key.departmentid, departmentName = g.Key.departmentname, depttotalEmployees = g.Count() }).ToArray();
                //}



               




            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public PAYCARELeaveDetailsDTO Getdesignation(PAYCARELeaveDetailsDTO data)
        {
         
            try
            {
                data.designation = (from a in _ChairmanDashboardContext.HR_Master_Designation
                                    from b in _ChairmanDashboardContext.HR_Master_Department
                                    from c in _ChairmanDashboardContext.HR_Master_Employee_DMO
                                    where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_Id == c.HRMD_Id && b.HRMD_Id == data.hrmd_id && a.HRMDES_ActiveFlag.Equals(true)
                                    )
                                    //select new PAYCARELeaveDetailsDTO
                                    //{
                                    //    HRMDES_Id=a.HRMDES_Id,
                                    //    designationname=a.HRMDES_DesignationName

                                    //}

                                   select new
                                   {
                                       HRMDES_Id = a.HRMDES_Id,
                                       designationname = a.HRMDES_DesignationName,
                                      
                                   })

                                     .Distinct().GroupBy(id => new { id.HRMDES_Id, id.designationname }).Select(g => new PAYCARELeaveDetailsDTO { HRMDES_Id = g.Key.HRMDES_Id, designationname = g.Key.designationname}).ToArray(); 



            }

            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }



        public PAYCARELeaveDetailsDTO showreport(PAYCARELeaveDetailsDTO data)
        {
            try
            {

           
            //data.MI_Id = 4;
            //data.HRMLY_Id = 3;
            //data.hrmd_id = 1;
            //data.HRMDES_Id = 1;
                //   data.masterleave = _ChairmanDashboardContext.HR_Master_Leave.Where(t => t.MI_Id == data.MI_Id).ToArray();
                data.leavedetails = (from a in _ChairmanDashboardContext.HR_Emp_Leave_StatusDMO
                                     from b in _ChairmanDashboardContext.HR_Emp_Leave_Trans_Details
                                     from c in _ChairmanDashboardContext.HR_Master_Employee_DMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && a.HRML_Id == b.HRML_Id && c.HRMDES_Id == data.HRMDES_Id && c.HRMD_Id == data.hrmd_id && a.HRMLY_Id == data.HRMLY_Id && b.HRELTD_LWPFlag.Equals(true))

                                     select new PAYCARELeaveDetailsDTO
                                     {

                                         HRME_Id = a.HRME_Id,
                                         empname = c.HRME_EmployeeFirstName,
                                         HRML_Id = a.HRML_Id,
                                         HRELS_CBLeaves = a.HRELS_CBLeaves,
                                         HRELS_TotalLeaves = a.HRELS_TotalLeaves,
                                         HRELS_TransLeaves = a.HRELS_TransLeaves,
                                         lop = b.HRELTD_TotDays,
                                         doj = c.HRME_DOJ
                                     }

                    ).ToArray();

                data.masterleave = (from a in _ChairmanDashboardContext.HR_Emp_Leave_StatusDMO
                                   // from b in _ChairmanDashboardContext.HR_Emp_Leave_Trans_Details
                                    from c in _ChairmanDashboardContext.HR_Master_Employee_DMO
                                    from d in _ChairmanDashboardContext.HR_Master_Leave
                                    where ( 
                                    a.MI_Id == d.MI_Id 
                                   &&
                                    a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id 
                                  //  && a.HRME_Id == b.HRME_Id 
                                    && a.HRME_Id == c.HRME_Id
                                   // && a.HRML_Id == b.HRML_Id 
                                    && c.HRMDES_Id == data.HRMDES_Id && c.HRMD_Id == data.hrmd_id && a.HRMLY_Id == data.HRMLY_Id && a.HRML_Id == d.HRML_Id 
                                   // && b.HRELTD_LWPFlag.Equals(true) 
                                    )

                                    select new 
                                    {
                                        HRML_Id = a.HRML_Id,
                                        HRML_LeaveName = d.HRML_LeaveName

                                    }).
Distinct().GroupBy(id => new { id.HRML_Id, id.HRML_LeaveName }).Select(g => new PAYCARELeaveDetailsDTO { HRML_Id = g.Key.HRML_Id, HRML_LeaveName = g.Key.HRML_LeaveName }).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }





    }
}
