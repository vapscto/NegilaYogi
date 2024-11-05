using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentIdCardFormatImpl : Interfaces.StudentIdCardFormatInterface
    {
        public AdmissionFormContext _context;
        public StudentIdCardFormatImpl(AdmissionFormContext _cont)
        {
            _context = _cont;
        }
        public StudentIdCardFormatDTO OnLoadStudentIdCardDetails(StudentIdCardFormatDTO data)
        {
            try
            {
                data.GetAcademicYearList = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetClassList = (from a in _context.Masterclasscategory
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentIdCardFormatDTO OnChangeYear(StudentIdCardFormatDTO data)
        {
            try
            {
                data.GetClassList = (from a in _context.Masterclasscategory
                                     from b in _context.School_M_Class
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentIdCardFormatDTO OnChangeClass(StudentIdCardFormatDTO data)
        {
            try
            {
                data.GetSectionList = (from a in _context.Masterclasscategory
                                       from b in _context.School_M_Class
                                       from c in _context.AdmSection
                                       from d in _context.AdmSchoolMasterClassCatSec
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == d.ASMCC_Id && c.ASMS_Id == d.ASMS_Id && d.ASMCCS_ActiveFlg == true
                                       && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.Is_Active == true)
                                       select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentIdCardFormatDTO OnChangeSection(StudentIdCardFormatDTO data)
        {
            try
            {
                data.GetStudentList = (from a in _context.Adm_M_Student
                                       from b in _context.SchoolYearWiseStudent
                                       where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                       && b.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && b.AMAY_ActiveFlag == 1)
                                       select new StudentIdCardFormatDTO
                                       {
                                           AMST_Id = b.AMST_Id,
                                           StudentName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                           (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                           (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_FirstName) +
                                           (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo))
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentIdCardFormatDTO GetReportDetails(StudentIdCardFormatDTO data)
        {
            try
            {
                string AMST_Id = "0";
                List<long> amstid = new List<long>();
                if (data.StudentTempList.Length > 0)
                {
                    foreach (var c in data.StudentTempList)
                    {
                        AMST_Id += "," + c.AMST_Id;
                        amstid.Add(c.AMST_Id);
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_StudentIdCard_FormatDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.VarChar){Value = data.MI_Id});
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.VarChar){Value = data.ASMAY_Id});
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.VarChar){Value = data.ASMCL_Id});
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.VarChar){Value = data.ASMS_Id});
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",SqlDbType.VarChar){Value = AMST_Id });


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
                            data.cardData = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //addedd by adarsh
                    string html = "";
                    string accountname = "";
                    string accesskey = "";
                    ReadTemplateFromAzure objj = new ReadTemplateFromAzure();
                    var datatstu = _context.IVRM_Storage_path_Details.ToList();
                    if (datatstu.Count() > 0)
                    {
                        accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                        html = objj.getHtmlContentFromAzure(accountname, accesskey, "files/" + data.MI_Id, "studentidcarddesign.html", 0);
                        if (html != "")
                        {
                           data.retrunMsg1 = "datafound";
                           data.retrunMsg = html;
                        }
                        else
                        {
                          data.retrunMsg = "nodata";
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
    }
}
