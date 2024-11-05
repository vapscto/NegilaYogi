using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class BookDetailsImportImpl : Interfaces.BookDetailsImportInterface
    {


        private LibraryContext _LibraryContext;
        ILogger<BookDetailsImportImpl> _acdimpl;

        public BookDetailsImportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public async Task<BookDetailsImportDTO> save_excel_data(BookDetailsImportDTO data)
        {

            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            data.book_msg_type = "";
            try
            {
                List<BookDetailsImportDTO> failedlist = new List<BookDetailsImportDTO>();


                for (int i = 0; i < data.newlstget1.Length; i++)
                {
                    try
                    {

                        BookRegisterDMO enq = new BookRegisterDMO();

                        data.newlstget1[i].MI_Id = data.MI_Id;

                        if ((Convert.ToString(data.newlstget1[i].LMB_BookTitle) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BookTitle), @"[a-zA-Z]")))
                            {
                                enq.LMB_BookTitle = data.newlstget1[i].LMB_BookTitle;
                            }
                            else
                            {
                                data.book_msg_type = "Book Title Name is Not Valid as It Should Contain Only Characters";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Title Name can not be Null and Empty!!";
                            return data;
                        }
                        
                        if ((Convert.ToString(data.newlstget1[i].LMB_Edition) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_Edition), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_Edition).Length <= 15))
                            {
                                enq.LMB_Edition = data.newlstget1[i].LMB_Edition;
                            }

                            else
                            {
                                data.book_msg_type = "Book Edition Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Edition Code can not be null and Empty!!";
                            return data;
                        }

                        if ((Convert.ToString(data.newlstget1[i].LMB_PublishedYear) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_PublishedYear), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_PublishedYear).Length <= 10))
                            {
                                enq.LMB_PublishedYear = data.newlstget1[i].LMB_PublishedYear;
                            }

                            else
                            {
                                data.book_msg_type = "Book Publish Year Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is Ten!!";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Publish Year Code can not be null and Empty!!";
                            return data;
                        }


                        //=====getting  ISBN No  =====//
                        if ((Convert.ToString(data.newlstget1[i].LMB_ISBNNo) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_ISBNNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_ISBNNo).Length <= 15))
                            {
                                enq.LMB_ISBNNo = data.newlstget1[i].LMB_ISBNNo;
                            }

                            else
                            {
                                data.book_msg_type = "Book ISBN Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15 !!";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book ISBN Code can not be null and Empty!!";
                            return data;
                        }

                        //=====getting Book Type  =====//
                        if ((Convert.ToString(data.newlstget1[i].LMB_BookType) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BookType), @"[a-zA-Z]")))
                            {
                                enq.LMB_ISBNNo = data.newlstget1[i].LMB_BookType;
                            }

                            else
                            {
                                data.book_msg_type = "Book Type  is Not Valid as It Should Contain Only Characters!!!";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Type can not be null and Empty!!";
                            return data;
                        }
                       

                        //=====getting Book Copies  =====//
                        if ((Convert.ToString(data.newlstget1[i].LMB_NoOfCopies) != null)) 
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_NoOfCopies), @"[0-9]")) && (Convert.ToString(data.newlstget1[i].LMB_NoOfCopies).Length <= 15))
                            {
                                enq.LMB_NoOfCopies = Convert.ToInt64(data.newlstget1[i].LMB_NoOfCopies);
                            }

                            else
                            {
                                data.book_msg_type = "Book Copies are Not Valid as It Should Contain Only Numeric Characters Max Length is 15";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Copies can not be null and Empty!!";
                            return data;
                        }

                        //=======Bill No For Book=======//
                        if ((Convert.ToString(data.newlstget1[i].LMB_BillNo) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BillNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_BillNo).Length <= 15))
                            {
                                enq.LMB_BillNo = data.newlstget1[i].LMB_BillNo;
                            }
                            else
                            {
                                data.book_msg_type = "Bill Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Bill Code can not be null and Empty!!";
                            return data;
                        }

                        //=====Total price For Book==========//
                        if ((Convert.ToString(data.newlstget1[i].LMB_NetPrice) != null))
                        {
                            if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_NetPrice), @"[0-9.]")) && (Convert.ToString(data.newlstget1[i].LMB_NetPrice).Length <= 15))
                            {
                                enq.LMB_NetPrice = data.newlstget1[i].LMB_NetPrice;
                            }
                            else
                            {
                                data.book_msg_type = "Book Total Amount is Not Valid as It Should Contain Only Numeric Characters Max Length is 15";
                                return data;
                            }
                        }
                        else
                        {
                            data.book_msg_type = "Book Total Amount can not be null and Empty!!";
                            return data;
                        }
                      

                        long LMC_Id;
                        //--getting category id from category name --//
                        var cat_id_count = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_CategoryName.TrimEnd().TrimStart().ToLower() == data.newlstget1[i].LMC_CategoryName.ToLower()).ToList();
                        if (cat_id_count.Count() > 0)
                        {
                            var cat_id = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_CategoryName.TrimEnd().TrimStart().ToLower() == data.newlstget1[i].LMC_CategoryName.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                            enq.LMC_Id = cat_id.LMC_Id;
                            LMC_Id= cat_id.LMC_Id;
                        }
                        else
                        {
                            data.book_msg_type = "Category Name is Not Available for" + " " + data.newlstget1[i].LMC_Id + " " + "Category code";
                            return data;
                        }


                        long sub_id;
                        //=====getting LMS_SubjectName Id from LMS_SubjectName Name========//
                        var sub_name_count = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMS_SubjectName.TrimStart().TrimEnd().ToLower()).ToList();

                        if (sub_name_count.Count() > 0)
                        {
                            var sub_name = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMS_SubjectName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                            enq.LMS_Id = sub_name.LMS_Id;
                            sub_id = sub_name.LMS_Id;
                        }
                        else
                        {
                            data.book_msg_type = "LMS_SubjectName Name is Not Available for" + " " + data.newlstget1[i].LMS_Id + " " + "LMS_SubjectName code";
                            return data;
                        }


                        long LMD_Id;
                        //=====getting Dept Id from Dept Name========//
                        var dep_name_count = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMD_DepartmentName.TrimStart().TrimEnd().ToLower()).ToList();
                        if (dep_name_count.Count() > 0)
                        {
                            var LMD_DepartmentName = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMD_DepartmentName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                            enq.LMD_Id = LMD_DepartmentName.LMD_Id;
                            LMD_Id = LMD_DepartmentName.LMD_Id;
                        }
                        else
                        {
                            data.book_msg_type = "Department Name is Not Available for" + " " + data.newlstget1[i].LMD_Id + " " + "Department code";
                            return data;
                        }


                        long lang_id;
                        //========getting Language Id From Language Name======//
                        var lang_name_count = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LML_LanguageName.TrimStart().TrimEnd().ToLower()).ToList();
                        if (lang_name_count.Count() > 0)
                        {
                            var lang_name = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LML_LanguageName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                            enq.LML_Id = lang_name.LML_Id;
                            lang_id = lang_name.LML_Id;
                        }
                        else
                        {
                            data.book_msg_type = "Language Name is Not Available for" + " " + data.newlstget1[i].LML_Id + " " + "Language code";
                            return data;
                        }


                        //long Donor_id;
                        ////=======getting Donor Id From Donar Name==========//
                        //var donr_name_count = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == data.MI_Id && t.Donor_Name.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Donor_Name.TrimStart().TrimEnd().ToLower()).ToList();
                        //if (donr_name_count.Count() > 0)
                        //{
                        //    var donr_name = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == data.MI_Id && t.Donor_Name.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Donor_Name.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                        //    enq.Donor_Id = donr_name.Donor_Id;
                        //    Donor_id = donr_name.Donor_Id;
                        //}
                        //else
                        //{
                        //    data.book_msg_type = "Donor Name is Not Available for" + " " + data.newlstget1[i].Donor_Id + " " + "Donor code";
                        //    return data;
                        //}


                        //long LMV_Id;
                        ////======getting Vendor Id from Vendor Name========//
                        //var vendr_name_count = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMV_VendorName.TrimStart().TrimEnd().ToLower()).ToList();
                        //if (vendr_name_count.Count() > 0)
                        //{
                        //    var vendr_name = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMV_VendorName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                        //    enq.LMV_Id = vendr_name.LMV_Id;
                        //    LMV_Id = vendr_name.LMV_Id;
                        //}
                        //else
                        //{
                        //    data.book_msg_type = "Vendor Name is Not Available for" + " " + data.newlstget1[i].LMV_Id + " " + "Vendor code";
                        //    return data;
                        //}


                        //long class_id;
                        ////====getting class id from Class name=====//
                        //var clss_name_count = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ClassName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].ASMCL_ClassName.TrimStart().TrimEnd().ToLower()).ToList();
                        //if (clss_name_count.Count() > 0)
                        //{
                        //    var clss_name = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ClassName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].ASMCL_ClassName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                        //    enq.ForTheClass = clss_name.ASMCL_Id;
                        //    class_id = clss_name.ASMCL_Id;
                        //}
                        //else
                        //{
                        //    data.book_msg_type = "Class Name is Not Available for" + " " + data.newlstget1[i].ASMCL_Id + " " + "Class code";
                        //    return data;
                        //}


                        //long publ_id;
                        ////======getting publisher id from publisher name=======//
                        //var publsr_name_count = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMP_PublisherName.TrimStart().TrimEnd().ToLower()).ToList();
                        //if (publsr_name_count.Count() > 0)
                        //{
                        //    var publsr_name = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMP_PublisherName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                        //    enq.LMP_Id = publsr_name.LMP_Id;
                        //    publ_id = publsr_name.LMP_Id;
                        //}
                        //else
                        //{
                        //    data.book_msg_type = "Publisher Name is Not Available for" + " " + data.newlstget1[i].LMP_Id + " " + "Publisher code";
                        //    return data;
                        //}



                        long authr_id;
                        //======getting author id from author name=======//
                        var authr_name_count = _LibraryContext.MasterAuthorDMO.Where(t =>  t.LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower()).ToList();

                        if (authr_name_count.Count() > 0)
                        {
                            var authr_name = _LibraryContext.MasterAuthorDMO.Where(t =>  t.LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                           // enq.LMBA_Id = authr_name.LMBA_Id;
                            authr_id = authr_name.LMBA_Id;
                        }
                        else
                        {
                            data.book_msg_type = "Author Name is Not Available for" + " " + data.newlstget1[i].LMBA_Id + " " + "Author code";
                            return data;
                        }


                        data.newlstget1[i].MB_Call_No = "";
                        data.newlstget1[i].LMB_ClassNo = "";
                        data.newlstget1[i].MB_Subtitle = "";
                        data.newlstget1[i].LMB_Price = 0;
                        data.newlstget1[i].LMB_Discount = 0;
                        data.newlstget1[i].MB_Disc_Type = "";
                        data.newlstget1[i].LMB_VolNo = "";
                        data.newlstget1[i].Binding_Type = "";
                    
                        data.newlstget1[i].Bibliography_Page = "";
                        data.newlstget1[i].Index_Page = "";
                        data.newlstget1[i].Voucher_No = "";
                        data.newlstget1[i].MB_Pages = 0;
                        data.newlstget1[i].Source_Type = "";
                        data.newlstget1[i].MB_Remarks = "";
                        data.newlstget1[i].MB_Keywords = "";
                        data.newlstget1[i].New_Arrival = true;
                        data.newlstget1[i].Book_Image = "";
                        data.newlstget1[i].With_Accessories = true;
                        data.newlstget1[i].CurrencyType = "";
                        data.newlstget1[i].Invoice_No = "";

                        data.newlstget1[i].Purchase_Date = DateTime.Now;
                        data.newlstget1[i].LMB_EntryDate = DateTime.Now;


                        data.newlstget1[i].CreatedDate = DateTime.Now;
                        data.newlstget1[i].UpdatedDate = DateTime.Now;
                        data.newlstget1[i].MB_ActiveFlag = true;
                        data.newlstget1[i].With_Accessories = true;

                        _LibraryContext.Add(enq);
                       
                            //this data store in accession table

                            Lib_M_Book_Accn_DMO enq2 = new Lib_M_Book_Accn_DMO();
                            data.newlstget1[i].MI_Id = data.MI_Id;

                            enq2.LMB_Id = enq.LMB_Id;

                            long rack_id;
                          //========getting Rack id From Rack Name=========//
                            var rack_name_count = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Rack_Name.TrimStart().TrimEnd().ToLower()).ToList();
                            if (rack_name_count.Count() > 0)
                            {
                                var rack_name = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Rack_Name.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                               // enq2.Rack_Id = rack_name.Rack_Id;
                                rack_id = rack_name.LMRA_Id;
                            }
                            //else
                            //{
                            //    data.book_msg_type = "Rack Name is Not Available for" + " " + data.newlstget1[i].Rack_Id + " " + "Rack code";
                            //    return data;
                            //}


                            //=======getting Accession No From Accission table==========//
                            if ((Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo) != null))
                            {
                                if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo).Length <= 15))
                                {
                                    enq2.LMBANO_AccessionNo = data.newlstget1[i].LMBANO_AccessionNo;
                                }

                                else
                                {
                                    data.book_msg_type = "Book Accession Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                    return data;
                                }
                            }
                            else
                            {
                                data.book_msg_type = "Book Accession Code can not be null";
                                return data;
                            }

                            data.newlstget1[i].Book_Avail_Status = "";
                            data.newlstget1[i].Delete_Date = null;
                            data.newlstget1[i].Delete_Reason = null;
                          //  data.newlstget1[i].Login_Id = 0;
                            data.newlstget1[i].Book_NumericPart = 0;
                            data.newlstget1[i].Book_Prefix = null;
                            data.newlstget1[i].Book_Suffix = null;
                            enq2.LMBANO_ActiveFlg = true;
                            enq2.CreatedDate = DateTime.Now;
                            enq2.UpdatedDate = DateTime.Now;
                          

                            _LibraryContext.Add(enq2);
                       
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }
                data.failedlist = failedlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (finalnames != "" && failcount > 0)
            {
                data.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";
            }
            else
            {
                data.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + 0 + "'";
            }

            return data;
        }


        public async Task<BookDetailsImportDTO> checkvalidation(BookDetailsImportDTO data)
        {
            try
            {
                data.book_msg_type = "";
                try
                {
                    List<BookDetailsImportDTO> failedlist = new List<BookDetailsImportDTO>();


                    for (int i = 0; i < data.newlstget1.Length; i++)
                    {
                        try
                        {

                            BookRegisterDMO enq = new BookRegisterDMO();

                            data.newlstget1[i].MI_Id = data.MI_Id;

                            try
                            {
                                if ((Convert.ToString(data.newlstget1[i].LMB_BookTitle) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BookTitle), @"[a-zA-Z]")))
                                    {
                                        enq.LMB_BookTitle = data.newlstget1[i].LMB_BookTitle;
                                    }
                                    else
                                    {
                                        data.book_msg_type = "Book Title Name is Not Valid as It Should Contain Only Characters";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Title Name can not be Null and Empty!!";
                                    return data;
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                if ((Convert.ToString(data.newlstget1[i].LMB_Edition) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_Edition), @"^[a-zA-Z0-9/\-@]*$" )) && (Convert.ToString(data.newlstget1[i].LMB_Edition).Length <= 15))
                                    {
                                        enq.LMB_Edition = data.newlstget1[i].LMB_Edition;
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book Edition Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Edition Code can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                if ((Convert.ToString(data.newlstget1[i].LMB_PublishedYear) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_PublishedYear), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_PublishedYear).Length <= 10))
                                    {
                                        enq.LMB_PublishedYear = data.newlstget1[i].LMB_PublishedYear;
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book Publish Year Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is Ten!!";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Publish Year Code can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=====getting  ISBN No  =====//
                                if ((Convert.ToString(data.newlstget1[i].LMB_ISBNNo) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_ISBNNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_ISBNNo).Length <= 15))
                                    {
                                        enq.LMB_ISBNNo = data.newlstget1[i].LMB_ISBNNo;
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book ISBN Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15 !!";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book ISBN Code can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=====getting Book Type  =====//
                                if ((Convert.ToString(data.newlstget1[i].LMB_BookType) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BookType), @"[a-zA-Z]")))
                                    {
                                        enq.LMB_ISBNNo = data.newlstget1[i].LMB_BookType;
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book Type  is Not Valid as It Should Contain Only Characters!!!";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Type can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=====getting Book Copies  =====//
                                if ((Convert.ToString(data.newlstget1[i].LMB_NoOfCopies) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_NoOfCopies), @"[0-9]")) && (Convert.ToString(data.newlstget1[i].LMB_NoOfCopies).Length <= 15))
                                    {
                                        enq.LMB_NoOfCopies = Convert.ToInt64(data.newlstget1[i].LMB_NoOfCopies);
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book Copies are Not Valid as It Should Contain Only Numeric Characters Max Length is 15";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Copies can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=======Bill No For Book=======//
                                if ((Convert.ToString(data.newlstget1[i].LMB_BillNo) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_BillNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMB_BillNo).Length <= 15))
                                    {
                                        enq.LMB_BillNo = data.newlstget1[i].LMB_BillNo;
                                    }
                                    else
                                    {
                                        data.book_msg_type = "Bill Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Bill Code can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=====Total price For Book==========//
                                if ((Convert.ToString(data.newlstget1[i].LMB_NetPrice) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMB_NetPrice), @"[0-9.]")) && (Convert.ToString(data.newlstget1[i].LMB_NetPrice).Length <= 15))
                                    {
                                        enq.LMB_NetPrice = data.newlstget1[i].LMB_NetPrice;
                                    }
                                    else
                                    {
                                        data.book_msg_type = "Book Total Amount is Not Valid as It Should Contain Only Numeric Characters Max Length is 15";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Total Amount can not be null and Empty!!";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {

                                long LMC_Id;
                                //--getting category id from category name --//
                                var cat_id_count = _LibraryContext.MasterCategoryDMO.Where(t => t.LMC_CategoryName.TrimEnd().TrimStart().ToLower() == data.newlstget1[i].LMC_CategoryName.ToLower()).ToList();
                                if (cat_id_count.Count() > 0)
                                {
                                    var cat_id = _LibraryContext.MasterCategoryDMO.Where(t => t.LMC_CategoryName.TrimEnd().TrimStart().ToLower() == data.newlstget1[i].LMC_CategoryName.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                    enq.LMC_Id = cat_id.LMC_Id;
                                    LMC_Id = cat_id.LMC_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Category Name is Not Available for" + " " + data.newlstget1[i].LMC_Id + " " + "Category code";
                                    return data;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long sub_id;
                                //=====getting LMS_SubjectName Id from LMS_SubjectName Name========//
                                var sub_name_count = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMS_SubjectName.TrimStart().TrimEnd().ToLower()).ToList();

                                if (sub_name_count.Count() > 0)
                                {
                                    var sub_name = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMS_SubjectName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                    enq.LMS_Id = sub_name.LMS_Id;
                                    sub_id = sub_name.LMS_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "LMS_SubjectName Name is Not Available for" + " " + data.newlstget1[i].LMS_Id + " " + "LMS_SubjectName code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {

                                long LMD_Id;
                                //=====getting Dept Id from Dept Name========//
                                var dep_name_count = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMD_DepartmentName.TrimStart().TrimEnd().ToLower()).ToList();
                                if (dep_name_count.Count() > 0)
                                {
                                    var LMD_DepartmentName = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMD_DepartmentName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                    enq.LMD_Id = LMD_DepartmentName.LMD_Id;
                                    LMD_Id = LMD_DepartmentName.LMD_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Department Name is Not Available for" + " " + data.newlstget1[i].LMD_Id + " " + "Department code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {

                                long lang_id;
                                //========getting Language Id From Language Name======//
                                var lang_name_count = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LML_LanguageName.TrimStart().TrimEnd().ToLower()).ToList();
                                if (lang_name_count.Count() > 0)
                                {
                                    var lang_name = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LML_LanguageName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                    enq.LML_Id = lang_name.LML_Id;
                                    lang_id = lang_name.LML_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Language Name is Not Available for" + " " + data.newlstget1[i].LML_Id + " " + "Language code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long Donor_id;
                                //=======getting Donor Id From Donar Name==========//
                                var donr_name_count = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == data.MI_Id && t.Donor_Name.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Donor_Name.TrimStart().TrimEnd().ToLower()).ToList();
                                if (donr_name_count.Count() > 0)
                                {
                                    var donr_name = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == data.MI_Id && t.Donor_Name.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Donor_Name.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                   // enq.Donor_Id = donr_name.Donor_Id;
                                    Donor_id = donr_name.Donor_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Donor Name is Not Available for" + " " + data.newlstget1[i].Donor_Id + " " + "Donor code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long LMV_Id;
                                //======getting Vendor Id from Vendor Name========//
                                var vendr_name_count = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMV_VendorName.TrimStart().TrimEnd().ToLower()).ToList();
                                if (vendr_name_count.Count() > 0)
                                {
                                    var vendr_name = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMV_VendorName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                   // enq.LMV_Id = vendr_name.LMV_Id;
                                    LMV_Id = vendr_name.LMV_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Vendor Name is Not Available for" + " " + data.newlstget1[i].LMV_Id + " " + "Vendor code";
                                    return data;
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long class_id;
                                //====getting class id from Class name=====//
                                var clss_name_count = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ClassName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].ASMCL_ClassName.TrimStart().TrimEnd().ToLower()).ToList();
                                if (clss_name_count.Count() > 0)
                                {
                                    var clss_name = _LibraryContext.Adm_School_M_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ClassName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].ASMCL_ClassName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                   // enq.ForTheClass = clss_name.ASMCL_Id;
                                    class_id = clss_name.ASMCL_Id;
                                }
                                //else
                                //{
                                //    data.book_msg_type = "Class Name is Not Available for" + " " + data.newlstget1[i].ASMCL_Id + " " + "Class code";
                                //    return data;
                                //}
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long publ_id;
                                //======getting publisher id from publisher name=======//
                                var publsr_name_count = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMP_PublisherName.TrimStart().TrimEnd().ToLower()).ToList();
                                if (publsr_name_count.Count() > 0)
                                {
                                    var publsr_name = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMP_PublisherName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                    enq.LMP_Id = publsr_name.LMP_Id;
                                    publ_id = publsr_name.LMP_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Publisher Name is Not Available for" + " " + data.newlstget1[i].LMP_Id + " " + "Publisher code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                long authr_id;
                                //======getting author id from author name=======//
                                var authr_name_count = _LibraryContext.MasterAuthorDMO.Where(t => t.LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower()).ToList();

                                if (authr_name_count.Count() > 0)
                                {
                                    var authr_name = _LibraryContext.MasterAuthorDMO.Where(t => t.LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorFirstName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorMiddleName.TrimStart().TrimEnd().ToLower() && t.LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].LMBA_AuthorLastName.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                   // enq.LMBA_Id = authr_name.LMBA_Id;
                                    authr_id = authr_name.LMBA_Id;
                                }
                                else
                                {
                                    data.book_msg_type = "Author Name is Not Available for" + " " + data.newlstget1[i].LMBA_Id + " " + "Author code";
                                    return data;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            Lib_M_Book_Accn_DMO enq2 = new Lib_M_Book_Accn_DMO();

                            try
                            {
                                long rack_id;
                                //========getting Rack id From Rack Name=========//
                                var rack_name_count = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Rack_Name.TrimStart().TrimEnd().ToLower()).ToList();
                                if (rack_name_count.Count() > 0)
                                {
                                    var rack_name = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName.TrimStart().TrimEnd().ToLower() == data.newlstget1[i].Rack_Name.TrimStart().TrimEnd().ToLower()).FirstOrDefault();
                                  //  enq2.Rack_Id = rack_name.Rack_Id;
                                    rack_id = rack_name.LMRA_Id;
                                }
                                //else
                                //{
                                //    data.book_msg_type = "Rack Name is Not Available for" + " " + data.newlstget1[i].Rack_Id + " " + "Rack code";
                                //    return data;
                                //}
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }


                            try
                            {
                                //=======getting Accession No From Accission table==========//
                                if ((Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo) != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo), @"^[a-zA-Z0-9]*$")) && (Convert.ToString(data.newlstget1[i].LMBANO_AccessionNo).Length <= 15))
                                    {
                                        enq2.LMBANO_AccessionNo = data.newlstget1[i].LMBANO_AccessionNo;
                                    }

                                    else
                                    {
                                        data.book_msg_type = "Book Accession Code is Not Valid as It Should Contain Only Alphanumeric Characters and Max Length is 15";
                                        return data;
                                    }
                                }
                                else
                                {
                                    data.book_msg_type = "Book Accession Code can not be null";
                                    return data;
                                }
                            }
                            catch(Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                            }

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

              
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
