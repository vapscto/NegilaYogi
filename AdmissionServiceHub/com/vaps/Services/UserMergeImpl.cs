using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class UserMergeImpl : Interfaces.UserMergeInterface
    {
        public AdmissionFormContext _AdmissionFormContext;
        public UserManager<ApplicationUser> _UserManager;
        public DomainModelMsSqlServerContext _db;
        public ILogger<UserMergeImpl> _log;

        public UserMergeImpl(AdmissionFormContext _Admission, UserManager<ApplicationUser> _User, DomainModelMsSqlServerContext _dbd, ILogger<UserMergeImpl> _logger)
        {
            _AdmissionFormContext = _Admission;
            _UserManager = _User;
            _db = _dbd;
            _log = _logger;
        }
        public UserMergeDTO getalldetails(UserMergeDTO data)
        {
            try
            {
                data.getstudentdetails = (from a in _AdmissionFormContext.StudentSiblingDMO
                                          from b in _AdmissionFormContext.Adm_M_Student
                                          from c in _AdmissionFormContext.SchoolYearWiseStudent
                                          where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && c.ASMAY_Id == data.ASMAY_Id
                                          && a.AMSTS_SiblingsOrder == 1)
                                          select new UserMergeDTO
                                          {
                                              studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                            (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                            (b.AMST_LastName == null || b.AMST_LastName == "" ? "" : " " + b.AMST_LastName) +
                                            (b.AMST_AdmNo == null || b.AMST_AdmNo == "" ? "" : " : " + b.AMST_AdmNo)),
                                              AMST_Id = b.AMST_Id
                                          }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }
        public UserMergeDTO onstudentnamechange(UserMergeDTO data)
        {
            try
            {
                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_Siblings_Employee_Student_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.NVarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.NVarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_HRME_Id", SqlDbType.NVarChar)
                    {
                        Value = data.AMST_HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.NVarChar)
                    {
                        Value = "sibling"
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
                        data.getuserdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.InnerException);
                    }                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }
        public UserMergeDTO mergeuserdetails(UserMergeDTO data)
        {
            try
            {

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
