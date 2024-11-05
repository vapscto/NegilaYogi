using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;

namespace WebApplication1.Services
{
    public class InstitutionUserMappingImpl : Interfaces.InstitutionUserMappingInterface
    {
        public DomainModelMsSqlServerContext _db;
        public InstitutionUserMappingImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public InstitutionUserMappingDTO loaddata(InstitutionUserMappingDTO data)
        {
            try
            {
                data.getinstitution = _db.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().OrderBy(a => a.MI_Name).ToArray();

                data.getinstitutionloaddata = (from a in _db.Institution
                                               from b in _db.IVRM_Payment_User_MappingDMO
                                               where (a.MI_Id == b.MI_Id)
                                               select new InstitutionUserMappingDTO
                                               {
                                                   MI_Id = b.MI_Id,
                                                   MI_Name = a.MI_Name
                                               }).Distinct().OrderBy(a => a.MI_Name).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public InstitutionUserMappingDTO onchangeinst(InstitutionUserMappingDTO data)
        {
            try
            {
                data.getemployeedetails = (from a in _db.Institution
                                           from b in _db.HR_Master_Employee_DMO
                                           where (a.MI_Id == b.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && b.MI_Id == data.MI_Id)
                                           select new InstitutionUserMappingDTO
                                           {
                                               HRME_Id = b.HRME_Id,
                                               employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName) + " : " + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " " + b.HRME_EmployeeCode)).Trim(),
                                           }).Distinct().OrderBy(a => a.employeename).ToArray();

                data.getsavedemployee = _db.IVRM_Payment_User_MappingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public InstitutionUserMappingDTO savedetails(InstitutionUserMappingDTO data)
        {
            try
            {
                if (data.saveselectedlist.Length > 0)
                {

                    List<long> empid = new List<long>();

                    foreach (var c in data.saveselectedlist)
                    {
                        empid.Add(c.HRME_Id);
                    }

                    Array removehrmeids = _db.IVRM_Payment_User_MappingDMO.Where(t => !empid.Contains(t.HRME_Id) && t.MI_Id == data.MI_Id).ToArray();

                    foreach (IVRM_Payment_User_MappingDMO ph1 in removehrmeids)
                    {
                        _db.Remove(ph1);
                    }


                    foreach (var c in data.saveselectedlist)
                    {
                        var getuserid = _db.Staff_User_Login.Where(a => a.Emp_Code == c.HRME_Id).ToList();

                        if (getuserid.Count > 0)
                        {
                            var checkhrmeid = _db.IVRM_Payment_User_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id).ToList();

                            if (checkhrmeid.Count > 0)
                            {
                                var checkresult = _db.IVRM_Payment_User_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id);
                                checkresult.UpdatedDate = DateTime.Now;
                                _db.Update(checkresult);
                            }
                            else
                            {
                                IVRM_Payment_User_MappingDMO iVRM_Payment_User_MappingDMO = new IVRM_Payment_User_MappingDMO();
                                iVRM_Payment_User_MappingDMO.MI_Id = data.MI_Id;
                                iVRM_Payment_User_MappingDMO.HRME_Id = c.HRME_Id;
                                iVRM_Payment_User_MappingDMO.IVRMPUM_ActiveFlag = true;
                                iVRM_Payment_User_MappingDMO.User_Id = getuserid.FirstOrDefault().Id;
                                iVRM_Payment_User_MappingDMO.CreatedDate = DateTime.Now;
                                iVRM_Payment_User_MappingDMO.UpdatedDate = DateTime.Now;
                                _db.Add(iVRM_Payment_User_MappingDMO);
                            }
                        }
                    }

                    var i = _db.SaveChanges();

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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public InstitutionUserMappingDTO viewdetails(InstitutionUserMappingDTO data)
        {
            try
            {
                data.getviewemployeedetails = (from a in _db.IVRM_Payment_User_MappingDMO
                                               from b in _db.HR_Master_Employee_DMO
                                               where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false
                                               && b.MI_Id == data.MI_Id)
                                               select new InstitutionUserMappingDTO
                                               {
                                                   HRME_Id = b.HRME_Id,
                                                   employeename = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " +
                                                   (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ?
                                                   " " : b.HRME_EmployeeLastName)).Trim(),
                                                   employeecode = b.HRME_EmployeeCode
                                               }).Distinct().OrderBy(a => a.employeename).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public InstitutionUserMappingDTO savepaymentremarks(InstitutionUserMappingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                IVRM_Payment_Subscription_Login_RemarksDMO iVRM_Payment_Subscription_RemarksDetilsDMO = new IVRM_Payment_Subscription_Login_RemarksDMO();
                iVRM_Payment_Subscription_RemarksDetilsDMO.UserId = data.IVRMUL_Id;
                iVRM_Payment_Subscription_RemarksDetilsDMO.MI_Id = data.MI_Id;
                iVRM_Payment_Subscription_RemarksDetilsDMO.IVRMPSLR_LoginDatetime = indiantime0;
                iVRM_Payment_Subscription_RemarksDetilsDMO.CreatedDate = indiantime0;
                iVRM_Payment_Subscription_RemarksDetilsDMO.UpdatedDate = indiantime0;
                iVRM_Payment_Subscription_RemarksDetilsDMO.IVRMPSLR_Remarks = data.subscriptionremarks;

                _db.Add(iVRM_Payment_Subscription_RemarksDetilsDMO);
                var i = _db.SaveChanges();

                if (i > 0)
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
        public InstitutionUserMappingDTO getvmspaymentsubsctiptionreport(InstitutionUserMappingDTO data)
        {
            try
            {
                DateTime fromdatenew = new DateTime();
                string convertfromdate = "";
                fromdatenew = Convert.ToDateTime(data.fromdate.Value.Date);
                convertfromdate = fromdatenew.ToString("yyyy-MM-dd");

                DateTime todatenew = new DateTime();
                string converttodate = "";
                todatenew = Convert.ToDateTime(data.todate.Value.Date);
                converttodate = todatenew.ToString("yyyy-MM-dd");

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_GET_PAYMENT_NOTIFICATION_REMARKS_REPORT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = convertfromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = converttodate });

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

                        data.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
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