using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;
using System.IO;
using CommonLibrary;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Globalization;

namespace WebApplication1.Services
{
    public class enquiryreportImpl : Interfaces.enquiryreportInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
         new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly enquiryreportContext _enquiryreportContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;


        string Is_middlename_null;
        string Is_lastname_null;
        string Is_address2_null;
        string Is_address3_null;
        string Is_enquiry_details_null;
        string Is_enquiry_number_null;


        public enquiryreportImpl(enquiryreportContext enquiryreportContext, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext db)
        {
            _enquiryreportContext = enquiryreportContext;
            _UserManager = UserManager;
            _db = db;
        }


        public WrittenTestMarksBindDataDTO getenqyearlist(WrittenTestMarksBindDataDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.fillyear = year.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<WrittenTestMarksBindDataDTO> searchenquiry(WrittenTestMarksBindDataDTO acd)
        {
            List<Enq> result = new List<Enq>();
            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "EnquiryReport_modified";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.VarChar)
                {
                    Value = acd.ASMAY_Id
                });

                cmd.Parameters.Add(new SqlParameter("@RoleId",
                    SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt32(id.Id)
                });
                cmd.Parameters.Add(new SqlParameter("@RoleTypeId",
                   SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(id2.IVRMRT_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@From_Date",
               SqlDbType.VarChar)
                {
                    Value = acd.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@To_Date",
               SqlDbType.VarChar)
                {
                    Value = acd.To_Date
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //var data = cmd.ExecuteNonQuery();

                try
                {
                    // var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            if (dataReader["PASE_MiddleName"] != System.DBNull.Value)
                            {
                                Is_middlename_null = dataReader["PASE_MiddleName"].ToString();

                            }
                            else
                            {
                                Is_middlename_null = "Not Available";
                            }


                            if (dataReader["PASE_LastName"] != System.DBNull.Value)
                            {
                                Is_lastname_null = dataReader["PASE_LastName"].ToString();

                            }
                            else
                            {
                                Is_lastname_null = "Not Available";
                            }



                            if (dataReader["PASE_Address2"] != System.DBNull.Value)
                            {
                                Is_address2_null = dataReader["PASE_Address2"].ToString();

                            }
                            else
                            {
                                Is_address2_null = "Not Available";
                            }


                            if (dataReader["PASE_Address3"] != System.DBNull.Value)
                            {
                                Is_address3_null = dataReader["PASE_Address3"].ToString();

                            }
                            else
                            {
                                Is_address3_null = "Not Available";
                            }

                            if (dataReader["PASE_EnquiryDetails"] != System.DBNull.Value)
                            {
                                Is_enquiry_details_null = dataReader["PASE_EnquiryDetails"].ToString();

                            }
                            else
                            {
                                Is_enquiry_details_null = "Not Available";
                            }

                            if (dataReader["PASE_EnquiryNo"] != System.DBNull.Value)
                            {
                                Is_enquiry_number_null = dataReader["PASE_EnquiryNo"].ToString();

                            }
                            else
                            {
                                Is_enquiry_number_null = "Not Available";
                            }

                            result.Add(new Enq
                            {
                                UserName = dataReader["UserName"].ToString(),
                                PASE_emailid = dataReader["PASE_emailid"].ToString(),
                                PASE_MobileNo = Convert.ToInt64(dataReader["PASE_MobileNo"]),
                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                PASE_Date = Convert.ToDateTime(dataReader["PASE_Date"]),
                                PASE_EnquiryNo = Is_enquiry_number_null,
                                PASE_FirstName = dataReader["PASE_FirstName"].ToString(),
                                PASE_MiddleName = Is_middlename_null,
                                PASE_LastName = Is_lastname_null,
                                PASE_Address1 = dataReader["PASE_Address1"].ToString(),
                                PASE_Address2 = Is_address2_null,
                                PASE_Address3 = Is_address3_null,
                                PASE_EnquiryDetails = Is_enquiry_details_null,
                            });
                            acd.SearchstudentDetails = result.ToArray();
                        }
                        switch (acd.searchType)
                        {

                            //case "all":
                            //    acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).ToArray();

                            //    if (acd.SearchstudentDetails.Length > 0)
                            //    {
                            //        acd.count = acd.SearchstudentDetails.Length;
                            //    }
                            //    else
                            //    {
                            //        acd.count = 0;
                            //    }
                            //    break;


                            case "all":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).ToArray();
                                //var lookup = result.ToLookup(w=>w.searchString);
                                //acd.SearchstudentDetails = lookup.OrderByDescending(c=>c.Where)
                              

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;




                            case "first_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_FirstName.Contains(acd.searchString)).ToArray();
                              
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "middle_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_MiddleName == acd.searchString).ToArray();
                                
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "last_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_LastName == acd.searchString).ToArray();
                               
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "email_id":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_emailid == acd.searchString).ToArray();

                               
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "mobile_no":
                              
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w =>Convert.ToString (w.PASE_MobileNo) == acd.searchString).ToArray();
                                
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "enquiry_no":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_EnquiryNo == acd.searchString).ToArray();
                               
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "address1":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_Address1 == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "address2":
                                 acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_Address2 == acd.searchString).ToArray();
                               
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "address3":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASE_Address3 == acd.searchString).ToArray();
                                
                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "class":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.ASMCL_ClassName == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;



                            case "enquiry_date":
                                DateTime date = DateTime.ParseExact(acd.searchString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w =>w.PASE_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return acd;
            }
        }
    }
}

            