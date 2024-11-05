using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class Staff_BookTranasctionImpl
    {

        public LibraryContext _LibraryContext;

        public Staff_BookTranasctionImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }


        public Staff_BookTranasctionDTO getdetails(Staff_BookTranasctionDTO data)
        {

            //try
            //{

            //    data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag==true)
            //                        select new Staff_BookTranasctionDTO
            //                        {
            //                            HRME_Id = a.HRME_Id,
            //                            MI_Id = a.MI_Id,
            //                            HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
            //                            HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
            //                            HRME_EmployeeLastName = a.HRME_EmployeeLastName
            //                        }).Distinct().OrderBy(t => t.HRME_Id).ToArray();


            //    data.configdata = _LibraryContext.CirculationParameterDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

            //    //using this condition we get only issue book 
            //    var aaa = _LibraryContext.BookTransactionDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            //    List<long> LMBANO_Id = new List<long>();
            //    if (aaa.Count() > 0)
            //    {
            //        foreach (var it in aaa)
            //        {
            //            if (it.Book_Trans_Status == "Issue")
            //            {
            //                LMBANO_Id.Add(it.LMBANO_Id);
            //            }


            //        }
            //    }

            //    //related LMBANO_Id all data comming from Maser book Table
            //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
            //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                      from c in _LibraryContext.LIB_Master_Book_VendorDMO
            //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_BookType != "Reference" && a.LMB_Id==c.LMB_Id)
            //                      select new Staff_BookTranasctionDTO
            //                      {

            //                          LMB_Id = a.LMB_Id,
            //                          MI_Id = a.MI_Id,
            //                          //LMBA_Id = a.LMBA_Id,
            //                          LMB_ClassNo = a.LMB_ClassNo,
            //                          LMB_BookTitle = a.LMB_BookTitle,
            //                          LMS_Id = a.LMS_Id,
            //                          LMD_Id = a.LMD_Id,
            //                          LMP_Id = a.LMP_Id,
            //                          LMB_Price = a.LMB_Price,
            //                          LMB_VolNo = a.LMB_VolNo,
            //                          LMB_EntryDate = a.LMB_EntryDate,
            //                          LML_Id = a.LML_Id,
            //                         // Donor_Id = a.Donor_Id,
            //                          LMV_Id = c.LMV_Id,
            //                          LMC_Id = a.LMC_Id,
            //                          LMBANO_Id = b.LMBANO_Id,
            //                        //  Rack_Id = b.Rack_Id,


            //                      }).Distinct().ToArray();


            //    //get all related data for bind table grid in Html
            //    data.getalldata = (from a in _LibraryContext.BookTransactionDMO
            //                       from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                       from c in _LibraryContext.BookRegisterDMO
            //                       from d in _LibraryContext.MasterEmployee
            //                       where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id==c.MI_Id && b.LMB_Id == c.LMB_Id && a.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id)
            //                       select new Staff_BookTranasctionDTO
            //                       {
            //                           Book_Trans_Id = a.Book_Trans_Id,
            //                           LMB_Id = c.LMB_Id,
            //                           LMBANO_AccessionNo=b.LMBANO_AccessionNo,
            //                           MI_Id = a.MI_Id,
            //                           ASMAY_Id = a.ASMAY_Id,
            //                           LMBANO_Id = a.LMBANO_Id,
            //                           LMB_BookType=c.LMB_BookType,
            //                           LMB_BookTitle =c.LMB_BookTitle,
            //                           Issue_Date = a.Issue_Date,
            //                           Due_Date = a.Due_Date,
            //                           Return_Date = a.Return_Date,
            //                           Book_Trans_Status = a.Book_Trans_Status,
            //                           Fine_Amount = a.Fine_Amount,
            //                           Renewal_Counter = a.Renewal_Counter,
            //                           Waived_Amount = a.Waived_Amount,
            //                           HRME_Id=d.HRME_Id,
            //                           HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                           HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                           HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                           HRME_EmployeeCode=d.HRME_EmployeeCode,
                                       

            //                       }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }



        public Staff_BookTranasctionDTO Savedata(Staff_BookTranasctionDTO data)
        {
            try
            {
                //if (data.Book_Trans_Id > 0)
                //{
                
                //    data.returnval = true;
                //}
                //else
                //{
                //    var Duplicate = _LibraryContext.BookTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.LMBANO_Id == data.LMBANO_Id && t.Book_Trans_Status =="Issue").ToArray();
                //    if (Duplicate.Count() > 0)
                //    {
                //        data.duplicate = true;

                //    }
                //    else
                //    {
                //        BookTransactionDMO obj = new BookTransactionDMO();

                //        obj.MI_Id = data.MI_Id;
                //        obj.ASMAY_Id = data.ASMAY_Id;
                //        obj.HRME_Id = data.HRME_Id;
                //        obj.LMBANO_Id = data.LMBANO_Id;
                //        obj.Issue_Date = data.Issue_Date;
                //        obj.Due_Date = data.Due_Date;
                //        obj.Return_Date = data.Return_Date;
                //        obj.Fine_Amount = 0;
                //        obj.Waived_Amount = 0;
                //        obj.Renewal_Counter = 0;
                //        obj.Book_Trans_Status = "Issue";
                //        obj.AMST_Id = 0;
                //        obj.FODM_Id = 0;
                //        obj.Guest_Id = 0;
                //        obj.LMD_Id = 0;
                //        obj.CreatedDate = DateTime.Now;
                //        obj.UpdatedDate = DateTime.Now;

                //        _LibraryContext.Add(obj);
                //        int rowAffected = _LibraryContext.SaveChanges();

                //        if (rowAffected > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public Staff_BookTranasctionDTO get_bookdetails(Staff_BookTranasctionDTO data)
        {
            try
            {
                //get LMBANO_Id and related data and get Maxday's & max renewal day's, and how many item issue for that LMBANO_Id
                data.bookdetails = (from a in _LibraryContext.BookRegisterDMO
                                    from b in _LibraryContext.Lib_M_Book_Accn_DMO
                                    from c in _LibraryContext.CirculationParameterDMO
                                    from d in _LibraryContext.LIB_Master_Book_VendorDMO
                                    where (a.LMB_Id == b.LMB_Id && a.LMC_Id == c.LMC_Id && b.LMBANO_Id == data.LMBANO_Id && a.MI_Id == data.MI_Id && a.LMB_Id==d.LMB_Id)
                                    select new Staff_BookTranasctionDTO
                                    {
                                        LMB_Id = a.LMB_Id,
                                        LMBANO_Id = b.LMBANO_Id,
                                        LMB_BookTitle = a.LMB_BookTitle,
                                        LMB_VolNo = a.LMB_VolNo,
                                        LMB_EntryDate = a.LMB_EntryDate,
                                        LMB_Price = a.LMB_Price,
                                        LMB_ClassNo = a.LMB_ClassNo,
                                      //  LMBA_Id = a.LMBA_Id,
                                        LMD_Id = a.LMD_Id,
                                        LMS_Id = a.LMS_Id,
                                        LMP_Id = a.LMP_Id,
                                        LML_Id = a.LML_Id,
                                       // Donor_Id = a.Donor_Id,
                                        LMV_Id = d.LMV_Id,
                                        LMC_Id = a.LMC_Id,
                                        Max_Issue_Days = c.Max_Issue_Days,
                                        Max_No_Renewals = c.Max_No_Renewals,
                                        Max_Issue_Items = c.Max_Issue_Items,
                                    }
                                  ).Distinct().ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public Staff_BookTranasctionDTO get_Staffdetails(Staff_BookTranasctionDTO data)
        {
            try
            {
                //In this query we et Staff details according to HRME_Id
                data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
                                    from b in _LibraryContext.HR_Master_Department
                                    from c in _LibraryContext.HR_Master_Designation
                                    where (a.HRMD_Id == b.HRMD_Id && a.MI_Id==b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id==data.MI_Id && a.HRME_Id==data.HRME_Id && a.HRME_ActiveFlag==true && b.HRMD_ActiveFlag==true && c.HRMDES_ActiveFlag==true)
                                    select new Staff_BookTranasctionDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        MI_Id = a.MI_Id,
                                        HRME_EmployeeCode=a.HRME_EmployeeCode,
                                        HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                        HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                        HRME_MobileNo=a.HRME_MobileNo,
                                        HRMD_DepartmentName=b.HRMD_DepartmentName,
                                        HRMD_Id=b.HRMD_Id,
                                        HRMDES_DesignationName=c.HRMDES_DesignationName,
                                        HRMDES_Id=c.HRMDES_Id,
                                        HRME_Photo=a.HRME_Photo,

                                    }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public Staff_BookTranasctionDTO Editdata(Staff_BookTranasctionDTO data)
        {
            //try
            //{
            //    //in this query we edit only selected data like AMST_Id and LMBANO_Id
            //    data.editlist = (from a in _LibraryContext.BookTransactionDMO
            //                     from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                     from c in _LibraryContext.BookRegisterDMO
            //                     from d in _LibraryContext.MasterEmployee
            //                     where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && a.MI_Id==c.MI_Id && a.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.Book_Trans_Id == data.Book_Trans_Id)
            //                     select new Staff_BookTranasctionDTO
            //                     {
            //                         Book_Trans_Id = a.Book_Trans_Id,
            //                         LMB_Id = c.LMB_Id,
            //                         HRME_Id=d.HRME_Id,
            //                         MI_Id = a.MI_Id,
            //                         LMBANO_Id = a.LMBANO_Id,
            //                         Issue_Date = a.Issue_Date,
            //                         Due_Date = a.Due_Date,
            //                         Return_Date = a.Return_Date,
            //                         Book_Trans_Status = a.Book_Trans_Status,
            //                         LMBANO_AccessionNo = b.LMBANO_AccessionNo,
            //                         LMB_BookType = c.LMB_BookType,
            //                         LMB_BookTitle = c.LMB_BookTitle,
            //                         Fine_Amount = a.Fine_Amount,
            //                         Renewal_Counter = a.Renewal_Counter,
            //                         Waived_Amount = a.Waived_Amount,
            //                         HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
            //                         HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
            //                         HRME_EmployeeLastName = d.HRME_EmployeeLastName,
            //                         HRME_EmployeeCode = d.HRME_EmployeeCode,

            //                     }).Distinct().ToArray();

            //    //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
            //    data.booktitle = (from a in _LibraryContext.BookRegisterDMO
            //                      from b in _LibraryContext.Lib_M_Book_Accn_DMO
            //                      from c in _LibraryContext.CirculationParameterDMO
            //                      from d in _LibraryContext.BookTransactionDMO
            //                      from e in _LibraryContext.LIB_Master_Book_VendorDMO
            //                      where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMC_Id == c.LMC_Id && d.LMBANO_Id == b.LMBANO_Id && d.Book_Trans_Id == data.Book_Trans_Id && d.MI_Id == c.MI_Id && a.LMB_Id==e.LMB_Id)
            //                      select new Staff_BookTranasctionDTO
            //                      {

            //                          LMB_Id = a.LMB_Id,
            //                          MI_Id = a.MI_Id,
            //                        //  LMBA_Id = a.LMBA_Id,
            //                          LMB_ClassNo = a.LMB_ClassNo,
            //                          LMB_BookTitle = a.LMB_BookTitle,
            //                          LMS_Id = a.LMS_Id,
            //                          LMD_Id = a.LMD_Id,
            //                          LMP_Id = a.LMP_Id,
            //                          LMB_Price = a.LMB_Price,
            //                          LMB_VolNo = a.LMB_VolNo,
            //                          LMB_EntryDate = a.LMB_EntryDate,
            //                          LML_Id = a.LML_Id,
            //                        //  Donor_Id = a.Donor_Id,
            //                          LMV_Id = e.LMV_Id,
            //                          LMC_Id = a.LMC_Id,
            //                          LMBANO_Id = b.LMBANO_Id,
            //                       //   Rack_Id = b.Rack_Id,
            //                          Max_Issue_Days = c.Max_Issue_Days,
            //                          Max_No_Renewals = c.Max_No_Renewals,
            //                          Max_Issue_Items = c.Max_Issue_Items,


            //                      }).Distinct().ToArray();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }



        public Staff_BookTranasctionDTO returndata(Staff_BookTranasctionDTO data)
        {
            //try
            //{
            //    //for update data
            //    var update = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id == data.Book_Trans_Id).SingleOrDefault();


            //    //check Fine & From Date And To Date category wise for single Transaction
            //    var fine = (from a in _LibraryContext.MasterTimeSlabDMO
            //                from b in _LibraryContext.BookRegisterDMO
            //                from c in _LibraryContext.Lib_M_Book_Accn_DMO
            //                from d in _LibraryContext.BookTransactionDMO
            //                where (a.MI_Id == b.MI_Id && a.LMC_Id == b.LMC_Id && b.LMB_Id == c.LMB_Id && c.LMBANO_Id == d.LMBANO_Id && a.MI_Id == data.MI_Id && d.Book_Trans_Id == data.Book_Trans_Id && a.Slab_ActiveFlag==true)
            //                select a).ToList();
            //    DateTime today = DateTime.Now;
            //    today = today.Date;
            //    update.Return_Date = update.Return_Date.Date;
            //    double difftotaldays = 0;

            //  //check Difference Between Actual return date and Today's Return date 
            //    if (today>update.Return_Date)
            //    {
            //        difftotaldays = (today - update.Return_Date).TotalDays;
            //    }
                
            //    double finalfine_amount = 0;
            //    if (fine.Count>0)
            //    {
            //        if (difftotaldays>0)
            //        {

            //            //check fine amount according to from date and To Date
            //            var finamt = fine.Where(t =>t.From_Day<=Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_Amount;

            //            //check fine type per day or Not
            //            var finetype = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_FineType;

            //            if (finetype != "p")
            //            {
            //                finalfine_amount = finamt;
            //            }
            //            else
            //            {
            //                finalfine_amount = finamt * difftotaldays;
            //            }
            //        }
                  
            //    }


            //    update.Return_Date = DateTime.Now;
            //    update.Book_Trans_Status = "Return";
            //    update.Fine_Amount = Convert.ToDecimal(finalfine_amount); //if any fine then set
            //    update.UpdatedDate = DateTime.Now;
            //    _LibraryContext.Update(update);
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
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }



        public Staff_BookTranasctionDTO renewaldata(Staff_BookTranasctionDTO data)
        {
            //try
            //{
                //if (data.Book_Trans_Id > 0)
                //{
                //    //check duplicate data
                //    var Duplicate = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id != data.Book_Trans_Id && t.Book_Trans_Status == data.Book_Trans_Status && t.LMBANO_Id != data.LMBANO_Id && t.MI_Id == data.MI_Id).ToArray();
                //    if (Duplicate.Count() > 0)
                //    {
                //        data.duplicate = true;
                //    }
                //    else
                //    {
                //        //for update data
                //        var update = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id == data.Book_Trans_Id).SingleOrDefault();

                //        //check Fine & From Date And To Date category wise for single Transaction
                //        var fine = (from a in _LibraryContext.MasterTimeSlabDMO
                //                    from b in _LibraryContext.BookRegisterDMO
                //                    from c in _LibraryContext.Lib_M_Book_Accn_DMO
                //                    from d in _LibraryContext.BookTransactionDMO
                //                    where (a.MI_Id == b.MI_Id && a.LMC_Id == b.LMC_Id && b.LMB_Id == c.LMB_Id && c.LMBANO_Id == d.LMBANO_Id  && a.MI_Id == data.MI_Id && d.Book_Trans_Id == data.Book_Trans_Id && a.Slab_ActiveFlag == true)
                //                    select a).ToList();

                //        DateTime today = DateTime.Now;
                //        today = today.Date;
                //        update.Return_Date = update.Return_Date.Date;
                //        double difftotaldays = 0;

                //        //check Difference Between Actual return date and Today's Return date 
                //        if (today > update.Return_Date)
                //        {
                //            difftotaldays = (today - update.Return_Date).TotalDays;
                //        }

                //        double finalfine_amount = 0;
                //        if (fine.Count > 0)
                //        {
                //            if (difftotaldays > 0)
                //            {
                //                //check fine amount according to from date and To Date
                //                var finamt = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_Amount;

                //                //check fine type per day or Not
                //                var finetype = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_FineType;

                //                if (finetype != "p")
                //                {
                //                    finalfine_amount = finamt;
                //                }
                //                else
                //                {
                //                    finalfine_amount = finamt * difftotaldays;
                //                }
                //            }

                //        }

                //        //check Maximum Renewal time according to Circulation parameter Page
                //        if (data.Max_No_Renewals > update.Renewal_Counter)
                //        {
                //            data.renew = false;
                //            update.Issue_Date = data.Issue_Date;
                //            update.Renewal_Counter = update.Renewal_Counter + 1;
                //            update.Due_Date = data.Due_Date;
                //            update.Return_Date = data.Return_Date;
                //            update.Book_Trans_Status = "Issue";
                //            update.Fine_Amount = Convert.ToDecimal(finalfine_amount);//if any fine then set
                //            update.UpdatedDate = DateTime.Now;
                //            _LibraryContext.Update(update);

                //        }
                //        else
                //        {
                //            data.renew = true;
                //        }

                //        int rowAffected = _LibraryContext.SaveChanges();

                //        if (rowAffected > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }

        //public Staff_BookTranasctionDTO getdetails(Staff_BookTranasctionDTO data)
        //{

        //    try
        //    {

        //        data.stafftlist = (from a in _LibraryContext.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag==true)
        //                            select new Staff_BookTranasctionDTO
        //                            {
        //                                HRME_Id = a.HRME_Id,
        //                                MI_Id = a.MI_Id,
        //                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
        //                                HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
        //                                HRME_EmployeeLastName = a.HRME_EmployeeLastName
        //                            }).Distinct().OrderBy(t => t.HRME_Id).ToArray();


        //        data.configdata = _LibraryContext.CirculationParameterDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

        //        //using this condition we get only issue book 
        //        var aaa = _LibraryContext.BookTransactionDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
        //        List<long> LMBANO_Id = new List<long>();
        //        if (aaa.Count() > 0)
        //        {
        //            foreach (var it in aaa)
        //            {
        //                if (it.Book_Trans_Status == "Issue")
        //                {
        //                    LMBANO_Id.Add(it.LMBANO_Id);
        //                }


        //            }
        //        }

        //        //related LMBANO_Id all data comming from Maser book Table
        //        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
        //                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
        //                          from c in _LibraryContext.LIB_Master_Book_VendorDMO
        //                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && !LMBANO_Id.Contains(b.LMBANO_Id) && a.LMB_BookType != "Reference" && a.LMB_Id==c.LMB_Id)
        //                          select new Staff_BookTranasctionDTO
        //                          {

        //                              LMB_Id = a.LMB_Id,
        //                              MI_Id = a.MI_Id,
        //                              //LMBA_Id = a.LMBA_Id,
        //                              LMB_ClassNo = a.LMB_ClassNo,
        //                              LMB_BookTitle = a.LMB_BookTitle,
        //                              LMS_Id = a.LMS_Id,
        //                              LMD_Id = a.LMD_Id,
        //                              LMP_Id = a.LMP_Id,
        //                              LMB_Price = a.LMB_Price,
        //                              LMB_VolNo = a.LMB_VolNo,
        //                              LMB_EntryDate = a.LMB_EntryDate,
        //                              LML_Id = a.LML_Id,
        //                             // Donor_Id = a.Donor_Id,
        //                              LMV_Id = c.LMV_Id,
        //                              LMC_Id = a.LMC_Id,
        //                              LMBANO_Id = b.LMBANO_Id,
        //                            //  Rack_Id = b.Rack_Id,


        //                          }).Distinct().ToArray();


        //        //get all related data for bind table grid in Html
        //        data.getalldata = (from a in _LibraryContext.BookTransactionDMO
        //                           from b in _LibraryContext.Lib_M_Book_Accn_DMO
        //                           from c in _LibraryContext.BookRegisterDMO
        //                           from d in _LibraryContext.MasterEmployee
        //                           where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && a.MI_Id==c.MI_Id && b.LMB_Id == c.LMB_Id && a.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id)
        //                           select new Staff_BookTranasctionDTO
        //                           {
        //                               Book_Trans_Id = a.Book_Trans_Id,
        //                               LMB_Id = c.LMB_Id,
        //                               LMBANO_AccessionNo=b.LMBANO_AccessionNo,
        //                               MI_Id = a.MI_Id,
        //                               ASMAY_Id = a.ASMAY_Id,
        //                               LMBANO_Id = a.LMBANO_Id,
        //                               LMB_BookType=c.LMB_BookType,
        //                               LMB_BookTitle =c.LMB_BookTitle,
        //                               Issue_Date = a.Issue_Date,
        //                               Due_Date = a.Due_Date,
        //                               Return_Date = a.Return_Date,
        //                               Book_Trans_Status = a.Book_Trans_Status,
        //                               Fine_Amount = a.Fine_Amount,
        //                               Renewal_Counter = a.Renewal_Counter,
        //                               Waived_Amount = a.Waived_Amount,
        //                               HRME_Id=d.HRME_Id,
        //                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
        //                               HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
        //                               HRME_EmployeeLastName = d.HRME_EmployeeLastName,
        //                               HRME_EmployeeCode=d.HRME_EmployeeCode,
                                       

        //                           }).Distinct().OrderBy(a => a.Book_Trans_Status).ToArray();


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO Savedata(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        if (data.Book_Trans_Id > 0)
        //        {
                
        //            data.returnval = true;
        //        }
        //        else
        //        {
        //            var Duplicate = _LibraryContext.BookTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.HRME_Id && t.LMBANO_Id == data.LMBANO_Id && t.Book_Trans_Status =="Issue").ToArray();
        //            if (Duplicate.Count() > 0)
        //            {
        //                data.duplicate = true;

        //            }
        //            else
        //            {
        //                BookTransactionDMO obj = new BookTransactionDMO();

        //                obj.MI_Id = data.MI_Id;
        //                obj.ASMAY_Id = data.ASMAY_Id;
        //                obj.HRME_Id = data.HRME_Id;
        //                obj.LMBANO_Id = data.LMBANO_Id;
        //                obj.Issue_Date = data.Issue_Date;
        //                obj.Due_Date = data.Due_Date;
        //                obj.Return_Date = data.Return_Date;
        //                obj.Fine_Amount = 0;
        //                obj.Waived_Amount = 0;
        //                obj.Renewal_Counter = 0;
        //                obj.Book_Trans_Status = "Issue";
        //                obj.AMST_Id = 0;
        //                obj.FODM_Id = 0;
        //                obj.Guest_Id = 0;
        //                obj.LMD_Id = 0;
        //                obj.CreatedDate = DateTime.Now;
        //                obj.UpdatedDate = DateTime.Now;

        //                _LibraryContext.Add(obj);
        //                int rowAffected = _LibraryContext.SaveChanges();

        //                if (rowAffected > 0)
        //                {
        //                    data.returnval = true;
        //                }
        //                else
        //                {
        //                    data.returnval = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO get_bookdetails(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        //get LMBANO_Id and related data and get Maxday's & max renewal day's, and how many item issue for that LMBANO_Id
        //        data.bookdetails = (from a in _LibraryContext.BookRegisterDMO
        //                            from b in _LibraryContext.Lib_M_Book_Accn_DMO
        //                            from c in _LibraryContext.CirculationParameterDMO
        //                            from d in _LibraryContext.LIB_Master_Book_VendorDMO
        //                            where (a.LMB_Id == b.LMB_Id && a.LMC_Id == c.LMC_Id && b.LMBANO_Id == data.LMBANO_Id && a.MI_Id == data.MI_Id && a.LMB_Id==d.LMB_Id)
        //                            select new Staff_BookTranasctionDTO
        //                            {
        //                                LMB_Id = a.LMB_Id,
        //                                LMBANO_Id = b.LMBANO_Id,
        //                                LMB_BookTitle = a.LMB_BookTitle,
        //                                LMB_VolNo = a.LMB_VolNo,
        //                                LMB_EntryDate = a.LMB_EntryDate,
        //                                LMB_Price = a.LMB_Price,
        //                                LMB_ClassNo = a.LMB_ClassNo,
        //                              //  LMBA_Id = a.LMBA_Id,
        //                                LMD_Id = a.LMD_Id,
        //                                LMS_Id = a.LMS_Id,
        //                                LMP_Id = a.LMP_Id,
        //                                LML_Id = a.LML_Id,
        //                               // Donor_Id = a.Donor_Id,
        //                                LMV_Id = d.LMV_Id,
        //                                LMC_Id = a.LMC_Id,
        //                                Max_Issue_Days = c.Max_Issue_Days,
        //                                Max_No_Renewals = c.Max_No_Renewals,
        //                                Max_Issue_Items = c.Max_Issue_Items,
        //                            }
        //                          ).Distinct().ToArray();




        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO get_Staffdetails(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        //In this query we et Staff details according to HRME_Id
        //        data.selctstaffdata = (from a in _LibraryContext.MasterEmployee
        //                            from b in _LibraryContext.HR_Master_Department
        //                            from c in _LibraryContext.HR_Master_Designation
        //                            where (a.HRMD_Id == b.HRMD_Id && a.MI_Id==b.MI_Id && c.HRMDES_Id == a.HRMDES_Id && a.MI_Id == c.MI_Id && a.MI_Id==data.MI_Id && a.HRME_Id==data.HRME_Id && a.HRME_ActiveFlag==true && b.HRMD_ActiveFlag==true && c.HRMDES_ActiveFlag==true)
        //                            select new Staff_BookTranasctionDTO
        //                            {
        //                                HRME_Id = a.HRME_Id,
        //                                MI_Id = a.MI_Id,
        //                                HRME_EmployeeCode=a.HRME_EmployeeCode,
        //                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
        //                                HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
        //                                HRME_EmployeeLastName = a.HRME_EmployeeLastName,
        //                                HRME_MobileNo=a.HRME_MobileNo,
        //                                HRMD_DepartmentName=b.HRMD_DepartmentName,
        //                                HRMD_Id=b.HRMD_Id,
        //                                HRMDES_DesignationName=c.HRMDES_DesignationName,
        //                                HRMDES_Id=c.HRMDES_Id,
        //                                HRME_Photo=a.HRME_Photo,

        //                            }).Distinct().ToArray();



        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO Editdata(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        //in this query we edit only selected data like AMST_Id and LMBANO_Id
        //        data.editlist = (from a in _LibraryContext.BookTransactionDMO
        //                         from b in _LibraryContext.Lib_M_Book_Accn_DMO
        //                         from c in _LibraryContext.BookRegisterDMO
        //                         from d in _LibraryContext.MasterEmployee
        //                         where (a.MI_Id == d.MI_Id && a.LMBANO_Id == b.LMBANO_Id && b.LMB_Id == c.LMB_Id && a.MI_Id==c.MI_Id && a.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.Book_Trans_Id == data.Book_Trans_Id)
        //                         select new Staff_BookTranasctionDTO
        //                         {
        //                             Book_Trans_Id = a.Book_Trans_Id,
        //                             LMB_Id = c.LMB_Id,
        //                             HRME_Id=d.HRME_Id,
        //                             MI_Id = a.MI_Id,
        //                             LMBANO_Id = a.LMBANO_Id,
        //                             Issue_Date = a.Issue_Date,
        //                             Due_Date = a.Due_Date,
        //                             Return_Date = a.Return_Date,
        //                             Book_Trans_Status = a.Book_Trans_Status,
        //                             LMBANO_AccessionNo = b.LMBANO_AccessionNo,
        //                             LMB_BookType = c.LMB_BookType,
        //                             LMB_BookTitle = c.LMB_BookTitle,
        //                             Fine_Amount = a.Fine_Amount,
        //                             Renewal_Counter = a.Renewal_Counter,
        //                             Waived_Amount = a.Waived_Amount,
        //                             HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
        //                             HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName,
        //                             HRME_EmployeeLastName = d.HRME_EmployeeLastName,
        //                             HRME_EmployeeCode = d.HRME_EmployeeCode,

        //                         }).Distinct().ToArray();

        //        //here we bind only issued book for return in drop-downlist becz if book is return that data not bind with Dropdown 
        //        data.booktitle = (from a in _LibraryContext.BookRegisterDMO
        //                          from b in _LibraryContext.Lib_M_Book_Accn_DMO
        //                          from c in _LibraryContext.CirculationParameterDMO
        //                          from d in _LibraryContext.BookTransactionDMO
        //                          from e in _LibraryContext.LIB_Master_Book_VendorDMO
        //                          where (a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMC_Id == c.LMC_Id && d.LMBANO_Id == b.LMBANO_Id && d.Book_Trans_Id == data.Book_Trans_Id && d.MI_Id == c.MI_Id && a.LMB_Id==e.LMB_Id)
        //                          select new Staff_BookTranasctionDTO
        //                          {

        //                              LMB_Id = a.LMB_Id,
        //                              MI_Id = a.MI_Id,
        //                            //  LMBA_Id = a.LMBA_Id,
        //                              LMB_ClassNo = a.LMB_ClassNo,
        //                              LMB_BookTitle = a.LMB_BookTitle,
        //                              LMS_Id = a.LMS_Id,
        //                              LMD_Id = a.LMD_Id,
        //                              LMP_Id = a.LMP_Id,
        //                              LMB_Price = a.LMB_Price,
        //                              LMB_VolNo = a.LMB_VolNo,
        //                              LMB_EntryDate = a.LMB_EntryDate,
        //                              LML_Id = a.LML_Id,
        //                            //  Donor_Id = a.Donor_Id,
        //                              LMV_Id = e.LMV_Id,
        //                              LMC_Id = a.LMC_Id,
        //                              LMBANO_Id = b.LMBANO_Id,
        //                           //   Rack_Id = b.Rack_Id,
        //                              Max_Issue_Days = c.Max_Issue_Days,
        //                              Max_No_Renewals = c.Max_No_Renewals,
        //                              Max_Issue_Items = c.Max_Issue_Items,


        //                          }).Distinct().ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO returndata(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        //for update data
        //        var update = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id == data.Book_Trans_Id).SingleOrDefault();


        //        //check Fine & From Date And To Date category wise for single Transaction
        //        var fine = (from a in _LibraryContext.MasterTimeSlabDMO
        //                    from b in _LibraryContext.BookRegisterDMO
        //                    from c in _LibraryContext.Lib_M_Book_Accn_DMO
        //                    from d in _LibraryContext.BookTransactionDMO
        //                    where (a.MI_Id == b.MI_Id && a.LMC_Id == b.LMC_Id && b.LMB_Id == c.LMB_Id && c.LMBANO_Id == d.LMBANO_Id && a.MI_Id == data.MI_Id && d.Book_Trans_Id == data.Book_Trans_Id && a.Slab_ActiveFlag==true)
        //                    select a).ToList();
        //        DateTime today = DateTime.Now;
        //        today = today.Date;
        //        update.Return_Date = update.Return_Date.Date;
        //        double difftotaldays = 0;

        //      //check Difference Between Actual return date and Today's Return date 
        //        if (today>update.Return_Date)
        //        {
        //            difftotaldays = (today - update.Return_Date).TotalDays;
        //        }
                
        //        double finalfine_amount = 0;
        //        if (fine.Count>0)
        //        {
        //            if (difftotaldays>0)
        //            {

        //                //check fine amount according to from date and To Date
        //                var finamt = fine.Where(t =>t.From_Day<=Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_Amount;

        //                //check fine type per day or Not
        //                var finetype = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_FineType;

        //                if (finetype != "p")
        //                {
        //                    finalfine_amount = finamt;
        //                }
        //                else
        //                {
        //                    finalfine_amount = finamt * difftotaldays;
        //                }
        //            }
                  
        //        }


        //        update.Return_Date = DateTime.Now;
        //        update.Book_Trans_Status = "Return";
        //        update.Fine_Amount = Convert.ToDecimal(finalfine_amount); //if any fine then set
        //        update.UpdatedDate = DateTime.Now;
        //        _LibraryContext.Update(update);
        //        int rowAffected = _LibraryContext.SaveChanges();

        //        if (rowAffected > 0)
        //        {
        //            data.returnval = true;
        //        }
        //        else
        //        {
        //            data.returnval = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



        //public Staff_BookTranasctionDTO renewaldata(Staff_BookTranasctionDTO data)
        //{
        //    try
        //    {
        //        if (data.Book_Trans_Id > 0)
        //        {
        //            //check duplicate data
        //            var Duplicate = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id != data.Book_Trans_Id && t.Book_Trans_Status == data.Book_Trans_Status && t.LMBANO_Id != data.LMBANO_Id && t.MI_Id == data.MI_Id).ToArray();
        //            if (Duplicate.Count() > 0)
        //            {
        //                data.duplicate = true;
        //            }
        //            else
        //            {
        //                //for update data
        //                var update = _LibraryContext.BookTransactionDMO.Where(t => t.Book_Trans_Id == data.Book_Trans_Id).SingleOrDefault();

        //                //check Fine & From Date And To Date category wise for single Transaction
        //                var fine = (from a in _LibraryContext.MasterTimeSlabDMO
        //                            from b in _LibraryContext.BookRegisterDMO
        //                            from c in _LibraryContext.Lib_M_Book_Accn_DMO
        //                            from d in _LibraryContext.BookTransactionDMO
        //                            where (a.MI_Id == b.MI_Id && a.LMC_Id == b.LMC_Id && b.LMB_Id == c.LMB_Id && c.LMBANO_Id == d.LMBANO_Id  && a.MI_Id == data.MI_Id && d.Book_Trans_Id == data.Book_Trans_Id && a.Slab_ActiveFlag == true)
        //                            select a).ToList();

        //                DateTime today = DateTime.Now;
        //                today = today.Date;
        //                update.Return_Date = update.Return_Date.Date;
        //                double difftotaldays = 0;

        //                //check Difference Between Actual return date and Today's Return date 
        //                if (today > update.Return_Date)
        //                {
        //                    difftotaldays = (today - update.Return_Date).TotalDays;
        //                }

        //                double finalfine_amount = 0;
        //                if (fine.Count > 0)
        //                {
        //                    if (difftotaldays > 0)
        //                    {
        //                        //check fine amount according to from date and To Date
        //                        var finamt = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_Amount;

        //                        //check fine type per day or Not
        //                        var finetype = fine.Where(t => t.From_Day <= Convert.ToInt32(difftotaldays) && t.To_Day >= Convert.ToInt32(difftotaldays)).SingleOrDefault().Slab_FineType;

        //                        if (finetype != "p")
        //                        {
        //                            finalfine_amount = finamt;
        //                        }
        //                        else
        //                        {
        //                            finalfine_amount = finamt * difftotaldays;
        //                        }
        //                    }

        //                }

        //                //check Maximum Renewal time according to Circulation parameter Page
        //                if (data.Max_No_Renewals > update.Renewal_Counter)
        //                {
        //                    data.renew = false;
        //                    update.Issue_Date = data.Issue_Date;
        //                    update.Renewal_Counter = update.Renewal_Counter + 1;
        //                    update.Due_Date = data.Due_Date;
        //                    update.Return_Date = data.Return_Date;
        //                    update.Book_Trans_Status = "Issue";
        //                    update.Fine_Amount = Convert.ToDecimal(finalfine_amount);//if any fine then set
        //                    update.UpdatedDate = DateTime.Now;
        //                    _LibraryContext.Update(update);

        //                }
        //                else
        //                {
        //                    data.renew = true;
        //                }

        //                int rowAffected = _LibraryContext.SaveChanges();

        //                if (rowAffected > 0)
        //                {
        //                    data.returnval = true;
        //                }
        //                else
        //                {
        //                    data.returnval = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            data.returnval = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}



    }
}
