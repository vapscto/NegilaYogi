using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODPaycareStaffDetailsImpl:Interfaces.HODPaycareStaffDetailsInterface
    {

        private readonly PortalContext _PortalContext;

        public DomainModelMsSqlServerContext _db;
        public HODPaycareStaffDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _PortalContext = cpContext;
            _db = db;
        }

        public HODPaycareStaffDetails_DTO Getdetails(HODPaycareStaffDetails_DTO data)
        {
            try
            {

                List<HODPaycareStaffDetails_DTO> result = new List<HODPaycareStaffDetails_DTO>();
                List<HODPaycareStaffDetails_DTO> result1 = new List<HODPaycareStaffDetails_DTO>();

                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();

                var departmentdropdown = (from a in _PortalContext.HOD_DMO
                                from b in _PortalContext.IVRM_HOD_Staff_DMO
                                from c in _PortalContext.HR_Master_Department
                                from d in _PortalContext.HR_Master_Employee_DMO
                                where ( c.MI_Id==d.MI_Id && c.HRMD_Id==d.HRMD_Id && d.MI_Id==a.MI_Id && d.HRME_Id==a.HRME_Id  && a.IHOD_Id == b.IHOD_Id && a.MI_Id == data.MI_Id && a.HRME_Id==loginData.FirstOrDefault().Emp_Code && a.IHOD_Flg == "HOD" && a.IHOD_ActiveFlag==true &&  d.HRME_LeftFlag == false)
                                          select c ).Distinct().ToList();

                //var departmentdropdown = _PortalContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Id).ToList();

                data.departmentdropdown = departmentdropdown.ToArray();
                if (data.hrmd_id == 0)
                {
                    var HRMD_Id = departmentdropdown[0].HRMD_Id;

                    data.hrmd_id = HRMD_Id;

                    data.departmentgraph = (from a in _PortalContext.HR_Master_Department
                                            from b in _PortalContext.HR_Master_Employee_DMO
                                            from c in _PortalContext.HR_Master_Designation
                                            from d in  _PortalContext.HOD_DMO
                                            from e in _PortalContext.IVRM_HOD_Staff_DMO
                                            where (a.MI_Id == b.MI_Id  && b.HRMD_Id == a.HRMD_Id && c.HRMDES_Id == b.HRMDES_Id && b.HRME_Id==d.HRME_Id && d.IHOD_Id==e.IHOD_Id && a.MI_Id == data.MI_Id && d.HRME_Id==loginData.FirstOrDefault().Emp_Code  && d.IHOD_Flg=="HOD" && b.HRME_ActiveFlag == true && c.HRMDES_ActiveFlag == true &&  b.HRME_LeftFlag == false
                                             && a.HRMD_ActiveFlag == true && d.IHOD_ActiveFlag == true)
                                            select new
                                            {
                                                departmentid = a.HRMD_Id,
                                                departmentname = a.HRMD_DepartmentName,
                                                depttotalEmployees = e.HRME_Id
                                            }).Distinct().GroupBy(id => new { id.departmentid, id.departmentname }).Select(g => new HODPaycareStaffDetails_DTO { hrmd_id = g.Key.departmentid, departmentName = g.Key.departmentname, depttotalEmployees = g.Count() }).ToArray();
                }



                data.filldesiganation = (from a in _PortalContext.HR_Master_Department
                                         from b in _PortalContext.HR_Master_Employee_DMO
                                         from c in _PortalContext.HR_Master_Designation
                                         from d in _PortalContext.HOD_DMO
                                         from e in _PortalContext.IVRM_HOD_Staff_DMO
                                         where (a.MI_Id == c.MI_Id && a.MI_Id == b.MI_Id &&  b.HRMD_Id == a.HRMD_Id  && c.HRMDES_Id == b.HRMDES_Id && b.HRME_Id==d.HRME_Id && d.IHOD_Id==e.IHOD_Id && d.IHOD_Flg=="HOD" && a.MI_Id == data.MI_Id && b.HRMD_Id == data.hrmd_id  && b.HRME_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRMD_ActiveFlag == true && b.HRME_LeftFlag==false && d.IHOD_ActiveFlag == true)
                                         select new
                                         {
                                             designationid = c.HRMDES_Id,
                                             designationname = c.HRMDES_DesignationName,
                                             totalEmployees = b.HRME_Id
                                         }).Distinct().GroupBy(id => new { id.designationid, id.designationname }).Select(g => new HODPaycareStaffDetails_DTO
                                         { HRMDES_Id = g.Key.designationid, designationname = g.Key.designationname, totalEmployees = g.Count() }).ToArray();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }



        public HODPaycareStaffDetails_DTO Getemppop(HODPaycareStaffDetails_DTO data)
        {

            try
            {
                List<HODPaycareStaffDetails_DTO> result1 = new List<HODPaycareStaffDetails_DTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOD_EMP_POPUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                   SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.user_id)
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

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }




    }
}
