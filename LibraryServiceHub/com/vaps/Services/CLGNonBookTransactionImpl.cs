using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class CLGNonBookTransactionImpl:Interfaces.CLGNonBookTransactionInterface
    {

        public LibraryContext _LibraryContext;

        DomainModelMsSqlServerContext _db;
        public CLGNonBookTransactionImpl(LibraryContext para, DomainModelMsSqlServerContext db)
        {
            _LibraryContext = para;
            _db = db;
        }


        public ClgNonBookTransaction_DTO getdetails_smartcard(ClgNonBookTransaction_DTO data)
        {
            try
            {
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          ////// from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.LMAL_Id == b.LMAL_Id && b.MI_Id == data.MI_Id &&  a.LMNBKL_ActiveFlg == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public ClgNonBookTransaction_DTO getdetails(ClgNonBookTransaction_DTO data)
        {
            try
            {
                List<long> clsids = new List<long>();
                string s = "";
                ////// ACR122U acr = new ACR122U();
                //////   acr.portsettings122u();
                ///////   s = acr.amstid.Trim().Replace("S", "").Replace("\0", "").Trim();

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
                    ////if (data.booktype == "issue")
                    ////{
                    ////    typeofbook = "Issue";
                    ////}
                    ////else if (data.booktype == "ref")
                    ////{
                    ////    typeofbook = "Reference";
                    ////}
                    ////else if (data.booktype == "nonbook")
                    ////{
                    ////    typeofbook = "nonbook";
                    ////}

                    //if (data.issuertype == "std")
                    //{
                    //    var libclasslist = (from a in _LibraryContext.LIB_Master_Library_DMO
                    //                        from b in _LibraryContext.LIB_User_Library_DMO
                    //                        from c in _LibraryContext.LIB_Library_Class_DMO
                    //                        where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == c.LMAL_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && c.LLC_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == liblist.FirstOrDefault().LMAL_Id
                    //                        select c).ToList();



                    //    if (libclasslist.Count > 0)
                    //    {
                    //        foreach (var item in libclasslist)
                    //        {
                    //            clsids.Add(item.ASMCL_Id);
                    //        }
                    //    }


                    //    //////if (libclasslist.Count==0)
                    //    //////{
                    //    //////    data.clamsg = "NS";
                    //    //////    return data;
                    //    //////}

                    //}

                    if (data.issuertype == "std")
                    {

                        var list = _LibraryContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id
                        ////// && t.Is_Active == true && t.ASMAY_ActiveFlag == 1 
                        && t.ASMAY_Id == data.ASMAY_Id).ToList();//////AcademicYear
                        data.yearlist = list.ToArray();

                        var currYear = _LibraryContext.AcademicYearDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                        data.currentYear = currYear.ToArray();


                        var courselist = (from a in _LibraryContext.MasterCourseDMO
                                          from b in _LibraryContext.CLG_Adm_College_AY_CourseDMO
                                          where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                          select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                        data.courselist = courselist.ToArray();
                        if (data.AMCO_Id == 0)
                        {
                            data.AMCO_Id = courselist.FirstOrDefault().AMCO_Id;
                        }

                        var branchlist = (from a in _LibraryContext.ClgMasterBranchDMO
                                          from b in _LibraryContext.CLG_Adm_College_AY_CourseDMO
                                          from c in _LibraryContext.CLG_Adm_College_AY_Course_BranchDMO
                                          where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                          select new ClgNonBookTransaction_DTO
                                          {
                                              AMB_Id = a.AMB_Id,
                                              AMB_BranchName = a.AMB_BranchName,
                                              AMB_BranchCode = a.AMB_BranchCode,
                                              AMB_BranchInfo = a.AMB_BranchInfo,
                                              AMB_BranchType = a.AMB_BranchType,
                                              AMB_Order = a.AMB_Order,
                                          }).Distinct().ToList();
                        data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

                        if (data.AMB_Id == 0)
                        {
                            data.AMB_Id = branchlist.FirstOrDefault().AMB_Id;
                        }

                        var semisterlist = (from a in _LibraryContext.CLG_Adm_Master_SemesterDMO
                                            from b in _LibraryContext.CLG_Adm_College_AY_CourseDMO
                                            from c in _LibraryContext.CLG_Adm_College_AY_Course_BranchDMO
                                            from d in _LibraryContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                            where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                            select new ClgNonBookTransaction_DTO
                                            {
                                                AMSE_Id = a.AMSE_Id,
                                                AMSE_SEMName = a.AMSE_SEMName,
                                                AMSE_SEMInfo = a.AMSE_SEMInfo,
                                                AMSE_SEMCode = a.AMSE_SEMCode,
                                                AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                                AMSE_SEMOrder = a.AMSE_SEMOrder,
                                                AMSE_Year = a.AMSE_Year,
                                                AMSE_EvenOdd = a.AMSE_EvenOdd
                                            }).Distinct().ToList();
                        data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

                        if (data.AMSE_Id == 0)
                        {
                            data.AMSE_Id = semisterlist.FirstOrDefault().AMSE_Id;
                        }


                        var sectionlist = (from a in _LibraryContext.Adm_College_Yearly_StudentDMO
                                           from b in _LibraryContext.Adm_College_Master_SectionDMO
                                           where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               ACMS_Id = b.ACMS_Id,
                                               ACMS_SectionName = b.ACMS_SectionName,
                                               ACMS_Order = b.ACMS_Order
                                           }).Distinct().ToList();

                        data.sectionlist = sectionlist.OrderBy(t => t.ACMS_Order).ToArray();

                        if (data.ACMS_Id == 0)
                        {
                            data.ACMS_Id = sectionlist.FirstOrDefault().ACMS_Id;
                        }

                        //////if (data.ASMCL_Id==0)
                        //////{
                        //////    data.ASMCL_Id = classlist.FirstOrDefault().ASMCL_Id;
                        //////}
                        //////if (data.ASMS_Id == 0)
                        //////{
                        //////    data.ASMS_Id = sectionlist.FirstOrDefault().ASMS_Id;
                        //////}

                        var studentlist = (from m in _LibraryContext.Adm_Master_College_StudentDMO
                                           from n in _LibraryContext.Adm_College_Yearly_StudentDMO
                                           where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               AMCST_Id = m.AMCST_Id,
                                               MI_Id = m.MI_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                               AMCST_MiddleName = m.AMCST_MiddleName,
                                               AMCST_LastName = m.AMCST_LastName,
                                               AMCST_AdmNo = m.AMCST_AdmNo

                                           }).Distinct().ToList();

                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.OrderBy(t => t.AMCST_FirstName).ToArray();
                            //data.studentCount = studentlist.Count;
                        }



                        using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LIB_GET_STUDENT_NONBOOK_TRANSACTION_COLLEGE";
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
                            cmd.Parameters.Add(new SqlParameter("@Type2",
                             SqlDbType.VarChar)
                            {
                                Value = "ALL"
                            });
                            cmd.Parameters.Add(new SqlParameter("@LMBANO_AccessionNo",
                             SqlDbType.VarChar)
                            {
                                Value = ""
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                             SqlDbType.BigInt)
                            {
                                Value = 0
                            });
                            cmd.Parameters.Add(new SqlParameter("@Book_Trans_Id",
                       SqlDbType.BigInt)
                            {
                                Value = data.Book_Trans_Id
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

                        ////////if (data.bookcat_type == "book")
                        ////////{
                        ////////    if (data.booktype == "nonbook")
                        ////////    {
                        ////////        //  get all related data for bind table grid in Html
                        ////////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////////                           from e in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                        ////////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////////                           from c in _LibraryContext.BookRegisterDMO
                        ////////                           from d in _LibraryContext.Adm_Master_College_StudentDMO
                        ////////                           from f in _LibraryContext.Adm_College_Yearly_StudentDMO
                        ////////                           from g in _LibraryContext.MasterCategoryDMO
                        ////////                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        ////////                           from i in _LibraryContext.Adm_School_M_ClassDMO
                        ////////                           from j in _LibraryContext.school_M_Section
                        ////////                           from k in _LibraryContext.LIB_Library_Class_DMO
                        ////////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                        ////////                           //&& a.LBTR_Status=="Issue"
                        ////////                           && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMCST_Id == d.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && a.MI_Id == i.MI_Id && i.ASMCL_Id == f.ASMCL_Id && i.ASMCL_ActiveFlag == true && j.MI_Id == a.MI_Id && j.ASMS_Id == f.ASMS_Id && j.ASMC_ActiveFlag == 1 && k.LMAL_Id == h.LMAL_Id && k.ASMCL_Id == i.ASMCL_Id && k.LLC_ActiveFlg == true)
                        ////////                           select new ClgNonBookTransaction_DTO
                        ////////                           {
                        ////////                               Book_Trans_Id = a.LBTR_Id,
                        ////////                               LMB_Id = c.LMB_Id,
                        ////////                               AMCST_Id = e.AMCST_Id,
                        ////////                               MI_Id = a.MI_Id,
                        ////////                               ASMAY_Id = data.ASMAY_Id,
                        ////////                               LMBANO_Id = b.LMBANO_Id,
                        ////////                               Issue_Date = a.LBTR_IssuedDate,
                        ////////                               Due_Date = a.LBTR_DueDate,
                        ////////                               Return_Date = a.LBTR_ReturnedDate,
                        ////////                               Book_Trans_Status = a.LBTR_Status,
                        ////////                               Fine_Amount = a.LBTR_TotalFine,
                        ////////                               //  Renewal_Counter = a.Renewal_Counter,
                        ////////                               Waived_Amount = a.LBTR_FineWaived,
                        ////////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////////                               AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                        ////////                               AMST_MiddleName = d.AMST_MiddleName,
                        ////////                               AMST_LastName = d.AMST_LastName,
                        ////////                               AMST_Photoname = d.AMST_Photoname,
                        ////////                               ASMCL_Id = f.ASMCL_Id,
                        ////////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                        ////////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////////                               ASMCL_ClassName = i.ASMCL_ClassName,
                        ////////                               ASMC_SectionName = j.ASMC_SectionName,
                        ////////                               ASMCL_Order = i.ASMCL_Order

                        ////////                           }).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        ////////    }
                        ////////    else
                        ////////    {
                        ////////        //  get all related data for bind table grid in Html
                        ////////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////////                           from e in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                        ////////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////////                           from c in _LibraryContext.BookRegisterDMO
                        ////////                           from d in _LibraryContext.Adm_Master_College_StudentDMO
                        ////////                           from f in _LibraryContext.Adm_College_Yearly_StudentDMO
                        ////////                           from g in _LibraryContext.MasterCategoryDMO
                        ////////                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        ////////                           from i in _LibraryContext.Adm_School_M_ClassDMO
                        ////////                           from j in _LibraryContext.school_M_Section
                        ////////                           from k in _LibraryContext.LIB_Library_Class_DMO
                        ////////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                        ////////                           //&& a.LBTR_Status=="Issue"
                        ////////                           && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMCST_Id == d.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Book" && c.LMB_BookType == typeofbook && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && a.MI_Id == i.MI_Id && i.ASMCL_Id == f.ASMCL_Id && i.ASMCL_ActiveFlag == true && j.MI_Id == a.MI_Id && j.ASMS_Id == f.ASMS_Id && j.ASMC_ActiveFlag == 1 && k.LMAL_Id == h.LMAL_Id && k.ASMCL_Id == i.ASMCL_Id && k.LLC_ActiveFlg == true)
                        ////////                           select new ClgNonBookTransaction_DTO
                        ////////                           {
                        ////////                               Book_Trans_Id = a.LBTR_Id,
                        ////////                               LMB_Id = c.LMB_Id,
                        ////////                               AMCST_Id = e.AMCST_Id,
                        ////////                               MI_Id = a.MI_Id,
                        ////////                               ASMAY_Id = data.ASMAY_Id,
                        ////////                               LMBANO_Id = b.LMBANO_Id,
                        ////////                               Issue_Date = a.LBTR_IssuedDate,
                        ////////                               Due_Date = a.LBTR_DueDate,
                        ////////                               Return_Date = a.LBTR_ReturnedDate,
                        ////////                               Book_Trans_Status = a.LBTR_Status,
                        ////////                               Fine_Amount = a.LBTR_TotalFine,
                        ////////                               //  Renewal_Counter = a.Renewal_Counter,
                        ////////                               Waived_Amount = a.LBTR_FineWaived,
                        ////////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////////                               AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                        ////////                               AMST_MiddleName = d.AMST_MiddleName,
                        ////////                               AMST_LastName = d.AMST_LastName,
                        ////////                               AMST_Photoname = d.AMST_Photoname,
                        ////////                               ASMCL_Id = f.ASMCL_Id,
                        ////////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                        ////////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////////                               ASMCL_ClassName = i.ASMCL_ClassName,
                        ////////                               ASMC_SectionName = j.ASMC_SectionName,
                        ////////                               ASMCL_Order = i.ASMCL_Order

                        ////////                           }).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        ////////    }
                        ////////}
                        ////////else
                        ////////{
                        ////////    if (data.booktype == "nonbook")
                        ////////    {
                        ////////        //  get all related data for bind table grid in Html
                        ////////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////////                           from e in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                        ////////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////////                           from c in _LibraryContext.BookRegisterDMO
                        ////////                           from d in _LibraryContext.Adm_Master_College_StudentDMO
                        ////////                           from f in _LibraryContext.Adm_College_Yearly_StudentDMO
                        ////////                           from g in _LibraryContext.MasterCategoryDMO
                        ////////                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        ////////                           from i in _LibraryContext.Adm_School_M_ClassDMO
                        ////////                           from j in _LibraryContext.school_M_Section
                        ////////                           from k in _LibraryContext.LIB_Library_Class_DMO
                        ////////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                        ////////                           //&& a.LBTR_Status=="Issue"
                        ////////                           && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMCST_Id == d.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && a.MI_Id == i.MI_Id && i.ASMCL_Id == f.ASMCL_Id && i.ASMCL_ActiveFlag == true && j.MI_Id == a.MI_Id && j.ASMS_Id == f.ASMS_Id && j.ASMC_ActiveFlag == 1 && k.LMAL_Id == h.LMAL_Id && k.ASMCL_Id == i.ASMCL_Id && k.LLC_ActiveFlg == true)
                        ////////                           select new ClgNonBookTransaction_DTO
                        ////////                           {
                        ////////                               Book_Trans_Id = a.LBTR_Id,
                        ////////                               LMB_Id = c.LMB_Id,
                        ////////                               AMCST_Id = e.AMCST_Id,
                        ////////                               MI_Id = a.MI_Id,
                        ////////                               ASMAY_Id = data.ASMAY_Id,
                        ////////                               LMBANO_Id = b.LMBANO_Id,
                        ////////                               Issue_Date = a.LBTR_IssuedDate,
                        ////////                               Due_Date = a.LBTR_DueDate,
                        ////////                               Return_Date = a.LBTR_ReturnedDate,
                        ////////                               Book_Trans_Status = a.LBTR_Status,
                        ////////                               Fine_Amount = a.LBTR_TotalFine,
                        ////////                               //  Renewal_Counter = a.Renewal_Counter,
                        ////////                               Waived_Amount = a.LBTR_FineWaived,
                        ////////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////////                               AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                        ////////                               AMST_MiddleName = d.AMST_MiddleName,
                        ////////                               AMST_LastName = d.AMST_LastName,
                        ////////                               AMST_Photoname = d.AMST_Photoname,
                        ////////                               ASMCL_Id = f.ASMCL_Id,
                        ////////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                        ////////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////////                               ASMCL_ClassName = i.ASMCL_ClassName,
                        ////////                               ASMC_SectionName = j.ASMC_SectionName,
                        ////////                               ASMCL_Order = i.ASMCL_Order

                        ////////                           }).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        ////////    }
                        ////////    else
                        ////////    {
                        ////////        //  get all related data for bind table grid in Html
                        ////////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////////                           from e in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                        ////////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////////                           from c in _LibraryContext.BookRegisterDMO
                        ////////                           from d in _LibraryContext.Adm_Master_College_StudentDMO
                        ////////                           from f in _LibraryContext.Adm_College_Yearly_StudentDMO
                        ////////                           from g in _LibraryContext.MasterCategoryDMO
                        ////////                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        ////////                           from i in _LibraryContext.Adm_School_M_ClassDMO
                        ////////                           from j in _LibraryContext.school_M_Section
                        ////////                           from k in _LibraryContext.LIB_Library_Class_DMO
                        ////////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == e.LBTR_Id
                        ////////                           //&& a.LBTR_Status=="Issue"
                        ////////                           && a.LBTR_ActiveFlg == true && e.LBTRS_ActiveFlg == true && f.AMCST_Id == d.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == g.MI_Id && c.LMC_Id == g.LMC_Id && g.LMC_BNBFlg == "Non-Book" && c.LMB_BookType == typeofbook && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && a.MI_Id == i.MI_Id && i.ASMCL_Id == f.ASMCL_Id && i.ASMCL_ActiveFlag == true && j.MI_Id == a.MI_Id && j.ASMS_Id == f.ASMS_Id && j.ASMC_ActiveFlag == 1 && k.LMAL_Id == h.LMAL_Id && k.ASMCL_Id == i.ASMCL_Id && k.LLC_ActiveFlg == true)
                        ////////                           select new ClgNonBookTransaction_DTO
                        ////////                           {
                        ////////                               Book_Trans_Id = a.LBTR_Id,
                        ////////                               LMB_Id = c.LMB_Id,
                        ////////                               AMCST_Id = e.AMCST_Id,
                        ////////                               MI_Id = a.MI_Id,
                        ////////                               ASMAY_Id = data.ASMAY_Id,
                        ////////                               LMBANO_Id = b.LMBANO_Id,
                        ////////                               Issue_Date = a.LBTR_IssuedDate,
                        ////////                               Due_Date = a.LBTR_DueDate,
                        ////////                               Return_Date = a.LBTR_ReturnedDate,
                        ////////                               Book_Trans_Status = a.LBTR_Status,
                        ////////                               Fine_Amount = a.LBTR_TotalFine,
                        ////////                               //  Renewal_Counter = a.Renewal_Counter,
                        ////////                               Waived_Amount = a.LBTR_FineWaived,
                        ////////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////////                               AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                        ////////                               AMST_MiddleName = d.AMST_MiddleName,
                        ////////                               AMST_LastName = d.AMST_LastName,
                        ////////                               AMST_Photoname = d.AMST_Photoname,
                        ////////                               ASMCL_Id = f.ASMCL_Id,
                        ////////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                        ////////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////////                               ASMCL_ClassName = i.ASMCL_ClassName,
                        ////////                               ASMC_SectionName = j.ASMC_SectionName,
                        ////////                               ASMCL_Order = i.ASMCL_Order

                        ////////                           }).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                        ////////    }
                        ////////}



                    }

                    else if (data.issuertype == "stf")
                    {
                        data.filldepartment = _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();
                        data.filldesignation = _LibraryContext.HR_Master_Designation.Where(a => a.MI_Id == data.MI_Id && a.HRMDES_ActiveFlag == true).Distinct().OrderBy(e => e.HRMDES_Order).ToArray();

                        data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               HRME_Id = a.HRME_Id,
                                               MI_Id = a.MI_Id,
                                               HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                               HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                               HRME_EmployeeLastName = a.HRME_EmployeeLastName
                                           }).Distinct().OrderBy(t => t.HRME_Id).ToArray();




                        //if (data.bookcat_type == "book")
                        //{
                        //    if (data.booktype == "nonbook")
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.MasterEmployee
                        //                           from f in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,
                        //                               HRME_Id = d.HRME_Id,
                        //                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                        //                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                        //                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                        //                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                        //                               HRMGT_Id = d.HRMGT_Id,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }
                        //    else
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.MasterEmployee
                        //                           from f in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,
                        //                               HRME_Id = d.HRME_Id,
                        //                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                        //                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                        //                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                        //                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                        //                               HRMGT_Id = d.HRMGT_Id,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }

                        //}
                        //else
                        //{
                        //if (data.booktype == "nonbook")
                        //{
                        //    //////get all related data for bind table grid in Html
                        //    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                        //                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                       from c in _LibraryContext.BookRegisterDMO
                        //                       from d in _LibraryContext.MasterEmployee
                        //                       from f in _LibraryContext.MasterCategoryDMO
                        //                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                       select new ClgNonBookTransaction_DTO
                        //                       {
                        //                           Book_Trans_Id = a.LBTR_Id,
                        //                           LMB_Id = c.LMB_Id,
                        //                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                           MI_Id = a.MI_Id,
                        //                           ////// ASMAY_Id = a.ASMAY_Id,
                        //                           LMBANO_Id = b.LMBANO_Id,
                        //                           ////// LMB_BookType = c.LMB_BookType,
                        //                           LMB_BookTitle = c.LMB_BookTitle,
                        //                           Issue_Date = a.LBTR_IssuedDate,
                        //                           Due_Date = a.LBTR_DueDate,
                        //                           Return_Date = a.LBTR_ReturnedDate,
                        //                           Book_Trans_Status = a.LBTR_Status,
                        //                           Fine_Amount = a.LBTR_TotalFine,
                        //                           ////// Renewal_Counter = a.Renewal_Counter,
                        //                           Waived_Amount = a.LBTR_FineWaived,
                        //                           HRME_Id = d.HRME_Id,
                        //                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                        //                           ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                        //                           //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                        //                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                        //                           HRMGT_Id = d.HRMGT_Id,
                        //                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                        //                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        //else
                        //{
                        //////get all related data for bind table grid in Html
                        data.getalldata = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                           from e in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                           from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                           from c in _LibraryContext.LIB_Master_NonBook_DMO
                                           from d in _LibraryContext.MasterEmployee
                                           from f in _LibraryContext.MasterCategoryDMO
                                           from h in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                           where (a.MI_Id == d.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && a.MI_Id == c.MI_Id && b.LMNBK_Id == c.LMNBK_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LNBTR_Id == a.LNBTR_Id && a.LNBTR_ActiveFlg == true && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && h.LMNBKL_ActiveFlg == true && c.LMNBK_Id == h.LMNBK_Id && h.LMAL_Id == data.LMAL_Id && a.LNBTR_Status== "Issue")
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               Book_Trans_Id = a.LNBTR_Id,
                                               LMB_Id = c.LMNBK_Id,
                                               LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                               MI_Id = a.MI_Id,
                                               ////// ASMAY_Id = a.ASMAY_Id,
                                               LMBANO_Id = b.LMNBKANO_Id,
                                               ////// LMB_BookType = c.LMB_BookType,
                                               LMB_BookTitle = c.LMNBK_NonBookTitle,
                                               Issue_Date = Convert.ToDateTime(a.LNBTR_IssuedDate),
                                               Due_Date = Convert.ToDateTime(a.LNBTR_DueDate),
                                               Return_Date = a.LNBTR_ReturnedDate,
                                               Book_Trans_Status = a.LNBTR_Status,
                                               Fine_Amount = a.LNBTR_TotalFine,
                                               ////// Renewal_Counter = a.Renewal_Counter,
                                               Waived_Amount = a.LNBTR_FineWaived,
                                               HRME_Id = d.HRME_Id,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                                               HRMGT_Id = d.HRMGT_Id,
                                               LBTR_Renewalcounter = a.LNBTR_RenewalCount,



                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        // }




                    }

                    else if (data.issuertype == "dep")
                    {
                        data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                               select new ClgNonBookTransaction_DTO
                                               {
                                                   HRMD_Id = a.HRMD_Id,
                                                   HRMD_DepartmentName = a.HRMD_DepartmentName,
                                                   HRMD_Order = a.HRMD_Order,

                                               }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


                        //if (data.bookcat_type == "book")
                        //{
                        //    if (data.booktype == "nonbook")
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.HR_Master_Department
                        //                           from f in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,
                        //                               HRMD_Id = d.HRMD_Id,
                        //                               HRMD_DepartmentName = d.HRMD_DepartmentName,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                        //                               ////// LMD_DepartmentCode = d.LMD_DepartmentCode,

                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }
                        //    else
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.HR_Master_Department
                        //                           from f in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,
                        //                               HRMD_Id = d.HRMD_Id,
                        //                               HRMD_DepartmentName = d.HRMD_DepartmentName,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                        //                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }
                        //}
                        //else
                        //{
                        //if (data.booktype == "nonbook")
                        //{
                        //    //////get all related data for bind table grid in Html
                        //    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                       from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                        //                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                       from c in _LibraryContext.BookRegisterDMO
                        //                       from d in _LibraryContext.HR_Master_Department
                        //                       from f in _LibraryContext.MasterCategoryDMO
                        //                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                       select new ClgNonBookTransaction_DTO
                        //                       {
                        //                           Book_Trans_Id = a.LBTR_Id,
                        //                           LMB_Id = c.LMB_Id,
                        //                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                           MI_Id = a.MI_Id,
                        //                           ////// ASMAY_Id = a.ASMAY_Id,
                        //                           LMBANO_Id = b.LMBANO_Id,
                        //                           //////// lmb_booktype = c.lmb_booktype,
                        //                           LMB_BookTitle = c.LMB_BookTitle,
                        //                           Issue_Date = a.LBTR_IssuedDate,
                        //                           Due_Date = a.LBTR_DueDate,
                        //                           Return_Date = a.LBTR_ReturnedDate,
                        //                           Book_Trans_Status = a.LBTR_Status,
                        //                           Fine_Amount = a.LBTR_TotalFine,
                        //                           ////// Renewal_Counter = a.Renewal_Counter,
                        //                           Waived_Amount = a.LBTR_FineWaived,
                        //                           HRMD_Id = d.HRMD_Id,
                        //                           HRMD_DepartmentName = d.HRMD_DepartmentName,
                        //                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,
                        //                           //////LMD_DepartmentCode = d.LMD_DepartmentCode,


                        //                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        //else
                        //{
                        //////get all related data for bind table grid in Html
                        data.getalldata = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                           from e in _LibraryContext.LIB_NonBook_Transaction_Department_DMO
                                           from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                           from c in _LibraryContext.LIB_Master_NonBook_DMO
                                           from d in _LibraryContext.HR_Master_Department
                                           from f in _LibraryContext.MasterCategoryDMO
                                           from h in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                           where (a.MI_Id == c.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && a.MI_Id == c.MI_Id && b.LMNBK_Id == c.LMNBK_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LNBTR_Id == a.LNBTR_Id && a.LNBTR_ActiveFlg == true && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LNBTR_Status == "Issue" && h.LMNBKL_ActiveFlg == true && c.LMNBK_Id == h.LMNBK_Id && h.LMAL_Id == data.LMAL_Id)
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               Book_Trans_Id = a.LNBTR_Id,
                                               LMB_Id = c.LMNBK_Id,
                                               LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                               MI_Id = a.MI_Id,
                                               ////// ASMAY_Id = a.ASMAY_Id,
                                               LMBANO_Id = b.LMNBKANO_Id,
                                               ////// LMB_BookType = c.LMB_BookType,
                                               LMB_BookTitle = c.LMNBK_NonBookTitle,
                                               Issue_Date = a.LNBTR_IssuedDate,
                                               Due_Date = a.LNBTR_DueDate,
                                               Return_Date = a.LNBTR_ReturnedDate,
                                               Book_Trans_Status = a.LNBTR_Status,
                                               Fine_Amount = a.LNBTR_TotalFine,
                                               ////// Renewal_Counter = a.Renewal_Counter,
                                               Waived_Amount = a.LNBTR_FineWaived,
                                               HRMD_Id = d.HRMD_Id,
                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
                                               LBTR_Renewalcounter = a.LNBTR_RenewalCount,


                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        // }






                    }

                    else if (data.issuertype == "gst")
                    {


                        //if (data.bookcat_type == "book")
                        //{
                        //    if (data.booktype == "nonbook")
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,

                        //                               LBTR_GuestName = a.LBTR_GuestName,

                        //                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                        //                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }
                        //    else
                        //    {
                        //        //////get all related data for bind table grid in Html
                        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                           from c in _LibraryContext.BookRegisterDMO
                        //                           from d in _LibraryContext.MasterCategoryDMO
                        //                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                           select new ClgNonBookTransaction_DTO
                        //                           {
                        //                               Book_Trans_Id = a.LBTR_Id,
                        //                               LMB_Id = c.LMB_Id,
                        //                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                               MI_Id = a.MI_Id,
                        //                               ////// ASMAY_Id = a.ASMAY_Id,
                        //                               LMBANO_Id = b.LMBANO_Id,
                        //                               ////// LMB_BookType = c.LMB_BookType,
                        //                               LMB_BookTitle = c.LMB_BookTitle,
                        //                               Issue_Date = a.LBTR_IssuedDate,
                        //                               Due_Date = a.LBTR_DueDate,
                        //                               Return_Date = a.LBTR_ReturnedDate,
                        //                               Book_Trans_Status = a.LBTR_Status,
                        //                               Fine_Amount = a.LBTR_TotalFine,
                        //                               ////// Renewal_Counter = a.Renewal_Counter,
                        //                               Waived_Amount = a.LBTR_FineWaived,

                        //                               LBTR_GuestName = a.LBTR_GuestName,

                        //                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                        //                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                        //                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //    }

                        //}
                        //else
                        //{
                        //if (data.booktype == "nonbook")
                        //{
                        //    //////get all related data for bind table grid in Html
                        //    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        //                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        //                       from c in _LibraryContext.BookRegisterDMO
                        //                       from d in _LibraryContext.MasterCategoryDMO
                        //                       from h in _LibraryContext.LIB_Master_Book_Library_DMO
                        //                       where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
                        //                       select new ClgNonBookTransaction_DTO
                        //                       {
                        //                           Book_Trans_Id = a.LBTR_Id,
                        //                           LMB_Id = c.LMB_Id,
                        //                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        //                           MI_Id = a.MI_Id,
                        //                           ////// ASMAY_Id = a.ASMAY_Id,
                        //                           LMBANO_Id = b.LMBANO_Id,
                        //                           ////// LMB_BookType = c.LMB_BookType,
                        //                           LMB_BookTitle = c.LMB_BookTitle,
                        //                           Issue_Date = a.LBTR_IssuedDate,
                        //                           Due_Date = a.LBTR_DueDate,
                        //                           Return_Date = a.LBTR_ReturnedDate,
                        //                           Book_Trans_Status = a.LBTR_Status,
                        //                           Fine_Amount = a.LBTR_TotalFine,
                        //                           //////// Renewal_Counter = a.Renewal_Counter,
                        //                           Waived_Amount = a.LBTR_FineWaived,

                        //                           LBTR_GuestName = a.LBTR_GuestName,

                        //                           LBTR_GuestContactNo = a.LBTR_GuestContactNo,
                        //                           LBTR_GuestEmailId = a.LBTR_GuestEmailId,
                        //                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,

                        //                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        //else
                        //{
                        //////get all related data for bind table grid in Html
                        data.getalldata = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                           from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                           from c in _LibraryContext.LIB_Master_NonBook_DMO
                                           from d in _LibraryContext.MasterCategoryDMO
                                           from h in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                           where (a.MI_Id == c.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && a.MI_Id == c.MI_Id && b.LMNBK_Id == c.LMNBK_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true  && a.LNBTR_GuestName != null && a.LNBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LNBTR_Status == "Issue" && h.LMNBKL_ActiveFlg == true && c.LMNBK_Id == h.LMNBK_Id && h.LMAL_Id == data.LMAL_Id)
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               Book_Trans_Id = a.LNBTR_Id,
                                               LMB_Id = c.LMNBK_Id,
                                               LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                               MI_Id = a.MI_Id,
                                               ////// ASMAY_Id = a.ASMAY_Id,
                                               LMBANO_Id = b.LMNBKANO_Id,
                                               //////// LMB_BookType = c.LMB_BookType,
                                               LMB_BookTitle = c.LMNBK_NonBookTitle,
                                               Issue_Date = a.LNBTR_IssuedDate,
                                               Due_Date = a.LNBTR_DueDate,
                                               Return_Date = a.LNBTR_ReturnedDate,
                                               Book_Trans_Status = a.LNBTR_Status,
                                               Fine_Amount = a.LNBTR_TotalFine,
                                               ////// Renewal_Counter = a.Renewal_Counter,
                                               Waived_Amount = a.LNBTR_FineWaived,

                                               LBTR_GuestName = a.LNBTR_GuestName,

                                               LBTR_GuestContactNo = a.LNBTR_GuestContactNo,
                                               LBTR_GuestEmailId = a.LNBTR_GuestEmailId,
                                               LBTR_Renewalcounter = a.LNBTR_RenewalCount,

                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        //}
                        //}





                    }





                    var aaa = _LibraryContext.LIB_NonBook_Transaction_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                    List<long> LMBANO_Id = new List<long>();
                    if (aaa.Count() > 0)
                    {
                        foreach (var it in aaa)
                        {
                            if (it.LNBTR_Status == "Issue")
                            {
                                LMBANO_Id.Add(it.LMNBKANO_Id);
                            }


                        }
                    }

                    //                if (data.bookcat_type == "book")
                    //                {
                    //                    if (data.booktype == "issue")
                    //                    {
                    //                        ////////related LMBANO_Id all data comming from Maser book Table
                    //                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                                          from c in _LibraryContext.MasterCategoryDMO
                    //                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && d.LMB_Id == a.LMB_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMBL_ActiveFlg == true)
                    //                                          select new ClgNonBookTransaction_DTO
                    //                                          {

                    //                                              LMB_Id = a.LMB_Id,
                    //                                              MI_Id = a.MI_Id,
                    //                                              ////// LMBA_Id = a.LMBA_Id,
                    //                                              LMB_ClassNo = a.LMB_ClassNo,
                    //                                              LMB_BookTitle = a.LMB_BookTitle,
                    //                                              //////LMS_Id = a.LMS_Id,
                    //                                              //////LMD_Id = a.LMD_Id,
                    //                                              ////// LMP_Id = a.LMP_Id,
                    //                                              LMB_Price = a.LMB_Price,
                    //                                              LMB_VolNo = a.LMB_VolNo,
                    //                                              LMB_EntryDate = a.LMB_EntryDate,
                    //                                              ////// LML_Id = a.LML_Id,
                    //                                              ////// Donor_Id = a.Donor_Id,
                    //                                              //////  LMV_Id = a.LMV_Id,
                    //                                              LMC_Id = a.LMC_Id,
                    //                                              LMBANO_Id = b.LMBANO_Id,
                    //                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //                                              //////  Rack_Id =b.Rack_Id,


                    //                                          }).Distinct().Take(15).ToArray();




                    //                    }
                    //                    else if (data.booktype == "ref")
                    //                    {
                    //                        //////related LMBANO_Id all data comming from Maser book Table
                    //                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                                          from c in _LibraryContext.MasterCategoryDMO
                    //                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && d.LMB_Id == a.LMB_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMBL_ActiveFlg == true)
                    //                                          select new ClgNonBookTransaction_DTO
                    //                                          {
                    //                                              LMB_Id = a.LMB_Id,
                    //                                              MI_Id = a.MI_Id,
                    //                                              ////// LMBA_Id = a.LMBA_Id,
                    //                                              LMB_ClassNo = a.LMB_ClassNo,
                    //                                              LMB_BookTitle = a.LMB_BookTitle,
                    //                                              //////LMS_Id = a.LMS_Id,
                    //                                              //////LMD_Id = a.LMD_Id,
                    //                                              ////// LMP_Id = a.LMP_Id,
                    //                                              LMB_Price = a.LMB_Price,
                    //                                              LMB_VolNo = a.LMB_VolNo,
                    //                                              LMB_EntryDate = a.LMB_EntryDate,
                    //                                              ////// LML_Id = a.LML_Id,
                    //                                              ////// Donor_Id = a.Donor_Id,
                    //                                              //////  LMV_Id = a.LMV_Id,
                    //                                              LMC_Id = a.LMC_Id,
                    //                                              LMBANO_Id = b.LMBANO_Id,
                    //                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //                                              //////  Rack_Id =b.Rack_Id,

                    //                                          }).Distinct().Take(15).ToArray();
                    //                    }
                    //                    else if (data.booktype == "nonbook")
                    //                    {
                    //                        //////related LMBANO_Id all data comming from Maser book Table
                    //                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                                          from c in _LibraryContext.MasterCategoryDMO
                    //                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType != "Issue" && a.LMB_BookType != "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && d.LMB_Id == a.LMB_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMBL_ActiveFlg == true
                    //)
                    //                                          select new ClgNonBookTransaction_DTO
                    //                                          {

                    //                                              LMB_Id = a.LMB_Id,
                    //                                              MI_Id = a.MI_Id,
                    //                                              ////// LMBA_Id = a.LMBA_Id,
                    //                                              LMB_ClassNo = a.LMB_ClassNo,
                    //                                              LMB_BookTitle = a.LMB_BookTitle,
                    //                                              //////LMS_Id = a.LMS_Id,
                    //                                              //////LMD_Id = a.LMD_Id,
                    //                                              ////// LMP_Id = a.LMP_Id,
                    //                                              LMB_Price = a.LMB_Price,
                    //                                              LMB_VolNo = a.LMB_VolNo,
                    //                                              LMB_EntryDate = a.LMB_EntryDate,
                    //                                              ////// LML_Id = a.LML_Id,
                    //                                              ////// Donor_Id = a.Donor_Id,
                    //                                              //////  LMV_Id = a.LMV_Id,
                    //                                              LMC_Id = a.LMC_Id,
                    //                                              LMBANO_Id = b.LMBANO_Id,
                    //                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //                                              //////  Rack_Id =b.Rack_Id,


                    //                                          }).Distinct().Take(15).ToArray();
                    //                    }
                    //                }
                    //                else
                    //                {
                    //if (data.booktype == "issue")
                    //{
                    //    //////related LMBANO_Id all data comming from Maser book Table
                    //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                      from c in _LibraryContext.MasterCategoryDMO
                    //                      from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && d.LMB_Id == a.LMB_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMBL_ActiveFlg == true)
                    //                      select new ClgNonBookTransaction_DTO
                    //                      {

                    //                          LMB_Id = a.LMB_Id,
                    //                          MI_Id = a.MI_Id,
                    //                          // LMBA_Id = a.LMBA_Id,
                    //                          LMB_ClassNo = a.LMB_ClassNo,
                    //                          LMB_BookTitle = a.LMB_BookTitle,
                    //                          //////LMS_Id = a.LMS_Id,
                    //                          //////LMD_Id = a.LMD_Id,
                    //                          ////// LMP_Id = a.LMP_Id,
                    //                          LMB_Price = a.LMB_Price,
                    //                          LMB_VolNo = a.LMB_VolNo,
                    //                          LMB_EntryDate = a.LMB_EntryDate,
                    //                          ////// LML_Id = a.LML_Id,
                    //                          ////// Donor_Id = a.Donor_Id,
                    //                          //////  LMV_Id = a.LMV_Id,
                    //                          LMC_Id = a.LMC_Id,
                    //                          LMBANO_Id = b.LMBANO_Id,
                    //                          LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //                          //////  Rack_Id =b.Rack_Id,


                    //                      }).Distinct().Take(15).ToArray();



                    //}
                    //else if (data.booktype == "ref")
                    //{
                    //    //////related LMBANO_Id all data comming from Maser book Table
                    //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                      from c in _LibraryContext.MasterCategoryDMO
                    //                      from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && d.LMB_Id == a.LMB_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMBL_ActiveFlg == true)
                    //                      select new ClgNonBookTransaction_DTO
                    //                      {
                    //                          LMB_Id = a.LMB_Id,
                    //                          MI_Id = a.MI_Id,
                    //                          ////// LMBA_Id = a.LMBA_Id,
                    //                          LMB_ClassNo = a.LMB_ClassNo,
                    //                          LMB_BookTitle = a.LMB_BookTitle,
                    //                          //////LMS_Id = a.LMS_Id,
                    //                          //////LMD_Id = a.LMD_Id,
                    //                          ////// LMP_Id = a.LMP_Id,
                    //                          LMB_Price = a.LMB_Price,
                    //                          LMB_VolNo = a.LMB_VolNo,
                    //                          LMB_EntryDate = a.LMB_EntryDate,
                    //                          ////// LML_Id = a.LML_Id,
                    //                          ////// Donor_Id = a.Donor_Id,
                    //                          //////  LMV_Id = a.LMV_Id,
                    //                          LMC_Id = a.LMC_Id,
                    //                          LMBANO_Id = b.LMBANO_Id,
                    //                          LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //                          //////  Rack_Id =b.Rack_Id,

                    //                      }).Distinct().Take(15).ToArray();
                    //}
                    //else if (data.booktype == "nonbook")
                    //{
                    //////related LMBANO_Id all data comming from Maser book Table
                    data.booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                      from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                      from c in _LibraryContext.MasterCategoryDMO
                                      from d in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                      where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMNBKANO_Id) && a.LMNBK_ActiveFlg == true && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && d.LMNBK_Id == a.LMNBK_Id && d.LMAL_Id == liblist.FirstOrDefault().LMAL_Id && d.LMNBKL_ActiveFlg == true)
                                      select new ClgNonBookTransaction_DTO
                                      {

                                          LMB_Id = a.LMNBK_Id,
                                          MI_Id = a.MI_Id,
                                          ////// LMBA_Id = a.LMBA_Id,
                                          LMB_ClassNo = a.LMNBK_ISBN,
                                          LMB_BookTitle = a.LMNBK_NonBookTitle,
                                          //////LMS_Id = a.LMS_Id,
                                          //////LMD_Id = a.LMD_Id,
                                          ////// LMP_Id = a.LMP_Id,
                                          LMB_Price = a.LMNBK_Price,
                                          LMB_VolNo = a.LMNBK_VolumeNo,
                                          LMB_EntryDate = a.LMNBK_PublishDate,
                                          ////// LML_Id = a.LML_Id,
                                          ////// Donor_Id = a.Donor_Id,
                                          //////  LMV_Id = a.LMV_Id,
                                          LMC_Id = a.LMC_Id,
                                          LMBANO_Id = b.LMNBKANO_Id,
                                          LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                          //////  Rack_Id =b.Rack_Id,


                                      }).Distinct().Take(15).ToArray();
                    // }
                    //}


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

        public ClgNonBookTransaction_DTO GetStudentDetails1(ClgNonBookTransaction_DTO data)
        {
            try
            {
                var branchlist = (from a in _LibraryContext.ClgMasterBranchDMO
                                  from b in _LibraryContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _LibraryContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgNonBookTransaction_DTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_Order = a.AMB_Order,
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

                if (data.AMB_Id == 0)
                {
                    if (branchlist.Count > 0)
                    {
                        data.AMB_Id = branchlist.FirstOrDefault().AMB_Id;
                    }

                }

                var semisterlist = (from a in _LibraryContext.CLG_Adm_Master_SemesterDMO
                                    from b in _LibraryContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _LibraryContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _LibraryContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new ClgNonBookTransaction_DTO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

                if (data.AMSE_Id == 0)
                {
                    if (semisterlist.Count > 0)
                    {
                        data.AMSE_Id = semisterlist.FirstOrDefault().AMSE_Id;
                    }

                }


                var sectionlist = (from a in _LibraryContext.Adm_College_Yearly_StudentDMO
                                   from b in _LibraryContext.Adm_College_Master_SectionDMO
                                   where a.ASMAY_Id == data.ASMAY_Id && b.ACMS_Id == a.ACMS_Id && b.MI_Id == data.MI_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMSE_Id == data.AMSE_Id
                                   select new ClgNonBookTransaction_DTO
                                   {
                                       ACMS_Id = b.ACMS_Id,
                                       ACMS_SectionName = b.ACMS_SectionName,
                                       ACMS_Order = b.ACMS_Order
                                   }).Distinct().ToList();

                data.sectionlist = sectionlist.OrderBy(t => t.ACMS_Order).ToArray();

                if (data.ACMS_Id == 0)
                {
                    if (sectionlist.Count > 0)
                    {
                        data.ACMS_Id = sectionlist.FirstOrDefault().ACMS_Id;
                    }

                }


                var studentlist = (from m in _LibraryContext.Adm_Master_College_StudentDMO
                                   from n in _LibraryContext.Adm_College_Yearly_StudentDMO
                                   where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 && n.AMCO_Id == data.AMCO_Id && n.AMB_Id == data.AMB_Id && n.AMSE_Id == data.AMSE_Id && n.ACMS_Id == data.ACMS_Id
                                   select new ClgNonBookTransaction_DTO
                                   {
                                       AMCST_Id = m.AMCST_Id,
                                       MI_Id = m.MI_Id,
                                       ASMAY_Id = m.ASMAY_Id,
                                       AMCST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                       AMCST_MiddleName = m.AMCST_MiddleName,
                                       AMCST_LastName = m.AMCST_LastName,
                                       AMCST_AdmNo = m.AMCST_AdmNo

                                   }).Distinct().ToList();

                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                   // data.studentCount = studentlist.Count;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public ClgNonBookTransaction_DTO Savedata(ClgNonBookTransaction_DTO data)
        {
            try
            {
                var mxcnt = 0;
                //var typeofbook = "";
                //if (data.booktype == "Issue")
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


                if (data.issuertype == "std")
                {
                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    var maxitem = (from a in _LibraryContext.BookTransactionDMO
                    ////                   from b in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                    ////                   from c in _LibraryContext.BookRegisterDMO
                    ////                   from d in _LibraryContext.Lib_M_Book_Accn_DMO
                    ////                   where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRS_ActiveFlg == true && b.AMCST_Id == data.AMCST_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                    ////                   select new ClgNonBookTransaction_DTO
                    ////                   {
                    ////                       LMBANO_Id = a.LMBANO_Id
                    ////                   }).ToList();
                    ////    mxcnt = maxitem.Count;
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                       from b in _LibraryContext.LIB_NonBook_Transaction_Student_College_DMO
                                       from c in _LibraryContext.LIB_Master_NonBook_DMO
                                       from d in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                       where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRS_ActiveFlg == true && b.AMCST_Id == data.AMCST_Id && a.LNBTR_Status == "Issue" && c.LMNBK_Id == d.LMNBK_Id && c.LMC_Id == cat_id
                                       && a.LMNBKANO_Id == d.LMNBKANO_Id
                                       select new ClgNonBookTransaction_DTO
                                       {
                                           LMBANO_Id = a.LMNBKANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                   //// }



                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                             from b in _LibraryContext.LIB_NonBook_Transaction_Student_College_DMO
                                             where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRS_ActiveFlg == true && a.LMNBKANO_Id == data.LMBANO_Id && b.AMCST_Id == data.AMCST_Id && a.LNBTR_Status == "Issue"
                                             select new ClgNonBookTransaction_DTO
                                             {
                                                 LMBANO_Id = a.LMNBKANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                LIB_NonBook_Transaction_DMO obj = new LIB_NonBook_Transaction_DMO();

                                obj.MI_Id = data.MI_Id;
                                //////obj.ASMAY_Id = data.ASMAY_Id;
                                ////// obj.AMST_Id = data.AMST_Id;
                                obj.LMNBKANO_Id = data.LMBANO_Id;
                                obj.LNBTR_IssuedDate = Convert.ToDateTime(data.Issue_Date);
                                obj.LNBTR_DueDate = Convert.ToDateTime(data.Due_Date);
                                obj.LNBTR_ReturnedDate = data.Return_Date;
                                obj.LNBTR_FineCollecetd = 0;
                                obj.LNBTR_FineWaived = 0;
                                obj.LNBTR_RenewalCount = 0;
                                obj.LNBTR_Status = "Issue";
                                //////obj.FODM_Id = 0;
                                //////obj.Guest_Id = 0;
                                ////// obj.HRME_Id = 0;
                                //////   obj.LMD_Id = 0;
                                obj.LNBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedBy = data.IVRMUL_Id;
                                obj.UpdatedBy = data.IVRMUL_Id;
                                _LibraryContext.Add(obj);

                                LIB_NonBook_Transaction_Student_College_DMO obj1 = new LIB_NonBook_Transaction_Student_College_DMO();
                                obj1.LNBTR_Id = obj.LNBTR_Id;
                                obj1.AMCST_Id = data.AMCST_Id;
                                obj1.LNBTRS_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);


                                int rowAffected = _LibraryContext.SaveChanges();
                                if (rowAffected > 0)
                                {
                                    long mobileno = 0;
                                    //////var mobilenolist = (from a in _db.Adm_Master_College_StudentDMO
                                    //////                    from b in _db.Adm_M_Student_MobileNo
                                    //////                    where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id
                                    //////                    select b).ToList();

                                    //////if (mobilenolist.Count > 0)
                                    //////{
                                    //////    if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                                    //////    {
                                    //////        mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                                    //////    }

                                    //////}
                                    //////else
                                    //////{
                                    var mobilenolist1 = (from a in _db.Adm_Master_College_StudentDMO
                                                         where a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                                                         select new CLGBookTransactionDTO
                                                         {
                                                             AMCST_MobileNo = a.AMCST_MobileNo
                                                         }).ToList();

                                    if (mobilenolist1.Count > 0)
                                    {
                                        if (mobilenolist1[0].AMCST_MobileNo != 0 && mobilenolist1[0].AMCST_MobileNo != null)
                                        {
                                            mobileno = Convert.ToInt64(mobilenolist1[0].AMCST_MobileNo);
                                        }

                                    }
                                    //////  }
                                    //////  mobileno = 9591081840;
                                    if (mobileno != 0)
                                    {
                                        SMS sms = new SMS(_db);
                                        //string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKISSUE", data.ASMAY_Id, data.AMCST_Id, data.LMBANO_Id).Result;
                                    }

                                    string email_id = "";
                                    //////var emailidlist = (from a in _db.Adm_Master_College_StudentDMO
                                    //////                   from b in _db.Adm_M_Student_Email_Id
                                    //////                   where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id
                                    //////                   select b).ToList();

                                    //////if (emailidlist.Count > 0)
                                    //////{
                                    //////    if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                                    //////    {
                                    //////        email_id = emailidlist[0].AMSTE_EmailId;
                                    //////    }

                                    //////}
                                    //////else
                                    //////{
                                    var emailidlist1 = (from a in _db.Adm_Master_College_StudentDMO
                                                        where a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                                                        select new CLGBookTransactionDTO
                                                        {
                                                            AMCST_emailId = a.AMCST_emailId
                                                        }).ToList();

                                    if (emailidlist1.Count > 0)
                                    {
                                        if (emailidlist1[0].AMCST_emailId != "")
                                        {
                                            email_id = emailidlist1[0].AMCST_emailId;
                                        }

                                    }
                                    ////// }
                                    ////// email_id = "praveenishwar@vapstech.com";
                                    if (email_id != "" && email_id != null)
                                    {
                                        Email email = new Email(_db);
                                        //string s = email.sendmail_library(data.MI_Id, email_id, "BOOKISSUE", data.ASMAY_Id, data.AMCST_Id, data.LMBANO_Id);
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

                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    var maxitem = (from a in _LibraryContext.BookTransactionDMO
                    ////                   from b in _LibraryContext.LIB_Book_Transaction_StaffDMO
                    ////                   from c in _LibraryContext.BookRegisterDMO
                    ////                   from d in _LibraryContext.Lib_M_Book_Accn_DMO
                    ////                   where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRST_ActiveFlg == true && b.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                    ////                   select new ClgNonBookTransaction_DTO
                    ////                   {
                    ////                       LMBANO_Id = a.LMBANO_Id
                    ////                   }).ToList();
                    ////    mxcnt = maxitem.Count;
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                       from b in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                       from c in _LibraryContext.LIB_Master_NonBook_DMO
                                       from d in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                       where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRST_ActiveFlg == true && b.HRME_Id == data.HRME_Id && a.LNBTR_Status == "Issue" && c.LMNBK_Id == d.LMNBK_Id && c.LMC_Id == cat_id
                                       && a.LMNBKANO_Id == d.LMNBKANO_Id
                                       select new ClgNonBookTransaction_DTO
                                       {
                                           LMBANO_Id = a.LMNBKANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                   //// }



                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                             from b in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                             where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRST_ActiveFlg == true && a.LMNBKANO_Id == data.LMBANO_Id && b.HRME_Id == data.HRME_Id && a.LNBTR_Status == "Issue"
                                             select new ClgNonBookTransaction_DTO
                                             {
                                                 LMBANO_Id = a.LMNBKANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                LIB_NonBook_Transaction_DMO obj = new LIB_NonBook_Transaction_DMO();

                                obj.MI_Id = data.MI_Id;
                                //////obj.ASMAY_Id = data.ASMAY_Id;
                                ////// obj.AMST_Id = data.AMST_Id;
                                obj.LMNBKANO_Id = data.LMBANO_Id;
                                obj.LNBTR_IssuedDate = Convert.ToDateTime(data.Issue_Date);
                                obj.LNBTR_DueDate = Convert.ToDateTime(data.Due_Date);
                                obj.LNBTR_ReturnedDate = data.Return_Date;
                                obj.LNBTR_FineCollecetd = 0;
                                obj.LNBTR_FineWaived = 0;
                                obj.LNBTR_RenewalCount = 0;
                                obj.LNBTR_Status = "Issue";
                                //////obj.FODM_Id = 0;
                                //////obj.Guest_Id = 0;
                                ////// obj.HRME_Id = 0;
                                //////   obj.LMD_Id = 0;
                                obj.LNBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedBy = data.IVRMUL_Id;
                                obj.UpdatedBy = data.IVRMUL_Id;                                

                                _LibraryContext.Add(obj);
                                ////// int rowAffected1 = _LibraryContext.SaveChanges();
                                LIB_NonBook_Transaction_Staff_DMO obj1 = new LIB_NonBook_Transaction_Staff_DMO();
                                obj1.LNBTR_Id = obj.LNBTR_Id;
                                obj1.HRME_Id = data.HRME_Id;
                                obj1.LNBTRST_ActiveFlg = true;
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

                else if (data.issuertype == "dep")
                {

                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    var maxitem = (from a in _LibraryContext.BookTransactionDMO
                    ////                   from b in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
                    ////                   from c in _LibraryContext.BookRegisterDMO
                    ////                   from d in _LibraryContext.Lib_M_Book_Accn_DMO
                    ////                   where a.LBTR_Id == b.LBTR_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && b.LBTRD_ActiveFlg == true && b.HRMD_Id == data.HRMD_Id && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LMBANO_Id == d.LMBANO_Id
                    ////                   select new ClgNonBookTransaction_DTO
                    ////                   {
                    ////                       LMBANO_Id = a.LMBANO_Id
                    ////                   }).ToList();
                    ////    mxcnt = maxitem.Count;
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                       from b in _LibraryContext.LIB_NonBook_Transaction_Department_DMO
                                       from c in _LibraryContext.LIB_Master_NonBook_DMO
                                       from d in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                       where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRST_ActiveFlg == true && b.HRMD_Id == data.HRMD_Id && a.LNBTR_Status == "Issue" && c.LMNBK_Id == d.LMNBK_Id && c.LMC_Id == cat_id && a.LMNBKANO_Id == d.LMNBKANO_Id
                                       select new ClgNonBookTransaction_DTO
                                       {
                                           LMBANO_Id = a.LMNBKANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                   //// }

                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }
                        else
                        {
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                             from b in _LibraryContext.LIB_NonBook_Transaction_Department_DMO
                                             where a.LNBTR_Id == b.LNBTR_Id && a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && b.LNBTRST_ActiveFlg == true && a.LMNBKANO_Id == data.LMBANO_Id && b.HRMD_Id == data.HRMD_Id && a.LNBTR_Status == "Issue"
                                             select new ClgNonBookTransaction_DTO
                                             {
                                                 LMBANO_Id = a.LMNBKANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                LIB_NonBook_Transaction_DMO obj = new LIB_NonBook_Transaction_DMO();

                                obj.MI_Id = data.MI_Id;
                                //////obj.ASMAY_Id = data.ASMAY_Id;
                                ////// obj.AMST_Id = data.AMST_Id;
                                obj.LMNBKANO_Id = data.LMBANO_Id;
                                obj.LNBTR_IssuedDate = Convert.ToDateTime(data.Issue_Date);
                                obj.LNBTR_DueDate = Convert.ToDateTime(data.Due_Date);
                                obj.LNBTR_ReturnedDate = data.Return_Date;
                                obj.LNBTR_FineCollecetd = 0;
                                obj.LNBTR_FineWaived = 0;
                                obj.LNBTR_RenewalCount = 0;
                                obj.LNBTR_Status = "Issue";
                                //////obj.FODM_Id = 0;
                                //////obj.Guest_Id = 0;
                                ////// obj.HRME_Id = 0;
                                //////   obj.LMD_Id = 0;
                                obj.LNBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedBy = data.IVRMUL_Id;
                                obj.UpdatedBy = data.IVRMUL_Id;

                                _LibraryContext.Add(obj);
                                ////// int rowAffected1 = _LibraryContext.SaveChanges();
                                LIB_NonBook_Transaction_Department_DMO obj1 = new LIB_NonBook_Transaction_Department_DMO();
                                obj1.LNBTR_Id = obj.LNBTR_Id;
                                obj1.HRMD_Id = data.HRMD_Id;
                                obj1.LNBTRST_ActiveFlg = true;
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


                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    var maxitem = (from a in _LibraryContext.BookTransactionDMO
                    ////                   from c in _LibraryContext.BookRegisterDMO
                    ////                   from d in _LibraryContext.Lib_M_Book_Accn_DMO
                    ////                   where a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && a.LBTR_Status == "Issue" && c.LMB_Id == d.LMB_Id && c.LMB_BookType == typeofbook && a.LBTR_GuestName == data.LBTR_GuestName && a.LBTR_GuestContactNo == data.LBTR_GuestContactNo && a.LBTR_GuestEmailId == data.LBTR_GuestEmailId && a.LMBANO_Id == d.LMBANO_Id
                    ////                   select new ClgNonBookTransaction_DTO
                    ////                   {
                    ////                       LMBANO_Id = a.LMBANO_Id
                    ////                   }).ToList();
                    ////    mxcnt = maxitem.Count;
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        var maxitem = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                       from c in _LibraryContext.LIB_Master_NonBook_DMO
                                       from d in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                       where a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && a.LNBTR_Status == "Issue" && c.LMNBK_Id == d.LMNBK_Id && c.LMC_Id == cat_id && a.LNBTR_GuestName == data.LBTR_GuestName && a.LNBTR_GuestContactNo == data.LBTR_GuestContactNo && a.LNBTR_GuestEmailId == data.LBTR_GuestEmailId && a.LMNBKANO_Id == d.LMNBKANO_Id
                                       select new ClgNonBookTransaction_DTO
                                       {
                                           LMBANO_Id = a.LMNBKANO_Id
                                       }).ToList();
                        mxcnt = maxitem.Count;
                   //// }

                    if (data.maxitem > mxcnt)
                    {
                        if (data.Book_Trans_Id > 0)
                        {

                        }

                        else
                        {
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                             where a.MI_Id == data.MI_Id && a.LNBTR_ActiveFlg == true && a.LMNBKANO_Id == data.LMBANO_Id && a.LNBTR_Status == "Issue" && a.LNBTR_GuestName == data.LBTR_GuestName && a.LNBTR_GuestContactNo == data.LBTR_GuestContactNo
                                             select new ClgNonBookTransaction_DTO
                                             {
                                                 LMBANO_Id = a.LMNBKANO_Id
                                             }).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;

                            }
                            else
                            {
                                LIB_NonBook_Transaction_DMO obj = new LIB_NonBook_Transaction_DMO();

                                obj.MI_Id = data.MI_Id;
                                //////obj.ASMAY_Id = data.ASMAY_Id;
                                ////// obj.AMCST_Id = data.AMCST_Id;
                                obj.LMNBKANO_Id = data.LMBANO_Id;
                                obj.LNBTR_IssuedDate = data.Issue_Date;
                                obj.LNBTR_DueDate = data.Due_Date;
                                obj.LNBTR_ReturnedDate = data.Return_Date;
                                obj.LNBTR_FineCollecetd = 0;
                                obj.LNBTR_FineWaived = 0;
                                obj.LNBTR_RenewalCount = 0;
                                obj.LNBTR_Status = "Issue";
                                obj.LNBTR_GuestContactNo = data.LBTR_GuestContactNo;
                                obj.LNBTR_GuestName = data.LBTR_GuestName;
                                obj.LNBTR_GuestEmailId = data.LBTR_GuestEmailId;
                                ////// obj.HRME_Id = 0;
                                //////   obj.LMD_Id = 0;
                                obj.LNBTR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedBy = data.IVRMUL_Id;
                                obj.UpdatedBy = data.IVRMUL_Id;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNonBookTransaction_DTO searchfilter(ClgNonBookTransaction_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();

                var aaa = _LibraryContext.LIB_NonBook_Transaction_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                List<long> LMBANO_Id = new List<long>();
                if (aaa.Count() > 0)
                {
                    foreach (var it in aaa)
                    {
                        if (it.LNBTR_Status == "Issue")
                        {
                            LMBANO_Id.Add(it.LMNBKANO_Id);
                        }


                    }
                }

//                if (data.bookcat_type == "book")
//                {
//                    if (data.booktype == "issue")
//                    {
//                        //////related LMBANO_Id all data comming from Maser book Table
//                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
//                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
//                                          from c in _LibraryContext.MasterCategoryDMO
//                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
//                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower() == data.searchfilter)) && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
//                                          select new ClgNonBookTransaction_DTO
//                                          {

//                                              LMB_Id = a.LMB_Id,
//                                              MI_Id = a.MI_Id,

//                                              LMB_ClassNo = a.LMB_ClassNo,
//                                              LMB_BookTitle = a.LMB_BookTitle,

//                                              LMB_Price = a.LMB_Price,
//                                              LMB_VolNo = a.LMB_VolNo,
//                                              LMB_EntryDate = a.LMB_EntryDate,

//                                              LMC_Id = a.LMC_Id,
//                                              LMBANO_Id = b.LMBANO_Id,
//                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo



//                                          }).Distinct().ToArray();




//                    }
//                    else if (data.booktype == "ref")
//                    {
//                        //////related LMBANO_Id all data comming from Maser book Table
//                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
//                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
//                                          from c in _LibraryContext.MasterCategoryDMO
//                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
//                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower() == data.searchfilter)) && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
//                                          select new ClgNonBookTransaction_DTO
//                                          {
//                                              LMB_Id = a.LMB_Id,
//                                              MI_Id = a.MI_Id,

//                                              LMB_ClassNo = a.LMB_ClassNo,
//                                              LMB_BookTitle = a.LMB_BookTitle,

//                                              LMB_Price = a.LMB_Price,
//                                              LMB_VolNo = a.LMB_VolNo,
//                                              LMB_EntryDate = a.LMB_EntryDate,

//                                              LMC_Id = a.LMC_Id,
//                                              LMBANO_Id = b.LMBANO_Id,
//                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo


//                                          }).Distinct().ToArray();
//                    }
//                    else if (data.booktype == "nonbook")
//                    {
//                        //////related LMBANO_Id all data comming from Maser book Table
//                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
//                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
//                                          from c in _LibraryContext.MasterCategoryDMO
//                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
//                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType != "Issue" && a.LMB_BookType != "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower() == data.searchfilter)) && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id
//)
//                                          select new ClgNonBookTransaction_DTO
//                                          {

//                                              LMB_Id = a.LMB_Id,
//                                              MI_Id = a.MI_Id,

//                                              LMB_ClassNo = a.LMB_ClassNo,
//                                              LMB_BookTitle = a.LMB_BookTitle,

//                                              LMB_Price = a.LMB_Price,
//                                              LMB_VolNo = a.LMB_VolNo,
//                                              LMB_EntryDate = a.LMB_EntryDate,

//                                              LMC_Id = a.LMC_Id,
//                                              LMBANO_Id = b.LMBANO_Id,
//                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo
//                                              //////  Rack_Id =b.Rack_Id,


//                                          }).Distinct().ToArray();
//                    }
//                }
//                else
//                {
                    //if (data.booktype == "issue")
                    //{
                    //    //////related LMBANO_Id all data comming from Maser book Table
                    //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                      from c in _LibraryContext.MasterCategoryDMO
                    //                      from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower() == data.searchfilter)) && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
                    //                      select new ClgNonBookTransaction_DTO
                    //                      {

                    //                          LMB_Id = a.LMB_Id,
                    //                          MI_Id = a.MI_Id,

                    //                          LMB_ClassNo = a.LMB_ClassNo,
                    //                          LMB_BookTitle = a.LMB_BookTitle,

                    //                          LMB_Price = a.LMB_Price,
                    //                          LMB_VolNo = a.LMB_VolNo,
                    //                          LMB_EntryDate = a.LMB_EntryDate,

                    //                          LMC_Id = a.LMC_Id,
                    //                          LMBANO_Id = b.LMBANO_Id,
                    //                          LMBANO_AccessionNo = b.LMBANO_AccessionNo



                    //                      }).Distinct().ToArray();




                    //}
                    //else if (data.booktype == "ref")
                    //{
                    //    //////related LMBANO_Id all data comming from Maser book Table
                    //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
                    //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //                      from c in _LibraryContext.MasterCategoryDMO
                    //                      from d in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && ((a.LMB_BookTitle.ToLower().Contains(data.searchfilter)) || (b.LMBANO_AccessionNo.ToLower() == data.searchfilter)) && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
                    //                      select new ClgNonBookTransaction_DTO
                    //                      {
                    //                          LMB_Id = a.LMB_Id,
                    //                          MI_Id = a.MI_Id,

                    //                          LMB_ClassNo = a.LMB_ClassNo,
                    //                          LMB_BookTitle = a.LMB_BookTitle,

                    //                          LMB_Price = a.LMB_Price,
                    //                          LMB_VolNo = a.LMB_VolNo,
                    //                          LMB_EntryDate = a.LMB_EntryDate,

                    //                          LMC_Id = a.LMC_Id,
                    //                          LMBANO_Id = b.LMBANO_Id,
                    //                          LMBANO_AccessionNo = b.LMBANO_AccessionNo
                    //                          //  Rack_Id =b.Rack_Id,

                    //                      }).Distinct().ToArray();
                    //}
                    //else if (data.booktype == "nonbook")
                    //{
                        //////related LMBANO_Id all data comming from Maser book Table
                        data.booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                          from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          from d in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                          where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMNBKANO_Id) && a.LMNBK_ActiveFlg == true && c.LMC_BNBFlg == "Non-Book" && ((a.LMNBK_NonBookTitle.ToLower().Contains(data.searchfilter)) || (b.LMNBKANO_AccnNo.ToLower() == data.searchfilter)) && d.LMNBKL_ActiveFlg == true && d.LMNBK_Id == a.LMNBK_Id && d.LMAL_Id == data.LMAL_Id
)
                                          select new ClgNonBookTransaction_DTO
                                          {
                                              LMB_Id = a.LMNBK_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_ClassNo = a.LMNBK_ISBN,
                                              LMB_BookTitle = a.LMNBK_NonBookTitle,
                                              LMB_Price = a.LMNBK_Price,
                                              LMB_VolNo = a.LMNBK_VolumeNo,
                                              LMB_EntryDate = a.LMNBK_PublishDate,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMNBKANO_Id,
                                              LMBANO_AccessionNo = b.LMNBKANO_AccnNo


                                          }).Distinct().ToArray();
                    ////}
                ////}


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public ClgNonBookTransaction_DTO searchfilterbarcode(ClgNonBookTransaction_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToLower();


                var aaa = _LibraryContext.LIB_NonBook_Transaction_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                List<long> LMBANO_Id = new List<long>();
                if (aaa.Count() > 0)
                {
                    foreach (var it in aaa)
                    {
                        if (it.LNBTR_Status == "Issue")
                        {
                            LMBANO_Id.Add(it.LMNBKANO_Id);
                        }


                    }
                }

////                if (data.bookcat_type == "book")
////                {
////                    if (data.booktype == "issue")
////                    {
////                        //////related LMBANO_Id all data comming from Maser book Table
////                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
////                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
////                                          from c in _LibraryContext.MasterCategoryDMO
////                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
////                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AccessionNo.ToLower() == data.searchfilter && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
////                                          select new ClgNonBookTransaction_DTO
////                                          {

////                                              LMB_Id = a.LMB_Id,
////                                              MI_Id = a.MI_Id,

////                                              LMB_ClassNo = a.LMB_ClassNo,
////                                              LMB_BookTitle = a.LMB_BookTitle,

////                                              LMB_Price = a.LMB_Price,
////                                              LMB_VolNo = a.LMB_VolNo,
////                                              LMB_EntryDate = a.LMB_EntryDate,

////                                              LMC_Id = a.LMC_Id,
////                                              LMBANO_Id = b.LMBANO_Id,
////                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo



////                                          }).Distinct().ToArray();




////                    }
////                    else if (data.booktype == "ref")
////                    {
////                        //////related LMBANO_Id all data comming from Maser book Table
////                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
////                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
////                                          from c in _LibraryContext.MasterCategoryDMO
////                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
////                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AccessionNo.ToLower() == data.searchfilter && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
////                                          select new ClgNonBookTransaction_DTO
////                                          {
////                                              LMB_Id = a.LMB_Id,
////                                              MI_Id = a.MI_Id,

////                                              LMB_ClassNo = a.LMB_ClassNo,
////                                              LMB_BookTitle = a.LMB_BookTitle,

////                                              LMB_Price = a.LMB_Price,
////                                              LMB_VolNo = a.LMB_VolNo,
////                                              LMB_EntryDate = a.LMB_EntryDate,

////                                              LMC_Id = a.LMC_Id,
////                                              LMBANO_Id = b.LMBANO_Id,
////                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo


////                                          }).Distinct().ToArray();
////                    }
////                    else if (data.booktype == "nonbook")
////                    {
////                        //////related LMBANO_Id all data comming from Maser book Table
////                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
////                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
////                                          from c in _LibraryContext.MasterCategoryDMO
////                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
////                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType != "Issue" && a.LMB_BookType != "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Book" && b.LMBANO_AccessionNo.ToLower() == data.searchfilter && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id
////)
////                                          select new ClgNonBookTransaction_DTO
////                                          {

////                                              LMB_Id = a.LMB_Id,
////                                              MI_Id = a.MI_Id,

////                                              LMB_ClassNo = a.LMB_ClassNo,
////                                              LMB_BookTitle = a.LMB_BookTitle,

////                                              LMB_Price = a.LMB_Price,
////                                              LMB_VolNo = a.LMB_VolNo,
////                                              LMB_EntryDate = a.LMB_EntryDate,

////                                              LMC_Id = a.LMC_Id,
////                                              LMBANO_Id = b.LMBANO_Id,
////                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo
////                                              //////  Rack_Id =b.Rack_Id,


////                                          }).Distinct().ToArray();
////                    }
////                }
////                else
////                {
////                    if (data.booktype == "issue")
////                    {
////                        //////related LMBANO_Id all data comming from Maser book Table
////                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
////                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
////                                          from c in _LibraryContext.MasterCategoryDMO
////                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
////                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Issue" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && b.LMBANO_AccessionNo.ToLower() == data.searchfilter && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
////                                          select new ClgNonBookTransaction_DTO
////                                          {

////                                              LMB_Id = a.LMB_Id,
////                                              MI_Id = a.MI_Id,

////                                              LMB_ClassNo = a.LMB_ClassNo,
////                                              LMB_BookTitle = a.LMB_BookTitle,

////                                              LMB_Price = a.LMB_Price,
////                                              LMB_VolNo = a.LMB_VolNo,
////                                              LMB_EntryDate = a.LMB_EntryDate,

////                                              LMC_Id = a.LMC_Id,
////                                              LMBANO_Id = b.LMBANO_Id,
////                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo



////                                          }).Distinct().ToArray();




////                    }
////                    else if (data.booktype == "ref")
////                    {
////                        //////related LMBANO_Id all data comming from Maser book Table
////                        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
////                                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
////                                          from c in _LibraryContext.MasterCategoryDMO
////                                          from d in _LibraryContext.LIB_Master_Book_Library_DMO
////                                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_ActiveFlg == true && a.LMB_BookType == "Reference" && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && b.LMBANO_AccessionNo.ToLower() == data.searchfilter && d.LMBL_ActiveFlg == true && d.LMB_Id == a.LMB_Id && d.LMAL_Id == data.LMAL_Id)
////                                          select new ClgNonBookTransaction_DTO
////                                          {
////                                              LMB_Id = a.LMB_Id,
////                                              MI_Id = a.MI_Id,

////                                              LMB_ClassNo = a.LMB_ClassNo,
////                                              LMB_BookTitle = a.LMB_BookTitle,

////                                              LMB_Price = a.LMB_Price,
////                                              LMB_VolNo = a.LMB_VolNo,
////                                              LMB_EntryDate = a.LMB_EntryDate,

////                                              LMC_Id = a.LMC_Id,
////                                              LMBANO_Id = b.LMBANO_Id,
////                                              LMBANO_AccessionNo = b.LMBANO_AccessionNo
////                                              //////  Rack_Id =b.Rack_Id,

////                                          }).Distinct().ToArray();
////                    }
////                    else if (data.booktype == "nonbook")
////                    {
                        //////related LMBANO_Id all data comming from Maser book Table
                        data.booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                          from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                          from c in _LibraryContext.MasterCategoryDMO
                                          from d in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                          where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMNBKANO_Id) && a.LMNBK_ActiveFlg == true  && a.MI_Id == c.MI_Id && a.LMC_Id == c.LMC_Id && c.LMC_BNBFlg == "Non-Book" && b.LMNBKANO_AccnNo.ToLower() == data.searchfilter && d.LMNBKL_ActiveFlg == true && d.LMNBK_Id == a.LMNBK_Id && d.LMAL_Id == data.LMAL_Id
)
                                          select new ClgNonBookTransaction_DTO
                                          {
                                              LMB_Id = a.LMNBK_Id,
                                              MI_Id = a.MI_Id,
                                              LMB_ClassNo = a.LMNBK_ISBN,
                                              LMB_BookTitle = a.LMNBK_NonBookTitle,
                                              LMB_Price = a.LMNBK_Price,
                                              LMB_VolNo = a.LMNBK_VolumeNo,
                                              LMB_EntryDate = a.LMNBK_PublishDate,
                                              LMC_Id = a.LMC_Id,
                                              LMBANO_Id = b.LMNBKANO_Id,
                                              LMBANO_AccessionNo = b.LMNBKANO_AccnNo


                                          }).Distinct().ToArray();
                ////    }
                ////}


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public ClgNonBookTransaction_DTO searchfilterbarcode1(ClgNonBookTransaction_DTO data)
        {

            //try
            //{
            //    if (data.searchfilter == null)
            //    {
            //        data.searchfilter = "";
            //    }

            //    List<long> clsids = new List<long>();
            //    List<LIB_Master_Library_DMO> liblist = new List<LIB_Master_Library_DMO>();
            //    if (data.LMAL_Id == 0)
            //    {
            //        liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
            //                   from b in _LibraryContext.LIB_User_Library_DMO
            //                       ////// from c in _LibraryContext.LIB_Library_Class_DMO
            //                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
            //                   select a).ToList();
            //    }
            //    else
            //    {
            //        liblist = (from a in _LibraryContext.LIB_Master_Library_DMO
            //                   from b in _LibraryContext.LIB_User_Library_DMO
            //                       ////// from c in _LibraryContext.LIB_Library_Class_DMO
            //                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_Id == data.LMAL_Id
            //                   select a).ToList();
            //    }

            //    data.msterliblist = liblist.ToArray();
            //    data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
            //                          from b in _LibraryContext.LIB_User_Library_DMO
            //                              ////// from c in _LibraryContext.LIB_Library_Class_DMO
            //                          where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
            //                          select a).ToArray();




            //    if (liblist.Count > 0)
            //    {
            //        data.LMAL_Id = liblist.FirstOrDefault().LMAL_Id;


            //        var typeofbook = "";
            //        if (data.booktype == "issue")
            //        {
            //            typeofbook = "Issue";
            //        }
            //        else if (data.booktype == "ref")
            //        {
            //            typeofbook = "Reference";
            //        }
            //        else if (data.booktype == "nonbook")
            //        {
            //            typeofbook = "nonbook";
            //        }

            //        if (data.searchfilter == "")
            //        {


            //            if (data.issuertype == "std")
            //            {

            //                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
            //                {
            //                    cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_COLLEGE";
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.CommandTimeout = 8000000;

            //                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //                    SqlDbType.BigInt)
            //                    {
            //                        Value = data.MI_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Type",
            //                  SqlDbType.VarChar)
            //                    {
            //                        Value = typeofbook
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            //                  SqlDbType.BigInt)
            //                    {
            //                        Value = data.ASMAY_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
            //                 SqlDbType.VarChar)
            //                    {
            //                        Value = data.LMAL_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Type2",
            //                  SqlDbType.VarChar)
            //                    {
            //                        Value = "ALL"
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@LMBANO_AccessionNo",
            //                SqlDbType.VarChar)
            //                    {
            //                        Value = ""
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
            //                     SqlDbType.BigInt)
            //                    {
            //                        Value = 0
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Book_Trans_Id",
            //           SqlDbType.BigInt)
            //                    {
            //                        Value = data.Book_Trans_Id
            //                    });
            //                    if (cmd.Connection.State != ConnectionState.Open)
            //                        cmd.Connection.Open();

            //                    var retObject = new List<dynamic>();
            //                    try
            //                    {
            //                        using (var dataReader = cmd.ExecuteReader())
            //                        {
            //                            while (dataReader.Read())
            //                            {
            //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                                {
            //                                    dataRow.Add(
            //                                    dataReader.GetName(iFiled),
            //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                                    );
            //                                }
            //                                retObject.Add((ExpandoObject)dataRow);
            //                            }
            //                        }
            //                        data.getalldata = retObject.ToArray();
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine(ex.Message);
            //                    }
            //                }
            //            }
            //            else if (data.issuertype == "stf")
            //            {
            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }

            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }




            //            }
            //            else if (data.issuertype == "dep")
            //            {
            //                data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
            //                                       select new ClgNonBookTransaction_DTO
            //                                       {
            //                                           HRMD_Id = a.HRMD_Id,
            //                                           HRMD_DepartmentName = a.HRMD_DepartmentName,
            //                                           HRMD_Order = a.HRMD_Order,

            //                                       }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               ////// LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,


            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }






            //            }


            //            else if (data.issuertype == "gst")
            //            {


            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }

            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id)
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }





            //            }
            //        }
            //        else
            //        {


            //            if (data.issuertype == "std")
            //            {
            //                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
            //                {
            //                    cmd.CommandText = "LIB_GET_STUDENT_BOOK_TRANSACTION_COLLEGE";
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.CommandTimeout = 8000000;

            //                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
            //                    SqlDbType.BigInt)
            //                    {
            //                        Value = data.MI_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Type",
            //                  SqlDbType.VarChar)
            //                    {
            //                        Value = typeofbook
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            //                  SqlDbType.BigInt)
            //                    {
            //                        Value = data.ASMAY_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
            //                 SqlDbType.VarChar)
            //                    {
            //                        Value = data.LMAL_Id
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Type2",
            //                  SqlDbType.VarChar)
            //                    {
            //                        Value = "ACCNO"
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@LMBANO_AccessionNo",
            //                SqlDbType.VarChar)
            //                    {
            //                        Value = data.searchfilter.Trim()
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
            //                     SqlDbType.BigInt)
            //                    {
            //                        Value = 0
            //                    });
            //                    cmd.Parameters.Add(new SqlParameter("@Book_Trans_Id",
            //           SqlDbType.BigInt)
            //                    {
            //                        Value = data.Book_Trans_Id
            //                    });
            //                    if (cmd.Connection.State != ConnectionState.Open)
            //                        cmd.Connection.Open();

            //                    var retObject = new List<dynamic>();
            //                    try
            //                    {
            //                        using (var dataReader = cmd.ExecuteReader())
            //                        {
            //                            while (dataReader.Read())
            //                            {
            //                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                                {
            //                                    dataRow.Add(
            //                                    dataReader.GetName(iFiled),
            //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                                    );
            //                                }
            //                                retObject.Add((ExpandoObject)dataRow);
            //                            }
            //                        }
            //                        data.getalldata = retObject.ToArray();
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine(ex.Message);
            //                    }
            //                }

            //            }
            //            else if (data.issuertype == "stf")
            //            {
            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }

            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterEmployee
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRME_Id = d.HRME_Id,
            //                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
            //                                               HRMGT_Id = d.HRMGT_Id,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }




            //            }
            //            else if (data.issuertype == "dep")
            //            {
            //                data.departmentlist = (from a in _LibraryContext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
            //                                       select new ClgNonBookTransaction_DTO
            //                                       {
            //                                           HRMD_Id = a.HRMD_Id,
            //                                           HRMD_DepartmentName = a.HRMD_DepartmentName,
            //                                           HRMD_Order = a.HRMD_Order,

            //                                       }).Distinct().OrderBy(t => t.HRMD_Order).ToArray();


            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               ////// LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,
            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,


            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from e in _LibraryContext.LIB_Book_Transaction_DepartmentDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.HR_Master_Department
            //                                           from f in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.MI_Id == d.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,
            //                                               HRMD_Id = d.HRMD_Id,
            //                                               HRMD_DepartmentName = d.HRMD_DepartmentName,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                               //////LMD_DepartmentCode = d.LMD_DepartmentCode,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }






            //            }


            //            else if (data.issuertype == "gst")
            //            {


            //                if (data.bookcat_type == "book")
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,


            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }

            //                }
            //                else
            //                {
            //                    if (data.booktype == "nonbook")
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                    else
            //                    {
            //                        //////get all related data for bind table grid in Html
            //                        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                                           from c in _LibraryContext.BookRegisterDMO
            //                                           from d in _LibraryContext.MasterCategoryDMO
            //                                           from h in _LibraryContext.LIB_Master_Book_Library_DMO
            //                                           where (a.MI_Id == c.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && a.MI_Id == data.MI_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && a.LBTR_GuestName != null && a.LBTR_GuestName != "" && a.MI_Id == d.MI_Id && c.LMC_Id == d.LMC_Id && d.LMC_BNBFlg == "Non-Book" && a.LBTR_Status == "Issue" && h.LMBL_ActiveFlg == true && c.LMB_Id == h.LMB_Id && h.LMAL_Id == data.LMAL_Id && b.LMBANO_AccessionNo.ToLower() == data.searchfilter.ToLower())
            //                                           select new ClgNonBookTransaction_DTO
            //                                           {
            //                                               Book_Trans_Id = a.LBTR_Id,
            //                                               LMB_Id = c.LMB_Id,
            //                                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                                               MI_Id = a.MI_Id,
            //                                               ////// ASMAY_Id = a.ASMAY_Id,
            //                                               LMBANO_Id = b.LMBANO_Id,
            //                                               ////// LMB_BookType = c.LMB_BookType,
            //                                               LMB_BookTitle = c.LMB_BookTitle,
            //                                               Issue_Date = a.LBTR_IssuedDate,
            //                                               Due_Date = a.LBTR_DueDate,
            //                                               Return_Date = a.LBTR_ReturnedDate,
            //                                               Book_Trans_Status = a.LBTR_Status,
            //                                               Fine_Amount = a.LBTR_TotalFine,
            //                                               ////// Renewal_Counter = a.Renewal_Counter,
            //                                               Waived_Amount = a.LBTR_FineWaived,

            //                                               LBTR_GuestName = a.LBTR_GuestName,

            //                                               LBTR_GuestContactNo = a.LBTR_GuestContactNo,
            //                                               LBTR_GuestEmailId = a.LBTR_GuestEmailId,
            //                                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,

            //                                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
            //                    }
            //                }


            //            }
            //        }


            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}

            return data;
        }

        public ClgNonBookTransaction_DTO get_bookdetails(ClgNonBookTransaction_DTO data)
        {
            try
            {
                //////get LMBANO_Id and related data and get Maxday's & max renewal day's, and how many item issue for that LMBANO_Id
                //////data.bookdetails = (from a in _LibraryContext.BookRegisterDMO
                //////                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                //////                   // from c in _LibraryContext.CirculationParameterDMO
                //////                    where (a.LMB_Id == b.LMB_Id  && b.LMB_Id == data.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_ActiveFlg == true && b.LMBANO_Id==data.LMBANO_Id)
                //////                    select new ClgNonBookTransaction_DTO
                //////                    {
                //////                        LMB_Id = a.LMB_Id,
                //////                        LMBANO_Id = b.LMBANO_Id,
                //////                        LMB_BookTitle = a.LMB_BookTitle,
                //////                        LMB_VolNo = a.LMB_VolNo,
                //////                        LMB_EntryDate = a.LMB_EntryDate,
                //////                        LMB_Price = a.LMB_Price,
                //////                        LMB_ClassNo = a.LMB_ClassNo,
                //////                        //  LMBA_Id = a.LMBA_Id,
                //////                     //   LMD_Id = a.LMD_Id,
                //////                        LMS_Id = a.LMS_Id,
                //////                        //  LMP_Id = a.LMP_Id,
                //////                       // LML_Id = a.LML_Id,
                //////                        //  Donor_Id = a.Donor_Id,
                //////                        //  LMV_Id = a.LMV_Id,
                //////                      //  LMC_Id = a.LMC_Id,
                //////                      //  Max_Issue_Days = c.Max_Issue_Days,
                //////                        //Max_No_Renewals = c.Max_No_Renewals,
                //////                       // Max_Issue_Items = c.Max_Issue_Items,
                //////                    }
                //////                  ).Distinct().ToArray();

                if (data.LMB_Id == 0 && data.LMBANO_Id > 0)
                {
                    var bookid = (from a in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                  from b in _LibraryContext.LIB_Master_NonBook_DMO
                                  where a.LMNBK_Id == b.LMNBK_Id && a.LMNBKANO_ActiveFlg == true && a.LMNBKANO_Id == data.LMBANO_Id && b.LMNBK_ActiveFlg == true && b.MI_Id == data.MI_Id
                                  select new ClgNonBookTransaction_DTO
                                  {
                                      LMB_Id = b.LMNBK_Id
                                  }).ToList();
                    if (bookid.Count > 0)
                    {
                        data.LMB_Id = bookid[0].LMB_Id;
                    }

                }

                #region Book  DETAILS
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_NONBOOKDETAILS_TRANS";
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

                    //if (data.bookcat_type == "book")
                    //{
                    //    if (data.booktype == "issue")
                    //    {
                    //        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    //                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    //                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Issue")
                    //                                     select new ClgNonBookTransaction_DTO
                    //                                     {
                    //                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    //                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    //                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    //                                     }).ToArray();


                    //    }
                    //    else
                    //    if (data.booktype == "ref")
                    //    {
                    //        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    //                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    //                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Reference")
                    //                                     select new ClgNonBookTransaction_DTO
                    //                                     {
                    //                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    //                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    //                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    //                                     }).ToArray();


                    //    }
                    //}
                    //else
                    //{
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "GUEST" && b.LMC_Id == cat_id)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    //}


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNonBookTransaction_DTO studentdetails(ClgNonBookTransaction_DTO data)
        {
            try
            {


                if (data.issuertype == "std")
                {
                    //In this query we et student details according to AMCST_Id
                    var studentdata = (from a in _LibraryContext.Adm_Master_College_StudentDMO
                                       from b in _LibraryContext.Adm_College_Yearly_StudentDMO
                                       from c in _LibraryContext.MasterCourseDMO
                                       from d in _LibraryContext.ClgMasterBranchDMO
                                       from e in _LibraryContext.CLG_Adm_Master_SemesterDMO
                                       from f in _LibraryContext.Adm_College_Master_SectionDMO
                                       where (a.AMCST_Id == b.AMCST_Id && c.AMCO_Id == b.AMCO_Id && c.AMCO_ActiveFlag == true && a.MI_Id == data.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && d.AMB_Id == b.AMB_Id && a.AMCST_SOL == "S" && d.AMB_ActiveFlag == true && e.AMSE_Id == b.AMSE_Id && e.AMSE_ActiveFlg == true && f.ACMS_Id == b.ACMS_Id && f.MI_Id == a.MI_Id)
                                       select new ClgNonBookTransaction_DTO
                                       {
                                           AMCST_Id = a.AMCST_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           AMST_Photoname = a.AMCST_StudentPhoto,
                                           AMCST_FirstName = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                           AMST_MiddleName = a.AMCST_MiddleName,
                                           AMST_LastName = a.AMCST_LastName,
                                           AMST_RegistrationNo = a.AMCST_RegistrationNo,
                                           AMST_AdmNo = a.AMCST_AdmNo,
                                           AMCO_CourseName = c.AMCO_CourseName,
                                           AMB_BranchName = d.AMB_BranchName,
                                           AMAY_RollNo = b.ACYST_RollNo,
                                           AMSE_SEMName = e.AMSE_SEMName,
                                           ACMS_SectionName = f.ACMS_SectionName,
                                           AMCO_Id = b.AMCO_Id,
                                           ASYST_Id = b.ACYST_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id,
                                           ACMS_Id = b.ACMS_Id,

                                           //  AMST_FatherMobleNo = a.AMST_FatherMobleNo


                                       }).Distinct().ToArray();

                    data.studentdata = studentdata.ToArray();
                    //var clasidlist = _LibraryContext.Adm_College_Yearly_StudentDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && t.AMAY_ActiveFlag == 1).ToList();

                    //long classid = 0;
                    //if (clasidlist.Count > 0)
                    //{
                    //    classid = clasidlist[0].ASMCL_Id;
                    //}


                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    if (data.booktype == "ref")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPASC_ActiveFlg == true
                    ////                                     //  && b.ASMCL_Id == classid 
                    ////                                     && a.LBCPA_IssueRefFlg == "Reference" && a.LBCPA_Flg == "STUDENT")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPASC_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPASC_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPASC_NoOfItems,

                    ////                                     }).ToArray();


                    ////    }

                    ////    else
                    ////if (data.booktype == "issue")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPASC_ActiveFlg == true
                    ////                                     // && b.ASMCL_Id == classid
                    ////                                     && a.LBCPA_IssueRefFlg == "Issue" && a.LBCPA_Flg == "STUDENT")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPASC_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPASC_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPASC_NoOfItems,

                    ////                                     }).ToArray();


                    ////    }
                    ////}
                    ////else
                    ////{
                    long cat_id = 0;
                    var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                    if (category_id.Count > 0)
                    {
                        cat_id = category_id[0].LMC_Id;
                    }

                    data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO
                                                 where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPASC_ActiveFlg == true          && b.LMC_Id== cat_id
                                                 // && b.ASMCL_Id == classid  
                                                 && a.LNBCPA_Flg == "STUDENT")
                                                 select new ClgNonBookTransaction_DTO
                                                 {
                                                     Max_Issue_Days = b.LNBCPASC_IssueDays,
                                                     Max_No_Renewals = b.LNBCPASC_NoOfRenewals,
                                                     Max_Issue_Items = b.LNBCPASC_NoOfItems,

                                                 }).ToArray();
                    //// }

                    if (data.app == "W")
                    {

                        var studentlist = (from m in _LibraryContext.Adm_Master_College_StudentDMO
                                           from n in _LibraryContext.Adm_College_Yearly_StudentDMO
                                           where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") && m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1
                                           && n.AMCO_Id == studentdata.FirstOrDefault().AMCO_Id
                                           && n.AMB_Id == studentdata.FirstOrDefault().AMB_Id
                                            && n.AMSE_Id == studentdata.FirstOrDefault().AMSE_Id
                                             && n.ACMS_Id == studentdata.FirstOrDefault().ACMS_Id
                                           select new ClgNonBookTransaction_DTO
                                           {
                                               AMCST_Id = m.AMCST_Id,
                                               MI_Id = m.MI_Id,
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMST_FirstName = ((m.AMCST_FirstName == null ? " " : m.AMCST_FirstName) + " " + (m.AMCST_MiddleName == null ? " " : m.AMCST_MiddleName) + " " + (m.AMCST_LastName == null ? " " : m.AMCST_LastName)).Trim(),
                                               AMCST_MiddleName = m.AMCST_MiddleName,
                                               AMST_LastName = m.AMCST_LastName,
                                               AMST_AdmNo = m.AMCST_AdmNo

                                           }).Distinct().ToList();

                        if (studentlist.Count > 0)
                        {
                            data.studentlist = studentlist.OrderBy(t => t.AMST_FirstName).ToArray();
                            //data.studentCount = studentlist.Count;
                        }

                        var typeofbook = "nonbook";
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
                            cmd.CommandText = "LIB_GET_STUDENT_NONBOOK_TRANSACTION_COLLEGE";
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
                            cmd.Parameters.Add(new SqlParameter("@Type2",
                             SqlDbType.VarChar)
                            {
                                Value = "STD"
                            });
                            cmd.Parameters.Add(new SqlParameter("@LMBANO_AccessionNo",
                             SqlDbType.VarChar)
                            {
                                Value = ""
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                             SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Book_Trans_Id",
                       SqlDbType.BigInt)
                            {
                                Value = data.Book_Trans_Id
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

                }



                else if (data.issuertype == "stf")
                {


                    //In this query we et Staff details according to HRME_Id
                    data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
                                           from b in _LibraryContext.HR_Master_Department
                                           from c in _LibraryContext.HR_Master_Designation
                                           where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                           select new ClgNonBookTransaction_DTO
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

                                           }).Distinct().ToArray();



                    var clasidlist = _LibraryContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.HRME_ActiveFlag == true).ToList();

                    long? classid = 0;
                    if (clasidlist.Count > 0)
                    {
                        classid = clasidlist[0].HRMGT_Id;
                    }





                 ////   if (data.bookcat_type == "book")
                 ////   {
                 ////       if (data.booktype == "issue")
                 ////       {
                 ////           data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                 ////                                        from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                 ////                                        where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == classid && a.LBCPA_IssueRefFlg == "Issue" && a.LBCPA_Flg == "STAFF")
                 ////                                        select new ClgNonBookTransaction_DTO
                 ////                                        {
                 ////                                            Max_Issue_Days = b.LBCPAST_IssueDays,
                 ////                                            Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                 ////                                            Max_Issue_Items = b.LBCPAST_NoOfItems,

                 ////                                        }).ToArray();


                 ////       }
                 ////       else

                 ////if (data.booktype == "ref")
                 ////       {
                 ////           data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                 ////                                        from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                 ////                                        where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == classid && a.LBCPA_IssueRefFlg == "Reference" && a.LBCPA_Flg == "STAFF")
                 ////                                        select new ClgNonBookTransaction_DTO
                 ////                                        {
                 ////                                            Max_Issue_Days = b.LBCPAST_IssueDays,
                 ////                                            Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                 ////                                            Max_Issue_Items = b.LBCPAST_NoOfItems,

                 ////                                        }).ToArray();


                 ////       }
                 ////   }
                 ////   else
                 ////   {
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAST_ActiveFlg == true && b.LMC_Id == cat_id && b.HRMGT_Id == classid && a.LNBCPA_Flg == "STAFF")
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAST_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAST_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAST_NoOfItems,

                                                     }).ToArray();
                    ////}

                    if (data.app == "W")
                    {
                        var typeofbook = "nonbook";
                        ////if (data.booktype == "issue")
                        ////{
                        ////    typeofbook = "Issue";
                        ////}
                        ////else if (data.booktype == "ref")
                        ////{
                        ////    typeofbook = "Reference";
                        ////}
                        ////else if (data.booktype == "nonbook")
                        ////{
                        ////    typeofbook = "nonbook";
                        ////}

                        ////if (data.bookcat_type == "book")
                        ////{
                        ////    if (data.booktype == "nonbook")
                        ////    {
                        ////        //////get all related data for bind table grid in Html
                        ////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                        ////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////                           from c in _LibraryContext.BookRegisterDMO
                        ////                           from d in _LibraryContext.MasterEmployee
                        ////                           from f in _LibraryContext.MasterCategoryDMO
                        ////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                        ////                           select new ClgNonBookTransaction_DTO
                        ////                           {
                        ////                               Book_Trans_Id = a.LBTR_Id,
                        ////                               LMB_Id = c.LMB_Id,
                        ////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////                               MI_Id = a.MI_Id,
                        ////                               ////// ASMAY_Id = a.ASMAY_Id,
                        ////                               LMBANO_Id = b.LMBANO_Id,
                        ////                               ////// LMB_BookType = c.LMB_BookType,
                        ////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////                               Issue_Date = a.LBTR_IssuedDate,
                        ////                               Due_Date = a.LBTR_DueDate,
                        ////                               Return_Date = a.LBTR_ReturnedDate,
                        ////                               Book_Trans_Status = a.LBTR_Status,
                        ////                               Fine_Amount = a.LBTR_TotalFine,
                        ////                               ////// Renewal_Counter = a.Renewal_Counter,
                        ////                               Waived_Amount = a.LBTR_FineWaived,
                        ////                               HRME_Id = d.HRME_Id,
                        ////                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                        ////                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                        ////                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                        ////                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                        ////                               HRMGT_Id = d.HRMGT_Id,
                        ////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                        ////                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        ////    }
                        ////    else
                        ////    {
                        ////        //////get all related data for bind table grid in Html
                        ////        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                        ////                           from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                        ////                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
                        ////                           from c in _LibraryContext.BookRegisterDMO
                        ////                           from d in _LibraryContext.MasterEmployee
                        ////                           from f in _LibraryContext.MasterCategoryDMO

                        ////                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType == typeofbook && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                        ////                           select new ClgNonBookTransaction_DTO
                        ////                           {
                        ////                               Book_Trans_Id = a.LBTR_Id,
                        ////                               LMB_Id = c.LMB_Id,
                        ////                               LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                        ////                               MI_Id = a.MI_Id,
                        ////                               ////// ASMAY_Id = a.ASMAY_Id,
                        ////                               LMBANO_Id = b.LMBANO_Id,
                        ////                               ////// LMB_BookType = c.LMB_BookType,
                        ////                               LMB_BookTitle = c.LMB_BookTitle,
                        ////                               Issue_Date = a.LBTR_IssuedDate,
                        ////                               Due_Date = a.LBTR_DueDate,
                        ////                               Return_Date = a.LBTR_ReturnedDate,
                        ////                               Book_Trans_Status = a.LBTR_Status,
                        ////                               Fine_Amount = a.LBTR_TotalFine,
                        ////                               ////// Renewal_Counter = a.Renewal_Counter,
                        ////                               Waived_Amount = a.LBTR_FineWaived,
                        ////                               HRME_Id = d.HRME_Id,
                        ////                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                        ////                               ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                        ////                               //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                        ////                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                        ////                               HRMGT_Id = d.HRMGT_Id,
                        ////                               LBTR_Renewalcounter = a.LBTR_Renewalcounter,



                        ////                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                        ////    }

                        ////}
                        ////else
                        ////{
                            ////if (data.booktype == "nonbook")
                            ////{
                            ////    //////get all related data for bind table grid in Html
                            ////    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
                            ////                       from e in _LibraryContext.LIB_Book_Transaction_StaffDMO
                            ////                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
                            ////                       from c in _LibraryContext.BookRegisterDMO
                            ////                       from d in _LibraryContext.MasterEmployee
                            ////                       from f in _LibraryContext.MasterCategoryDMO
                            ////                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id == c.MI_Id && b.LMB_Id == c.LMB_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LBTR_Id == a.LBTR_Id && a.LBTR_ActiveFlg == true && c.LMB_BookType != "Issue" && c.LMB_BookType != "Reference" && a.MI_Id == f.MI_Id && c.LMC_Id == f.LMC_Id && f.LMC_BNBFlg == "Non-Book" && d.HRME_Id == data.HRME_Id && a.LBTR_Status == "Issue")
                            ////                       select new ClgNonBookTransaction_DTO
                            ////                       {
                            ////                           Book_Trans_Id = a.LBTR_Id,
                            ////                           LMB_Id = c.LMB_Id,
                            ////                           LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                            ////                           MI_Id = a.MI_Id,
                            ////                           ////// ASMAY_Id = a.ASMAY_Id,
                            ////                           LMBANO_Id = b.LMBANO_Id,
                            ////                           ////// LMB_BookType = c.LMB_BookType,
                            ////                           LMB_BookTitle = c.LMB_BookTitle,
                            ////                           Issue_Date = a.LBTR_IssuedDate,
                            ////                           Due_Date = a.LBTR_DueDate,
                            ////                           Return_Date = a.LBTR_ReturnedDate,
                            ////                           Book_Trans_Status = a.LBTR_Status,
                            ////                           Fine_Amount = a.LBTR_TotalFine,
                            ////                           ////// Renewal_Counter = a.Renewal_Counter,
                            ////                           Waived_Amount = a.LBTR_FineWaived,
                            ////                           HRME_Id = d.HRME_Id,
                            ////                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                            ////                           ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                            ////                           //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                            ////                           HRME_EmployeeCode = d.HRME_EmployeeCode,
                            ////                           HRMGT_Id = d.HRMGT_Id,
                            ////                           LBTR_Renewalcounter = a.LBTR_Renewalcounter,




                            ////                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                            ////}
                            ////else
                            ////{
                                //////get all related data for bind table grid in Html
                                data.getalldata = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                                   from e in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                                   from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                                   from c in _LibraryContext.LIB_Master_NonBook_DMO
                                                   from d in _LibraryContext.MasterEmployee
                                                   from f in _LibraryContext.MasterCategoryDMO

                                                   where (a.MI_Id == d.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && a.MI_Id == c.MI_Id && b.LMNBK_Id == c.LMNBK_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && e.LNBTR_Id == a.LNBTR_Id && a.LNBTR_ActiveFlg == true && f.MI_Id == a.MI_Id && f.LMC_Id == c.LMC_Id && f.LMC_BNBFlg == "Non-Book" && d.HRME_Id == data.HRME_Id && a.LNBTR_Status == "Issue")
                                                   select new ClgNonBookTransaction_DTO
                                                   {
                                                       Book_Trans_Id = a.LNBTR_Id,
                                                       LMB_Id = c.LMNBK_Id,
                                                       LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                                       MI_Id = a.MI_Id,
                                                       ////// ASMAY_Id = a.ASMAY_Id,
                                                       LMBANO_Id = b.LMNBKANO_Id,
                                                       ////// LMB_BookType = c.LMB_BookType,
                                                       LMB_BookTitle = c.LMNBK_NonBookTitle,
                                                       Issue_Date = a.LNBTR_IssuedDate,
                                                       Due_Date = a.LNBTR_DueDate,
                                                       Return_Date = a.LNBTR_ReturnedDate,
                                                       Book_Trans_Status = a.LNBTR_Status,
                                                       Fine_Amount = a.LNBTR_TotalFine,
                                                       ////// Renewal_Counter = a.Renewal_Counter,
                                                       Waived_Amount = a.LNBTR_FineWaived,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       ////// HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                                       //////  HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRMGT_Id = d.HRMGT_Id,
                                                       LBTR_Renewalcounter = a.LNBTR_RenewalCount,



                                                   }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();
                           //// }
                        ////}
                    }


                }



                else if (data.issuertype == "dep")
                {


                    ////////In this query we et Staff details according to HRME_Id
                    //////data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
                    //////                       from b in _LibraryContext.HR_Master_Department
                    //////                       from c in _LibraryContext.HR_Master_Designation
                    //////                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                    //////                       select new ClgNonBookTransaction_DTO
                    //////                       {
                    //////                           HRME_Id = a.HRME_Id,
                    //////                           MI_Id = a.MI_Id,
                    //////                           HRME_EmployeeCode = a.HRME_EmployeeCode,
                    //////                           HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                    //////                           HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                    //////                           HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                    //////                           HRME_MobileNo = a.HRME_MobileNo,
                    //////                           HRMD_DepartmentName = b.HRMD_DepartmentName,
                    //////                           HRMD_Id = b.HRMD_Id,
                    //////                           HRMDES_DesignationName = c.HRMDES_DesignationName,
                    //////                           HRMDES_Id = c.HRMDES_Id,
                    //////                           HRME_Photo = a.HRME_Photo,

                    //////                       }).Distinct().ToArray();



                    //////var clasidlist = _LibraryContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.HRME_ActiveFlag == true).ToList();

                    //////long? classid = 0;
                    //////if (clasidlist.Count > 0)
                    //////{
                    //////    classid = clasidlist[0].HRMGT_Id;
                    //////}



         ////           if (data.bookcat_type == "book")
         ////           {
         ////               if (data.booktype == "issue")
         ////               {
         ////                   data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
         ////                                                from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
         ////                                                where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Issue")
         ////                                                select new ClgNonBookTransaction_DTO
         ////                                                {
         ////                                                    Max_Issue_Days = b.LBCPAO_IssueDays,
         ////                                                    Max_No_Renewals = b.LBCPAO_NoOfRenewals,
         ////                                                    Max_Issue_Items = b.LBCPAO_NoOfItems,

         ////                                                }).ToArray();


         ////               }
         ////               else
         ////if (data.booktype == "ref")
         ////               {
         ////                   data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
         ////                                                from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
         ////                                                where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Reference")
         ////                                                select new ClgNonBookTransaction_DTO
         ////                                                {
         ////                                                    Max_Issue_Days = b.LBCPAO_IssueDays,
         ////                                                    Max_No_Renewals = b.LBCPAO_NoOfRenewals,
         ////                                                    Max_Issue_Items = b.LBCPAO_NoOfItems,

         ////                                                }).ToArray();


         ////               }
         ////           }
         ////           else
         ////           {
                        long cat_id = 0;
                        var category_id = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMNBK_Id == data.LMB_Id && t.LMNBK_ActiveFlg == true).Distinct().ToList();
                        if (category_id.Count > 0)
                        {
                            cat_id = category_id[0].LMC_Id;
                        }

                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "DEPARTMENT" && b.LMC_Id == cat_id)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    ////}




                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNonBookTransaction_DTO Editdata(ClgNonBookTransaction_DTO data)
        {
            try
            {
                if (data.issuertype == "std")
                {
                    //////in this query we edit only selected data like AMCST_Id and LMBANO_Id
                    //////var editlist = (from a in _LibraryContext.BookTransactionDMO
                    //////                from e in _LibraryContext.LIB_Book_Transaction_Student_CollegeDMO
                    //////                from b in _LibraryContext.Lib_M_Book_Accn_DMO
                    //////                from c in _LibraryContext.BookRegisterDMO
                    //////                from d in _LibraryContext.Adm_Master_College_StudentDMO
                    //////                where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.LBTR_Id == data.Book_Trans_Id && a.LBTR_Id == e.LBTR_Id)
                    //////                select new ClgNonBookTransaction_DTO
                    //////                {
                    //////                    Book_Trans_Id = a.LBTR_Id,
                    //////                    LMB_Id = c.LMB_Id,
                    //////                    AMCST_Id = e.AMCST_Id,
                    //////                    MI_Id = a.MI_Id,
                    //////                    //ASMAY_Id = a.ASMAY_Id,
                    //////                    LMBANO_Id = a.LMBANO_Id,
                    //////                    Issue_Date = a.LBTR_IssuedDate,
                    //////                    Due_Date = a.LBTR_DueDate,
                    //////                    Return_Date = a.LBTR_ReturnedDate,
                    //////                    Book_Trans_Status = a.LBTR_Status,
                    //////                    LMB_BookTitle = c.LMB_BookTitle,
                    //////                    AMST_FirstName = ((d.AMST_FirstName == null ? " " : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null ? " " : d.AMST_MiddleName) + " " + (d.AMST_LastName == null ? " " : d.AMST_LastName)).Trim(),
                    //////                    AMST_MiddleName = d.AMST_MiddleName,
                    //////                    AMST_LastName = d.AMST_LastName,
                    //////                    AMST_Photoname = d.AMST_Photoname,
                    //////                    LMBANO_AccessionNo = b.LMBANO_AccessionNo,
                    //////                    AMST_AdmNo = d.AMST_AdmNo
                    //////                }).Distinct().ToList();




                    //////data.editlist = editlist.Distinct().ToArray();

                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_GET_STUDENT_NON_BOOK_TRANSACTION_COLLEGE";
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
                      //      Value = ""
                      //  });
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
                        cmd.Parameters.Add(new SqlParameter("@Type2",
                         SqlDbType.VarChar)
                        {
                            Value = "EDIT"
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMBANO_AccessionNo",
                         SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                         SqlDbType.BigInt)
                        {
                            Value = 0
                        });
                        cmd.Parameters.Add(new SqlParameter("@Book_Trans_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.Book_Trans_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] ////// use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.editlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //////here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitlelist = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                         from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                         from d in _LibraryContext.LIB_NonBook_Transaction_DMO
                                         where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && d.LMNBKANO_Id == b.LMNBKANO_Id && d.LNBTR_Id == data.Book_Trans_Id && a.LMNBK_ActiveFlg == true)
                                         select new ClgNonBookTransaction_DTO
                                         {

                                             LMB_Id = a.LMNBK_Id,
                                             MI_Id = a.MI_Id,
                                             //////   LMBA_Id = a.LMBA_Id,
                                             LMB_ClassNo = a.LMNBK_ISBN,
                                             LMB_BookTitle = a.LMNBK_NonBookTitle,
                                             ////// d.tLMS_Id = a.LMS_Id,
                                             //////  LMD_Id = a.LMD_Id,
                                             //////    LMP_Id = a.LMP_Id,
                                             LMB_Price = a.LMNBK_Price,
                                             LMB_VolNo = a.LMNBK_VolumeNo,
                                             LMB_EntryDate = a.LMNBK_PublishDate,
                                             //////LML_Id = a.LML_Id,
                                             ////// Donor_Id = a.Donor_Id,
                                             //////  LMV_Id = a.LMV_Id,
                                             LMC_Id = a.LMC_Id,
                                             LMBANO_Id = b.LMNBKANO_Id,
                                             //////  Rack_Id = b.Rack_Id,
                                             //////  Max_Issue_Days = e.LNBCPAS_IssueDays,
                                             //////Max_No_Renewals = e.LNBCPAS_NoOfRenewals,
                                             //////   Max_Issue_Items = e.LNBCPAS_NoOfItems,
                                             LMBANO_AccessionNo = b.LMNBKANO_AccnNo,


                                         }).Distinct().ToList();


                    data.booktitle = booktitlelist.ToArray();



                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    if (data.booktype == "issue")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPASC_ActiveFlg == true
                    ////                                     ////// && b.ASMCL_Id == data.ASMCL_Id 
                    ////                                     && a.LBCPA_IssueRefFlg == "Issue")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPASC_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPASC_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPASC_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////    else if (data.booktype == "ref")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true
                    ////                                     && b.LBCPASC_ActiveFlg == true
                    ////                                     ////// && b.ASMCL_Id == data.ASMCL_Id
                    ////                                     && a.LBCPA_IssueRefFlg == "Reference")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPASC_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPASC_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPASC_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////}
                    ////else
                    ////{
                      long cat_id = 0;
                        if (booktitlelist.Count > 0)
                        {
                            cat_id = booktitlelist[0].LMC_Id;
                        }
                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPASC_ActiveFlg == true && b.LMC_Id == cat_id/* && b.ASMCL_Id == data.ASMCL_Id*/)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPASC_IssueDays,
                                                         Max_No_Renewals = b.LNBCPASC_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPASC_NoOfItems,

                                                     }).ToArray();
                    ////}



                }
                else if (data.issuertype == "stf")
                {
                    //////in this query we edit only selected data like AMCST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     from e in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from c in _LibraryContext.LIB_Master_NonBook_DMO
                                     from d in _LibraryContext.MasterEmployee
                                     where (a.MI_Id == d.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && b.LMNBK_Id == c.LMNBK_Id && a.MI_Id == c.MI_Id && e.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.LNBTR_Id == data.Book_Trans_Id && a.LNBTR_Id == e.LNBTR_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {
                                         Book_Trans_Id = a.LNBTR_Id,
                                         LMB_Id = c.LMNBK_Id,
                                         HRME_Id = d.HRME_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         Issue_Date = a.LNBTR_IssuedDate,
                                         Due_Date = a.LNBTR_DueDate,
                                         Return_Date = a.LNBTR_ReturnedDate,
                                         Book_Trans_Status = a.LNBTR_Status,
                                         LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                         //////LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMNBK_NonBookTitle,
                                         Fine_Amount = a.LNBTR_TotalFine,
                                         //////Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LNBTR_TotalFine,
                                         HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                         //////  HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
                                         ///////      HRME_EmployeeLastName = d.HRME_EmployeeLastName,
                                         HRME_EmployeeCode = d.HRME_EmployeeCode,
                                         HRMD_Id = Convert.ToInt64(d.HRMD_Id),
                                         HRMDES_Id = Convert.ToInt64(d.HRMDES_Id)

                                     }).Distinct().ToArray();

                    //////here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from d in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     from e in _LibraryContext.LIB_NonBook_Transaction_Staff_DMO
                                     where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && d.LNBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMNBK_Id == b.LMNBK_Id && d.LNBTR_Id == e.LNBTR_Id && e.HRME_Id == data.HRME_Id && d.LMNBKANO_Id == b.LMNBKANO_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {

                                         LMB_Id = a.LMNBK_Id,
                                         MI_Id = a.MI_Id,
                                         //////  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMNBK_ISBN,
                                         LMB_BookTitle = a.LMNBK_NonBookTitle,
                                         ////// LMS_Id = a.LMS_Id,
                                         //////LMD_Id = a.LMD_Id,
                                         //////  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMNBK_Price,
                                         LMB_VolNo = a.LMNBK_VolumeNo,
                                         LMB_EntryDate = a.LMNBK_PublishDate,
                                         ////// LML_Id = a.LML_Id,
                                         //////  Donor_Id = a.Donor_Id,
                                         ////// LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         //////   Rack_Id = b.Rack_Id,
                                         ////// Max_Issue_Days = c.Max_Issue_Days,
                                         ////// Max_No_Renewals = c.Max_No_Renewals,
                                         //////  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();

                    data.booktitle = booktitle.ToArray();

                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    if (data.booktype == "issue")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg == "Issue")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAST_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAST_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////    else if (data.booktype == "ref")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg == "Reference")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAST_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAST_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAST_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }
                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAST_ActiveFlg == true && b.LMC_Id == cat_id && b.HRMGT_Id == data.HRMGT_Id)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAST_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAST_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAST_NoOfItems,

                                                     }).ToArray();
                   //// }



                }

                else if (data.issuertype == "dep")
                {
                    //////in this query we edit only selected data like AMCST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     from e in _LibraryContext.LIB_NonBook_Transaction_Department_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from c in _LibraryContext.LIB_Master_NonBook_DMO
                                     from d in _LibraryContext.HR_Master_Department
                                     where (a.MI_Id == d.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && b.LMNBK_Id == c.LMNBK_Id && a.MI_Id == c.MI_Id && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && a.LNBTR_Id == data.Book_Trans_Id && a.LNBTR_Id == e.LNBTR_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {
                                         Book_Trans_Id = a.LNBTR_Id,
                                         LMB_Id = c.LMNBK_Id,
                                         HRMD_Id = d.HRMD_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         Issue_Date = a.LNBTR_IssuedDate,
                                         Due_Date = a.LNBTR_DueDate,
                                         Return_Date = a.LNBTR_ReturnedDate,
                                         Book_Trans_Status = a.LNBTR_Status,
                                         LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                         //////LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMNBK_NonBookTitle,
                                         Fine_Amount = a.LNBTR_TotalFine,
                                         //////Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LNBTR_FineWaived,
                                         HRMD_DepartmentName = d.HRMD_DepartmentName,
                                         ////// LMD_DepartmentCode = d.LMD_DepartmentCode,
                                     }).Distinct().ToArray();

                    //////here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from d in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     from e in _LibraryContext.LIB_NonBook_Transaction_Department_DMO
                                     where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && d.LNBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMNBK_Id == b.LMNBK_Id && d.LNBTR_Id == e.LNBTR_Id && e.HRMD_Id == data.HRMD_Id && d.LMNBKANO_Id == b.LMNBKANO_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {

                                         LMB_Id = a.LMNBK_Id,
                                         MI_Id = a.MI_Id,
                                         //////  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMNBK_ISBN,
                                         LMB_BookTitle = a.LMNBK_NonBookTitle,
                                         ////// LMS_Id = a.LMS_Id,
                                         //////LMD_Id = a.LMD_Id,
                                         //////  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMNBK_Price,
                                         LMB_VolNo = a.LMNBK_VolumeNo,
                                         LMB_EntryDate = a.LMNBK_PublishDate,
                                         ////// LML_Id = a.LML_Id,
                                         //////  Donor_Id = a.Donor_Id,
                                         ////// LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         //////   Rack_Id = b.Rack_Id,
                                         ////// Max_Issue_Days = c.Max_Issue_Days,
                                         ////// Max_No_Renewals = c.Max_No_Renewals,
                                         //////  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();



                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    if (data.booktype == "issue")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Issue")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////    else if (data.booktype == "ref")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "DEPARTMENT" && a.LBCPA_IssueRefFlg == "Reference")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////}
                    ////else
                    ////{
                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }


                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "DEPARTMENT" && b.LMC_Id == cat_id)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                    ////}




                }

                else if (data.issuertype == "gst")
                {
                    //////in this query we edit only selected data like AMCST_Id and LMBANO_Id
                    data.editlist = (from a in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from c in _LibraryContext.LIB_Master_NonBook_DMO
                                     where (a.MI_Id == c.MI_Id && a.LMNBKANO_Id == b.LMNBKANO_Id && b.LMNBK_Id == c.LMNBK_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.LNBTR_Id == data.Book_Trans_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {
                                         Book_Trans_Id = a.LNBTR_Id,
                                         LMB_Id = c.LMNBK_Id,
                                         MI_Id = a.MI_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         Issue_Date = a.LNBTR_IssuedDate,
                                         Due_Date = a.LNBTR_DueDate,
                                         Return_Date = a.LNBTR_ReturnedDate,
                                         Book_Trans_Status = a.LNBTR_Status,
                                         LMBANO_AccessionNo = b.LMNBKANO_AccnNo,
                                         //////LMB_BookType = c.LMB_BookType,
                                         LMB_BookTitle = c.LMNBK_NonBookTitle,
                                         Fine_Amount = a.LNBTR_TotalFine,
                                         //////Renewal_Counter = a.Renewal_Counter,
                                         Waived_Amount = a.LNBTR_FineWaived,
                                         LBTR_GuestName = a.LNBTR_GuestName,
                                         LBTR_GuestContactNo = a.LNBTR_GuestContactNo,
                                         LBTR_GuestEmailId = a.LNBTR_GuestEmailId,

                                     }).Distinct().ToArray();

                    //////here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
                    var booktitle = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     from d in _LibraryContext.LIB_NonBook_Transaction_DMO
                                     where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && d.LNBTR_Id == data.Book_Trans_Id && d.MI_Id == a.MI_Id && a.LMNBK_Id == b.LMNBK_Id && b.LMNBKANO_Id == d.LMNBKANO_Id)
                                     select new ClgNonBookTransaction_DTO
                                     {

                                         LMB_Id = a.LMNBK_Id,
                                         MI_Id = a.MI_Id,
                                         //////  LMBA_Id = a.LMBA_Id,
                                         LMB_ClassNo = a.LMNBK_ISBN,
                                         LMB_BookTitle = a.LMNBK_NonBookTitle,
                                         ////// LMS_Id = a.LMS_Id,
                                         //////LMD_Id = a.LMD_Id,
                                         //////  LMP_Id = a.LMP_Id,
                                         LMB_Price = a.LMNBK_Price,
                                         LMB_VolNo = a.LMNBK_VolumeNo,
                                         LMB_EntryDate = a.LMNBK_PublishDate,
                                         ////// LML_Id = a.LML_Id,
                                         //////  Donor_Id = a.Donor_Id,
                                         ////// LMV_Id = e.LMV_Id,
                                         LMC_Id = a.LMC_Id,
                                         LMBANO_Id = b.LMNBKANO_Id,
                                         //////   Rack_Id = b.Rack_Id,
                                         ////// Max_Issue_Days = c.Max_Issue_Days,
                                         ////// Max_No_Renewals = c.Max_No_Renewals,
                                         //////  Max_Issue_Items = c.Max_Issue_Items,


                                     }).Distinct().ToList();


                    ////data.booktitle = booktitle.ToArray();
                    ////if (data.bookcat_type == "book")
                    ////{
                    ////    if (data.booktype == "issue")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Issue")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////    else if (data.booktype == "ref")
                    ////    {
                    ////        data.circularparamdetails = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                    ////                                     from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                    ////                                     where (a.MI_Id == data.MI_Id && a.LBCPA_Id == b.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true && a.LBCPA_Flg == "GUEST" && a.LBCPA_IssueRefFlg == "Reference")
                    ////                                     select new ClgNonBookTransaction_DTO
                    ////                                     {
                    ////                                         Max_Issue_Days = b.LBCPAO_IssueDays,
                    ////                                         Max_No_Renewals = b.LBCPAO_NoOfRenewals,
                    ////                                         Max_Issue_Items = b.LBCPAO_NoOfItems,

                    ////                                     }).ToArray();
                    ////    }
                    ////}
                    ////else
                    ////{



                        long cat_id = 0;
                        if (booktitle.Count > 0)
                        {
                            cat_id = booktitle[0].LMC_Id;
                        }


                        data.circularparamdetails = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                                     where (a.MI_Id == data.MI_Id && a.LNBCPA_Id == b.LNBCPA_Id && a.LNBCPA_ActiveFlg == true && b.LNBCPAO_ActiveFlg == true && a.LNBCPA_Flg == "GUEST" && b.LMC_Id == cat_id)
                                                     select new ClgNonBookTransaction_DTO
                                                     {
                                                         Max_Issue_Days = b.LNBCPAO_IssueDays,
                                                         Max_No_Renewals = b.LNBCPAO_NoOfRenewals,
                                                         Max_Issue_Items = b.LNBCPAO_NoOfItems,

                                                     }).ToArray();
                   //// }



                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNonBookTransaction_DTO returndata(ClgNonBookTransaction_DTO data)
        {
            try
            {

                //////for update data
                var update = _LibraryContext.LIB_NonBook_Transaction_DMO.Where(t => t.LNBTR_Id == data.Book_Trans_Id).SingleOrDefault();
                decimal finalfine_amount = 0;
                if (data.issuertype == "std")
                {

                    //////check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //////check Difference Between Actual return date and Today's Return date 
                    if (today > update.LNBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                    }
                    //////difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //////check fine amount according to from date and To Date
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
                    //////check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //////check Difference Between Actual return date and Today's Return date 
                    if (today > update.LNBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                    }
                    ////// difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //////check fine amount according to from date and To Date
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
                    //////check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //////check Difference Between Actual return date and Today's Return date 
                    if (today > update.LNBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                    }
                    ////// difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //////check fine amount according to from date and To Date
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
                    //////check Fine & From Date And To Date category wise for single Transaction
                    var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                    DateTime today = DateTime.Now;
                    today = today.Date;
                    update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                    double difftotaldays = 0;

                    //////check Difference Between Actual return date and Today's Return date 
                    if (today > update.LNBTR_ReturnedDate)
                    {
                        difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                    }
                    ////// difftotaldays = 16;

                    if (fine.Count > 0)
                    {
                        var remaindays = difftotaldays;
                        foreach (var item in fine)
                        {
                            if (difftotaldays > 0)
                            {

                                //////check fine amount according to from date and To Date
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

                update.LNBTR_ReturnedDate = DateTime.Now;
                update.LNBTR_Status = "Return";
                update.LNBTR_FineCollecetd = update.LNBTR_FineCollecetd + Convert.ToDecimal(finalfine_amount);
                update.LNBTR_TotalFine = update.LNBTR_TotalFine + Convert.ToDecimal(finalfine_amount);
                //////if any fine then setupdate.LBTR_FineCollected+Convert.ToDecimal(finalfine_amount); 
                update.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(update);
                int rowAffected = _LibraryContext.SaveChanges();

                if (rowAffected > 0)
                {

                    if (data.issuertype == "std")
                    {

                        var studentid = _LibraryContext.LIB_NonBook_Transaction_Student_College_DMO.Single(t => t.LNBTR_Id == update.LNBTR_Id && t.LNBTRS_ActiveFlg == true);
                        long mobileno = 0;
                        //////var mobilenolist = (from a in _db.Adm_Master_College_StudentDMO
                        //////                    from b in _db.Multiple_Mobile_DMO
                        //////                    where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == studentid.AMCST_Id && a.MI_Id == data.MI_Id
                        //////                    select b).ToList();

                        //////if (mobilenolist.Count > 0)
                        //////{
                        //////    if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                        //////    {
                        //////        mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                        //////    }

                        //////}
                        //////else
                        //////{
                        var mobilenolist1 = (from a in _db.Adm_Master_College_StudentDMO
                                             where a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                                             select new ClgNonBookTransaction_DTO
                                             {
                                                 AMCST_MobileNo = a.AMCST_MobileNo
                                             }).ToList();
                        if (mobilenolist1.Count > 0)
                        {
                            if (mobilenolist1[0].AMCST_MobileNo != 0 && mobilenolist1[0].AMCST_MobileNo != null)
                            {
                                mobileno = Convert.ToInt64(mobilenolist1[0].AMCST_MobileNo);
                            }
                        }
                        ////// }
                        ////// mobileno = 9591081840;
                        if (mobileno != 0)
                        {
                            SMS sms = new SMS(_db);
                            //string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKRETURN", data.ASMAY_Id, studentid.AMCST_Id, update.LMNBKANO_Id).Result;
                        }

                        string email_id = "";
                        //////var emailidlist = (from a in _db.Adm_Master_College_StudentDMO
                        //////                   from b in _db.Adm_M_Student_Email_Id
                        //////                   where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == studentid.AMCST_Id && a.MI_Id == data.MI_Id
                        //////                   select b).ToList();

                        //////if (emailidlist.Count > 0)
                        //////{
                        //////    if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                        //////    {
                        //////        email_id = emailidlist[0].AMSTE_EmailId;
                        //////    }

                        //////}
                        //////else
                        //////{
                        var emailidlist1 = (from a in _db.Adm_Master_College_StudentDMO
                                            where a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id
                                            select new ClgNonBookTransaction_DTO
                                            {
                                                AMCST_emailId = a.AMCST_emailId
                                            }).ToList();
                        if (emailidlist1.Count > 0)
                        {
                            if (emailidlist1[0].AMCST_emailId != "")
                            {
                                email_id = emailidlist1[0].AMCST_emailId;
                            }

                        }
                        ////// }
                        ////// email_id = "praveenishwar@vapstech.com";
                        if (email_id != "" && email_id != null)
                        {
                            Email email = new Email(_db);
                           // string s = email.sendmail_library(data.MI_Id, email_id, "BOOKRETURN", data.ASMAY_Id, studentid.AMCST_Id, update.LMNBKANO_Id);
                        }
                    }
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

        public ClgNonBookTransaction_DTO renewaldata(ClgNonBookTransaction_DTO data)
        {
            try
            {
                if (data.Book_Trans_Id > 0)
                {

                    //////for update data
                    var update = _LibraryContext.LIB_NonBook_Transaction_DMO.Where(t => t.LNBTR_Id == data.Book_Trans_Id).SingleOrDefault();

                    decimal finalfine_amount = 0;
                    if (data.issuertype == "std")
                    {

                        //////check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STUDENT" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //////check Difference Between Actual return date and Today's Return date 
                        if (today > update.LNBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                        }
                        //////  difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //////check fine amount according to from date and To Date
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
                        //////check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "STAFF" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //////check Difference Between Actual return date and Today's Return date 
                        if (today > update.LNBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                        }
                        ////// difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //////check fine amount according to from date and To Date
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
                        //////check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "DEPARTMENT" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //////check Difference Between Actual return date and Today's Return date 
                        if (today > update.LNBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                        }
                        ////// difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //////check fine amount according to from date and To Date
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
                        //////check Fine & From Date And To Date category wise for single Transaction
                        var fine = _LibraryContext.MasterTimeSlabDMO.Where(t => t.MI_Id == data.MI_Id && t.LFSE_UserType == "GUEST" && t.LFSE_ActiveFlg == true).ToList();
                        DateTime today = DateTime.Now;
                        today = today.Date;
                        update.LNBTR_ReturnedDate = update.LNBTR_ReturnedDate;
                        double difftotaldays = 0;

                        //////check Difference Between Actual return date and Today's Return date 
                        if (today > update.LNBTR_ReturnedDate)
                        {
                            difftotaldays = (today - update.LNBTR_ReturnedDate).TotalDays;
                        }
                        ////// difftotaldays = 16;

                        if (fine.Count > 0)
                        {
                            var remaindays = difftotaldays;
                            foreach (var item in fine)
                            {
                                if (difftotaldays > 0)
                                {

                                    //////check fine amount according to from date and To Date
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
                    ////check Maximum Renewal time according to Circulation parameter Page
                    if (data.Max_No_Renewals > update.LNBTR_RenewalCount)
                    {
                        data.renew = false;
                        update.LNBTR_IssuedDate = data.Issue_Date;
                        update.LNBTR_RenewalCount = update.LNBTR_RenewalCount + 1;
                        update.LNBTR_DueDate = data.Due_Date;
                        update.LNBTR_ReturnedDate = data.Return_Date;
                        update.LNBTR_Status = "Issue";
                        update.LNBTR_FineCollecetd = update.LNBTR_FineCollecetd + Convert.ToDecimal(finalfine_amount);
                        update.LNBTR_TotalFine = update.LNBTR_TotalFine + Convert.ToDecimal(finalfine_amount);
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

                            var studentid = _LibraryContext.LIB_NonBook_Transaction_Student_College_DMO.Single(t => t.LNBTR_Id == update.LNBTR_Id && t.LNBTRS_ActiveFlg == true);
                            long mobileno = 0;
                            //////var mobilenolist = (from a in _db.Adm_Master_College_StudentDMO
                            //////                    from b in _db.Adm_M_Student_MobileNo
                            //////                    where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == studentid.AMCST_Id && a.MI_Id == data.MI_Id
                            //////                    select b).ToList();

                            //////if (mobilenolist.Count > 0)
                            //////{
                            //////    if (mobilenolist[0].AMSTSMS_MobileNo != "" && mobilenolist[0].AMSTSMS_MobileNo != null)
                            //////    {
                            //////        mobileno = Convert.ToInt64(mobilenolist[0].AMSTSMS_MobileNo);
                            //////    }

                            //////}
                            //////else
                            //////{
                            var mobilenolist1 = (from a in _db.Adm_Master_College_StudentDMO
                                                 where a.MI_Id == data.MI_Id && a.AMCST_Id == studentid.AMCST_Id
                                                 select new ClgNonBookTransaction_DTO
                                                 {
                                                     AMCST_MobileNo = a.AMCST_MobileNo
                                                 }).ToList();

                            if (mobilenolist1.Count > 0)
                            {
                                if (mobilenolist1[0].AMCST_MobileNo != 0 && mobilenolist1[0].AMCST_MobileNo != null)
                                {
                                    mobileno = Convert.ToInt64(mobilenolist1[0].AMCST_MobileNo);
                                }

                            }
                            ////// }
                            //////  mobileno = 9591081840;
                            if (mobileno != 0)
                            {
                                SMS sms = new SMS(_db);
                            //    string s = sms.sendSms_library(data.MI_Id, mobileno, "BOOKRENEWAL", data.ASMAY_Id, studentid.AMCST_Id, update.LMNBKANO_Id).Result;
                            }

                            string email_id = "";
                            //////var emailidlist = (from a in _db.Adm_Master_College_StudentDMO
                            //////                   from b in _db.Adm_M_Student_Email_Id
                            //////                   where a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == studentid.AMCST_Id && a.MI_Id == data.MI_Id
                            //////                   select b).ToList();

                            //////if (emailidlist.Count > 0)
                            //////{
                            //////    if (emailidlist[0].AMSTE_EmailId != "" && emailidlist[0].AMSTE_EmailId != null)
                            //////    {
                            //////        email_id = emailidlist[0].AMSTE_EmailId;
                            //////    }

                            //////}
                            //////else
                            //////{
                            var emailidlist1 = (from a in _db.Adm_Master_College_StudentDMO
                                                where a.MI_Id == data.MI_Id && a.AMCST_Id == studentid.AMCST_Id
                                                select new ClgNonBookTransaction_DTO
                                                {
                                                    AMCST_emailId = a.AMCST_emailId
                                                }).ToList();

                            if (emailidlist1.Count > 0)
                            {
                                if (emailidlist1[0].AMCST_emailId != "")
                                {
                                    email_id = emailidlist1[0].AMCST_emailId;
                                }

                            }
                            //  }
                            //   email_id = "praveenishwar@vapstech.com";
                            if (email_id != "" && email_id != null)
                            {
                                Email email = new Email(_db);
                               // string s = email.sendmail_library(data.MI_Id, email_id, "BOOKRENEWAL", data.ASMAY_Id, studentid.AMCST_Id, update.LMNBKANO_Id);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgNonBookTransaction_DTO get_staff1(ClgNonBookTransaction_DTO data)
        {
            try
            {
                data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                   select new ClgNonBookTransaction_DTO
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

        public ClgNonBookTransaction_DTO getdepchange(ClgNonBookTransaction_DTO data)
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


    }
}
