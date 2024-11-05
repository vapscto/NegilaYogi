using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class Employee_MedicalRecordImpl : Interfaces.Employee_MedicalRecordInterface
    {
        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<IVRM_PushNotificationImpl> _logPortal;
        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _fees;
        public Employee_MedicalRecordImpl(PortalContext portalContext, DomainModelMsSqlServerContext context, FeeGroupContext fee, DomainModelMsSqlServerContext db, ILogger<IVRM_PushNotificationImpl> log)
        {
            _PortalContext = portalContext;
            _db = db;
            _logPortal = log;
            _context = context;
            _fees = fee;
        }
        public Employee_MedicalRecordDTO Getdetails(Employee_MedicalRecordDTO data)
        {
            try
            {

                data.staffarray = _PortalContext.HR_Master_Employee_DMO.Where(R => R.MI_Id == data.MI_Id && R.HRME_ActiveFlag == true && R.HRME_LeftFlag == false).Distinct().ToArray();

                //    data.appliedgrid = _PortalContext.HR_Employee_MedicalRecordDMO.Where(R => R.MI_Id == data.MI_Id && R.HREMR_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Medical_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = 1 });
                    cmd.Parameters.Add(new SqlParameter("@HREMR_Id", SqlDbType.VarChar) { Value = 0 });


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
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.appliedgrid = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }


        public Employee_MedicalRecordDTO savedetail(Employee_MedicalRecordDTO data)
        {
            try
            {
                var count = 0;
                // $scope.documentListOtherDetails = [];
                if (data.HREMR_Id > 0)
                {
                    if (data.filelistMedical != null && data.filelistMedical.Length > 0)
                    {
                        var updatelist = _PortalContext.HR_Employee_MedicalRecord_FileDMO.Where(R => R.HREMR_Id == data.HREMR_Id).ToList();
                        if (updatelist.Count > 0)
                        {
                            foreach (var d in updatelist)
                            {
                                _PortalContext.Remove(d);
                            }
                            foreach (var d in data.filelistMedical)
                            {
                                HR_Employee_MedicalRecord_FileDMO objj = new HR_Employee_MedicalRecord_FileDMO();
                                objj.HREMR_Id = data.HREMR_Id;
                                objj.HREMRF_FileName = d.HREMRF_FileName;
                                objj.HREMRF_FilePath = d.HREMRF_FilePath;
                                objj.HREMRF_ActiveFlag = true;
                                objj.HREMRF_CreatedDate = DateTime.Now;
                                objj.HREMRF_UpdatedDate = DateTime.Now;
                                objj.HREMRF_CreatedBy = data.UserId;
                                objj.HREMRF_UpdatedBy = data.UserId;
                                _PortalContext.Add(objj);

                            }
                            int i = _PortalContext.SaveChanges();
                            if (i > 0)
                            {
                                data.returnval = "save";
                            }
                            else
                            {
                                data.returnval = "notsave";
                            }

                        }

                    }
                }
                else
                {
                    if (data.Medicallisttwo != null && data.Medicallisttwo.Length > 0)
                    {

                   
                    foreach (var d in data.Medicallisttwo)
                    {
                        var duplicate = _PortalContext.HR_Employee_MedicalRecordDMO.Where(R => R.HREMR_ActiveFlag == true && R.MI_Id == data.MI_Id && R.HRME_Id == d.HRME_Id && R.HREMR_TestDate == d.HREMR_TestDate).Distinct().ToList();
                        if (duplicate.Count > 0)
                        {
                            count = count + 1;
                        }

                        else
                        {
                            HR_Employee_MedicalRecordDMO obj = new HR_Employee_MedicalRecordDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.HRME_Id = d.HRME_Id;
                            obj.HREMR_TestDate = d.HREMR_TestDate;
                            obj.HREMR_TestName = d.HREMR_TestName;
                            obj.HREMR_Remarks = d.HREMR_Remarks;
                            obj.HREMR_ActiveFlag = true;
                            obj.HREMR_CreatedDate = DateTime.Now;
                            obj.HREMR_UpdatedDate = DateTime.Now;
                            obj.HREMR_CreatedBy = data.UserId;
                            obj.HREMR_UpdatedBy = data.UserId;
                            _PortalContext.Add(obj);
                            if (d.filelistMedical != null && d.filelistMedical.Length > 0)
                            {
                                foreach (var t in d.filelistMedical)
                                {
                                    HR_Employee_MedicalRecord_FileDMO objj = new HR_Employee_MedicalRecord_FileDMO();
                                    objj.HREMR_Id = obj.HREMR_Id;
                                    objj.HREMRF_FileName = t.HREMRF_FileName;
                                    objj.HREMRF_FilePath = t.HREMRF_FilePath;
                                    objj.HREMRF_ActiveFlag = true;
                                    objj.HREMRF_CreatedDate = DateTime.Now;
                                    objj.HREMRF_UpdatedDate = DateTime.Now;
                                    objj.HREMRF_CreatedBy = data.UserId;
                                    objj.HREMRF_UpdatedBy = data.UserId;
                                    _PortalContext.Add(objj);
                                }
                            }




                        }

                        int i = _PortalContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                        data.count = count;
                    }
                    }
                }


            }
            catch (Exception ex)
            {
                data.returnval = "admin";
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public Employee_MedicalRecordDTO deactivate(Employee_MedicalRecordDTO dto)
        {
            try
            {

                var result = _PortalContext.HR_Employee_MedicalRecordDMO.Single(t => t.MI_Id == dto.MI_Id && t.HREMR_Id == dto.HREMR_Id);
                if (result.HREMR_ActiveFlag == true)
                {
                    result.HREMR_ActiveFlag = false;
                }
                else
                {
                    result.HREMR_ActiveFlag = true;
                }
                //result.HREMR_UpdatedBy = dto.UserId;
                result.HREMR_UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);
                var flag = _PortalContext.SaveChanges();
                if (flag > 0)
                {
                    dto.returnVal = true;
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Employee_MedicalRecordDTO viewData(Employee_MedicalRecordDTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _PortalContext.HR_Employee_MedicalRecordDMO
                                       from b in _PortalContext.HR_Employee_MedicalRecord_FileDMO
                                       where a.HREMR_Id == b.HREMR_Id && a.HREMR_Id == dto.HREMR_Id
                                       select new Employee_MedicalRecordDTO
                                       {
                                           HREMRF_FileName = b.HREMRF_FileName,
                                           HREMRF_FilePath = b.HREMRF_FilePath,
                                           HREMR_Id = a.HREMR_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public Employee_MedicalRecordDTO onclick_employee(Employee_MedicalRecordDTO dto)
        {
            try
             {
                dto.attachementlist = (from a in _PortalContext.HR_Employee_MedicalRecordDMO
                                       from b in _PortalContext.HR_Employee_MedicalRecord_FileDMO
                                       from c in _PortalContext.HR_Master_Employee_DMO
                                       from d in _PortalContext.IVRM_Staff_User_Login
                                       where d.Id == dto.UserId &&  d.Emp_Code == c.HRME_Id && a.HRME_Id == c.HRME_Id && a.HREMR_Id==b.HREMR_Id
                                       select new Employee_MedicalRecordDTO
                                       {
                                           HREMRF_FileName = b.HREMRF_FileName,
                                           HREMRF_FilePath = b.HREMRF_FilePath,
                                           HREMR_Id = a.HREMR_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
