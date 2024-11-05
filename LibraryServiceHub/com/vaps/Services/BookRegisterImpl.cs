using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class BookRegisterImpl : Interfaces.BookRegisterInterface
    {

        public LibraryContext _LibraryContext;
        readonly ILogger<BookRegisterImpl> _logger;
        public BookRegisterImpl(LibraryContext context, DomainModelMsSqlServerContext context1, ILogger<BookRegisterImpl> log)
        {
            _LibraryContext = context;
            _logger = log;
        }


        public BookRegisterDTO getdetails(BookRegisterDTO data)
        {

            try
            {
                data.subjectlist = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_ActiveFlg == true).Distinct().OrderBy(t => t.LMS_Id).ToArray();
                data.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_ActiveFlg == true).Distinct().OrderBy(t => t.LMD_Id).ToArray();

                data.racklist = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_ActiveFlag == true).Distinct().OrderBy(t => t.LMRA_Id).ToArray();

                data.langlist = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_ActiveFlg == true).Distinct().OrderBy(t => t.LML_Id).ToArray();

                // data.donorlist = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id ==  data.MI_Id  && t.Donor_ActiveFlag==true).Distinct().ToArray();

                data.vendorlist = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_ActiveFlg == true).Distinct().OrderBy(t => t.LMV_Id).ToArray();

                data.publisherlst = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_ActiveFlg == true).Distinct().OrderBy(t => t.LMP_Id).ToArray();
                data.accessorieslist = _LibraryContext.LIB_Master_Accessories_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAC_ActiveFlg == true).Distinct().OrderBy(t => t.LMAC_Id).ToArray();
                //data.librarylist = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == id).Distinct().OrderBy(t => t.LMAL_Id).ToArray();

                var librarylist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                   from b in _LibraryContext.LIB_User_Library_DMO
                                   where (a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true)
                                   select a).Distinct().OrderBy(t => t.LMAL_Id).ToList();

                data.librarylist = librarylist.ToArray();
                //if (librarylist.Count > 0)
                //{
                //    if (data.LMAL_Id == 0)
                //    {
                //        data.LMAL_Id = librarylist.FirstOrDefault().LMAL_Id;
                //    }

                //}
                //   data.authorlst = _LibraryContext.MasterAuthorDMO.Where(t => t.LMBA_ActiveFlg == true).Distinct().ToArray();

                data.authorlst = (from a in _LibraryContext.LIB_Master_Author_DMO.Where(a => a.MI_Id == data.MI_Id && a.LMAU_ActiveFlg == true)
                                  select new BookRegisterDTO
                                  {
                                      LMAU_Id = a.LMAU_Id,
                                      LMAU_AuthorFirstName = ((a.LMAU_AuthorFirstName == null ? " " : a.LMAU_AuthorFirstName) + " " + (a.LMAU_AuthorMiddleName == null ? " " : a.LMAU_AuthorMiddleName) + " " + (a.LMAU_AuthorLastName == null ? " " : a.LMAU_AuthorLastName)).Trim(),
                                      // LMAU_AuthorMiddleName = a.LMAU_AuthorMiddleName,
                                      // LMAU_AuthorLastName = a.LMAU_AuthorLastName,
                                  }).Distinct().OrderBy(a => a.LMBA_Id).Take(20).ToArray();


                //data.classlist = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).Distinct().ToArray();

                data.categorylist = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_ActiveFlag == true).Distinct().ToArray();
                //&& t.LMC_BNBFlg== "Book"



                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_BOOK_REGISTER_MAIN_LOAD_NEW";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.IVRMUL_Id
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }

        public BookRegisterDTO Ckeck_ISBNNO(BookRegisterDTO data)
        {
            try
            {
                var receipt_count = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_ISBNNo.Contains(data.LMB_ISBNNo)).ToList().Count;
                if (receipt_count == 0)
                {
                    data.returnval = false;
                }
                else if (receipt_count > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public BookRegisterDTO chekAccno(BookRegisterDTO data)
        {
            try
            {
                var receipt_count = _LibraryContext.Lib_M_Book_Accn_DMO.Where(t => t.LMBANO_AccessionNo.Contains(data.LMBANO_AccessionNo)).ToList().Count;
                if (receipt_count == 0)
                {
                    data.returnval = false;
                }
                else if (receipt_count > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BookRegisterDTO Ckeck_LMBANO_AccessionNo(BookRegisterDTO data)
        {
            try
            {
                var acn_count = (from a in _LibraryContext.BookRegisterDMO
                                 from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                 where a.MI_Id == data.MI_Id && a.LMB_Id == b.LMB_Id && b.LMBANO_AccessionNo.Trim() == data.LMBANO_AccessionNo.Trim() && b.LMBANO_AvialableStatus.Trim() == "Available"
                                 select b
                              ).ToList();


                if (acn_count.Count == 0)
                {
                    data.returnval = false;
                }
                else if (acn_count.Count > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BookRegisterDTO Addaccnno(BookRegisterDTO data)
        {
            try
            {
                var booklist = _LibraryContext.BookRegisterDMO.Where(w => w.MI_Id == data.MI_Id && w.LMB_Id == data.LMB_Id).ToList();
                if (booklist.Count > 0)
                {
                    foreach (var act1 in data.savetmpdata)
                    {
                        Lib_M_Book_Accn_DMO obj2 = new Lib_M_Book_Accn_DMO();
                        obj2.LMB_Id = data.LMB_Id;
                        obj2.LMBANO_AccessionNo = act1.LMBANO_No;
                        obj2.LMBANO_AvialableStatus = "Available";
                        obj2.LMRA_Id = data.LMRA_Id;
                        obj2.LMBANO_LostDamagedDate = DateTime.Now;
                        obj2.LMBANO_LostDamagedReason = "";
                        obj2.LMBANO_ModeOfPayment = "";
                        obj2.LMBANO_AmountCollected = 0;
                        obj2.LMBANO_LostDamagedFlg = false;
                        obj2.LMBANO_ActiveFlg = true;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj2);
                    }


                    var booklist1 = _LibraryContext.BookRegisterDMO.Where(w => w.MI_Id == data.MI_Id && w.LMB_Id == data.LMB_Id).SingleOrDefault();

                    var newprice = booklist1.LMB_Price * data.LMB_NoOfCopies;

                    booklist1.LMB_NetPrice = booklist1.LMB_NetPrice + Convert.ToDecimal(newprice);
                    booklist1.LMB_NoOfCopies = Convert.ToInt64(booklist1.LMB_NoOfCopies + data.LMB_NoOfCopies);
                    _LibraryContext.Update(booklist1);

                }

                var receipt_count = _LibraryContext.SaveChanges();
                if (receipt_count == 0)
                {
                    data.returnval = false;
                }
                else if (receipt_count > 0)
                {
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public BookRegisterDTO Savedata(BookRegisterDTO data)
        {
            try
            {
                if (data.LMB_Id > 0)
                {
                    var update1 = _LibraryContext.BookRegisterDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();


                    update1.LMB_WithAccessories = data.With_Accessories;

                    update1.LMB_NoOfCopies = Convert.ToInt64(data.LMB_NoOfCopies);

                    update1.LMB_Price = data.LMB_Price;

                    update1.LMB_NetPrice = data.LMB_NetPrice;
                    update1.LMB_Discount = Convert.ToInt64(data.LMB_Discount);

                    update1.UpdatedDate = DateTime.Now;

                    _LibraryContext.Update(update1);

                    int c = 0;

                    foreach (var abc in data.savetmpdata)
                    {
                        var accno1 = (from a in _LibraryContext.BookRegisterDMO
                                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                      where (a.LMB_Id == b.LMB_Id && a.LMB_Id != data.LMB_Id && a.MI_Id == data.MI_Id && b.LMBANO_AccessionNo == abc.LMBANO_No)
                                      select b).ToArray().ToList();


                        if (accno1.Count() > 0)
                        {
                            data.chkaccessionno = true;
                            c += 1;
                        }
                    }

                    if (data.With_Accessories == true)
                    {

                        var acclist = _LibraryContext.LIB_Master_Book_Accessories_DMO.Where(ff => ff.LMB_Id == data.LMB_Id).ToList();
                        if (acclist.Count > 0)
                        {


                            var acclist1 = _LibraryContext.LIB_Master_Book_Accessories_DMO.Where(ff => ff.LMB_Id == data.LMB_Id).SingleOrDefault();

                            acclist1.LMAC_Id = data.LMAC_Id;
                            acclist1.UpdatedDate = DateTime.Now;
                            _LibraryContext.Update(acclist1);


                        }
                        else
                        {
                            LIB_Master_Book_Accessories_DMO obj6 = new LIB_Master_Book_Accessories_DMO();
                            obj6.LMB_Id = data.LMB_Id;
                            obj6.LMAC_Id = data.LMAC_Id;
                            obj6.LMBAC_ActiveFlg = true;
                            obj6.CreatedDate = DateTime.Now;
                            obj6.UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj6);
                        }

                    }



                    if (c == 0)
                    {

                        if (data.LMBANO_Id == 0)
                        {
                            foreach (var act1 in data.savetmpdata)
                            {
                                Lib_M_Book_Accn_DMO obj2 = new Lib_M_Book_Accn_DMO();
                                obj2.LMB_Id = data.LMB_Id;
                                obj2.LMBANO_AccessionNo = act1.LMBANO_No;
                                obj2.LMBANO_AvialableStatus = data.LMBANO_AvialableStatus;
                                obj2.LMRA_Id = data.LMRA_Id;
                                obj2.LMBANO_LostDamagedDate = DateTime.Now;
                                obj2.LMBANO_LostDamagedReason = "";
                                obj2.LMBANO_ModeOfPayment = "";
                                obj2.LMBANO_AmountCollected = 0;
                                obj2.LMBANO_LostDamagedFlg = false;
                                obj2.LMBANO_ActiveFlg = true;
                                obj2.CreatedDate = DateTime.Now;
                                obj2.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj2);
                            }
                        }
                        else
                        {
                            foreach (var act1 in data.savetmpdata)
                            {

                                var update2 = _LibraryContext.Lib_M_Book_Accn_DMO.Where(t => t.LMBANO_Id == data.LMBANO_Id).SingleOrDefault();

                                update2.LMBANO_AccessionNo = act1.LMBANO_No;
                                update2.LMBANO_AvialableStatus = data.LMBANO_AvialableStatus;
                                update2.LMRA_Id = data.LMRA_Id;
                                update2.UpdatedDate = DateTime.Now;
                                _LibraryContext.Update(update2);
                            }
                        }
                    }




                    if (data.LMB_Id > 0)
                    {

                        var check = _LibraryContext.MasterAuthorDMO.Where(t => t.LMB_Id == data.LMB_Id).ToArray().ToList();

                        if (check.Count() > 0)
                        {

                            string firstname = "";
                            string middlename = "";
                            string lastname = "";

                            var update4 = _LibraryContext.MasterAuthorDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();

                            var name = _LibraryContext.LIB_Master_Author_DMO.Where(d => d.LMAU_Id == data.LMBA_Id).ToList();

                            if (name.Count > 0)
                            {
                                firstname = name.FirstOrDefault().LMAU_AuthorFirstName;
                                middlename = name.FirstOrDefault().LMAU_AuthorMiddleName;
                                lastname = name.FirstOrDefault().LMAU_AuthorLastName;

                                update4.LMBA_AuthorFirstName = firstname;
                                update4.LMBA_AuthorMiddleName = middlename;
                                update4.LMBA_AuthorLastName = lastname;
                                update4.UpdatedDate = DateTime.Now;
                                update4.LMAU_Id = data.LMBA_Id;
                                _LibraryContext.Update(update4);
                            }
                            else
                            {
                                firstname = data.LMBA_AuthorFirstName;
                                middlename = data.LMBA_AuthorMiddleName;
                                lastname = data.LMBA_AuthorLastName;

                                LIB_Master_Author_DMO obj9 = new LIB_Master_Author_DMO();

                                obj9.MI_Id = data.MI_Id;
                                obj9.LMAU_AuthorFirstName = firstname;
                                obj9.LMAU_AuthorMiddleName = middlename;
                                obj9.LMAU_AuthorLastName = lastname;
                                obj9.LMAU_EmailId = "";
                                obj9.LMAU_MobileNo = 0;
                                obj9.LMAU_PhoneNo = "";
                                obj9.LMAU_Address = "";
                                obj9.CreatedDate = DateTime.Now;
                                obj9.UpdatedDate = DateTime.Now;
                                obj9.LMAU_ActiveFlg = true;

                                _LibraryContext.Add(obj9);


                                update4.LMBA_AuthorFirstName = firstname;
                                update4.LMBA_AuthorMiddleName = middlename;
                                update4.LMBA_AuthorLastName = lastname;
                                update4.UpdatedDate = DateTime.Now;
                                update4.LMAU_Id = obj9.LMAU_Id;
                                _LibraryContext.Update(update4);

                            }




                        }
                        else
                        {
                            if (data.LMBA_Id > 0)
                            {
                                var authorid = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAU_Id == data.LMBA_Id).ToList();
                                if (authorid.Count > 0)
                                {
                                    var update45 = _LibraryContext.MasterAuthorDMO.Where(t => t.LMB_Id == data.LMB_Id).ToList();
                                    if (update45.Count > 0)
                                    {
                                        var update47 = _LibraryContext.MasterAuthorDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();

                                        var name = _LibraryContext.LIB_Master_Author_DMO.Where(d => d.LMAU_Id == data.LMBA_Id).ToList();
                                        string firstname = name.FirstOrDefault().LMAU_AuthorFirstName;
                                        string middlename = name.FirstOrDefault().LMAU_AuthorMiddleName;
                                        string lastname = name.FirstOrDefault().LMAU_AuthorLastName;

                                        update47.LMBA_AuthorFirstName = firstname;
                                        update47.LMBA_AuthorMiddleName = middlename;
                                        update47.LMBA_AuthorLastName = lastname;
                                        update47.UpdatedDate = DateTime.Now;
                                        update47.LMAU_Id = data.LMBA_Id;
                                        _LibraryContext.Update(update47);
                                    }
                                    else
                                    {

                                        string firstname1 = authorid.FirstOrDefault().LMAU_AuthorFirstName;
                                        string middlename1 = authorid.FirstOrDefault().LMAU_AuthorMiddleName;
                                        string lastname1 = authorid.FirstOrDefault().LMAU_AuthorLastName;
                                        MasterAuthorDMO obj13 = new MasterAuthorDMO();
                                        obj13.LMB_Id = data.LMB_Id;
                                        obj13.LMAU_Id = data.LMBA_Id;
                                        obj13.LMBA_AuthorFirstName = firstname1;
                                        obj13.LMBA_AuthorMiddleName = middlename1;
                                        obj13.LMBA_AuthorLastName = lastname1;
                                        obj13.LMBA_MainAuthorFlg = true;
                                        obj13.LMBA_ActiveFlg = true;
                                        obj13.CreatedDate = DateTime.Now;
                                        obj13.UpdatedDate = DateTime.Now;
                                        _LibraryContext.Add(obj13);
                                    }

                                }
                            }
                            else
                            {
                                LIB_Master_Author_DMO obj9 = new LIB_Master_Author_DMO();

                                obj9.MI_Id = data.MI_Id;
                                obj9.LMAU_AuthorFirstName = data.LMBA_AuthorFirstName;
                                obj9.LMAU_AuthorMiddleName = data.LMBA_AuthorMiddleName;
                                obj9.LMAU_AuthorLastName = data.LMBA_AuthorLastName;
                                obj9.LMAU_EmailId = "";
                                obj9.LMAU_MobileNo = 0;
                                obj9.LMAU_PhoneNo = "";
                                obj9.LMAU_Address = "";
                                obj9.CreatedDate = DateTime.Now;
                                obj9.UpdatedDate = DateTime.Now;
                                obj9.LMAU_ActiveFlg = true;

                                _LibraryContext.Add(obj9);


                                MasterAuthorDMO obj4 = new MasterAuthorDMO();
                                obj4.LMB_Id = data.LMB_Id;
                                obj4.LMAU_Id = obj9.LMAU_Id;
                                obj4.LMBA_AuthorFirstName = data.LMBA_AuthorFirstName;
                                obj4.LMBA_AuthorMiddleName = data.LMBA_AuthorMiddleName;
                                obj4.LMBA_AuthorLastName = data.LMBA_AuthorLastName;
                                obj4.LMBA_MainAuthorFlg = true;
                                obj4.LMBA_ActiveFlg = true;
                                obj4.CreatedDate = DateTime.Now;
                                obj4.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj4);
                            }




                        }

                    }


                    if (data.BookFilesPdf != null && data.BookFilesPdf.Length > 0)
                    {
                        var BookFileList = _LibraryContext.Master_Book_FilesDMO.Where(R => R.LMB_Id == data.LMB_Id).ToList();


                        foreach (var d in BookFileList)
                        {

                            _LibraryContext.Remove(d);
                        }
                        foreach (var d in data.BookFilesPdf)
                        {
                            Master_Book_FilesDMO obj = new Master_Book_FilesDMO();
                            obj.LMB_Id = data.LMB_Id;
                            obj.LMBFILE_FileName = d.LMBFILE_FileName;
                            obj.LMBFILE_FilePath = d.LMBFILE_FilePath;
                            obj.LMBFILE_ActiveFlg = true;
                            obj.LMBFILE_CreatedBy = data.UserId;
                            obj.LMBFILE_UpdatedBy = data.UserId;
                            obj.LMBFILE_CreatedDate = DateTime.Now;
                            obj.LMBFILE_UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj);

                        }


                    }
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
                else
                {
                    //var Duplicate = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMBA_Id == data.LMBA_Id && t.LMS_Id == data.LMS_Id && t.LMB_BookTitle==data.LMB_BookTitle).ToList();

                    //var Duplicate = (from a in _LibraryContext.BookRegisterDMO
                    //                 from b in _LibraryContext.MasterAuthorDMO
                    //                 where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_Id != data.LMB_Id && b.LMBA_Id == data.LMBA_Id && a.LMS_Id == data.LMS_Id && a.LMB_BookTitle == data.LMB_BookTitle)
                    //                 select a).ToArray().ToList();


                    //var Duplicate = (from a in _LibraryContext.BookRegisterDMO
                    //                 from b in _LibraryContext.LIB_Master_Book_Library_DMO
                    //                 where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMS_Id == data.LMS_Id && a.LMB_BookTitle.Trim() == data.LMB_BookTitle.Trim() && b.LMAL_Id==data.LMAL_Id)
                    //                 select a).ToArray().ToList();

                    //if (Duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    BookRegisterDMO obj1 = new BookRegisterDMO();

                    obj1.MI_Id = data.MI_Id;
                    obj1.LMB_BookTitle = data.LMB_BookTitle;
                    obj1.LMB_BookSubTitle = data.MB_Subtitle;
                    obj1.LMC_Id = data.LMC_Id;
                    obj1.LMB_BookType = data.LMB_BookType;
                    obj1.LMS_Id = data.LMS_Id;
                    obj1.LMD_Id = data.LMD_Id;
                    obj1.LMB_BindingType = data.Binding_Type;
                    obj1.LMB_Edition = data.LMB_Edition;
                    obj1.LMB_PublishedYear = data.LMB_PublishedYear;
                    // obj1.LMB_EntryDate = data.LMB_EntryDate;
                    obj1.LMB_EntryDate = Convert.ToDateTime(data.LMB_EntryDate);
                    obj1.LMB_CallNo = data.MB_Call_No;
                    obj1.LMB_ISBNNo = data.LMB_ISBNNo;
                    obj1.LMB_ClassNo = data.LMB_ClassNo;
                    obj1.LMB_VolNo = data.LMB_VolNo;
                    obj1.LMB_NoOfPages = data.MB_Pages;
                    obj1.LMB_BookImage = data.Book_Image;
                    obj1.LMB_NoOfCopies = Convert.ToInt64(data.LMB_NoOfCopies);
                    // obj1.LMB_PurchaseDate = data.Purchase_Date;
                    obj1.LMB_PurchaseDate = Convert.ToDateTime(data.Purchase_Date);
                    obj1.LML_Id = data.LML_Id;
                    obj1.LMB_BillNo = data.LMB_BillNo;
                    obj1.LMB_WithAccessories = data.With_Accessories;
                    // obj1.Invoice_No = data.Invoice_No;
                    obj1.LMB_VoucherNo = data.Voucher_No;
                    obj1.LMB_Price = data.LMB_Price;
                    obj1.LMB_DonorName = data.Donor_Id;
                    obj1.LMB_CurrenceyType = data.CurrencyType;
                    //  obj1.Source_Type = data.Source_Type;
                    obj1.LMB_NetPrice = data.LMB_NetPrice;
                    // obj1.LMV_Id = data.LMV_Id;
                    //  obj1.ForTheClass = data.ForTheClass; //This Will be Change into asmcL_Id(using this id take class Name)
                    obj1.LMB_Keywords = data.MB_Keywords;
                    obj1.LMP_Id = data.LMP_Id;
                    obj1.LMB_Remarks = data.MB_Remarks;
                    // obj1.MB_Disc_Type = data.MB_Disc_Type;
                    obj1.LMB_Discount = Convert.ToInt64(data.LMB_Discount);
                    obj1.LMB_Biblography = data.Bibliography_Page;
                    obj1.LMB_IndexPage = data.Index_Page;
                    // obj1.LMBA_Id = data.LMBA_Id;
                    obj1.LMB_PurOrDonated = data.LMB_PurOrDonated;
                    obj1.LMB_DonorAddress = data.LMB_DonorAddress;
                    obj1.LMB_BookNo = data.LMB_BookNo;


                    obj1.LMB_ActiveFlg = true;
                    obj1.CreatedDate = DateTime.Now;
                    obj1.UpdatedDate = DateTime.Now;

                    _LibraryContext.Add(obj1);


                    if (data.BookFilesPdf != null && data.BookFilesPdf.Length > 0)
                    {

                        foreach (var d in data.BookFilesPdf)
                        {
                            Master_Book_FilesDMO obj = new Master_Book_FilesDMO();
                            obj.LMB_Id = obj1.LMB_Id;
                            obj.LMBFILE_FileName = d.LMBFILE_FileName;
                            obj.LMBFILE_FilePath = d.LMBFILE_FilePath;
                            obj.LMBFILE_ActiveFlg = true;
                            obj.LMBFILE_CreatedBy = data.UserId;
                            obj.LMBFILE_UpdatedBy = data.UserId;
                            obj.LMBFILE_CreatedDate = DateTime.Now;
                            obj.LMBFILE_UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj);

                        }


                    }

                    LIB_Master_Book_VendorDMO obj3 = new LIB_Master_Book_VendorDMO();

                    obj3.LMB_Id = obj1.LMB_Id;
                    obj3.LMV_Id = data.LMV_Id;
                    obj3.LMBV_ActiveFlg = true;
                    obj3.CreatedDate = DateTime.Now;
                    obj3.UpdatedDate = DateTime.Now;
                    _LibraryContext.Add(obj3);


                    LIB_Master_Book_KeyFactor_DMO keyfact = new LIB_Master_Book_KeyFactor_DMO();

                    //keyfact.LMBKF_Id = data.LMBKF_Id;
                    keyfact.LMB_Id = obj1.LMB_Id;
                    keyfact.LMBKF_KeyFactor = data.LMBKF_KeyFactor;
                    keyfact.LMBKF_PageNo = data.LMBKF_PageNo;
                    keyfact.LMBKF_ActiveFlg = data.LMBKF_ActiveFlg;
                    keyfact.CreatedBy = data.UserId;
                    keyfact.UpdatedBy = data.UserId;
                    keyfact.CreatedDate = DateTime.Now;
                    keyfact.UpdatedDate = DateTime.Now;
                    _LibraryContext.Add(keyfact);

                    if (data.LMBA_Id == 0)
                    {
                        LIB_Master_Author_DMO obj9 = new LIB_Master_Author_DMO();

                        obj9.MI_Id = data.MI_Id;
                        obj9.LMAU_AuthorFirstName = data.LMBA_AuthorFirstName;
                        obj9.LMAU_AuthorMiddleName = data.LMBA_AuthorMiddleName;
                        obj9.LMAU_AuthorLastName = data.LMBA_AuthorLastName;
                        obj9.LMAU_EmailId = "";
                        obj9.LMAU_MobileNo = 0;
                        obj9.LMAU_PhoneNo = "";
                        obj9.LMAU_Address = "";
                        obj9.CreatedDate = DateTime.Now;
                        obj9.UpdatedDate = DateTime.Now;
                        obj9.LMAU_ActiveFlg = true;

                        _LibraryContext.Add(obj9);


                        MasterAuthorDMO obj4 = new MasterAuthorDMO();
                        obj4.LMB_Id = obj1.LMB_Id;
                        obj4.LMAU_Id = obj9.LMAU_Id;
                        obj4.LMBA_AuthorFirstName = data.LMBA_AuthorFirstName;
                        obj4.LMBA_AuthorMiddleName = data.LMBA_AuthorMiddleName;
                        obj4.LMBA_AuthorLastName = data.LMBA_AuthorLastName;
                        obj4.LMBA_MainAuthorFlg = true;
                        obj4.LMBA_ActiveFlg = true;
                        obj4.CreatedDate = DateTime.Now;
                        obj4.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj4);
                    }
                    else
                    {
                        if (data.LMBA_Id > 0)
                        {
                            var author = _LibraryContext.LIB_Master_Author_DMO.Where(d => d.LMAU_Id == data.LMBA_Id).ToList();
                            var author1 = author.FirstOrDefault().LMAU_AuthorFirstName;
                            var autho21 = author.FirstOrDefault().LMAU_AuthorMiddleName;
                            var author12 = author.FirstOrDefault().LMAU_AuthorLastName;

                            MasterAuthorDMO obj4 = new MasterAuthorDMO();
                            obj4.LMB_Id = obj1.LMB_Id;
                            obj4.LMBA_AuthorFirstName = author1;
                            obj4.LMBA_AuthorMiddleName = autho21;
                            obj4.LMBA_AuthorLastName = author12;
                            obj4.LMBA_MainAuthorFlg = true;
                            obj4.LMBA_ActiveFlg = true;
                            obj4.CreatedDate = DateTime.Now;
                            obj4.UpdatedDate = DateTime.Now;
                            obj4.LMAU_Id = data.LMBA_Id;
                            _LibraryContext.Add(obj4);
                        }


                    }




                    LIB_Master_Book_Library_DMO obj5 = new LIB_Master_Book_Library_DMO();
                    obj5.LMB_Id = obj1.LMB_Id;
                    obj5.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                    obj5.LMBL_ActiveFlg = true;
                    obj5.CreatedDate = DateTime.Now;
                    obj5.UpdatedDate = DateTime.Now;

                    LIB_Master_Book_Accessories_DMO obj6 = new LIB_Master_Book_Accessories_DMO();
                    obj6.LMB_Id = obj1.LMB_Id;
                    obj6.LMAC_Id = data.LMAC_Id;
                    obj6.LMBAC_ActiveFlg = true;
                    obj6.CreatedDate = DateTime.Now;
                    obj6.UpdatedDate = DateTime.Now;
                    _LibraryContext.Add(obj6);

                    _LibraryContext.Add(obj5);



                    int c = 0;

                    foreach (var abc in data.savetmpdata)
                    {
                        var accno1 = (from a in _LibraryContext.BookRegisterDMO
                                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && b.LMBANO_AccessionNo == abc.LMBANO_No)
                                      select b).ToArray().ToList();


                        if (accno1.Count() > 0)
                        {
                            data.chkaccessionno = true;
                            c += 1;
                        }
                    }


                    if (c == 0)
                    {
                        foreach (var act1 in data.savetmpdata)
                        {
                            Lib_M_Book_Accn_DMO obj2 = new Lib_M_Book_Accn_DMO();
                            obj2.LMB_Id = obj1.LMB_Id;
                            obj2.LMBANO_AccessionNo = act1.LMBANO_No;
                            obj2.LMBANO_AvialableStatus = data.LMBANO_AvialableStatus;
                            obj2.LMRA_Id = data.LMRA_Id;
                            obj2.LMBANO_LostDamagedDate = DateTime.Now;
                            obj2.LMBANO_LostDamagedReason = "";
                            obj2.LMBANO_ModeOfPayment = "";
                            obj2.LMBANO_AmountCollected = 0;
                            obj2.LMBANO_LostDamagedFlg = false;
                            obj2.LMBANO_ActiveFlg = true;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj2);
                        }
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

                    //if (data.chkaccessionno != true)
                    //{

                    //    Lib_M_Book_Accn_DMO obj2 = new Lib_M_Book_Accn_DMO();
                    //    obj2.LMB_Id = obj1.LMB_Id;
                    //    obj2.LMBANO_AccessionNo = abc.LMBANO_No;
                    //    obj2.LMBANO_AvialableStatus = data.LMBANO_AvialableStatus;
                    //    obj2.LMRA_Id = data.LMRA_Id;
                    //    obj2.LMBANO_LostDamagedDate = DateTime.Now;
                    //    obj2.LMBANO_LostDamagedReason = "";
                    //    obj2.LMBANO_ModeOfPayment = "";
                    //    obj2.LMBANO_AmountCollected = 0;
                    //    obj2.LMBANO_LostDamagedFlg = false;
                    //    obj2.LMBANO_ActiveFlg = true;
                    //    obj2.CreatedDate = DateTime.Now;
                    //    obj2.UpdatedDate = DateTime.Now;
                    //    _LibraryContext.Add(obj2);

                    //    int rowAffected = _LibraryContext.SaveChanges();

                    //    if (rowAffected > 0)
                    //    {
                    //        data.returnval = true;
                    //    }
                    //    else
                    //    {
                    //        data.returnval = false;
                    //    }
                    //}

                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("library");
            }
            return data;
        }
        public BookRegisterDTO Tab2Savedata(BookRegisterDTO data)
        {
            try
            {
                if (data.LMB_Id > 0)
                {
                    var update1 = _LibraryContext.BookRegisterDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();

                    update1.LMB_PurchaseDate = data.Purchase_Date;
                    update1.LML_Id = data.LML_Id;
                    update1.LMB_BillNo = data.LMB_BillNo;
                    update1.LMB_VoucherNo = data.Voucher_No;
                    update1.LMB_DonorName = data.Donor_Id;
                    update1.LMB_CurrenceyType = data.CurrencyType;
                    update1.LMB_Keywords = data.MB_Keywords;
                    if(data.AuthFlag == true)
                    {
                        var Duplicate = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_EMailId.Contains(data.LMP_EMailId) | t.LMP_PhoneNo.Contains(data.LMP_PhoneNo) | t.LMP_MobileNo == data.LMP_MobileNo).Distinct().ToList();

                        if (Duplicate.Count() > 0)
                        {
                            update1.LMP_Id = data.LMP_Id;
                        }
                        else
                        {
                            MasterPublisherDMO obj = new MasterPublisherDMO();
                            obj.MI_Id = data.MI_Id;
                            obj.LMP_PublisherName = data.LMP_PublisherName;
                            obj.LMP_Address = data.LMP_Address;
                            obj.LMP_EMailId = data.LMP_EMailId;
                            obj.LMP_PhoneNo = data.LMP_PhoneNo;
                            obj.LMP_MobileNo = data.LMP_MobileNo;
                            obj.LMP_ActiveFlg = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj);
                            update1.LMP_Id = obj.LMP_Id;
                        }
                    
                       
                    }
                    else
                    {
                        update1.LMP_Id = data.LMP_Id;
                    }
                                    
                    update1.LMB_Remarks = data.MB_Remarks;
                    update1.LMB_Biblography = data.Bibliography_Page;
                    update1.LMB_IndexPage = data.Index_Page;
                    update1.LMB_PurOrDonated = data.LMB_PurOrDonated;
                    update1.LMB_DonorAddress = data.LMB_DonorAddress;
                    update1.UpdatedDate = DateTime.Now;
                    _LibraryContext.Update(update1);
                    if (data.LMV_Id > 0)
                    {
                        var chk = _LibraryContext.LIB_Master_Book_VendorDMO.Where(t => t.LMB_Id == data.LMB_Id).ToList();

                        if (chk.Count > 0)
                        {
                            var update3 = _LibraryContext.LIB_Master_Book_VendorDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();

                            update3.LMV_Id = data.LMV_Id;
                            update3.UpdatedDate = DateTime.Now;
                            _LibraryContext.Update(update3);

                        }
                        else
                        {
                            LIB_Master_Book_VendorDMO obj4 = new LIB_Master_Book_VendorDMO();
                            obj4.LMB_Id = data.LMB_Id;
                            obj4.LMV_Id = data.LMV_Id;
                            obj4.LMBV_ActiveFlg = true;
                            obj4.CreatedDate = DateTime.Now;
                            obj4.UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(obj4);
                        }
                    }
                    else
                    {
                        var chk1 = _LibraryContext.LIB_Master_Book_VendorDMO.Where(t => t.LMB_Id == data.LMB_Id).ToList();

                        if (chk1.Count > 0)
                        {
                            foreach (var item in chk1)
                            {
                                _LibraryContext.Remove(chk1);
                            }
                        }
                    }


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


                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_BOOK_REGISTER_MAIN_LOAD_NEW";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.IVRMUL_Id
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("library");
            }
            return data;
        }
        public BookRegisterDTO Tab1Savedata(BookRegisterDTO data)
        {
            try
            {
                if (data.LMB_Id > 0)
                {

                    //var Duplicate = (from a in _LibraryContext.BookRegisterDMO
                    //                 from b in _LibraryContext.MasterAuthorDMO
                    //                 where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_Id != data.LMB_Id && a.LMS_Id == data.LMS_Id && a.LMB_BookTitle == data.LMB_BookTitle)
                    //                 select a).ToArray().ToList();

                    //if (Duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    var update1 = _LibraryContext.BookRegisterDMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();
                    update1.UpdatedDate = DateTime.Now;
                    update1.LMB_BookTitle = data.LMB_BookTitle;
                    update1.LMB_BookSubTitle = data.MB_Subtitle;
                    update1.LMC_Id = data.LMC_Id;
                    update1.LMB_BookType = data.LMB_BookType;
                    update1.LMS_Id = data.LMS_Id;
                    update1.LMD_Id = data.LMD_Id;
                    update1.LMB_BindingType = data.Binding_Type;
                    update1.LMB_Edition = data.LMB_Edition;
                    update1.LMB_PublishedYear = data.LMB_PublishedYear;
                    update1.LMB_EntryDate = data.LMB_EntryDate;
                    update1.LMB_CallNo = data.MB_Call_No;
                    update1.LMB_ISBNNo = data.LMB_ISBNNo;
                    update1.LMB_ClassNo = data.LMB_ClassNo;
                    update1.LMB_VolNo = data.LMB_VolNo;
                    update1.LMB_NoOfPages = data.MB_Pages;
                    update1.LMB_BookImage = data.Book_Image;


                    // update1.LML_Id = data.LML_Id;
                    update1.LMB_BookNo = data.LMB_BookNo;
                    _LibraryContext.Update(update1);
                    int c = 0;




                    var keyfact = _LibraryContext.LIB_Master_Book_KeyFactor_DMO.Where(t => t.LMB_Id == data.LMB_Id).ToList();
                    if (keyfact.Count > 0)
                    {
                        var keybook = _LibraryContext.LIB_Master_Book_KeyFactor_DMO.Where(t => t.LMB_Id == data.LMB_Id).SingleOrDefault();

                        keybook.LMB_Id = data.LMB_Id;
                        keybook.LMBKF_KeyFactor = data.LMBKF_KeyFactor;
                        keybook.LMBKF_PageNo = data.LMBKF_PageNo;
                        keybook.UpdatedBy = data.UserId;
                        keybook.UpdatedDate = DateTime.Now;

                        _LibraryContext.Update(keybook);
                    }
                    else
                    {
                        LIB_Master_Book_KeyFactor_DMO bookkey = new LIB_Master_Book_KeyFactor_DMO();

                        bookkey.LMB_Id = data.LMB_Id;
                        bookkey.LMBKF_KeyFactor = data.LMBKF_KeyFactor;
                        bookkey.LMBKF_PageNo = data.LMBKF_PageNo;
                        bookkey.LMBKF_ActiveFlg = true;
                        bookkey.UpdatedBy = data.UserId;
                        bookkey.CreatedBy = data.UserId;
                        bookkey.UpdatedDate = DateTime.Now;
                        bookkey.CreatedDate = DateTime.Now;
                        _LibraryContext.Add(bookkey);
                    }


                    var check1 = _LibraryContext.LIB_Master_Book_Library_DMO.Where(t => t.LMB_Id == data.LMB_Id).ToArray().ToList();

                    if (check1.Count() > 0)
                    {
                        var update8 = _LibraryContext.LIB_Master_Book_Library_DMO.Where(t => t.LMB_Id == data.LMB_Id /*&& t.LMBL_Id==data.LMBL_Id*/).FirstOrDefault();
                        update8.LMB_Id = data.LMB_Id;
                        update8.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                        update8.LMBL_ActiveFlg = true;
                        update8.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update8);
                    }
                    else
                    {
                        LIB_Master_Book_Library_DMO obj5 = new LIB_Master_Book_Library_DMO();
                        obj5.LMB_Id = data.LMB_Id;
                        obj5.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                        obj5.LMBL_ActiveFlg = true;
                        obj5.CreatedDate = DateTime.Now;
                        obj5.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj5);

                    }




                    int rowAffected = _LibraryContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    //}
                }
                else
                {


                    //var Duplicate = (from a in _LibraryContext.BookRegisterDMO
                    //                 from b in _LibraryContext.MasterAuthorDMO
                    //                 where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMS_Id == data.LMS_Id && a.LMB_BookTitle == data.LMB_BookTitle)
                    //                 select a).ToArray().ToList();

                    //if (Duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    BookRegisterDMO obj1 = new BookRegisterDMO();

                    obj1.MI_Id = data.MI_Id;
                    obj1.LMB_BookTitle = data.LMB_BookTitle;
                    obj1.LMB_BookSubTitle = data.MB_Subtitle;
                    obj1.LMC_Id = data.LMC_Id;
                    obj1.LMB_BookType = data.LMB_BookType;
                    obj1.LMS_Id = data.LMS_Id;
                    obj1.LMD_Id = data.LMD_Id;
                    obj1.LMB_BindingType = data.Binding_Type;
                    obj1.LMB_Edition = data.LMB_Edition;
                    obj1.LMB_PublishedYear = data.LMB_PublishedYear;
                    obj1.LMB_EntryDate = data.LMB_EntryDate;
                    // obj1.LMB_EntryDate = Convert.ToDateTime(data.LMB_EntryDate);
                    obj1.LMB_CallNo = data.MB_Call_No;
                    obj1.LMB_ISBNNo = data.LMB_ISBNNo;
                    obj1.LMB_ClassNo = data.LMB_ClassNo;
                    obj1.LMB_VolNo = data.LMB_VolNo;
                    obj1.LMB_NoOfPages = data.MB_Pages;
                    obj1.LMB_BookImage = data.Book_Image;

                    //obj1.LML_Id = data.LML_Id;
                    obj1.LMB_BookNo = data.LMB_BookNo;


                    obj1.LMB_ActiveFlg = true;
                    obj1.CreatedDate = DateTime.Now;
                    obj1.UpdatedDate = DateTime.Now;

                    _LibraryContext.Add(obj1);

                    LIB_Master_Book_KeyFactor_DMO keyfact = new LIB_Master_Book_KeyFactor_DMO();

                    //keyfact.LMBKF_Id = data.LMBKF_Id;
                    keyfact.LMB_Id = obj1.LMB_Id;
                    keyfact.LMBKF_KeyFactor = data.LMBKF_KeyFactor;
                    keyfact.LMBKF_PageNo = data.LMBKF_PageNo;
                    keyfact.LMBKF_ActiveFlg = data.LMBKF_ActiveFlg;
                    keyfact.CreatedBy = data.UserId;
                    keyfact.UpdatedBy = data.UserId;
                    keyfact.CreatedDate = DateTime.Now;
                    keyfact.UpdatedDate = DateTime.Now;
                    _LibraryContext.Add(keyfact);





                    LIB_Master_Book_Library_DMO obj5 = new LIB_Master_Book_Library_DMO();
                    obj5.LMB_Id = obj1.LMB_Id;
                    obj5.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                    obj5.LMBL_ActiveFlg = true;
                    obj5.CreatedDate = DateTime.Now;
                    obj5.UpdatedDate = DateTime.Now;
                    _LibraryContext.Add(obj5);


                    int rowAffected = _LibraryContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        data.LMB_Id = obj1.LMB_Id;
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }


                    //}
                }

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_BOOK_REGISTER_MAIN_LOAD_NEW";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.IVRMUL_Id
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("library");
            }




            return data;
        }
        public async Task<BookRegisterDTO> Editdata(BookRegisterDTO data)
        {
            try
            {

                //data.editlis = (from a in _LibraryContext.BookRegisterDMO                               
                //                from m in _LibraryContext.Lib_M_Book_Accn_DMO
                //                from b in _LibraryContext.MasterSubject_DMO
                //                from c in _LibraryContext.MasterDepartmentDMO
                //                from e in _LibraryContext.MasterLanguageDMO
                //                from k in _LibraryContext.MasterAuthorDMO
                //                from r in _LibraryContext.RackDetailsDMO
                //                from ac in _LibraryContext.LIB_Master_Book_VendorDMO
                //                where (a.LMB_Id == m.LMB_Id && a.LMB_Id == k.LMB_Id && a.LMD_Id == c.LMD_Id && a.LMS_Id==b.LMS_Id  && a.LML_Id==e.LML_Id  && r.LMRA_Id==m.LMRA_Id && a.LMB_Id==ac.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_Id==data.LMB_Id)
                //                select new BookRegisterDTO
                //                {                                   
                //                    MI_Id = a.MI_Id,
                //                    LMB_Id = a.LMB_Id,
                //                    LMBANO_Id = m.LMBANO_Id,
                //                    LMB_BookTitle = a.LMB_BookTitle,
                //                    MB_Subtitle = a.LMB_BookSubTitle,
                //                    LMC_Id = a.LMC_Id,
                //                    LMB_BookType = a.LMB_BookType,
                //                    LMS_Id = a.LMS_Id,
                //                    LMD_Id = a.LMD_Id,
                //                    With_Accessories = a.LMB_WithAccessories,
                //                    Binding_Type = a.LMB_BindingType,
                //                    LMB_Edition = a.LMB_Edition,
                //                    LMB_PublishedYear = a.LMB_PublishedYear,
                //                    LMB_EntryDate = a.LMB_EntryDate,
                //                    MB_Call_No = a.LMB_CallNo,
                //                    LMB_ISBNNo = a.LMB_ISBNNo,
                //                    LMB_ClassNo = a.LMB_ClassNo,
                //                    LMB_VolNo = a.LMB_VolNo,
                //                    MB_Pages = a.LMB_NoOfPages,
                //                    Book_Image = a.LMB_BookImage,
                //                    LMB_NoOfCopies = a.LMB_NoOfCopies,
                //                    Purchase_Date = a.LMB_PurchaseDate,
                //                    LML_Id = a.LML_Id,
                //                    LMB_BillNo = a.LMB_BillNo,
                //                    Voucher_No = a.LMB_VoucherNo,
                //                    LMB_Price = a.LMB_Price,
                //                    CurrencyType = a.LMB_CurrenceyType,
                //                    LMB_NetPrice = a.LMB_NetPrice,
                //                    MB_Keywords = a.LMB_Keywords,
                //                    LMP_Id = a.LMP_Id,
                //                    MB_Remarks = a.LMB_Remarks,
                //                    LMB_Discount = a.LMB_Discount,
                //                    Bibliography_Page = a.LMB_Biblography,
                //                    Index_Page = a.LMB_IndexPage,
                //                    LMBA_Id = k.LMBA_Id,
                //                    MB_ActiveFlag = m.LMBANO_ActiveFlg,
                //                    LMBANO_AccessionNo = m.LMBANO_AccessionNo,
                //                    Donor_Name = a.LMB_DonorName,
                //                    LMBA_AuthorFirstName = k.LMBA_AuthorFirstName,
                //                    LMBA_AuthorMiddleName = k.LMBA_AuthorMiddleName,
                //                    LMBA_AuthorLastName = k.LMBA_AuthorLastName,
                //                    LMB_PurOrDonated = a.LMB_PurOrDonated,
                //                    LMB_DonorAddress = a.LMB_DonorAddress,
                //                    LMBANO_AvialableStatus = m.LMBANO_AvialableStatus,
                //                    LMRA_Id = m.LMRA_Id,
                //                    LMB_BookNo = a.LMB_BookNo,


                //                }).ToArray();
                data.authorlst = (from a in _LibraryContext.LIB_Master_Author_DMO.Where(a => a.LMAU_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                  select new BookRegisterDTO
                                  {
                                      LMAU_Id = a.LMAU_Id,
                                      LMAU_AuthorFirstName = ((a.LMAU_AuthorFirstName == null ? " " : a.LMAU_AuthorFirstName) + " " + (a.LMAU_AuthorMiddleName == null ? " " : a.LMAU_AuthorMiddleName) + " " + (a.LMAU_AuthorLastName == null ? " " : a.LMAU_AuthorLastName)).Trim(),
                                      // LMBA_AuthorMiddleName = a.LMAU_AuthorMiddleName,
                                      // LMBA_AuthorLastName = a.LMAU_AuthorLastName,
                                  }).Distinct().OrderBy(a => a.LMBA_Id).Take(20).ToArray();

                var retObject1 = new List<dynamic>();
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Library_Edit_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMB_Id",
                    SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.LMB_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.LMBANO_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.editlis = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                data.BookFilesPdfEdit = _LibraryContext.Master_Book_FilesDMO.Where(R => R.LMB_Id == data.LMB_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookRegisterDTO deactiveY(BookRegisterDTO data)
        {
            try
            {
                var result = _LibraryContext.Lib_M_Book_Accn_DMO.Single(t => t.LMBANO_Id == data.LMBANO_Id);


                //var result = (from a in _LibraryContext.BookRegisterDMO
                //                 from b in _LibraryContext.Lib_M_Book_Accn_DMO
                //              where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_Id != data.LMB_Id && b.LMBANO_AccessionNo == data.LMBANO_No)
                //                 select b.LMBANO_Id);

                //var result1 = _LibraryContext.Lib_M_Book_Accn_DMO.Single(t => t.LMBANO_Id == Convert.ToInt64(result));


                if (result.LMBANO_ActiveFlg == true)
                {
                    result.LMBANO_ActiveFlg = false;
                }
                else if (result.LMBANO_ActiveFlg == false)
                {
                    result.LMBANO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BookRegisterDTO searching(BookRegisterDTO data)
        {
            try
            {

                string Book_Prefix = "";
                List<long> LMS_Id = new List<long>();
                List<long> LMS_Idtwo = new List<long>();
                var retObject1 = new List<dynamic>();
                if (data.Delete_Reason == "8")
                {
                    // Book_Prefix=
                    var FirstSummary = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id && R.LMS_Level == 1 && R.LMS_SubjectNo.Trim().Contains(data.Book_Prefix)
                   ).ToList();

                    if (FirstSummary.Count > 0)
                    {
                        Book_Prefix = "0";
                        foreach (var d in FirstSummary)
                        {
                            
                            LMS_Id.Add(d.LMS_Id);
                        }
                        var Seccondsummary = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id && R.LMS_Level == 2
                        && LMS_Id.Contains(R.LMS_ParentId)
                        ).ToList();

                        if (Seccondsummary.Count > 0)
                        {
                            
                            foreach (var d in Seccondsummary)
                            {                              
                                LMS_Idtwo.Add(d.LMS_Id);
                            }
                        }
                        var ThirdSummary = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id && R.LMS_Level == 3
                             && LMS_Idtwo.Contains(R.LMS_ParentId)).ToList();
                        if (ThirdSummary.Count > 0)
                        {
                            foreach (var d in ThirdSummary)
                            {
                                Book_Prefix = Book_Prefix + ',' + d.LMS_Id;
                               
                            }
                        }

                    }
                }
                else if (data.Delete_Reason == "9")
                {
                    //Book_Prefix= 
                    Book_Prefix = "0";
                    var FirstSummary = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id && R.LMS_Level == 2 && R.LMS_SubjectNo.Trim().Contains(data.Book_Prefix)
                     ).ToList();
                    if (FirstSummary.Count > 0)
                    {
                        
                        foreach (var d in FirstSummary)
                        {
                            Book_Prefix = Book_Prefix + ',' + d.LMS_Id;
                            LMS_Id.Add(d.LMS_Id);
                        }
                    }
                    var ThirdSummary = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id && R.LMS_Level == 3
                            && LMS_Id.Contains(R.LMS_ParentId)).ToList();
                    if (ThirdSummary.Count > 0)
                    {
                        foreach (var d in ThirdSummary)
                        {
                            Book_Prefix = Book_Prefix + ',' + d.LMS_Id;

                        }
                    }
                }
                else
                {
                    Book_Prefix = data.Book_Prefix;
                }

                // var  LMS_Id=
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_GET_BOOK_REGISTER_SEARCH_NEWONE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.IVRMUL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.VarChar)
                    {
                        Value = data.Delete_Reason
                    });
                    cmd.Parameters.Add(new SqlParameter("@TXTSEARCH",
                    SqlDbType.VarChar)
                    {
                        Value = Book_Prefix
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.alldata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public BookRegisterDTO searchfilter(BookRegisterDTO data)
        {
            try
            {

                //data.LMBA_AuthorFirstName = data.LMBA_AuthorFirstName.ToUpper();
                //data.authorlst = (from a in _LibraryContext.LIB_Master_Author_DMO.Where(a => a.LMAU_ActiveFlg == true && a.MI_Id==data.MI_Id && ((a.LMAU_AuthorFirstName.Trim().ToUpper() + a.LMAU_AuthorMiddleName.Trim().ToUpper() + ' ' + a.LMAU_AuthorFirstName.Trim().ToUpper()).StartsWith(data.LMBA_AuthorFirstName) || a.LMAU_AuthorFirstName.ToUpper().StartsWith(data.LMBA_AuthorFirstName)))
                //                  select new BookRegisterDTO
                //                  {
                //                      LMAU_Id = a.LMAU_Id,
                //                      LMAU_AuthorFirstName = ((a.LMAU_AuthorFirstName == null ? " " : a.LMAU_AuthorFirstName) + " " + (a.LMAU_AuthorMiddleName == null ? " " : a.LMAU_AuthorMiddleName) + " " + (a.LMAU_AuthorLastName == null ? " " : a.LMAU_AuthorLastName)).Trim(),
                //                      //LMBA_AuthorMiddleName = a.LMAU_AuthorMiddleName,
                //                      //LMBA_AuthorLastName = a.LMAU_AuthorLastName,
                //                  }).Distinct().OrderBy(a => a.LMBA_Id).ToArray();


                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_search";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@searchtext",
                      SqlDbType.VarChar)
                    {
                        Value = data.LMBA_AuthorFirstName
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

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
                        data.authorlst = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.LMAU_AuthorFirstName = data.LMBA_AuthorFirstName.ToUpper();
                //data.LMAU_AuthorFirstName = data.LMBA_AuthorMiddleName.ToUpper();
                //data.LMAU_AuthorFirstName = data.LMBA_AuthorLastName.ToUpper();
                //data.authorlst = (from a in _LibraryContext.LIB_Master_Author_DMO.Where(a => a.LMAU_ActiveFlg == true && data.LMAU_AuthorFirstName.Contains(a.LMAU_AuthorFirstName.Trim().ToUpper()) || data.LMAU_AuthorFirstName.Contains(a.LMAU_AuthorMiddleName.Trim().ToUpper()) || data.LMAU_AuthorFirstName.Contains(a.LMAU_AuthorLastName.Trim().ToUpper()))
                //                  select new BookRegisterDTO
                //                  {
                //                      LMBA_Id = a.LMAU_Id,
                //                      LMBA_AuthorFirstName = ((a.LMAU_AuthorFirstName == null ? " " : a.LMAU_AuthorFirstName) + " " + (a.LMAU_AuthorMiddleName == null ? " " : a.LMAU_AuthorMiddleName) + " " + (a.LMAU_AuthorLastName == null ? " " : a.LMAU_AuthorLastName)).Trim(),
                //                      LMBA_AuthorMiddleName = a.LMAU_AuthorMiddleName,
                //                      LMBA_AuthorLastName = a.LMAU_AuthorLastName,
                //                  }).Distinct().OrderBy(a => a.LMBA_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public async Task<BookRegisterDTO> changelibrary(BookRegisterDTO data)
        {
            try
            {

                var retObject1 = new List<dynamic>();
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_MasterBook_Accor_to_Library";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.alldata = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
