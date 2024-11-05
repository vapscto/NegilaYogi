using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using System.Net;

namespace LibraryServiceHub.com.vaps.Services
{
    public class BookTransactionImpl : Interfaces.BookTransactionInterface
    {
        public LibraryContext _LibraryContext;

        DomainModelMsSqlServerContext _db;
        public BookTransactionImpl(LibraryContext para, DomainModelMsSqlServerContext db)
        {
            _LibraryContext = para;
            _db = db;
        }


        public BookTransactionDTO getdetails_smartcard(BookTransactionDTO data)
        {
            try
            {
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();

             

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        //getdetailsReturn
        public BookTransactionDTO getdetailsReturn(BookTransactionDTO data)
        {
            try
            {
                List<long> clsids = new List<long>();
                string s = "";


                List<LIB_Master_Library_DMO> liblist = new List<LIB_Master_Library_DMO>();


                if (data.LMAL_Id == 0)
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                               select a).ToList();
                }
                else
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == data.LMAL_Id
                               select a).ToList();
                }

                data.msterliblist = liblist.ToArray();
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();


                if (liblist.Count > 0)
                {
                    data.LMAL_Id = liblist.FirstOrDefault().LMAL_Id;


                    var typeofbook = "";
                    if (data.booktype == "issue")
                    {
                        typeofbook = "Issue";
                    }
                    else if (data.booktype == "ref")
                    {
                        typeofbook = "Reference";
                    }
                    else if (data.booktype == "nonbook")
                    {
                        typeofbook = "nonbook";
                    }

                    if (data.issuertype == "std")
                    {
                        var libclasslist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                            from b in _LibraryContext.LIB_User_Library_DMO
                                            from c in _LibraryContext.LIB_Library_Class_DMO
                                            where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == c.LMAL_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && c.LLC_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == liblist.FirstOrDefault().LMAL_Id
                                            select c).ToList();






                        if (libclasslist.Count == 0)
                        {
                            data.clamsg = "NS";
                            return data;
                        }

                    }

                    if (data.issuertype == "std")
                    {
                        var list = _LibraryContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_ActiveFlag == 1 && t.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                        data.yearlist = list.ToArray();

                        var currYear = _LibraryContext.AcademicYearDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                        data.currentYear = currYear.ToArray();

                        var classlist = (from t in _LibraryContext.School_M_Class
                                         from c in _LibraryContext.LIB_Library_Class_DMO
                                         where (t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_Id == c.ASMCL_Id && c.LLC_ActiveFlg == true && c.LMAL_Id == data.LMAL_Id)
                                         select t).Distinct().OrderBy(e => e.ASMCL_Order).ToList();
                        data.classlist = classlist.ToArray();

                        var sectionlist = _LibraryContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(e => e.ASMC_Order).ToList();
                        data.sectionlist = sectionlist.ToArray();

                        if (data.ASMCL_Id == 0)
                        {
                            data.ASMCL_Id = classlist.FirstOrDefault().ASMCL_Id;
                        }
                        if (data.ASMS_Id == 0)
                        {
                            data.ASMS_Id = sectionlist.FirstOrDefault().ASMS_Id;
                        }


                        //var studentlist = (from m in _LibraryContext.Adm_M_Student
                        //                   from n in _LibraryContext.School_Adm_Y_StudentDMO
                        //                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1

                        //                   select new BookTransactionDTO
                        //                   {
                        //                       AMST_Id = m.AMST_Id,
                        //                       MI_Id = m.MI_Id,
                        //                       ASMAY_Id = m.ASMAY_Id,
                        //                       AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                        //                       AMST_MiddleName = m.AMST_MiddleName,
                        //                       AMST_LastName = m.AMST_LastName,
                        //                       AMST_AdmNo = m.AMST_AdmNo

                        //                   }).Distinct().ToList();

                        using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_STTHOMOS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 8000000;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Type",
                          SqlDbType.VarChar)
                            {
                                Value = "Std"
                            });
                            cmd.Parameters.Add(new SqlParameter("@StaffI_d",
                          SqlDbType.BigInt)
                            {
                                Value = 0
                            });
                            cmd.Parameters.Add(new SqlParameter("@Typetwo",
                          SqlDbType.VarChar)
                            {
                                Value = 0
                            });
                            cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.LMAL_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                          SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            //@Typetwo
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
                                data.studentlist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.studentlist != null && data.studentlist.Length > 0)
                        {

                            data.studentCount = data.studentlist.Length;
                        }

                        using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_SCHOOL";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 8000000;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Type",
                          SqlDbType.VarChar)
                            {
                                Value = typeofbook
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                          SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                         SqlDbType.VarChar)
                            {
                                Value = data.LMAL_Id
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
                                data.getalldata = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }
                    else if (data.issuertype == "stf")
                    {
                        data.filldepartment = _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                        data.filldesignation = _LibraryContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                        data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                           select new Staff_BookTranasctionDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               MI_Id = a.MI_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                           }).Distinct().OrderBy(t => t.HRME_Id).ToArray();




                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }

                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }

                    }
                    else if (data.issuertype == "dep")
                    {
                        data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                               select new BookTransactionDTO
                                               {
                                                   HRMD_Id = a.HRMD_Id,
                                                   HRMD_DepartmentName = a.HRMD_DepartmentName,
                                                   HRMD_Order = a.HRMD_Order,

                                               }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       // LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,


                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }






                    }


                    else if (data.issuertype == "gst")
                    {


                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }

                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }

                    }




                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_GET_BOOKS_FOR_TRANSACTION_Stthomos";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                        {
                            Value = typeofbook
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                     SqlDbType.VarChar)
                        {
                            Value = data.LMAL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type2",
                     SqlDbType.VarChar)
                        {
                            Value = "LOAD"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Searctext",
                  SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
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
                            data.booktitle = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //adedd
                    if (data.maxmsg=="onload")
                    {
                        
                        var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);

                    }
                }
                else
                {
                    data.msg = "N";
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //getdetails

        public BookTransactionDTO getdetails(BookTransactionDTO data)
        {
            try
            {
                List<long> clsids = new List<long>();
                string s = "";


                List<LIB_Master_Library_DMO> liblist = new List<LIB_Master_Library_DMO>();


                if (data.LMAL_Id == 0)
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                               select a).ToList();
                }
                else
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == data.LMAL_Id
                               select a).ToList();
                }

                data.msterliblist = liblist.ToArray();
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();


                if (liblist.Count > 0)
                {
                    data.LMAL_Id = liblist.FirstOrDefault().LMAL_Id;


                    var typeofbook = "";
                    if (data.booktype == "issue")
                    {
                        typeofbook = "Issue";
                    }
                    else if (data.booktype == "ref")
                    {
                        typeofbook = "Reference";
                    }
                    else if (data.booktype == "nonbook")
                    {
                        typeofbook = "nonbook";
                    }

                    if (data.issuertype == "std")
                    {
                        var libclasslist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                            from b in _LibraryContext.LIB_User_Library_DMO
                                            from c in _LibraryContext.LIB_Library_Class_DMO
                                            where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == c.LMAL_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && c.LLC_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == liblist.FirstOrDefault().LMAL_Id
                                            select c).ToList();



                        //if (libclasslist.Count>0)
                        //{
                        //    foreach (var item in libclasslist)
                        //    {
                        //        clsids.Add(item.ASMCL_Id);
                        //    }
                        //}


                        if (libclasslist.Count == 0)
                        {
                            data.clamsg = "NS";
                            return data;
                        }

                    }

                    if (data.issuertype == "std")
                    {
                        var list = _LibraryContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_ActiveFlag == 1 && t.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                        data.yearlist = list.ToArray();

                        var currYear = _LibraryContext.AcademicYearDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                        data.currentYear = currYear.ToArray();

                        var classlist = (from t in _LibraryContext.School_M_Class
                                         from c in _LibraryContext.LIB_Library_Class_DMO
                                         where (t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true && t.ASMCL_Id == c.ASMCL_Id && c.LLC_ActiveFlg == true && c.LMAL_Id == data.LMAL_Id)
                                         select t).Distinct().OrderBy(e => e.ASMCL_Order).ToList();
                        data.classlist = classlist.ToArray();

                        var sectionlist = _LibraryContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(e => e.ASMC_Order).ToList();
                        data.sectionlist = sectionlist.ToArray();

                        if (data.ASMCL_Id == 0)
                        {
                            data.ASMCL_Id = classlist.FirstOrDefault().ASMCL_Id;
                        }
                        if (data.ASMS_Id == 0)
                        {
                            data.ASMS_Id = sectionlist.FirstOrDefault().ASMS_Id;
                        }

                        var studentlist = (from m in _LibraryContext.Adm_M_Student
                                           from n in _LibraryContext.School_Adm_Y_StudentDMO
                                           where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id
                                           select new BookTransactionDTO
                                           {
                                               AMST_Id = m.AMST_Id,
                                               MI_Id = m.MI_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                               AMST_MiddleName = m.AMST_MiddleName,
                                               AMST_LastName = m.AMST_LastName,
                                               AMST_AdmNo = m.AMST_AdmNo

                                           }).Distinct().ToList();

                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                            data.studentCount = studentlist.Count;
                        }

                        using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_SCHOOL";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 8000000;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Type",
                          SqlDbType.VarChar)
                            {
                                Value = typeofbook
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                          SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                         SqlDbType.VarChar)
                            {
                                Value = data.LMAL_Id
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
                                data.getalldata = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }
                    else if (data.issuertype == "stf")
                    {
                        data.filldepartment = _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                        data.filldesignation = _LibraryContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                        data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                           select new Staff_BookTranasctionDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               MI_Id = a.MI_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                           }).Distinct().OrderBy(t => t.HRME_Id).ToArray();




                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }

                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }

                    }
                    else if (data.issuertype == "dep")
                    {
                        data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                               select new BookTransactionDTO
                                               {
                                                   HRMD_Id = a.HRMD_Id,
                                                   HRMD_DepartmentName = a.HRMD_DepartmentName,
                                                   HRMD_Order = a.HRMD_Order,

                                               }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       // LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,


                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.HR_Master_Department
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRMD_Id = d.HRMD_Id,
                                                       HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }






                    }


                    else if (data.issuertype == "gst")
                    {


                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }

                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterCategoryDMO
                                                   from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                   where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,

                                                       LBTR_GuestName = a.LBTR_GuestName,

                                                       LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                       LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }

                    }




                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_GET_BOOKS_FOR_TRANSACTION";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                      SqlDbType.VarChar)
                        {
                            Value = typeofbook
                        });

                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                     SqlDbType.VarChar)
                        {
                            Value = data.LMAL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type2",
                     SqlDbType.VarChar)
                        {
                            Value = "LOAD"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Searctext",
                  SqlDbType.VarChar)
                        {
                            Value = ""
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
                            data.booktitle = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (data.maxmsg == "onload")
                    {
                       
                        var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);

                    }
                }
                else
                {
                    data.msg = "N";
                }
                //adedd New  Things
              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //GettransctionDetails
        public BookTransactionDTO GettransctionDetails(BookTransactionDTO data)
        {
            try
            {
                
                List<BookTransactionDTO> result = new List<BookTransactionDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "LIB_RFCARD_Tranasction_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCTST_IP", SqlDbType.VarChar) { Value = data.AMCTST_IP });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new BookTransactionDTO
                                {
                                    IVRMUL_Id = Convert.ToInt32(dataReader["Login_Id"].ToString()),

                                });

                                result = result.ToList();

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (result != null && result.Count > 0)
                {
                    data.UserSsdo_Id = result[0].IVRMUL_Id;
                }
                //data.UserSsdo_Id
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_STTHOMOS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = data.searctype
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffI_d",
                  SqlDbType.BigInt)
                    {
                        Value = data.UserSsdo_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Typetwo",
                         SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.booktitle = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public BookTransactionDTO GetStudentDetails1(BookTransactionDTO data)
        {
            try
            {
                var studentlist = (from m in _LibraryContext.Adm_M_Student
                                   from n in _LibraryContext.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id
                                   select new BookTransactionDTO
                                   {
                                       AMST_Id = m.AMST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       AMST_AdmNo = m.AMST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                    data.studentCount = studentlist.Count;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        //GettransctionDetails
        public BookTransactionDTO Savedata(BookTransactionDTO data)
        {
            try
            {
                var mxcnt = 0;
                var typeofbook = "";
                if (data.booktype == "issue")
                {
                    typeofbook = "Issue";
                }
                else if (data.booktype == "ref")
                {
                    typeofbook = "Reference";
                }
                else if (data.booktype == "nonbook")
                {
                    typeofbook = "nonbook";
                }


                if (data.issuertype == "std")
                {
                    if (data.bookcat_type == "book")
                    {
                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRS_ActiveFlg == true && b.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRS_ActiveFlg == true && b.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && c.LMC_Id == cat_id
                                       && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }



                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.BookTransactionDMO
                                             from b in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                             where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRS_ActiveFlg == true && a.LMBANO_Id == data.LMBANO_Id && b.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue"
                                             select new BookTransactionDTO
                                             {
                                                 LMBANO_Id = a.LMBANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                BookTransactionDMO obj = new BookTransactionDMO();

                                obj.MI_Id = data.MI_Id;
                                //obj.ASMAY_Id = data.ASMAY_Id;
                                // obj.AMST_Id = data.AMST_Id;
                                obj.LMBANO_Id = data.LMBANO_Id;
                                obj.LBTR_IssuedDate = data.Issue_Date;
                                obj.LBTR_DueDate = data.Due_Date;
                                obj.LBTR_ReturnedDate = data.Return_Date;
                                obj.LBTR_FineCollected = 0;
                                obj.LBTR_FineWaived = 0;
                                obj.LBTR_Renewalcounter = 0;
                                obj.LBTR_Status = "Issue";
                                //obj.FODM_Id = 0;
                                //obj.Guest_Id = 0;
                                // obj.HRME_Id = 0;
                                //   obj.LMD_Id = 0;
                                obj.LBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj);
                                /// int rowAffected1 = _LibraryContext.SaveChanges();
                                LIB_Book_Transaction_StudentDMO obj1 = new LIB_Book_Transaction_StudentDMO();
                                obj1.LBTR_Id = obj.LBTR_Id;
                                obj1.AMST_Id = data.AMST_Id;
                                obj1.LBTRS_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);

                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    long mobileno = 0;
                                    var mobilenolist = (from a in _db.Adm_M_Student
                                                        from b in _db.Adm_M_Student_MobileNo
                                                        where a.AMST_Id == b.AMST_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id
                                                        select b).ToList();

                                    if (mobilenolist.Count > 0)
                                    {
                                        if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                                        {
                                            mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                                        }

                                    }
                                    else
                                    {
                                        var mobilenolist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).ToList();

                                        if (mobilenolist1.Count > 0)
                                        {
                                            if (mobilenolist1[0].AMST_MobileNo != 0)
                                            {
                                                mobileno = mobilenolist1[0].AMST_MobileNo;
                                            }

                                        }
                                    }
                                    //  mobileno = 9591081840;
                                    if (mobileno != 0)
                                    {
                                        SMS sms = new SMS(_db);
                                        string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKISSUE", data.ASMAY_Id, data.AMST_Id, data.LMBANO_Id).Result;
                                    }

                                    string email_id = "";
                                    var emailidlist = (from a in _db.Adm_M_Student
                                                       from b in _db.Adm_M_Student_Email_Id
                                                       where a.AMST_Id == b.AMST_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id
                                                       select b).ToList();

                                    if (emailidlist.Count > 0)
                                    {
                                        if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                                        {
                                            email_id = emailidlist[0].AMSTE_EmailId;
                                        }

                                    }
                                    else
                                    {
                                        var emailidlist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id).ToList();

                                        if (emailidlist1.Count > 0)
                                        {
                                            if (emailidlist1[0].AMST_emailId != "")
                                            {
                                                email_id = emailidlist1[0].AMST_emailId;
                                            }

                                        }
                                    }
                                    // email_id = "praveenishwar@vapstech.com";
                                    if (email_id != "" && email_id != null)
                                    {
                                        Email email = new Email(_db);
                                        string s = email.sendmail_library(data.MI_Id, email_id, "BOOKISSUE", data.ASMAY_Id, data.AMST_Id, data.LMBANO_Id);
                                    }

                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        data.maxmsg = "exceed";
                    }

                }
                else if (data.issuertype == "stf")
                {

                    if (data.bookcat_type == "book")
                    {
                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRST_ActiveFlg == true && b.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRST_ActiveFlg == true && b.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && c.LMC_Id == cat_id
                                       && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }



                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.BookTransactionDMO
                                             from b in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                             where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRST_ActiveFlg == true && a.LMBANO_Id == data.LMBANO_Id && b.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue"
                                             select new BookTransactionDTO
                                             {
                                                 LMBANO_Id = a.LMBANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                BookTransactionDMO obj = new BookTransactionDMO();

                                obj.MI_Id = data.MI_Id;
                                //obj.ASMAY_Id = data.ASMAY_Id;
                                // obj.AMST_Id = data.AMST_Id;
                                obj.LMBANO_Id = data.LMBANO_Id;
                                obj.LBTR_IssuedDate = data.Issue_Date;
                                obj.LBTR_DueDate = data.Due_Date;
                                obj.LBTR_ReturnedDate = data.Return_Date;
                                obj.LBTR_FineCollected = 0;
                                obj.LBTR_FineWaived = 0;
                                obj.LBTR_Renewalcounter = 0;
                                obj.LBTR_Status = "Issue";
                                //obj.FODM_Id = 0;
                                //obj.Guest_Id = 0;
                                // obj.HRME_Id = 0;
                                //   obj.LMD_Id = 0;
                                obj.LBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj);
                                /// int rowAffected1 = _LibraryContext.SaveChanges();
                                LIB_Book_Transaction_StaffDMO obj1 = new LIB_Book_Transaction_StaffDMO();
                                obj1.LBTR_Id = obj.LBTR_Id;
                                obj1.HRME_Id = data.HRME_Id;
                                obj1.LBTRST_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);

                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    long mobileno = 0;
                                    string emailid = "";
                                    var mobilenolist = _LibraryContext.Emp_MobileNo.Where(e => e.HRMEMNO_DeFaultFlag == "default" && e.HRME_Id == data.HRME_Id).ToList();
                                    if (mobilenolist.Count > 0)
                                    {
                                        if (mobilenolist[0].HRMEMNO_MobileNo != 0 && mobilenolist[0].HRMEMNO_MobileNo != null)
                                        {
                                            mobileno = Convert.ToInt64(mobilenolist[0].HRMEMNO_MobileNo);
                                            //  mobileno = 9591081840;
                                            if (mobileno != 0)
                                            {
                                                SMS sms = new SMS(_db);

                                                string s = sms.sendSms_library(data.MI_Id, mobileno, "STAFFBOOKISSUE", data.ASMAY_Id, data.HRME_Id, data.LMBANO_Id).Result;
                                            }

                                        }


                                    }
                                    var mobilenolist12 = _LibraryContext.Emp_Email_Id.Where(e => e.HRMEM_DeFaultFlag == "default" && e.HRME_Id == data.HRME_Id).ToList();
                                    if (mobilenolist12.Count > 0)
                                    {
                                        if (mobilenolist12[0].HRMEM_EmailId != "" && mobilenolist12[0].HRMEM_EmailId != null)
                                        {
                                            emailid = mobilenolist12[0].HRMEM_EmailId;
                                            //emailid = "praveenishwar@vapstech.com";
                                            if (emailid != "" && emailid != null)
                                            {
                                                Email email = new Email(_db);
                                                string s = email.sendmail_library(data.MI_Id, emailid, "STAFFBOOKISSUE", data.ASMAY_Id, data.HRME_Id, data.LMBANO_Id);
                                            }
                                        }
                                    }




                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        data.maxmsg = "exceed";
                    }

                }
                else if (data.issuertype == "dep")
                {

                    if (data.bookcat_type == "book")
                    {
                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRD_ActiveFlg == true && b.HRMD_Id == data.HRMD_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from b in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRD_ActiveFlg == true && b.HRMD_Id == data.HRMD_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && c.LMC_Id == cat_id && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }

                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }
                        else
                        {
                            var Duplicate = (from a in _LibraryContext.BookTransactionDMO
                                             from b in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                             where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRD_ActiveFlg == true && a.LMBANO_Id == data.LMBANO_Id && b.HRMD_Id == data.HRMD_Id && a.LBTR_Status == "Issue"
                                             select new BookTransactionDTO
                                             {
                                                 LMBANO_Id = a.LMBANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                BookTransactionDMO obj = new BookTransactionDMO();

                                obj.MI_Id = data.MI_Id;
                                //obj.ASMAY_Id = data.ASMAY_Id;
                                // obj.AMST_Id = data.AMST_Id;
                                obj.LMBANO_Id = data.LMBANO_Id;
                                obj.LBTR_IssuedDate = data.Issue_Date;
                                obj.LBTR_DueDate = data.Due_Date;
                                obj.LBTR_ReturnedDate = data.Return_Date;
                                obj.LBTR_FineCollected = 0;
                                obj.LBTR_FineWaived = 0;
                                obj.LBTR_Renewalcounter = 0;
                                obj.LBTR_Status = "Issue";
                                //obj.FODM_Id = 0;
                                //obj.Guest_Id = 0;
                                // obj.HRME_Id = 0;
                                //   obj.LMD_Id = 0;
                                obj.LBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj);
                                /// int rowAffected1 = _LibraryContext.SaveChanges();
                                LIB_Book_Transaction_DepartmentDMO obj1 = new LIB_Book_Transaction_DepartmentDMO();
                                obj1.LBTR_Id = obj.LBTR_Id;
                                obj1.HRMD_Id = data.HRMD_Id;
                                obj1.LBTRD_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);

                                int rowAffected = _LibraryContext.SaveChanges();

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
                    else
                    {
                        data.maxmsg = "exceed";
                    }


                }

                else if (data.issuertype == "gst")
                {


                    if (data.bookcat_type == "book")
                    {
                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LBTR_GuestName == data.LBTR_GuestName && a.LBTR_GuestContactNo == data.LBTR_GuestContactNo && a.LBTR_GuestEmailId == data.LBTR_GuestEmailId && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.BookTransactionDMO
                                       from c in _LibraryContext.BookRegisterDMO
                                       from d in _LibraryContext.Lib_M_Book_Accn_DMO
                                       where a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && c.LMC_Id == cat_id && a.LBTR_GuestName == data.LBTR_GuestName && a.LBTR_GuestContactNo == data.LBTR_GuestContactNo && a.LBTR_GuestEmailId == data.LBTR_GuestEmailId && a.LMBANO_Id == d.LMBANO_Id
                                       select new BookTransactionDTO
                                       {
                                           LMBANO_Id = a.LMBANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                    }

                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.BookTransactionDMO
                                             where a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && a.LMBANO_Id == data.LMBANO_Id && a.LBTR_Status == "Issue" && a.LBTR_GuestName == data.LBTR_GuestName && a.LBTR_GuestContactNo == data.LBTR_GuestContactNo
                                             select new BookTransactionDTO
                                             {
                                                 LMBANO_Id = a.LMBANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                BookTransactionDMO obj = new BookTransactionDMO();

                                obj.MI_Id = data.MI_Id;
                                //obj.ASMAY_Id = data.ASMAY_Id;
                                // obj.AMST_Id = data.AMST_Id;
                                obj.LMBANO_Id = data.LMBANO_Id;
                                obj.LBTR_IssuedDate = data.Issue_Date;
                                obj.LBTR_DueDate = data.Due_Date;
                                obj.LBTR_ReturnedDate = data.Return_Date;
                                obj.LBTR_FineCollected = 0;
                                obj.LBTR_FineWaived = 0;
                                obj.LBTR_Renewalcounter = 0;
                                obj.LBTR_Status = "Issue";
                                obj.LBTR_GuestContactNo = data.LBTR_GuestContactNo;
                                obj.LBTR_GuestName = data.LBTR_GuestName;
                                obj.LBTR_GuestEmailId = data.LBTR_GuestEmailId;
                                // obj.HRME_Id = 0;
                                //   obj.LMD_Id = 0;
                                obj.LBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj);

                                int rowAffected = _LibraryContext.SaveChanges();

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
                    else
                    {
                        data.maxmsg = "exceed";
                    }

                }

                
                var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO searchfilter(BookTransactionDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();

                var typeofbook = "";
                if (data.booktype == "issue")
                {
                    typeofbook = "Issue";
                }
                else if (data.booktype == "ref")
                {
                    typeofbook = "Reference";
                }
                else if (data.booktype == "nonbook")
                {
                    typeofbook = "nonbook";
                }
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_BOOKS_FOR_TRANSACTION";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = typeofbook
                    });
                    //  cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //SqlDbType.BigInt)
                    //  {
                    //      Value = data.ASMAY_Id
                    //  });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type2",
                  SqlDbType.VarChar)
                    {
                        Value = "SEARCH"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searctext",
                 SqlDbType.VarChar)
                    {
                        Value = data.searchfilter
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
                        data.booktitle = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public BookTransactionDTO searchfilteravail(BookTransactionDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();

                //var typeofbook = "";
                //if (data.booktype == "issue")
                //{
                //    typeofbook = "Issue";
                //}
                //else if (data.booktype == "ref")
                //{
                //    typeofbook = "Reference";
                //}
                //else if (data.booktype == "nonbook")
                //{
                //    typeofbook = "nonbook";
                //}
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_LOAD_BOOKS_FOR_AVAILABILITY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    //  cmd.Parameters.Add(new SqlParameter("@Type",
                    //SqlDbType.VarChar)
                    //  {
                    //      Value = typeofbook
                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //SqlDbType.BigInt)
                    //  {
                    //      Value = data.ASMAY_Id
                    //  });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type2",
                  SqlDbType.VarChar)
                    {
                        Value = "SEARCH"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searctext",
                 SqlDbType.VarChar)
                    {
                        Value = data.searchfilter
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
                        data.avilbooktitle = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public BookTransactionDTO searchfilterbarcode(BookTransactionDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();

                var typeofbook = "";
                if (data.booktype == "issue")
                {
                    typeofbook = "Issue";
                }
                else if (data.booktype == "ref")
                {
                    typeofbook = "Reference";
                }
                else if (data.booktype == "nonbook")
                {
                    typeofbook = "nonbook";
                }
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_BOOKS_FOR_TRANSACTION";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = typeofbook
                    });
                    //  cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //SqlDbType.BigInt)
                    //  {
                    //      Value = data.ASMAY_Id
                    //  });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type2",
                  SqlDbType.VarChar)
                    {
                        Value = "BARCODE"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searctext",
                 SqlDbType.VarChar)
                    {
                        Value = data.searchfilter
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
                        data.booktitle = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }



            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }


        public BookTransactionDTO searchfilterbarcode1(BookTransactionDTO data)

        {

            try
            {
                if (data.searchfilter == null)
                {
                    data.searchfilter = "";
                }

                List<long> clsids = new List<long>();
                List<LIB_Master_Library_DMO> liblist = new List<LIB_Master_Library_DMO>();
                if (data.LMAL_Id == 0)
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                               select a).ToList();
                }
                else
                {
                    liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == data.LMAL_Id
                               select a).ToList();
                }

                data.msterliblist = liblist.ToArray();
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();




                if (liblist.Count > 0)
                {
                    data.LMAL_Id = liblist.FirstOrDefault().LMAL_Id;


                    var typeofbook = "";
                    if (data.booktype == "issue")
                    {
                        typeofbook = "Issue";
                    }
                    else if (data.booktype == "ref")
                    {
                        typeofbook = "Reference";
                    }
                    else if (data.booktype == "nonbook")
                    {
                        typeofbook = "nonbook";
                    }

                    if (data.searchfilter == "")
                    {
                        if (data.issuertype == "std")
                        {
                            var libclasslist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                                from b in _LibraryContext.LIB_User_Library_DMO
                                                from c in _LibraryContext.LIB_Library_Class_DMO
                                                where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == c.LMAL_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && c.LLC_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == liblist.FirstOrDefault().LMAL_Id
                                                select c).ToList();



                            if (libclasslist.Count > 0)
                            {
                                foreach (var item in libclasslist)
                                {
                                    clsids.Add(item.ASMCL_Id);
                                }
                            }


                            if (libclasslist.Count == 0)
                            {
                                data.clamsg = "NS";
                                return data;
                            }

                        }

                        if (data.issuertype == "std")
                        {
                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_SCHOOL";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 8000000;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@Type",
                              SqlDbType.VarChar)
                                {
                                    Value = typeofbook
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                              SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = data.LMAL_Id
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
                                    data.getalldata = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else if (data.issuertype == "stf")
                        {
                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }

                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }




                        }
                        else if (data.issuertype == "dep")
                        {
                            data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                                   select new BookTransactionDTO
                                                   {
                                                       HRMD_Id = a.HRMD_Id,
                                                       HRMD_DepartmentName = a.HRMD_DepartmentName,
                                                       HRMD_Order = a.HRMD_Order,

                                                   }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           // LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,


                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }






                        }


                        else if (data.issuertype == "gst")
                        {
                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }

                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }

                        }
                    }
                    else
                    {
                        if (data.issuertype == "std")
                        {
                            var libclasslist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                                from b in _LibraryContext.LIB_User_Library_DMO
                                                from c in _LibraryContext.LIB_Library_Class_DMO
                                                where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == c.LMAL_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && c.LLC_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == liblist.FirstOrDefault().LMAL_Id
                                                select c).ToList();



                            if (libclasslist.Count > 0)
                            {
                                foreach (var item in libclasslist)
                                {
                                    clsids.Add(item.ASMCL_Id);
                                }
                            }


                            if (libclasslist.Count == 0)
                            {
                                data.clamsg = "NS";
                                return data;
                            }

                        }

                        if (data.issuertype == "std")
                        {
                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_SCHOOL_SEARCH";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 8000000;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@Type",
                              SqlDbType.VarChar)
                                {
                                    Value = typeofbook
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                              SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = data.LMAL_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@Searchtype",
                             SqlDbType.VarChar)
                                {
                                    Value = "ACN"
                                });
                                cmd.Parameters.Add(new SqlParameter("@Searchtext",
                             SqlDbType.VarChar)
                                {
                                    Value = data.searchfilter
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
                                    data.getalldata = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                        }
                        else if (data.issuertype == "stf")
                        {
                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.Trim().ToLower())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }

                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterEmployee
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRME_Id = d.HRME_Id,
                                                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                           // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                           //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                           HRMGT_Id = d.HRMGT_Id,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }




                        }
                        else if (data.issuertype == "dep")
                        {
                            data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                                   select new BookTransactionDTO
                                                   {
                                                       HRMD_Id = a.HRMD_Id,
                                                       HRMD_DepartmentName = a.HRMD_DepartmentName,
                                                       HRMD_Order = a.HRMD_Order,

                                                   }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           // LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,


                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.HR_Master_Department
                                                       from f in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,
                                                           HRMD_Id = d.HRMD_Id,
                                                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                           //LMD_DepartmentCode = d.LMD_DepartmentCode,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }






                        }


                        else if (data.issuertype == "gst")
                        {


                            if (data.bookcat_type == "book")
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }

                            }
                            else
                            {
                                if (data.booktype == "nonbook")
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                                else
                                {
                                    //get all related data for bind table grid in Html
                                    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                       from c in _LibraryContext.BookRegisterDMO
                                                       from d in _LibraryContext.MasterCategoryDMO
                                                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                                                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower().Trim() == data.searchfilter.ToLower().Trim())
                                                       select new BookTransactionDTO
                                                       {
                                                           Book_Trans_Id = a.LBTR_Id,
                                                           LMB_Id = c.LMB_Id,
                                                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                           MI_Id = a.MI_Id,
                                                           // ASMAY_Id = a.ASMAY_Id,
                                                           LMBANO_Id = b.LMBANO_Id,
                                                           // LMB_BookType = c.LMB_BookType,
                                                           LMB_BookTitle = c.LMB_BookTitle,
                                                           Issue_Date = a.LBTR_IssuedDate,
                                                           Due_Date = a.LBTR_DueDate,
                                                           Return_Date = a.LBTR_ReturnedDate,
                                                           Book_Trans_Status = a.LBTR_Status,
                                                           Fine_Amount = a.LBTR_TotalFine,
                                                           // Renewal_Counter = a.Renewal_Counter,
                                                           Waived_Amount = a.LBTR_FineWaived,

                                                           LBTR_GuestName = a.LBTR_GuestName,

                                                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                                                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                                                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                                }
                            }


                        }
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public BookTransactionDTO stdSearch_Grid(BookTransactionDTO data)
        {

            try
            {

                var typeofbook = "";
                // data.bookcat_type="Issue";
                string LBTR_Status = "";
                if (data.bookcat_type == "RETURN")
                {
                    LBTR_Status = "Return";
                }
                else
                {
                    LBTR_Status = "Issue";
                }
                if (data.booktype == "issue")
                {
                    typeofbook = "Issue";
                }
                else if (data.booktype == "ref")
                {
                    typeofbook = "Reference";
                }
                else if (data.booktype == "nonbook")
                {
                    typeofbook = "nonbook";
                }
                //ReturnStatus
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_SCHOOL_SEARCH_GRID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                  SqlDbType.VarChar)
                    {
                        Value = typeofbook
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searchtype",
                 SqlDbType.VarChar)
                    {
                        Value = data.searctype
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searchtext",
                 SqlDbType.VarChar)
                    {
                        Value = data.searchfilter
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Issue_Date",
                SqlDbType.Date)
                    {
                        Value = data.Issue_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@LBTR_Status",
               SqlDbType.VarChar)
                    {
                        Value = LBTR_Status
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
                        data.getalldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public BookTransactionDTO get_bookdetails(BookTransactionDTO data)
        {
            try
            {
                //get LMBANO_Id and related data and get Maxday's & max renewal day's, and how many item issue for that LMBANO_Id
                //data.bookdetails = (from a in _LibraryContext.BookRegisterDMO
                //                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                //                   // from c in _LibraryContext.CirculationParameterDMO
                //                    where (a.LMB_Id == b.LMB_Id  && b.LMB_Id == data.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && b.LMBANO_Id==data.LMBANO_Id)
                //                    select new BookTransactionDTO
                //                    {
                //                        LMB_Id = a.LMB_Id,
                //                        LMBANO_Id = b.LMBANO_Id,
                //                        LMB_BookTitle = a.LMB_BookTitle,
                //                        LMB_VolNo = a.LMB_VolNo,
                //                        LMB_EntryDate = a.LMB_EntryDate,
                //                        LMB_Price = a.LMB_Price,
                //                        LMB_ClassNo = a.LMB_ClassNo,
                //                        //  LMBA_Id = a.LMBA_Id,
                //                     //   LMD_Id = a.LMD_Id,
                //                        LMS_Id = a.LMS_Id,
                //                        //  LMP_Id = a.LMP_Id,
                //                       // LML_Id = a.LML_Id,
                //                        //  Donor_Id = a.Donor_Id,
                //                        //  LMV_Id = a.LMV_Id,
                //                      //  LMC_Id = a.LMC_Id,
                //                      //  Max_Issue_Days = c.Max_Issue_Days,
                //                        //Max_No_Renewals = c.Max_No_Renewals,
                //                       // Max_Issue_Items = c.Max_Issue_Items,
                //                    }
                //                  ).Distinct().ToArray();

                if (data.LMB_Id == 0 && data.LMBANO_Id > 0)
                {
                    var bookid = (from a in _LibraryContext.Lib_M_Book_Accn_DMO
                                  from b in _LibraryContext.BookRegisterDMO
                                  where a.LMB_Id == b.LMB_Id && a.LMBANO_ActiveFlg == true && a.LMBANO_Id == data.LMBANO_Id && b.LMB_ActiveFlg == true && b.MI_Id == data.MI_Id
                                  select new BookTransactionDTO
                                  {
                                      LMB_Id = b.LMB_Id
                                  }).ToList();
                    if (bookid.Count > 0)
                    {
                        data.LMB_Id = bookid[0].LMB_Id;
                    }

                }

                #region Book  DETAILS
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_BOOKDETAILS_TRANS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@LMB_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.LMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.LMBANO_Id
                    });

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
                        data.bookdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion



                if (data.issuertype == "gst")
                {

                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();


                        }
                        else
                        if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();


                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "GUEST" && b.LMC_Id == cat_id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    }


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO studentdetails(BookTransactionDTO data)
        {
            try
            {
               
                List<BookTransactionDTO> result = new List<BookTransactionDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "LIB_RFCARD_Tranasction_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCTST_IP", SqlDbType.VarChar) { Value = data.AMCTST_IP });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();  
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new BookTransactionDTO
                                {
                                    IVRMUL_Id = Convert.ToInt32(dataReader["Login_Id"].ToString()),

                                });

                                result = result.ToList();

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (result != null && result.Count > 0)
                {
                    if(data.issuertype == "std")
                    {
                        data.AMST_Id = result[0].IVRMUL_Id;
                    }
                    if (data.issuertype == "stf")
                    {
                        data.HRME_Id = result[0].IVRMUL_Id;
                    }
                }


                if (data.issuertype == "std")
                {
                    //In this query we et student details according to AMST_Id
                    var studentdata = (from a in _LibraryContext.Adm_M_Student
                                       from b in _LibraryContext.School_M_Class
                                       from c in _LibraryContext.School_Adm_Y_StudentDMO
                                       from d in _LibraryContext.school_M_Section
                                       where (a.AMST_Id == c.AMST_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.AMST_ActiveFlag == 1)
                                       select new BookTransactionDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           AMST_Photoname = a.AMST_Photoname,
                                           AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           ASMCL_ClassName = b.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName,
                                           AMAY_RollNo = c.AMAY_RollNo,
                                           ASMCL_Id = b.ASMCL_Id,
                                           ASYST_Id = c.ASYST_Id,
                                           ASMS_Id = d.ASMS_Id,
                                           AMST_FatherMobleNo = a.AMST_FatherMobleNo


                                       }).Distinct().ToArray();

                    data.studentdata = studentdata.ToArray();
                    var clasidlist = _LibraryContext.School_Adm_Y_StudentDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.AMST_Id && t.AMAY_ActiveFlag == 1).ToList();

                    long classid = 0;
                    if (clasidlist.Count > 0)
                    {
                        classid = clasidlist[0].ASMCL_Id;
                    }


                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAS_ActiveFlg == true && b.ASMCL_Id == classid && a.LBCPA_IssueRefFlg == "Reference" && a.LBCPA_Flg == "STUDENT")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAS_IssueDays,
                                                             Max_No_Renewals = b.LBCPAS_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAS_NoOfItems,

                                                         }).ToArray();


                        }

                        else
                    if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAS_ActiveFlg == true && b.ASMCL_Id == classid && a.LBCPA_IssueRefFlg == "Issue" && a.LBCPA_Flg == "STUDENT")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAS_IssueDays,
                                                             Max_No_Renewals = b.LBCPAS_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAS_NoOfItems,

                                                         }).ToArray();


                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAS_ActiveFlg == true && b.LMC_Id == cat_id && b.ASMCL_Id == classid && a.LNBCPA_Flg == "STUDENT")
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAS_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAS_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAS_NoOfItems,

                                                     }).ToArray();
                    }

                    if (data.app == "W")
                    {

                        var studentlist = (from m in _LibraryContext.Adm_M_Student
                                           from n in _LibraryContext.School_Adm_Y_StudentDMO
                                           where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classid && n.ASMS_Id == studentdata.FirstOrDefault().ASMS_Id
                                           select new BookTransactionDTO
                                           {
                                               AMST_Id = m.AMST_Id,
                                               MI_Id = m.MI_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                               AMST_MiddleName = m.AMST_MiddleName,
                                               AMST_LastName = m.AMST_LastName,
                                               AMST_AdmNo = m.AMST_AdmNo

                                           }).Distinct().ToList();

                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                            data.studentCount = studentlist.Count;
                        }

                        var typeofbook = "";
                        if (data.booktype == "issue")
                        {
                            typeofbook = "Issue";
                        }
                        else if (data.booktype == "ref")
                        {
                            typeofbook = "Reference";
                        }
                        else if (data.booktype == "nonbook")
                        {
                            typeofbook = "nonbook";
                        }
                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //  get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.Adm_M_Student
                                                   from f in _LibraryContext.School_Adm_Y_StudentDMO
                                                   from g in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                                                   //&& a.LBTR_Status=="Issue"
                                                   && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMST_Id == d.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Book" && e.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       AMST_Id = e.AMST_Id,
                                                       MI_Id = a.MI_Id,
                                                       ASMAY_Id = data.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       //  Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                                                       AMST_MiddleName = d.AMST_MiddleName,
                                                       AMST_LastName = d.AMST_LastName,
                                                       AMST_Photoname = d.AMST_Photoname,
                                                       ASMCL_Id = f.ASMCL_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //  get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.Adm_M_Student
                                                   from f in _LibraryContext.School_Adm_Y_StudentDMO
                                                   from g in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                                                   //&& a.LBTR_Status=="Issue"
                                                   && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMST_Id == d.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Book" && c.LMB_BookType == typeofbook && e.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       AMST_Id = e.AMST_Id,
                                                       MI_Id = a.MI_Id,
                                                       ASMAY_Id = data.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       //  Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                                                       AMST_MiddleName = d.AMST_MiddleName,
                                                       AMST_LastName = d.AMST_LastName,
                                                       AMST_Photoname = d.AMST_Photoname,
                                                       ASMCL_Id = f.ASMCL_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //  get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.Adm_M_Student
                                                   from f in _LibraryContext.School_Adm_Y_StudentDMO
                                                   from g in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                                                   //&& a.LBTR_Status=="Issue"
                                                   && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMST_Id == d.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Non-Book" && e.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       AMST_Id = e.AMST_Id,
                                                       MI_Id = a.MI_Id,
                                                       ASMAY_Id = data.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       //  Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                                                       AMST_MiddleName = d.AMST_MiddleName,
                                                       AMST_LastName = d.AMST_LastName,
                                                       AMST_Photoname = d.AMST_Photoname,
                                                       ASMCL_Id = f.ASMCL_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //  get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.Adm_M_Student
                                                   from f in _LibraryContext.School_Adm_Y_StudentDMO
                                                   from g in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                                                   //&& a.LBTR_Status=="Issue"
                                                   && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMST_Id == d.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Non-Book" && c.LMB_BookType == typeofbook && e.AMST_Id == data.AMST_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       AMST_Id = e.AMST_Id,
                                                       MI_Id = a.MI_Id,
                                                       ASMAY_Id = data.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       //  Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                                                       AMST_MiddleName = d.AMST_MiddleName,
                                                       AMST_LastName = d.AMST_LastName,
                                                       AMST_Photoname = d.AMST_Photoname,
                                                       ASMCL_Id = f.ASMCL_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo

                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }







                        }
                    }

                }
                else if (data.issuertype == "stf")
                {


                    //In this query we et Staff details according to HRME_Id
                    data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
                                           from b in _LibraryContext.HR_Master_Department
                                           from c in _LibraryContext.HR_Master_Designation
                                           where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                           select new Staff_BookTranasctionDTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               MI_Id = a.MI_Id,
                                               HRME_EmployeeCode = a.HRME_EmployeeCode,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                               HRME_MobileNo = a.HRME_MobileNo,
                                               HRMD_DepartmentName = b.HRMD_DepartmentName,
                                               HRMD_Id = b.HRMD_Id,
                                               HRMDES_DesignationName = c.HRMDES_DesignationName,
                                               HRMDES_Id = c.HRMDES_Id,
                                               HRME_Photo = a.HRME_Photo,
                                               HRME_EmailId = a.HRME_EmailId

                                           }).Distinct().ToArray();



                    var clasidlist = _LibraryContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.HRME_ActiveFlag == true).ToList();

                    long? classid = 0;
                    if (clasidlist.Count > 0)
                    {
                        classid = clasidlist[0].HRMGT_Id;
                    }





                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == classid && a.LBCPA_IssueRefFlg == "Issue" && a.LBCPA_Flg == "STAFF")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAST_IssueDays,
                                                             Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAST_NoOfItems,

                                                         }).ToArray();


                        }
                        else

                 if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == classid && a.LBCPA_IssueRefFlg == "Reference" && a.LBCPA_Flg == "STAFF")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAST_IssueDays,
                                                             Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAST_NoOfItems,

                                                         }).ToArray();


                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAST_ActiveFlg == true && b.LMC_Id == cat_id && b.HRMGT_Id == classid && a.LNBCPA_Flg == "STAFF")
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAST_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAST_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAST_NoOfItems,

                                                     }).ToArray();
                    }

                    if (data.app == "W")
                    {
                        var typeofbook = "";
                        if (data.booktype == "issue")
                        {
                            typeofbook = "Issue";
                        }
                        else if (data.booktype == "ref")
                        {
                            typeofbook = "Reference";
                        }
                        else if (data.booktype == "nonbook")
                        {
                            typeofbook = "nonbook";
                        }

                        if (data.bookcat_type == "book")
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }

                        }
                        else
                        {
                            if (data.booktype == "nonbook")
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO
                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                            else
                            {
                                //get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                                                   from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                                   from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                                   from c in _LibraryContext.BookRegisterDMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                                                   select new BookTransactionDTO
                                                   {
                                                       Book_Trans_Id = a.LBTR_Id,
                                                       LMB_Id = c.LMB_Id,
                                                       LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                                       MI_Id = a.MI_Id,
                                                       // ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMBANO_Id,
                                                       // LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMB_BookTitle,
                                                       Issue_Date = a.LBTR_IssuedDate,
                                                       Due_Date = a.LBTR_DueDate,
                                                       Return_Date = a.LBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LBTR_Status,
                                                       Fine_Amount = a.LBTR_TotalFine,
                                                       // Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       // HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            }
                        }
                    }


                }
                else if (data.issuertype == "dep")
                {


                    ////In this query we et Staff details according to HRME_Id
                    //data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
                    //                       from b in _LibraryContext.HR_Master_Department
                    //                       from c in _LibraryContext.HR_Master_Designation
                    //                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                    //                       select new Staff_BookTranasctionDTO
                    //                       {
                    //                           HRME_Id = a.HRME_Id,
                    //                           MI_Id = a.MI_Id,
                    //                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                    //                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //                           HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                    //                           HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                    //                           HRME_MobileNo = a.HRME_MobileNo,
                    //                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                    //                           HRMD_Id = b.HRMD_Id,
                    //                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                    //                           HRMDES_Id = c.HRMDES_Id,
                    //                           HRME_Photo = a.HRME_Photo,

                    //                       }).Distinct().ToArray();



                    //var clasidlist = _LibraryContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.HRME_ActiveFlag == true).ToList();

                    //long? classid = 0;
                    //if (clasidlist.Count > 0)
                    //{
                    //    classid = clasidlist[0].HRMGT_Id;
                    //}



                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();


                        }
                        else
         if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();


                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        var category_id = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_Id == data.LMB_Id && t.LMB_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "DEPARTMENT" && b.LMC_Id == cat_id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    }




                }


                //data.alldata = (from a in _LibraryContext.BookTransactionDMO
                //                from b in _LibraryContext.Adm_M_Student
                //                from c in _LibraryContext.Lib_M_Book_Accn_DMO
                //                from d in _LibraryContext.BookRegisterDMO
                //                from e in _LibraryContext.School_M_Class
                //                from f in _LibraryContext.School_Adm_Y_StudentDMO
                //                from g in _LibraryContext.school_M_Section
                //                where (a.AMST_Id == b.AMST_Id && a.LMBANO_Id == c.LMBANO_Id && c.LMB_Id == d.LMB_Id && d.ForTheClass == e.ASMCL_Id && f.AMST_Id == a.AMST_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                select new BookTransactionDTO
                //                {
                //                    AMST_Id = b.AMST_Id,
                //                    // AMST_FirstName=b.AMST_FirstName,
                //                    AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                //                    AMST_MiddleName = b.AMST_MiddleName,
                //                    AMST_LastName = b.AMST_LastName,
                //                    AMST_RegistrationNo = b.AMST_RegistrationNo,
                //                    AMST_AdmNo = b.AMST_AdmNo,
                //                    ASMCL_ClassName = e.ASMCL_ClassName,
                //                    ASMC_SectionName = g.ASMC_SectionName,
                //                    AMAY_RollNo = f.AMAY_RollNo,
                //                    Issue_Date = a.Issue_Date,
                //                    Due_Date = a.Due_Date,
                //                    Return_Date = a.Return_Date,
                //                    LMB_BookTitle = d.LMB_BookTitle,
                //                    LMB_VolNo = d.LMB_VolNo,
                //                    LMB_ClassNo = d.LMB_ClassNo,
                //                    LMB_Price = d.LMB_Price,
                //                    LMB_EntryDate = d.LMB_EntryDate,
                //                    Book_Trans_Status = a.Book_Trans_Status,
                //                }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO Editdata(BookTransactionDTO data)
        {
            try
            {
                List<BookTransactionDTO> bookdifferencedays = new List<BookTransactionDTO>();
                List<long> LBCPA_Id = new List<long>();
                if (data.issuertype == "std")
                {
                    //in this query we edit only selected data like AMST_Id and LMBANO_Id
                    var editlist = (from a in _LibraryContext.BookTransactionDMO
                                    from e in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                    from c in _LibraryContext.BookRegisterDMO
                                    from d in _LibraryContext.Adm_M_Student
                                    where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMST_Id == d.AMST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == data.Book_Trans_Id && a.LBTR_Id == e.LBTR_Id)
                                    select new BookTransactionDTO
                                    {
                                        Book_Trans_Id = a.LBTR_Id,
                                        LMB_Id = c.LMB_Id,
                                        AMST_Id = e.AMST_Id,
                                        MI_Id = a.MI_Id,
                                        //ASMAY_Id = a.ASMAY_Id,
                                        LMBANO_Id = a.LMBANO_Id,
                                        Issue_Date = a.LBTR_IssuedDate,
                                        Due_Date = a.LBTR_DueDate,
                                        Return_Date = a.LBTR_ReturnedDate,
                                        Book_Trans_Status = a.LBTR_Status,
                                        LMB_BookTitle = c.LMB_BookTitle,
                                        AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                                        AMST_MiddleName = d.AMST_MiddleName,
                                        AMST_LastName = d.AMST_LastName,
                                        AMST_Photoname = d.AMST_Photoname,
                                        LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                        AMST_AdmNo = d.AMST_AdmNo
                                    }).Distinct().ToList();




                    data.editlist = editlist.Distinct().ToArray();

                    //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitlelist = (from a in _LibraryContext.BookRegisterDMO
                                         from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                         from d in _LibraryContext.BookTransactionDMO
                                         where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && d.LMBANO_Id == b.LMBANO_Id && d.LBTR_Id == data.Book_Trans_Id && a.LMB_ActiveFlg == true)
                                         select new BookTransactionDTO
                                         {

                                             LMB_Id = a.LMB_Id,
                                             MI_Id = a.MI_Id,
                                             //   LMBA_Id = a.LMBA_Id,
                                             LMB_ClassNo = a.LMB_ClassNo,
                                             LMB_BookTitle = a.LMB_BookTitle,
                                             // d.tLMS_Id = a.LMS_Id,
                                             //  LMD_Id = a.LMD_Id,
                                             //    LMP_Id = a.LMP_Id,
                                             LMB_Price = a.LMB_Price,
                                             LMB_VolNo = a.LMB_VolNo,
                                             LMB_EntryDate = a.LMB_EntryDate,
                                             //LML_Id = a.LML_Id,
                                             // Donor_Id = a.Donor_Id,
                                             //  LMV_Id = a.LMV_Id,
                                             LMC_Id = a.LMC_Id,
                                             LMBANO_Id = b.LMBANO_Id,
                                             //  Rack_Id = b.Rack_Id,
                                             //  Max_Issue_Days = e.LNBCPAS_IssueDays,
                                             //Max_No_Renewals = e.LNBCPAS_NoOfRenewals,
                                             //   Max_Issue_Items = e.LNBCPAS_NoOfItems,
                                             LMBANO_AccessionNo = b.LMBANO_AccessionNo,


                                         }).Distinct().ToList();


                    data.booktitle = booktitlelist.ToArray();



                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAS_ActiveFlg == true && b.ASMCL_Id == data.ASMCL_Id && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAS_IssueDays,
                                                             Max_No_Renewals = b.LBCPAS_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAS_NoOfItems,

                                                         }).ToArray();
                        }
                        else if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAS_ActiveFlg == true && b.ASMCL_Id == data.ASMCL_Id && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAS_IssueDays,
                                                             Max_No_Renewals = b.LBCPAS_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAS_NoOfItems,

                                                         }).ToArray();
                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        if (booktitlelist.Count > 0)
                        {
                            cat_id = booktitlelist[0].LMC_Id;
                        }
                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAS_ActiveFlg == true && b.LMC_Id == cat_id && b.ASMCL_Id == data.ASMCL_Id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAS_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAS_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAS_NoOfItems,

                                                     }).ToArray();
                    }



                }
                else if (data.issuertype == "stf")
                {
                    //in this query we edit only selected data like AMST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.BookTransactionDMO
                                     from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from c in _LibraryContext.BookRegisterDMO
                                     from d in _LibraryContext.MasterEmployee
                                     where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == c.MI_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == data.Book_Trans_Id && a.LBTR_Id == e.LBTR_Id)
                                     select new BookTransactionDTO
                                     {
                                         Book_Trans_Id = a.LBTR_Id,
                                         LMB_Id = c.LMB_Id,
                                         HRME_Id = d.HRME_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         Issue_Date = a.LBTR_IssuedDate,
                                         Due_Date = a.LBTR_DueDate,
                                         Return_Date = a.LBTR_ReturnedDate,
                                         Book_Trans_Status = a.LBTR_Status,
                                         LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                         //LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMB_BookTitle,
                                         Fine_Amount = a.LBTR_TotalFine,
                                         //Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LBTR_FineWaived,
                                         HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                         //  HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                         ///      HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                         HRME_EmployeeCode = d.HRME_EmployeeCode,
                                         HRMD_Id = Convert.ToInt64(d.HRMD_Id),
                                         HRMDES_Id = Convert.ToInt64(d.HRMDES_Id)

                                     }).Distinct().ToArray();

                    //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.BookRegisterDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from d in _LibraryContext.BookTransactionDMO
                                     from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                                     where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && d.LBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMB_Id == b.LMB_Id && d.LBTR_Id == e.LBTR_Id && e.HRME_Id == data.HRME_Id && d.LMBANO_Id == b.LMBANO_Id)
                                     select new BookTransactionDTO
                                     {

                                         LMB_Id = a.LMB_Id,
                                         MI_Id = a.MI_Id,
                                         //  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMB_ClassNo,
                                         LMB_BookTitle = a.LMB_BookTitle,
                                         // LMS_Id = a.LMS_Id,
                                         //LMD_Id = a.LMD_Id,
                                         //  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMB_Price,
                                         LMB_VolNo = a.LMB_VolNo,
                                         LMB_EntryDate = a.LMB_EntryDate,
                                         // LML_Id = a.LML_Id,
                                         //  Donor_Id = a.Donor_Id,
                                         // LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         //   Rack_Id = b.Rack_Id,
                                         // Max_Issue_Days = c.Max_Issue_Days,
                                         // Max_No_Renewals = c.Max_No_Renewals,
                                         //  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();

                    data.booktitle = booktitle.ToArray();

                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAST_IssueDays,
                                                             Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAST_NoOfItems,

                                                         }).ToArray();
                        }
                        else if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAST_IssueDays,
                                                             Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAST_NoOfItems,

                                                         }).ToArray();
                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }
                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAST_ActiveFlg == true && b.LMC_Id == cat_id && b.HRMGT_Id == data.HRMGT_Id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAST_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAST_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAST_NoOfItems,

                                                     }).ToArray();
                    }



                }
                else if (data.issuertype == "dep")
                {
                    //in this query we edit only selected data like AMST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.BookTransactionDMO
                                     from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from c in _LibraryContext.BookRegisterDMO
                                     from d in _LibraryContext.HR_Master_Department
                                     where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == c.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == data.Book_Trans_Id && a.LBTR_Id == e.LBTR_Id)
                                     select new BookTransactionDTO
                                     {
                                         Book_Trans_Id = a.LBTR_Id,
                                         LMB_Id = c.LMB_Id,
                                         HRMD_Id = d.HRMD_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         Issue_Date = a.LBTR_IssuedDate,
                                         Due_Date = a.LBTR_DueDate,
                                         Return_Date = a.LBTR_ReturnedDate,
                                         Book_Trans_Status = a.LBTR_Status,
                                         LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                         //LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMB_BookTitle,
                                         Fine_Amount = a.LBTR_TotalFine,
                                         //Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LBTR_FineWaived,
                                         HRMD_DepartmentName = d.HRMD_DepartmentName,
                                         // LMD_DepartmentCode = d.LMD_DepartmentCode,
                                     }).Distinct().ToArray();

                    //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.BookRegisterDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from d in _LibraryContext.BookTransactionDMO
                                     from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                                     where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && d.LBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMB_Id == b.LMB_Id && d.LBTR_Id == e.LBTR_Id && e.HRMD_Id == data.HRMD_Id && d.LMBANO_Id == b.LMBANO_Id)
                                     select new BookTransactionDTO
                                     {

                                         LMB_Id = a.LMB_Id,
                                         MI_Id = a.MI_Id,
                                         //  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMB_ClassNo,
                                         LMB_BookTitle = a.LMB_BookTitle,
                                         // LMS_Id = a.LMS_Id,
                                         //LMD_Id = a.LMD_Id,
                                         //  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMB_Price,
                                         LMB_VolNo = a.LMB_VolNo,
                                         LMB_EntryDate = a.LMB_EntryDate,
                                         // LML_Id = a.LML_Id,
                                         //  Donor_Id = a.Donor_Id,
                                         // LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         //   Rack_Id = b.Rack_Id,
                                         // Max_Issue_Days = c.Max_Issue_Days,
                                         // Max_No_Renewals = c.Max_No_Renewals,
                                         //  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();



                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();
                        }
                        else if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();
                        }
                    }
                    else
                    {
                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }


                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "DEPARTMENT" && b.LMC_Id == cat_id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    }




                }

                else if (data.issuertype == "gst")
                {
                    //in this query we edit only selected data like AMST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.BookTransactionDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from c in _LibraryContext.BookRegisterDMO
                                     where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == data.Book_Trans_Id)
                                     select new BookTransactionDTO
                                     {
                                         Book_Trans_Id = a.LBTR_Id,
                                         LMB_Id = c.LMB_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         Issue_Date = a.LBTR_IssuedDate,
                                         Due_Date = a.LBTR_DueDate,
                                         Return_Date = a.LBTR_ReturnedDate,
                                         Book_Trans_Status = a.LBTR_Status,
                                         LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                                         //LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMB_BookTitle,
                                         Fine_Amount = a.LBTR_TotalFine,
                                         //Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LBTR_FineWaived,
                                         LBTR_GuestName = a.LBTR_GuestName,
                                         LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                                         LBTR_GuestEmailId = a.LBTR_GuestEmailId,

                                     }).Distinct().ToArray();

                    //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.BookRegisterDMO
                                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                     from d in _LibraryContext.BookTransactionDMO
                                     where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && d.LBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMB_Id == b.LMB_Id && b.LMBANO_Id == d.LMBANO_Id)
                                     select new BookTransactionDTO
                                     {

                                         LMB_Id = a.LMB_Id,
                                         MI_Id = a.MI_Id,
                                         //  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMB_ClassNo,
                                         LMB_BookTitle = a.LMB_BookTitle,
                                         // LMS_Id = a.LMS_Id,
                                         //LMD_Id = a.LMD_Id,
                                         //  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMB_Price,
                                         LMB_VolNo = a.LMB_VolNo,
                                         LMB_EntryDate = a.LMB_EntryDate,
                                         // LML_Id = a.LML_Id,
                                         //  Donor_Id = a.Donor_Id,
                                         // LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMBANO_Id,
                                         //   Rack_Id = b.Rack_Id,
                                         // Max_Issue_Days = c.Max_Issue_Days,
                                         // Max_No_Renewals = c.Max_No_Renewals,
                                         //  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();


                    data.booktitle = booktitle.ToArray();


                    if (data.bookcat_type == "book")
                    {
                        if (data.booktype == "issue")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Issue")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();
                        }
                        else if (data.booktype == "ref")
                        {
                            data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                         from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                                         where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Reference")
                                                         select new BookTransactionDTO
                                                         {
                                                             Max_Issue_Days = b.LBCPAO_IssueDays,
                                                             Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                                                             Max_Issue_Items = b.LBCPAO_NoOfItems,

                                                         }).ToArray();
                        }
                    }
                    else
                    {



                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }


                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "GUEST" && b.LMC_Id == cat_id)
                                                     select new BookTransactionDTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    }



                }
                var update = _LibraryContext.BookTransactionDMO.Where(t => t.LBTR_Id == data.Book_Trans_Id).SingleOrDefault();
                decimal finalfine_amount = 0;
                if (data.issuertype == "std")
                {

                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;


                    if (today > update.LBTR_ReturnedDate)
                    {
                        //stthomos
                        var FlagCount = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Where(R => R.LBCPA_Flg == "STUDENT" && R.MI_Id == data.MI_Id && R.LBCPA_ExcludeHolidayFlg == true).ToList();
                        if (FlagCount.Count > 0)
                        {
                            foreach (var d in FlagCount)
                            {
                                LBCPA_Id.Add(d.LBCPA_Id);
                            }
                        }
                        var Holidays = _LibraryContext.LIB_Circulation_Parameter_StudentDMO.Where(R => R.LBCPAS_ActiveFlg == true
                         && LBCPA_Id.Contains(R.LBCPA_Id) && R.ASMCL_Id == data.ASMCL_Id).ToList();
                        if (Holidays.Count > 0)
                        {
                            var todaydate = today.Date.ToString("yyyy-MM-dd");
                            var lbtr_returndate = update.LBTR_ReturnedDate.Date.ToString("yyyy-MM-dd");
                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Library_Diffrencetotaldays";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                                cmd.Parameters.Add(new SqlParameter("@today", SqlDbType.VarChar) { Value = todaydate });

                                cmd.Parameters.Add(new SqlParameter("@LBTR_ReturnedDate", SqlDbType.VarChar) { Value = lbtr_returndate });

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



                                            bookdifferencedays.Add(new BookTransactionDTO
                                            {
                                                difftotaldays = Convert.ToInt64(dataReader["differnce"].ToString()),

                                            });

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            difftotaldays = bookdifferencedays[0].difftotaldays;
                        }
                        else
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }

                    }
                    //difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "stf")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {

                        //difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        var FlagCount = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Where(R => R.LBCPA_Flg == "STAFF" && R.MI_Id == data.MI_Id && R.LBCPA_ExcludeHolidayFlg == true).ToList();
                        if (FlagCount.Count > 0)
                        {
                            foreach (var d in FlagCount)
                            {
                                LBCPA_Id.Add(d.LBCPA_Id);
                            }
                        }
                        var Holidays = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Where(R => R.LBCPAST_ActiveFlg == true
                         && LBCPA_Id.Contains(R.LBCPA_Id) && R.HRMGT_Id == data.HRMGT_Id).ToList();
                        if (Holidays.Count > 0)
                        {
                            var todaydate = today.Date.ToString("yyyy-MM-dd");
                            var lbtr_returndate = update.LBTR_ReturnedDate.Date.ToString("yyyy-MM-dd");

                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Library_Diffrencetotaldays";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                                cmd.Parameters.Add(new SqlParameter("@today", SqlDbType.VarChar) { Value = todaydate });

                                cmd.Parameters.Add(new SqlParameter("@LBTR_ReturnedDate", SqlDbType.VarChar) { Value = lbtr_returndate });

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



                                            bookdifferencedays.Add(new BookTransactionDTO
                                            {
                                                difftotaldays = Convert.ToInt64(dataReader["differnce"].ToString()),

                                            });

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            difftotaldays = bookdifferencedays[0].difftotaldays;
                        }
                        else
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }

                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "dep")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        var FlagCount = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Where(R => R.LBCPA_Flg == "DEPARTMENT" && R.LBCPA_ActiveFlg == true && R.MI_Id == data.MI_Id && R.LBCPA_ExcludeHolidayFlg == true).ToList();
                        if (FlagCount.Count > 0)
                        {
                            foreach (var d in FlagCount)
                            {
                                LBCPA_Id.Add(d.LBCPA_Id);
                            }
                        }
                        var Holidays = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Where(R => R.LBCPAO_ActiveFlg == true
                         && LBCPA_Id.Contains(R.LBCPA_Id)).ToList();
                        if (Holidays.Count > 0)
                        {
                            var todaydate = today.Date.ToString("yyyy-MM-dd");
                            var lbtr_returndate = update.LBTR_ReturnedDate.Date.ToString("yyyy-MM-dd");

                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Library_Diffrencetotaldays";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                                cmd.Parameters.Add(new SqlParameter("@today", SqlDbType.VarChar) { Value = todaydate });

                                cmd.Parameters.Add(new SqlParameter("@LBTR_ReturnedDate", SqlDbType.VarChar) { Value = lbtr_returndate });

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



                                            bookdifferencedays.Add(new BookTransactionDTO
                                            {
                                                difftotaldays = Convert.ToInt64(dataReader["differnce"].ToString()),

                                            });

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            difftotaldays = bookdifferencedays[0].difftotaldays;
                        }
                        else
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }

                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }

                else if (data.issuertype == "gst")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        var FlagCount = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Where(R => R.LBCPA_Flg == "GUEST" && R.LBCPA_ActiveFlg == true && R.MI_Id == data.MI_Id && R.LBCPA_ExcludeHolidayFlg == true).ToList();
                        if (FlagCount.Count > 0)
                        {
                            foreach (var d in FlagCount)
                            {
                                LBCPA_Id.Add(d.LBCPA_Id);
                            }
                        }
                        var Holidays = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Where(R => R.LBCPAO_ActiveFlg == true
                         && LBCPA_Id.Contains(R.LBCPA_Id)).ToList();
                        if (Holidays.Count > 0)
                        {
                            var todaydate = today.Date.ToString("yyyy-MM-dd");
                            var lbtr_returndate = update.LBTR_ReturnedDate.Date.ToString("yyyy-MM-dd");

                            using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Library_Diffrencetotaldays";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });

                                cmd.Parameters.Add(new SqlParameter("@today", SqlDbType.VarChar) { Value = todaydate });

                                cmd.Parameters.Add(new SqlParameter("@LBTR_ReturnedDate", SqlDbType.VarChar) { Value = lbtr_returndate });

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



                                            bookdifferencedays.Add(new BookTransactionDTO
                                            {
                                                difftotaldays = Convert.ToInt64(dataReader["differnce"].ToString()),

                                            });

                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            difftotaldays = bookdifferencedays[0].difftotaldays;
                        }
                        else
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }
                        //difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }


                update.LBTR_FineCollected = update.LBTR_FineCollected;
                update.LBTR_TotalFine = update.LBTR_TotalFine + Convert.ToDecimal(finalfine_amount);

                data.Fine_Amount = update.LBTR_TotalFine;
                data.finecollected = update.LBTR_FineCollected;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO returndata(BookTransactionDTO data)
        {
            try
            {

                //for update data
                var update = _LibraryContext.BookTransactionDMO.Where(t => t.LBTR_Id == data.Book_Trans_Id).SingleOrDefault();
                decimal finalfine_amount = 0;
                if (data.issuertype == "std")
                {

                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    //difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "stf")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "dep")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }

                else if (data.issuertype == "gst")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                //update.LBTR_ReturnedDate = DateTime.Now;
                update.LBTR_Status = "Return";
                //update.LBTR_FineCollected = update.LBTR_FineCollected+Convert.ToDecimal(finalfine_amount);
                //  update.LBTR_FineCollected = data.LBTR_FineCollected+ update.LBTR_FineCollected; 
                update.LBTR_FineCollected = data.LBTR_FineCollected;
                update.LBTR_TotalFine = update.LBTR_TotalFine + Convert.ToDecimal(finalfine_amount);
                //if any fine then setupdate.LBTR_FineCollected+Convert.ToDecimal(finalfine_amount); 
                update.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(update);
                int rowAffected = _LibraryContext.SaveChanges();

                if (rowAffected > 0)
                {

                    if (data.issuertype == "std")
                    {

                        var studentid = _LibraryContext.LIB_Book_Transaction_StudentDMO.Single(t => t.LBTR_Id == update.LBTR_Id && t.LBTRS_ActiveFlg == true);
                        long mobileno = 0;
                        var mobilenolist = (from a in _db.Adm_M_Student
                                            from b in _db.Adm_M_Student_MobileNo
                                            where a.AMST_Id == b.AMST_Id && a.AMST_Id == studentid.AMST_Id && a.MI_Id == data.MI_Id
                                            select b).ToList();

                        if (mobilenolist.Count > 0)
                        {
                            if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                            {
                                mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                            }

                        }
                        else
                        {
                            var mobilenolist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == studentid.AMST_Id).ToList();

                            if (mobilenolist1.Count > 0)
                            {
                                if (mobilenolist1[0].AMST_MobileNo != 0)
                                {
                                    mobileno = mobilenolist1[0].AMST_MobileNo;
                                }

                            }
                        }
                        // mobileno = 9591081840;
                        if (mobileno != 0)
                        {
                            SMS sms = new SMS(_db);
                            string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKRETURN", data.ASMAY_Id, studentid.AMST_Id, update.LMBANO_Id).Result;
                        }

                        string email_id = "";
                        var emailidlist = (from a in _db.Adm_M_Student
                                           from b in _db.Adm_M_Student_Email_Id
                                           where a.AMST_Id == b.AMST_Id && a.AMST_Id == studentid.AMST_Id && a.MI_Id == data.MI_Id
                                           select b).ToList();

                        if (emailidlist.Count > 0)
                        {
                            if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                            {
                                email_id = emailidlist[0].AMSTE_EmailId;
                            }

                        }
                        else
                        {
                            var emailidlist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == studentid.AMST_Id).ToList();

                            if (emailidlist1.Count > 0)
                            {
                                if (emailidlist1[0].AMST_emailId != "")
                                {
                                    email_id = emailidlist1[0].AMST_emailId;
                                }

                            }
                        }
                        // email_id = "praveenishwar@vapstech.com";
                        if (email_id != "" && email_id != null)
                        {
                            Email email = new Email(_db);
                            string s = email.sendmail_library(data.MI_Id, email_id, "BOOKRETURN", data.ASMAY_Id, studentid.AMST_Id, update.LMBANO_Id);
                        }
                    }



                    if (data.issuertype == "stf")
                    {

                        var studentid = _LibraryContext.LIB_Book_Transaction_StaffDMO.Single(t => t.LBTR_Id == update.LBTR_Id && t.LBTRST_ActiveFlg == true);
                        long mobileno = 0;
                        string email_id = "";
                        var mobilenolist = (from a in _LibraryContext.Emp_MobileNo
                                            where a.HRME_Id == studentid.HRME_Id && a.HRMEMNO_DeFaultFlag == "default"
                                            select a).ToList();

                        if (mobilenolist.Count > 0)
                        {
                            if (mobilenolist[0].HRMEMNO_MobileNo != 0 && mobilenolist[0].HRMEMNO_MobileNo != null)
                            {
                                mobileno = Convert.ToInt64(mobilenolist[0].HRMEMNO_MobileNo);
                            }
                            //mobileno = 9591081840;
                            if (mobileno != 0)
                            {
                                SMS sms = new SMS(_db);
                                string s = sms.sendSms_library(data.MI_Id, mobileno, "STAFFBOOKRETURN", data.ASMAY_Id, studentid.HRME_Id, update.LMBANO_Id).Result;
                            }


                        }
                        var mobilenolist123 = (from a in _LibraryContext.Emp_Email_Id
                                               where a.HRME_Id == studentid.HRME_Id && a.HRMEM_DeFaultFlag == "default"
                                               select a).ToList();
                        if (mobilenolist123.Count > 0)
                        {
                            if (mobilenolist123[0].HRMEM_EmailId != "" && mobilenolist123[0].HRMEM_EmailId != null)
                            {
                                email_id = mobilenolist123[0].HRMEM_EmailId;
                            }
                            // email_id = "praveenishwar@vapstech.com";
                            if (email_id != "" && email_id != null)
                            {
                                Email email = new Email(_db);
                                string s = email.sendmail_library(data.MI_Id, email_id, "STAFFBOOKRETURN", data.ASMAY_Id, studentid.HRME_Id, update.LMBANO_Id);
                            }
                        }




                        // email_id = "praveenishwar@vapstech.com";

                    }
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                //aded New Things
               
                var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO renewaldata(BookTransactionDTO data)
        {
            try
            {
                if (data.Book_Trans_Id > 0)
                {

                    //for update data
                    var update = _LibraryContext.BookTransactionDMO.Where(t => t.LBTR_Id == data.Book_Trans_Id).SingleOrDefault();

                    decimal finalfine_amount = 0;
                    if (data.issuertype == "std")
                    {

                        //check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //check Difference Between Actual return date and Today's Return date 
                        if (today > update.LBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }
                        //  difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //check fine amount according to from date and To Date
                                    if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                    {
                                        if (item.LFSE_ToDay >= difftotaldays)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                        else
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                            remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                        }
                                    }
                                    else
                                    {
                                        if (difftotaldays >= item.LFSE_FromDay)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                    }

                                }
                            }


                        }
                    }
                    else if (data.issuertype == "stf")
                    {
                        //check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //check Difference Between Actual return date and Today's Return date 
                        if (today > update.LBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }
                        // difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //check fine amount according to from date and To Date
                                    if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                    {
                                        if (item.LFSE_ToDay >= difftotaldays)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                        else
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                            remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                        }
                                    }
                                    else
                                    {
                                        if (difftotaldays >= item.LFSE_FromDay)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                    }

                                }
                            }


                        }
                    }
                    else if (data.issuertype == "dep")
                    {
                        //check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //check Difference Between Actual return date and Today's Return date 
                        if (today > update.LBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }
                        // difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //check fine amount according to from date and To Date
                                    if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                    {
                                        if (item.LFSE_ToDay >= difftotaldays)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                        else
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                            remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                        }
                                    }
                                    else
                                    {
                                        if (difftotaldays >= item.LFSE_FromDay)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                    }

                                }
                            }


                        }
                    }

                    else if (data.issuertype == "gst")
                    {
                        //check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //check Difference Between Actual return date and Today's Return date 
                        if (today > update.LBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                        }
                        // difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //check fine amount according to from date and To Date
                                    if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                    {
                                        if (item.LFSE_ToDay >= difftotaldays)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                        else
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                            remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                        }
                                    }
                                    else
                                    {
                                        if (difftotaldays >= item.LFSE_FromDay)
                                        {
                                            if (item.LFSE_PerDayFlg == true)
                                            {
                                                finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                            }
                                            else
                                            {
                                                finalfine_amount += item.LFSE_Amount;
                                            }

                                        }
                                    }

                                }
                            }


                        }
                    }
                    //check Maximum Renewal time according to Circulation parameter Page
                    if (data.Max_No_Renewals > update.LBTR_Renewalcounter)
                    {
                        data.renew = false;
                        update.LBTR_IssuedDate = data.Issue_Date;
                        update.LBTR_RenewedDate = data.Issue_Date;
                        update.LBTR_Renewalcounter = update.LBTR_Renewalcounter + 1;
                        update.LBTR_DueDate = data.Due_Date;
                        update.LBTR_ReturnedDate = data.Return_Date;
                        update.LBTR_Status = "Issue";
                        //update.LBTR_FineCollected = update.LBTR_FineCollected + Convert.ToDecimal(finalfine_amount);
                        update.LBTR_FineCollected = data.LBTR_FineCollected + update.LBTR_FineCollected;
                        update.LBTR_TotalFine = update.LBTR_TotalFine + Convert.ToDecimal(finalfine_amount);
                        update.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update);

                    }
                    else
                    {
                        data.renew = true;
                    }

                    int rowAffected = _LibraryContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        if (data.issuertype == "std")
                        {

                            var studentid = _LibraryContext.LIB_Book_Transaction_StudentDMO.Single(t => t.LBTR_Id == update.LBTR_Id && t.LBTRS_ActiveFlg == true);
                            long mobileno = 0;
                            var mobilenolist = (from a in _db.Adm_M_Student
                                                from b in _db.Adm_M_Student_MobileNo
                                                where a.AMST_Id == b.AMST_Id && a.AMST_Id == studentid.AMST_Id && a.MI_Id == data.MI_Id
                                                select b).ToList();

                            if (mobilenolist.Count > 0)
                            {
                                if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                                {
                                    mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                                }

                            }
                            else
                            {
                                var mobilenolist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == studentid.AMST_Id).ToList();

                                if (mobilenolist1.Count > 0)
                                {
                                    if (mobilenolist1[0].AMST_MobileNo != 0)
                                    {
                                        mobileno = mobilenolist1[0].AMST_MobileNo;
                                    }

                                }
                            }
                            //  mobileno = 9591081840;
                            if (mobileno != 0)
                            {
                                SMS sms = new SMS(_db);
                                string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKRENEWAL", data.ASMAY_Id, studentid.AMST_Id, update.LMBANO_Id).Result;
                            }

                            string email_id = "";
                            var emailidlist = (from a in _db.Adm_M_Student
                                               from b in _db.Adm_M_Student_Email_Id
                                               where a.AMST_Id == b.AMST_Id && a.AMST_Id == studentid.AMST_Id && a.MI_Id == data.MI_Id
                                               select b).ToList();

                            if (emailidlist.Count > 0)
                            {
                                if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                                {
                                    email_id = emailidlist[0].AMSTE_EmailId;
                                }

                            }
                            else
                            {
                                var emailidlist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == studentid.AMST_Id).ToList();

                                if (emailidlist1.Count > 0)
                                {
                                    if (emailidlist1[0].AMST_emailId != "")
                                    {
                                        email_id = emailidlist1[0].AMST_emailId;
                                    }

                                }
                            }
                            //   email_id = "praveenishwar@vapstech.com";
                            if (email_id != "" && email_id != null)
                            {
                                Email email = new Email(_db);
                                string s = email.sendmail_library(data.MI_Id, email_id, "BOOKRENEWAL", data.ASMAY_Id, studentid.AMST_Id, update.LMBANO_Id);
                            }
                        }

                        if (data.issuertype == "stf")
                        {

                            var studentid = _LibraryContext.LIB_Book_Transaction_StaffDMO.Single(t => t.LBTR_Id == update.LBTR_Id && t.LBTRST_ActiveFlg == true);
                            long mobileno = 0;
                            string email_id = "";
                            var mobilenolist = (from a in _LibraryContext.Emp_MobileNo
                                                where a.HRME_Id == studentid.HRME_Id && a.HRMEMNO_DeFaultFlag == "default"
                                                select a).ToList();

                            if (mobilenolist.Count > 0)
                            {
                                if (mobilenolist[0].HRMEMNO_MobileNo != 0 && mobilenolist[0].HRMEMNO_MobileNo != null)
                                {
                                    mobileno = Convert.ToInt64(mobilenolist[0].HRMEMNO_MobileNo);
                                }
                                // mobileno = 9591081840;
                                if (mobileno != 0)
                                {
                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms_library(data.MI_Id, mobileno, "STAFFBOOKRENEWAL", data.ASMAY_Id, studentid.HRME_Id, update.LMBANO_Id).Result;
                                }

                            }

                            var mobilenolist123 = (from a in _LibraryContext.Emp_Email_Id
                                                   where a.HRME_Id == studentid.HRME_Id && a.HRMEM_DeFaultFlag == "default"
                                                   select a).ToList();

                            if (mobilenolist123.Count > 0)
                            {
                                if (mobilenolist123[0].HRMEM_EmailId != "" && mobilenolist123[0].HRMEM_EmailId != null)
                                {
                                    email_id = mobilenolist123[0].HRMEM_EmailId;
                                }

                                if (email_id != "" && email_id != null)
                                {
                                    Email email = new Email(_db);
                                    string s = email.sendmail_library(data.MI_Id, email_id, "STAFFBOOKRENEWAL", data.ASMAY_Id, studentid.HRME_Id, update.LMBANO_Id);
                                }
                            }


                        }
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                else
                {
                    data.returnval = false;
                }
               
                var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookTransactionDTO get_staff1(BookTransactionDTO data)
        {
            try
            {
                data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                   select new Staff_BookTranasctionDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                       HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                   }).Distinct().OrderBy(t => t.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public BookTransactionDTO getdepchange(BookTransactionDTO data)
        {
            try
            {


                data.filldesignation = (from a in _LibraryContext.MasterEmployee
                                        from b in _LibraryContext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.HRMD_Id == a.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public BookTransactionDTO loadbookavail(BookTransactionDTO data)
        {

            try
            {
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_LOAD_BOOKS_FOR_AVAILABILITY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    //  cmd.Parameters.Add(new SqlParameter("@Type",
                    //SqlDbType.VarChar)
                    //  {
                    //      Value = typeofbook
                    //  });

                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type2",
                 SqlDbType.VarChar)
                    {
                        Value = "LOAD"
                    });
                    cmd.Parameters.Add(new SqlParameter("@Searctext",
              SqlDbType.VarChar)
                    {
                        Value = ""
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
                        data.avilbooktitle = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public BookTransactionDTO availget_bookdetails(BookTransactionDTO data)
        {

            try
            {
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_CHECK_AVAILABILITY_BOOK_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.LMBANO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
                        data.avilbookdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

               

                var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction_delete @p0,@p1", data.MI_Id, data.AMCTST_IP);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public BookTransactionDTO showfine(BookTransactionDTO data)
        {
            try
            {
                var update = _LibraryContext.BookTransactionDMO.Where(t => t.LBTR_Id == data.Book_Trans_Id).SingleOrDefault();
                decimal finalfine_amount = 0;
                if (data.issuertype == "std")
                {

                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    //difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "stf")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }
                else if (data.issuertype == "dep")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }

                else if (data.issuertype == "gst")
                {
                    //check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LBTR_ReturnedDate = update.LBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //check Difference Between Actual return date and Today's Return date 
                    if (today > update.LBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LBTR_ReturnedDate).TotalDays;
                    }
                    // difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //check fine amount according to from date and To Date
                                if (item.LFSE_SlabTypeFlg == "BETWEEN")
                                {
                                    if (item.LFSE_ToDay >= difftotaldays)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                    else
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(item.LFSE_ToDay);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                        remaindays = Convert.ToInt32(difftotaldays - item.LFSE_ToDay);
                                    }
                                }
                                else
                                {
                                    if (difftotaldays >= item.LFSE_FromDay)
                                    {
                                        if (item.LFSE_PerDayFlg == true)
                                        {
                                            finalfine_amount += item.LFSE_Amount * Convert.ToDecimal(remaindays);
                                        }
                                        else
                                        {
                                            finalfine_amount += item.LFSE_Amount;
                                        }

                                    }
                                }

                            }
                        }


                    }
                }


                update.LBTR_FineCollected = update.LBTR_FineCollected;
                update.LBTR_TotalFine = update.LBTR_TotalFine + Convert.ToDecimal(finalfine_amount);

                data.Fine_Amount = update.LBTR_TotalFine;
                data.finecollected = update.LBTR_FineCollected;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public BookTransactionDTO smsdue(BookTransactionDTO data)
        {
            try
            {
                DateTime dddate = DateTime.Now;
                dddate = dddate.AddDays(1);

                var duu_list = (from a in _LibraryContext.BookTransactionDMO
                                from b in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                from c in _LibraryContext.Lib_M_Book_Accn_DMO
                                where a.MI_Id == data.MI_Id && a.LBTR_Status.ToLower() == "issue" && a.LBTR_Id == b.LBTR_Id && a.LMBANO_Id == c.LMBANO_Id && Convert.ToDateTime(a.LBTR_DueDate).Date == dddate.Date
                                select new BookTransactionDTO
                                {
                                    LMBANO_Id = a.LMBANO_Id,
                                    AMST_Id = b.AMST_Id
                                }
      ).Distinct().ToList();
                if (duu_list.Count > 0)
                {

                    foreach (var item in duu_list)
                    {
                        long mobileno = 0;
                        var mobilenolist = (from a in _db.Adm_M_Student
                                            from b in _db.Adm_M_Student_MobileNo
                                            where a.AMST_Id == b.AMST_Id && a.AMST_Id == item.AMST_Id && a.MI_Id == data.MI_Id
                                            select b).ToList();

                        if (mobilenolist.Count > 0)
                        {
                            if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                            {
                                mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                            }

                        }
                        else
                        {
                            var mobilenolist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == item.AMST_Id).ToList();

                            if (mobilenolist1.Count > 0)
                            {
                                if (mobilenolist1[0].AMST_MobileNo != 0)
                                {
                                    mobileno = mobilenolist1[0].AMST_MobileNo;
                                }

                            }
                        }
                        //  mobileno = 9591081840;
                        if (mobileno != 0)
                        {
                            SMS sms = new SMS(_db);
                            string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKDUE", data.ASMAY_Id, item.AMST_Id, item.LMBANO_Id).Result;
                        }

                        string email_id = "";
                        var emailidlist = (from a in _db.Adm_M_Student
                                           from b in _db.Adm_M_Student_Email_Id
                                           where a.AMST_Id == b.AMST_Id && a.AMST_Id == item.AMST_Id && a.MI_Id == data.MI_Id
                                           select b).ToList();

                        if (emailidlist.Count > 0)
                        {
                            if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                            {
                                email_id = emailidlist[0].AMSTE_EmailId;
                            }

                        }
                        else
                        {
                            var emailidlist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == item.AMST_Id).ToList();

                            if (emailidlist1.Count > 0)
                            {
                                if (emailidlist1[0].AMST_emailId != "")
                                {
                                    email_id = emailidlist1[0].AMST_emailId;
                                }

                            }
                        }
                        //  email_id = "praveenishwar@vapstech.com";
                        if (email_id != "" && email_id != null)
                        {
                            Email email = new Email(_db);
                            string s = email.sendmail_library(data.MI_Id, email_id, "BOOKDUE", data.ASMAY_Id, item.AMST_Id, item.LMBANO_Id);
                        }

                        data.returnval = true;

                    }


                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        public BookTransactionDTO aftersmsdue(BookTransactionDTO data)
        {
            try
            {
                DateTime dddate = DateTime.Now;
                dddate = dddate.AddDays(1);

                var duu_list = (from a in _LibraryContext.BookTransactionDMO
                                from b in _LibraryContext.LIB_Book_Transaction_StudentDMO
                                from c in _LibraryContext.Lib_M_Book_Accn_DMO
                                where a.MI_Id == data.MI_Id && a.LBTR_Status.ToLower() == "issue" && a.LBTR_Id == b.LBTR_Id && a.LMBANO_Id == c.LMBANO_Id && Convert.ToDateTime(a.LBTR_DueDate).Date < dddate.Date
                                select new BookTransactionDTO
                                {
                                    LMBANO_Id = a.LMBANO_Id,
                                    AMST_Id = b.AMST_Id
                                }
      ).Distinct().ToList();
                if (duu_list.Count > 0)
                {

                    foreach (var item in duu_list)
                    {
                        long mobileno = 0;
                        var mobilenolist = (from a in _db.Adm_M_Student
                                                // from b in _db.Adm_M_Student_MobileNo
                                            where a.AMST_Id == item.AMST_Id && a.MI_Id == data.MI_Id
                                            select
                                            new BookTransactionDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FatherMobleNo = a.AMST_FatherMobleNo
                                            }).ToList();

                        if (mobilenolist.Count > 0)
                        {
                            if (mobilenolist[0].AMST_FatherMobleNo != null)
                            {
                                mobileno = Convert.ToInt64(mobilenolist[0].AMST_FatherMobleNo);
                            }

                        }
                        else
                        {
                            var mobilenolist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == item.AMST_Id).ToList();

                            if (mobilenolist1.Count > 0)
                            {
                                if (mobilenolist1[0].AMST_MobileNo != 0)
                                {
                                    mobileno = mobilenolist1[0].AMST_MobileNo;
                                }

                            }
                        }
                        //  mobileno = 9591081840;
                        if (mobileno != 0)
                        {
                            SMS sms = new SMS(_db);
                            string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKAFTERDUE", data.ASMAY_Id, item.AMST_Id, item.LMBANO_Id).Result;
                        }

                        string email_id = "";
                        var emailidlist = (from a in _db.Adm_M_Student
                                               // from b in _db.Adm_M_Student_Email_Id
                                           where a.AMST_Id == item.AMST_Id && a.MI_Id == data.MI_Id
                                           select
                                            new BookTransactionDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                AMST_FatheremailId = a.AMST_FatheremailId
                                            }).ToList();

                        if (emailidlist.Count > 0)
                        {
                            if (emailidlist[0].AMST_FatheremailId != "" && emailidlist[0].AMST_FatheremailId != null)
                            {
                                email_id = emailidlist[0].AMST_FatheremailId;
                            }

                        }
                        else
                        {
                            var emailidlist1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == item.AMST_Id).ToList();

                            if (emailidlist1.Count > 0)
                            {
                                if (emailidlist1[0].AMST_emailId != "")
                                {
                                    email_id = emailidlist1[0].AMST_emailId;
                                }

                            }
                        }
                        //  email_id = "praveenishwar@vapstech.com";
                        if (email_id != "" && email_id != null)
                        {
                            Email email = new Email(_db);
                            string s = email.sendmail_library(data.MI_Id, email_id, "BOOKAFTERDUE", data.ASMAY_Id, item.AMST_Id, item.LMBANO_Id);
                        }

                        data.returnval = true;

                    }


                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }

        //ShowDiffrentDays
        public BookTransactionDTO ShowDiffrentDays(BookTransactionDTO data)
        {
            try
            {
                var lbtr_returndate = data.Return_Date.Date.ToString("yyyy-MM-dd");
                var issuedate = data.Issue_Date.Value.ToString("yyyy-MM-dd");
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_Weekly_StThomosFinecalculation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                    SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Returndate",
                    SqlDbType.VarChar)
                    {
                        Value = lbtr_returndate
                    });
                    cmd.Parameters.Add(new SqlParameter("@issuedate",
                   SqlDbType.VarChar)
                    {
                        Value = issuedate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Duedate",
                           SqlDbType.VarChar)
                    {
                        Value = data.difftotaldays
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
                        data.configdata = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }
        //SaveSmartCard
        public BookTransactionDTO SaveSmartCard(BookTransactionDTO data)
        {
            try
            {
                //AMCTST_IP
                var contactExistsP = _LibraryContext.Database.ExecuteSqlCommand("LIB_RFCARD_Tranasction @p0,@p1,@p2,@p3", data.MI_Id, data.AMST_Id, data.AMCTST_IP, data.School_Flag);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
    }
}
