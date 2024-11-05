using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using LibraryServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class ImportLibrarydataImpl : Interfaces.ImportLibrarydataInterface
    {
        public LibraryContext _LibraryContext;
        public ImportLibrarydataImpl(LibraryContext context)
        {
            _LibraryContext = context;
        }

        public ImportLibrarydataDTO Savedata(ImportLibrarydataDTO data)
        {
            try
            {
                data.dupcnt = 0;
                data.failcnt = 0;
                data.suscnt = 0;
                var cccccc = 3;

                if (data.booktransIMP != null)
                {
                    if (data.booktransIMP.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.booktransIMP)
                        {

                            var stdlist = _LibraryContext.Adm_M_Student.Where(a => a.AMST_AdmNo.Trim() == item.adm_no.Trim()).ToList();
                            if (stdlist.Count > 0)
                            {
                                if (item.Accession_No != "NULL")
                                {

                                    var acclist = _LibraryContext.Lib_M_Book_Accn_DMO.Where(t => t.LMBANO_AccessionNo.Trim() == item.Accession_No).ToList();
                                    if (acclist.Count > 0)
                                    {

                                        BookTransactionDMO obj = new BookTransactionDMO();

                                        obj.MI_Id = data.MI_Id;
                                        obj.LMBANO_Id = acclist[0].LMBANO_Id;

                                        if (item.LBTR_IssuedDate != "NULL")
                                        {
                                            obj.LBTR_IssuedDate = Convert.ToDateTime(item.LBTR_IssuedDate);
                                        }
                                        else
                                        {
                                            obj.LBTR_IssuedDate = DateTime.Now;
                                        }
                                        if (item.LBTR_ReturnedDate != "NULL")
                                        {
                                            obj.LBTR_ReturnedDate = Convert.ToDateTime(item.LBTR_ReturnedDate);
                                            obj.LBTR_DueDate = Convert.ToDateTime(item.LBTR_ReturnedDate);
                                        }
                                        else
                                        {
                                            obj.LBTR_ReturnedDate = DateTime.Now;
                                            obj.LBTR_DueDate = DateTime.Now;
                                        }


                                        obj.LBTR_FineCollected = 0;
                                        obj.LBTR_FineWaived = 0;
                                        obj.LBTR_Renewalcounter = 0;
                                        obj.LBTR_Status = item.LBTR_Status.Trim();
                                        obj.LBTR_ActiveFlg = true;
                                        obj.CreatedDate = DateTime.Now;
                                        obj.UpdatedDate = DateTime.Now;

                                        _LibraryContext.Add(obj);
                                        /// int rowAffected1 = _LibraryContext.SaveChanges();
                                        LIB_Book_Transaction_StudentDMO obj1 = new LIB_Book_Transaction_StudentDMO();
                                        obj1.LBTR_Id = obj.LBTR_Id;
                                        obj1.AMST_Id = stdlist[0].AMST_Id;
                                        obj1.LBTRS_ActiveFlg = true;
                                        obj1.CreatedDate = DateTime.Now;
                                        obj1.UpdatedDate = DateTime.Now;

                                        _LibraryContext.Add(obj1);

                                        _LibraryContext.Add(obj);
                                        int rowAffected = _LibraryContext.SaveChanges();

                                        if (rowAffected > 0)
                                        {
                                            data.suscnt += 1;

                                        }
                                        else
                                        {
                                            data.failcnt += 1;

                                        }

                                    }


                                }

                            }


                        }
                        data.returnval = true;
                    }
                }



                if (data.masteracces != null)
                {
                    if (data.masteracces.Length > 0)
                    {
                        var parid = 0;
                        long LMP_Id = 0;
                        string LMB_CallNo = "";

                        foreach (var item in data.masteracces)
                        {
                            data.failcnt += 1;
                            if (item.LMP_PublisherName != "NULL" && item.LMP_PublisherName != "" && item.LMP_PublisherName != null && item.LMP_PublisherName != "NA")
                            {
                                var departmentlist = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.ToLower().Trim() == item.LMP_PublisherName.ToLower().Trim() && t.LMP_ActiveFlg == true).Distinct().ToList();
                                if (departmentlist.Count > 0)
                                {
                                    LMP_Id = departmentlist[0].LMP_Id;
                                }
                            }
                            if (item.LMB_CallNo != "NULL" && item.LMB_CallNo != "" && item.LMB_CallNo != null && item.LMB_CallNo != "NA")
                            {
                                LMB_CallNo = item.LMB_CallNo;
                            }

                            var bblist = (from a in _LibraryContext.BookRegisterDMO
                                          from b in _LibraryContext.LIB_Master_Book_Library_DMO
                                          where a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_BookTitle.ToLower().Trim() == item.LMB_BookSubTitle.ToLower().Trim()
                                          && b.LMAL_Id == data.LMAL_Id && a.LMP_Id == LMP_Id && a.LMB_CallNo== LMB_CallNo
                                          select new ImportLibrarydataDTO { LMB_Id = b.LMB_Id }).ToList();

                            //var bblist = _LibraryContext.BookRegisterDMO.Where(t => t.MI_Id == data.MI_Id && t.LMB_BookTitle.ToLower().Trim()==item.LMB_BookSubTitle.ToLower().Trim()).Select(d => new ImportLibrarydataDTO { LMB_Id = d.LMB_Id }).Distinct().ToList();

                            if (bblist.Count > 0)
                            {


                                Lib_M_Book_Accn_DMO obj = new Lib_M_Book_Accn_DMO();

                                if (item.LMB_BookSubTitle != "NULL" && item.LMB_BookSubTitle != "" && item.LMB_BookSubTitle != null)
                                {
                                    obj.LMB_Id = bblist[0].LMB_Id;
                                }

                                if (item.LMBANO_AccessionNo == "NULL" || item.LMBANO_AccessionNo == "" || item.LMBANO_AccessionNo == null)
                                {

                                }
                                else
                                {
                                    obj.LMBANO_AccessionNo = item.LMBANO_AccessionNo.Trim();
                                }

                                if (item.LMBANO_AvialableStatus != "NULL" && item.LMBANO_AvialableStatus != "" && item.LMBANO_AvialableStatus != null)
                                {
                                    obj.LMBANO_AvialableStatus = item.LMBANO_AvialableStatus;
                                }


                                if (item.LMBANO_DeletedDate != "NULL" && item.LMBANO_DeletedDate != "" && item.LMBANO_DeletedDate != null)
                                {
                                    obj.LMBANO_LostDamagedDate = Convert.ToDateTime(item.LMBANO_DeletedDate);
                                }
                                if (item.LMBANO_DeleteReason != "NULL" && item.LMBANO_DeleteReason != "" && item.LMBANO_DeleteReason != null)
                                {
                                    obj.LMBANO_LostDamagedReason = item.LMBANO_DeleteReason;
                                }

                                if (item.LMBANO_ActiveFlg == "NULL" || item.LMBANO_ActiveFlg == "" || item.LMBANO_ActiveFlg == "1")
                                {
                                    obj.LMBANO_ActiveFlg = true;
                                }
                                else
                                {
                                    obj.LMBANO_ActiveFlg = false;
                                }
                                if (item.Rack_Name != "NULL" && item.Rack_Name != "" && item.Rack_Name != null)
                                {
                                    var LMRA_Id = _LibraryContext.RackDetailsDMO.Where(R => R.LMRA_RackName == item.Rack_Name && R.MI_Id == data.MI_Id).FirstOrDefault().LMRA_Id;
                                    obj.LMRA_Id = Convert.ToInt64(LMRA_Id);
                                }
                                else
                                {
                                    var LMRA_Id = _LibraryContext.RackDetailsDMO.Where(R => R.MI_Id == data.MI_Id).FirstOrDefault().LMRA_Id;
                                    obj.LMRA_Id = Convert.ToInt64(LMRA_Id);
                                }


                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    //data.failcnt += 1;

                                }

                            }
                            else
                            {

                            }

                            data.returnval = true;
                        }
                    }
                }


                else
                    if (data.mastervender != null)
                {
                    if (data.mastervender.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.mastervender)
                        {

                            if (item.LMV_VendorName.Trim() != "NULL")
                            {
                                var mrauthour = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.Trim() == item.LMV_VendorName.Trim() && t.LMV_ActiveFlg == true).ToList();
                                if (mrauthour.Count > 0)
                                {
                                    data.LMV_Id = mrauthour[0].LMV_Id;
                                }
                                else
                                {
                                    MasterVanderDMO or = new MasterVanderDMO();
                                    or.MI_Id = data.MI_Id;
                                    or.LMV_VendorName = item.LMV_VendorName.Trim();
                                    or.LMV_ActiveFlg = true;
                                    or.CreatedDate = DateTime.Now;
                                    or.UpdatedDate = DateTime.Now;
                                    _LibraryContext.Add(or);
                                    _LibraryContext.SaveChanges();

                                    var mrauthour1 = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_VendorName.Trim() == item.LMV_VendorName.Trim() && t.LMV_ActiveFlg == true).ToList();
                                    if (mrauthour1.Count > 0)
                                    {
                                        data.LMV_Id = mrauthour1[0].LMV_Id;
                                    }

                                }


                                //var 
                                //var Duplicate = _LibraryContext.MasterAuthorDMO.Where(t => t.LMB_Id == data.MI_Id && t. == item.masterauthor).ToList();


                                //if (Duplicate.Count() > 0)
                                //{
                                //    data.dupcnt += 1;
                                //    //  data.duplicate = true;
                                //}
                                //else
                                //{

                                var existingauthor = (from a in _LibraryContext.BookRegisterDMO
                                                      from b in _LibraryContext.LIB_Master_Book_VendorDMO
                                                      from c in _LibraryContext.LIB_Master_Book_Library_DMO
                                                      where a.MI_Id == data.MI_Id && a.LMB_Id == b.LMB_Id && a.LMB_BookTitle.Trim() == item.LMB_BookSubTitle.Trim() && c.LMB_Id == a.LMB_Id && c.LMAL_Id == data.LMAL_Id
                                                      select new ImportLibrarydataDTO
                                                      {
                                                          LMB_Id = b.LMB_Id
                                                      }).ToList();

                                if (existingauthor.Count == 0)
                                {


                                    var bblist = (from a in _LibraryContext.BookRegisterDMO
                                                  from b in _LibraryContext.LIB_Master_Book_Library_DMO
                                                  where a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_BookTitle.ToLower().Trim() == item.LMB_BookSubTitle.ToLower().Trim() && b.LMAL_Id == data.LMAL_Id
                                                  select new ImportLibrarydataDTO { LMB_Id = b.LMB_Id }).ToList();

                                    if (bblist.Count > 0)
                                    {
                                        if (item.LMV_VendorName != "NULL" || item.LMV_VendorName != "" || item.LMV_VendorName != null)
                                        {

                                            LIB_Master_Book_VendorDMO obj = new LIB_Master_Book_VendorDMO();

                                            if (item.LMB_BookSubTitle != "NULL" && item.LMB_BookSubTitle != "" && item.LMB_BookSubTitle != null)
                                            {
                                                obj.LMB_Id = bblist[0].LMB_Id;
                                            }


                                            if (item.LMV_ActiveFlg == "NULL" || item.LMV_ActiveFlg == "" || item.LMV_ActiveFlg == "1")
                                            {
                                                obj.LMBV_ActiveFlg = true;
                                            }
                                            else
                                            {
                                                obj.LMBV_ActiveFlg = true;
                                            }
                                            obj.LMV_Id = data.LMV_Id;
                                            obj.CreatedDate = DateTime.Now;
                                            obj.UpdatedDate = DateTime.Now;
                                            _LibraryContext.Add(obj);
                                            int rowAffected = _LibraryContext.SaveChanges();

                                            if (rowAffected > 0)
                                            {
                                                data.suscnt += 1;

                                            }
                                            else
                                            {
                                                data.failcnt += 1;

                                            }
                                        }

                                    }
                                }
                                data.returnval = true;
                            }
                        }
                    }
                }

                else if (data.masterauthor != null)
                {
                    if (data.masterauthor.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterauthor)
                        {
                            data.failcnt += 1;
                            long LMP_Id = 0;
                            string LMB_CallNo = "";

                            if (item.LMBA_AuthorFirstName.Trim() != "NULL")
                            {
                                var mrauthour = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAU_AuthorFirstName.Trim() == item.LMBA_AuthorFirstName.Trim() && t.LMAU_ActiveFlg == true).ToList();
                                if (mrauthour.Count > 0)
                                {
                                    data.LMAU_Id = mrauthour[0].LMAU_Id;
                                }
                                else
                                {
                                    LIB_Master_Author_DMO or = new LIB_Master_Author_DMO();
                                    or.MI_Id = data.MI_Id;
                                    or.LMAU_AuthorFirstName = item.LMBA_AuthorFirstName.Trim();
                                    or.LMAU_ActiveFlg = true;
                                    or.CreatedDate = DateTime.Now;
                                    or.UpdatedDate = DateTime.Now;
                                    _LibraryContext.Add(or);
                                    _LibraryContext.SaveChanges();

                                    var mrauthour1 = _LibraryContext.LIB_Master_Author_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAU_AuthorFirstName.Trim() == item.LMBA_AuthorFirstName.Trim() && t.LMAU_ActiveFlg == true).ToList();
                                    if (mrauthour1.Count > 0)
                                    {
                                        data.LMAU_Id = mrauthour1[0].LMAU_Id;
                                    }

                                }
                                if (item.LMP_PublisherName != "NULL" && item.LMP_PublisherName != "" && item.LMP_PublisherName != null && item.LMP_PublisherName != "NA")
                                {
                                    var departmentlist = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id
                                    && t.LMP_PublisherName.ToLower().Trim() == item.LMP_PublisherName.ToLower().Trim() && t.LMP_ActiveFlg == true).Distinct().ToList();
                                    if (departmentlist.Count > 0)
                                    {
                                        LMP_Id = departmentlist[0].LMP_Id;
                                    }
                                }
                                if (item.LMB_CallNo != "NULL" && item.LMB_CallNo != "" && item.LMB_CallNo != null && item.LMB_CallNo != "NA")
                                {
                                    LMB_CallNo = item.LMB_CallNo;
                                }
                                var existingauthor = (from a in _LibraryContext.BookRegisterDMO
                                                      from b in _LibraryContext.MasterAuthorDMO
                                                      from bc in _LibraryContext.LIB_Master_Book_Library_DMO
                                                      where a.MI_Id == data.MI_Id && a.LMB_Id == b.LMB_Id && a.LMB_BookTitle.Trim() == item.LMB_BookSubTitle.Trim() && a.LMB_Id == bc.LMB_Id
                                                      && bc.LMAL_Id == data.LMAL_Id && a.LMP_Id == LMP_Id && a.LMB_CallNo== LMB_CallNo
                                                      select new ImportLibrarydataDTO
                                                      {
                                                          LMB_Id = b.LMB_Id
                                                      }).ToList();

                                if (existingauthor.Count == 0)
                                {
                                    var bblist = (from a in _LibraryContext.BookRegisterDMO
                                                  from b in _LibraryContext.LIB_Master_Book_Library_DMO
                                                  where a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_BookTitle.ToLower().Trim() == item.LMB_BookSubTitle.ToLower().Trim() && b.LMAL_Id == data.LMAL_Id && a.LMP_Id == LMP_Id && a.LMB_CallNo == LMB_CallNo
                                                  select new ImportLibrarydataDTO { LMB_Id = b.LMB_Id }).ToList();

                                    if (bblist.Count > 0)
                                    {
                                        if (item.LMBA_AuthorFirstName != "NULL" || item.LMBA_AuthorFirstName != "" || item.LMBA_AuthorFirstName != null)
                                        {

                                            MasterAuthorDMO obj = new MasterAuthorDMO();

                                            if (item.LMB_BookSubTitle != "NULL" && item.LMB_BookSubTitle != "" && item.LMB_BookSubTitle != null)
                                            {
                                                obj.LMB_Id = bblist[0].LMB_Id;
                                            }

                                            if (item.LMBA_AuthorFirstName == "NULL" || item.LMBA_AuthorFirstName == "" || item.LMBA_AuthorFirstName == null)
                                            {

                                            }
                                            else
                                            {
                                                obj.LMBA_AuthorFirstName = item.LMBA_AuthorFirstName;
                                            }

                                            if (item.LMBA_AuthorMiddleName != "NULL" && item.LMBA_AuthorMiddleName != "" && item.LMBA_AuthorMiddleName != null)
                                            {
                                                obj.LMBA_AuthorMiddleName = item.LMBA_AuthorMiddleName;
                                            }


                                            if (item.LMBA_AuthorLastName != "NULL" && item.LMBA_AuthorLastName != "" && item.LMBA_AuthorLastName != null)
                                            {
                                                obj.LMBA_AuthorLastName = item.LMBA_AuthorLastName;
                                            }

                                            if (item.LMBA_MainAuthorFlg == "NULL" || item.LMBA_MainAuthorFlg == "" || item.LMBA_MainAuthorFlg == "0")
                                            {
                                                obj.LMBA_MainAuthorFlg = false;
                                            }
                                            else
                                            {
                                                obj.LMBA_MainAuthorFlg = true;
                                            }
                                            if (item.LMBA_ActiveFlg == "NULL" || item.LMBA_ActiveFlg == "" || item.LMBA_ActiveFlg == "1")
                                            {
                                                obj.LMBA_ActiveFlg = true;
                                            }
                                            else
                                            {
                                                obj.LMBA_ActiveFlg = false;
                                            }
                                            obj.LMAU_Id = data.LMAU_Id;
                                            obj.CreatedDate = DateTime.Now;
                                            obj.UpdatedDate = DateTime.Now;
                                            _LibraryContext.Add(obj);
                                            int rowAffected = _LibraryContext.SaveChanges();

                                            if (rowAffected > 0)
                                            {
                                                data.suscnt += 1;

                                            }
                                            else
                                            {
                                                data.failcnt += 1;

                                            }
                                        }

                                    }
                                }


                                //}
                                data.returnval = true;
                            }
                        }
                    }
                }
                //  }

                else


             if (data.mastersub != null)
                {
                    if (data.mastersub.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.mastersub)
                        {
                            var Duplicate = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.Trim() == item.LMS_SubjectName.Trim()).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dupcnt += 1;
                                //  data.duplicate = true;
                            }
                            else
                            {
                                MasterSubject_DMO obj = new MasterSubject_DMO();

                                if (item.LMS_ParentId == "NULL" || item.LMS_ParentId == "" || item.LMS_ParentId == null)
                                {

                                }
                                else
                                {
                                    obj.LMS_ParentId = Convert.ToInt64(item.LMS_ParentId);
                                }

                                if (item.LMS_Level == "NULL" || item.LMS_Level == "" || item.LMS_Level == null)
                                {

                                }
                                else
                                {
                                    obj.LMS_Level = Convert.ToInt64(item.LMS_Level);
                                }
                                obj.MI_Id = data.MI_Id;


                                if (item.LMS_SubjectName == "NULL" || item.LMS_SubjectName == "" || item.LMS_SubjectName == null)
                                {

                                }
                                else
                                {
                                    obj.LMS_SubjectName = item.LMS_SubjectName.Trim();
                                }


                                if (item.LMS_SubjectNo == "NULL" || item.LMS_SubjectNo == "" || item.LMS_SubjectNo == null)
                                {

                                }
                                else
                                {
                                    obj.LMS_SubjectNo = item.LMS_SubjectNo;
                                }

                                if (item.LMS_ActiveFlg == "NULL" || item.LMS_ActiveFlg == "" || item.LMS_ActiveFlg == "1")
                                {
                                    obj.LMS_ActiveFlg = true;
                                }
                                else
                                {
                                    obj.LMS_ActiveFlg = false;
                                }

                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    data.failcnt += 1;

                                }

                            }
                            data.returnval = true;
                        }

                    }
                }

                else
          if (data.masterdep != null)
                {
                    if (data.masterdep.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterdep)
                        {
                            var Duplicate = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName == item.LMD_DepartmentName).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dupcnt += 1;
                                //  data.duplicate = true;
                            }
                            else
                            {
                                MasterDepartmentDMO obj = new MasterDepartmentDMO();


                                obj.MI_Id = data.MI_Id;
                                obj.LMD_DepartmentName = item.LMD_DepartmentName;

                                if (item.LMD_ActiveFlg == "NULL" || item.LMD_ActiveFlg == "" || item.LMD_ActiveFlg == "1")
                                {
                                    obj.LMD_ActiveFlg = true;
                                }
                                else
                                {
                                    obj.LMD_ActiveFlg = false;
                                }

                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    data.failcnt += 1;

                                }

                            }
                            data.returnval = true;
                        }

                    }
                }

                else

                 if (data.masterlang != null)
                {
                    if (data.masterlang.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterlang)
                        {
                            var Duplicate = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName == item.LML_LanguageName).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dupcnt += 1;
                                //  data.duplicate = true;
                            }
                            else
                            {
                                MasterLanguageDMO obj = new MasterLanguageDMO();


                                obj.MI_Id = data.MI_Id;
                                obj.LML_LanguageName = item.LML_LanguageName;

                                if (item.LML_ActiveFlg == "NULL" || item.LML_ActiveFlg == "" || item.LML_ActiveFlg == "1")
                                {
                                    obj.LML_ActiveFlg = true;
                                }
                                else
                                {
                                    obj.LML_ActiveFlg = false;
                                }

                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    data.failcnt += 1;

                                }

                            }
                            data.returnval = true;
                        }

                    }
                }
                else
                 if (data.masterpubl != null)
                {
                    if (data.masterpubl.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterpubl)
                        {
                            var Duplicate = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.Trim() == item.LMP_PublisherName.Trim()).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dupcnt += 1;
                                //  data.duplicate = true;
                            }
                            else
                            {
                                MasterPublisherDMO obj = new MasterPublisherDMO();


                                obj.MI_Id = data.MI_Id;
                                obj.LMP_PublisherName = item.LMP_PublisherName.Trim();

                                if (item.LMP_MobileNo == "NULL" || item.LMP_MobileNo == "" || item.LMP_MobileNo == "1")
                                {
                                    obj.LMP_MobileNo = 0;
                                }
                                else
                                {
                                    obj.LMP_MobileNo = Convert.ToInt64(item.LMP_MobileNo);
                                }
                                if (item.LMP_PhoneNo == "NULL")
                                {
                                    obj.LMP_PhoneNo = "";
                                }
                                else
                                {
                                    obj.LMP_PhoneNo = item.LMP_PhoneNo;
                                }

                                if (item.LMP_EMailId == "NULL")
                                {
                                    obj.LMP_EMailId = "";
                                }
                                else
                                {
                                    obj.LMP_EMailId = item.LMP_EMailId;
                                }

                                if (item.LMP_Address == "NULL")
                                {
                                    obj.LMP_Address = "";
                                }
                                else
                                {
                                    obj.LMP_Address = item.LMP_Address;
                                }
                                if (item.LMP_ActiveFlg == "NULL" || item.LMP_ActiveFlg == "" || item.LMP_ActiveFlg == "1")
                                {
                                    obj.LMP_ActiveFlg = true;
                                }
                                else
                                {
                                    obj.LMP_ActiveFlg = false;
                                }
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    data.failcnt += 1;

                                }

                            }
                            data.returnval = true;
                        }

                    }
                }
                else
                 if (data.masterrack != null)
                {
                    if (data.masterrack.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterrack)
                        {
                            var Duplicate = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName.Trim() == item.LMRA_RackName.Trim()).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dupcnt += 1;
                                //  data.duplicate = true;
                            }
                            else
                            {
                                RackDetailsDMO obj = new RackDetailsDMO();


                                obj.MI_Id = data.MI_Id;
                                obj.LMRA_RackName = item.LMRA_RackName.Trim();

                                if (item.LMRA_DisplayColour == "NULL" || item.LMRA_DisplayColour == "")
                                {
                                    obj.LMRA_DisplayColour = "";
                                }
                                else
                                {
                                    obj.LMRA_DisplayColour = item.LMRA_DisplayColour;
                                }
                                if (item.LMRA_Location == "NULL")
                                {
                                    obj.LMRA_Location = "";
                                }
                                else
                                {
                                    obj.LMRA_Location = item.LMRA_Location;
                                }

                                if (item.LMRA_FloorName == "NULL")
                                {
                                    obj.LMRA_FloorName = "";
                                }
                                else
                                {
                                    obj.LMRA_FloorName = item.LMRA_FloorName;
                                }

                                obj.LMRA_ActiveFlag = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _LibraryContext.Add(obj);
                                int rowAffected = _LibraryContext.SaveChanges();

                                if (rowAffected > 0)
                                {
                                    data.suscnt += 1;

                                }
                                else
                                {
                                    data.failcnt += 1;

                                }

                            }
                            data.returnval = true;
                        }

                    }
                }

                else
                 if (data.masterbook != null)
                {
                    if (data.masterbook.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.masterbook)
                        {
                            data.failcnt += 1;
                            long LMP_Id = 0;
                            string LMB_CallNo = "";
                            //LMP_PublisherName 
                            if (item.LMB_BookTitle != "NULL")
                            {
                                if (item.LMP_PublisherName != "NULL" && item.LMP_PublisherName != "" && item.LMP_PublisherName != null && item.LMP_PublisherName != "NA")
                                {
                                    var departmentlist = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.ToLower().Trim() == item.LMP_PublisherName.ToLower().Trim() && t.LMP_ActiveFlg == true).Distinct().ToList();
                                    if (departmentlist.Count > 0)
                                    {
                                        LMP_Id = departmentlist[0].LMP_Id;
                                    }
                                }
                                if (item.LMB_CallNo != "NULL" && item.LMB_CallNo != "" && item.LMB_CallNo != null && item.LMB_CallNo != "NA")
                                {
                                    LMB_CallNo = item.LMB_CallNo;
                                }

                                var Duplicate = (from a in _LibraryContext.BookRegisterDMO
                                                 from b in _LibraryContext.LIB_Master_Book_Library_DMO
                                                 where a.LMB_Id == b.LMB_Id && a.MI_Id == data.MI_Id && a.LMB_BookTitle.ToLower().Trim() == item.LMB_BookTitle.ToLower().Trim()
                                                 && b.LMAL_Id == data.LMAL_Id && a.LMP_Id == LMP_Id && a.LMB_CallNo== item.LMB_CallNo
                                                 select a).ToList();

                                if (Duplicate.Count() > 0)
                                {
                                    data.dupcnt += 1;
                                    //  data.duplicate = true;
                                }
                                else
                                {
                                    if (Duplicate.Count() == 0)
                                    {
                                        BookRegisterDMO obj = new BookRegisterDMO();

                                        obj.MI_Id = data.MI_Id;
                                        obj.LMB_BookTitle = item.LMB_BookTitle.Trim();

                                        if (item.LMB_BookSubTitle != "NULL" && item.LMB_BookSubTitle != "" && item.LMB_BookSubTitle != null && item.LMB_BookSubTitle != "NA")
                                        {
                                            obj.LMB_BookSubTitle = item.LMB_BookSubTitle;
                                        }

                                        if (item.LMB_CallNo != "NULL" && item.LMB_CallNo != "" && item.LMB_CallNo != null && item.LMB_CallNo != "NA")
                                        {
                                            obj.LMB_CallNo = item.LMB_CallNo;
                                        }
                                        if (item.LMB_BookNo != "NULL" && item.LMB_BookNo != "" && item.LMB_BookNo != null && item.LMB_CallNo != "NA")
                                        {
                                            obj.LMB_BookNo = item.LMB_BookNo;
                                        }

                                        if (item.LMB_ClassNo != "NULL" && item.LMB_ClassNo != "" && item.LMB_ClassNo != null && item.LMB_ClassNo != "NA")
                                        {
                                            obj.LMB_ClassNo = item.LMB_ClassNo;
                                        }
                                        if (item.LMB_ISBNNo != "NULL" && item.LMB_ISBNNo != "" && item.LMB_ISBNNo != null && item.LMB_ISBNNo != "NA")
                                        {
                                            obj.LMB_ISBNNo = item.LMB_ISBNNo;
                                        }

                                        if (item.LMB_VolNo != "NULL" && item.LMB_VolNo != "" && item.LMB_VolNo != null && item.LMB_VolNo != "NA")
                                        {
                                            obj.LMB_VolNo = item.LMB_VolNo;
                                        }
                                        //if (item.LMB_BookType != "NULL" && item.LMB_BookType != "" && item.LMB_BookType != null)
                                        //{
                                        //    obj.LMB_BookType = item.LMB_BookType;
                                        //}

                                        //if (item.LMB_BookType != "NULL" && item.LMB_BookType != "" && item.LMB_BookType != null)
                                        //{
                                        //        obj.LMB_BookType = item.LMB_BookType.Trim() ;
                                        //}
                                        obj.LMB_BookType = item.LMB_BookType.Trim();

                                        if (item.LMD_DepartmentName != "NULL" && item.LMD_DepartmentName != "" && item.LMD_DepartmentName != null && item.LMD_DepartmentName != "NA")
                                        {

                                            var departmentlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_DepartmentName.ToLower().Trim() == item.LMD_DepartmentName.ToLower().Trim() && t.LMD_ActiveFlg == true).Distinct().ToList();
                                            if (departmentlist.Count > 0)
                                            {
                                                obj.LMD_Id = departmentlist[0].LMD_Id;
                                            }
                                            else
                                            {
                                                MasterDepartmentDMO md = new MasterDepartmentDMO();
                                                md.MI_Id = data.MI_Id;
                                                md.LMD_DepartmentName = item.LMD_DepartmentName.ToUpper();

                                                md.LMD_ActiveFlg = true;


                                                md.CreatedDate = DateTime.Now;
                                                md.UpdatedDate = DateTime.Now;
                                                _LibraryContext.Add(md);
                                                _LibraryContext.SaveChanges();
                                                obj.LMD_Id = md.LMD_Id;
                                            }


                                        }

                                        if (item.LML_LanguageName != "NULL" && item.LML_LanguageName != "" && item.LML_LanguageName != null && item.LML_LanguageName != "NA")
                                        {

                                            var departmentlist = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_LanguageName.ToLower().Trim() == item.LML_LanguageName.ToLower().Trim() && t.LML_ActiveFlg == true).Distinct().ToList();
                                            if (departmentlist.Count > 0)
                                            {
                                                obj.LML_Id = departmentlist[0].LML_Id;
                                            }
                                            else
                                            {
                                                MasterLanguageDMO md = new MasterLanguageDMO();
                                                md.MI_Id = data.MI_Id;
                                                md.LML_LanguageName = item.LML_LanguageName.ToUpper();

                                                md.LML_ActiveFlg = true;


                                                md.CreatedDate = DateTime.Now;
                                                md.UpdatedDate = DateTime.Now;
                                                _LibraryContext.Add(md);
                                                _LibraryContext.SaveChanges();
                                                obj.LML_Id = md.LML_Id;
                                            }


                                        }


                                        if (item.LMS_SubjectName != "NULL" && item.LMS_SubjectName != "" && item.LMS_SubjectName != null && item.LMS_SubjectName != "NA")
                                        {
                                            var departmentlist = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_SubjectName.ToLower().Trim() == item.LMS_SubjectName.ToLower().Trim() && t.LMS_ActiveFlg == true).Distinct().ToList();
                                            if (departmentlist.Count > 0)
                                            {
                                                obj.LMS_Id = departmentlist[0].LMS_Id;
                                            }
                                            else
                                            {
                                                MasterSubject_DMO md = new MasterSubject_DMO();
                                                md.MI_Id = data.MI_Id;
                                                md.LMS_SubjectName = item.LMS_SubjectName.ToUpper();

                                                md.LMS_ActiveFlg = true;
                                                md.CreatedDate = DateTime.Now;
                                                md.UpdatedDate = DateTime.Now;
                                                _LibraryContext.Add(md);
                                                _LibraryContext.SaveChanges();
                                                obj.LMS_Id = md.LMS_Id;
                                            }


                                        }
                                        if (item.LMP_PublisherName != "NULL" && item.LMP_PublisherName != "" && item.LMP_PublisherName != null && item.LMP_PublisherName != "NA")
                                        {
                                            var departmentlist = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.ToLower().Trim() == item.LMP_PublisherName.ToLower().Trim() && t.LMP_ActiveFlg == true).Distinct().ToList();
                                            if (departmentlist.Count > 0)
                                            {
                                                obj.LMP_Id = departmentlist[0].LMP_Id;
                                            }
                                            else
                                            {
                                                MasterPublisherDMO md = new MasterPublisherDMO();
                                                md.MI_Id = data.MI_Id;
                                                md.LMP_PublisherName = item.LMP_PublisherName;

                                                md.LMP_ActiveFlg = true;
                                                md.CreatedDate = DateTime.Now;
                                                md.UpdatedDate = DateTime.Now;
                                                _LibraryContext.Add(md);

                                                obj.LMP_Id = md.LMP_Id;
                                                _LibraryContext.SaveChanges();

                                                var departmentlist1 = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_PublisherName.ToLower() == item.LMP_PublisherName.ToLower() && t.LMP_ActiveFlg == true).Distinct().ToList();
                                                if (departmentlist1.Count > 0)
                                                {
                                                    obj.LMP_Id = departmentlist1[0].LMP_Id;
                                                }
                                            }


                                        }

                                        if (item.LMC_Id != "NULL" && item.LMC_Id != "" && item.LMC_Id != null && item.LMC_Id != "NA")
                                        {
                                            var departmentlist = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_CategoryName.ToLower().Trim() == item.LMC_Id.ToLower().Trim() && t.LMC_ActiveFlag == true).Distinct().ToList();
                                            if (departmentlist.Count > 0)
                                            {
                                                obj.LMC_Id = departmentlist[0].LMC_Id;
                                            }
                                            else
                                            {
                                                cccccc += 1;
                                                MasterCategoryDMO md = new MasterCategoryDMO();
                                                md.MI_Id = data.MI_Id;
                                                md.LMC_CategoryName = item.LMC_Id;
                                                md.LMC_BNBFlg = "Book";
                                                md.LMC_ActiveFlag = true;
                                                md.LMC_CategoryCode = cccccc.ToString();
                                                md.CreatedDate = DateTime.Now;
                                                md.UpdatedDate = DateTime.Now;
                                                _LibraryContext.Add(md);

                                                obj.LMP_Id = md.LMC_Id;
                                                _LibraryContext.SaveChanges();

                                                var departmentlist1 = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_CategoryName.ToLower() == item.LMC_Id.ToLower() && t.LMC_ActiveFlag == true).Distinct().ToList();
                                                if (departmentlist1.Count > 0)
                                                {
                                                    obj.LMC_Id = departmentlist1[0].LMC_Id;
                                                }
                                            }


                                        }
                                        else
                                        {
                                            obj.LMC_Id = 3;
                                        }

                                        if (item.LMB_BillNo != "NULL" && item.LMB_BillNo != "" && item.LMB_BillNo != null && item.LMB_BillNo != "NA")
                                        {
                                            obj.LMB_BillNo = item.LMB_BillNo;
                                        }

                                        if (item.LMB_VoucherNo != "NULL" && item.LMB_VoucherNo != "" && item.LMB_VoucherNo != null && item.LMB_VoucherNo != "NA")
                                        {
                                            obj.LMB_VoucherNo = item.LMB_VoucherNo;
                                        }

                                        if (item.LMB_Price != "NULL" && item.LMB_Price != "" && item.LMB_Price != null && item.LMB_Price != "NA")
                                        {
                                            obj.LMB_Price = Convert.ToDecimal(item.LMB_Price);
                                        }
                                        else
                                        {
                                            obj.LMB_Price = 0;
                                        }


                                        if (item.LMB_Discount != "NULL" && item.LMB_Discount != "" && item.LMB_Discount != null && item.LMB_Discount != "NA")
                                        {
                                            obj.LMB_Discount = Convert.ToDecimal(item.LMB_Discount);
                                        }

                                        if (item.LMB_NetPrice != "NULL" && item.LMB_NetPrice != "" && item.LMB_NetPrice != null && item.LMB_NetPrice != "NA")
                                        {
                                            obj.LMB_NetPrice = Convert.ToDecimal(item.LMB_NetPrice);
                                        }

                                        if (item.LMB_BindingType != "NULL" && item.LMB_BindingType != "" && item.LMB_BindingType != null && item.LMB_BindingType != "NA")
                                        {
                                            obj.LMB_BindingType = item.LMB_BindingType;
                                        }

                                        if (item.LMB_PurchaseDate != "NULL" && item.LMB_PurchaseDate != "" && item.LMB_PurchaseDate != null && item.LMB_PurchaseDate != "NA")
                                        {
                                            // obj.LMB_PurchaseDate = Convert.ToDateTime(item.LMB_PurchaseDate);
                                            if (item.LMB_PurchaseDate.Contains("-"))
                                            {
                                                DateTime DT = DateTime.ParseExact(item.LMB_PurchaseDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                                obj.LMB_PurchaseDate = DT;
                                            }
                                            else
                                            {
                                                DateTime DT = DateTime.ParseExact(item.LMB_PurchaseDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                                obj.LMB_PurchaseDate = DT;
                                            }


                                            //DateTime date = DateTime.ParseExact(item.LMB_PurchaseDate.Trim(), "dd/MM/YYYY", null);
                                            // item.LMB_PurchaseDate = "10/19/2016";
                                            // 

                                        }
                                        else
                                        {
                                            obj.LMB_PurchaseDate = DateTime.Now;
                                        }
                                        //  obj.LMB_PurchaseDate = DateTime.Now;
                                        //   obj.LMB_EntryDate = DateTime.Now;
                                        if (item.LMB_EntryDate != "NULL" && item.LMB_EntryDate != "" && item.LMB_EntryDate != null && item.LMB_EntryDate != "NA")
                                        {

                                            if (item.LMB_EntryDate.Contains("-"))
                                            {
                                                DateTime DT = DateTime.ParseExact(item.LMB_EntryDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                                obj.LMB_EntryDate = DT;
                                                if (item.LMB_PurchaseDate == "NULL")
                                                {
                                                    obj.LMB_PurchaseDate = DT;
                                                }

                                            }
                                            else
                                            {
                                                DateTime DT = DateTime.ParseExact(item.LMB_EntryDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                                                obj.LMB_EntryDate = DT;
                                                if (item.LMB_PurchaseDate == "NULL")
                                                {
                                                    obj.LMB_PurchaseDate = DT;
                                                }
                                            }

                                            // DateTime date = DateTime.ParseExact(item.LMB_EntryDate, "dd/MM/YYYY", null);
                                            //obj.LMB_EntryDate = Convert.ToDateTime(item.LMB_EntryDate);
                                            //if (item.LMB_PurchaseDate == "NULL")
                                            //{
                                            //    obj.LMB_PurchaseDate = Convert.ToDateTime(item.LMB_EntryDate); ;
                                            //}

                                        }
                                        else
                                        {
                                            obj.LMB_EntryDate = DateTime.Now;
                                        }

                                        if (item.LMB_Biblography != "NULL" && item.LMB_Biblography != "" && item.LMB_Biblography != null && item.LMB_Biblography != "NA")
                                        {
                                            obj.LMB_Biblography = item.LMB_Biblography;
                                        }

                                        if (item.LMB_IndexPage != "NULL" && item.LMB_IndexPage != "" && item.LMB_IndexPage != null && item.LMB_IndexPage != "NA")
                                        {
                                            obj.LMB_IndexPage = item.LMB_IndexPage;
                                        }

                                        if (item.LMB_NoOfPages != "NULL" && item.LMB_NoOfPages != "" && item.LMB_NoOfPages != null && item.LMB_NoOfPages != "NA")
                                        {
                                            obj.LMB_NoOfPages = Convert.ToInt64(item.LMB_NoOfPages);
                                        }
                                        if (item.LMB_PublishedYear != "NULL" && item.LMB_PublishedYear != "" && item.LMB_PublishedYear != null && item.LMB_PublishedYear != "NA")
                                        {
                                            obj.LMB_PublishedYear = item.LMB_PublishedYear;
                                        }

                                        if (item.LMB_Edition != "NULL" && item.LMB_Edition != "" && item.LMB_Edition != null && item.LMB_Edition != "NA")
                                        {
                                            obj.LMB_Edition = item.LMB_Edition;
                                        }

                                        if (item.LMB_PurOrDonated != "NULL" && item.LMB_PurOrDonated != "" && item.LMB_PurOrDonated != null && item.LMB_PurOrDonated != "NA")
                                        {
                                            obj.LMB_PurOrDonated = item.LMB_PurOrDonated;
                                        }
                                        if (item.LMB_DonorName != "NULL" && item.LMB_DonorName != "" && item.LMB_DonorName != null && item.LMB_DonorName != "NA")
                                        {
                                            obj.LMB_DonorName = item.LMB_DonorName;
                                        }

                                        if (item.LMB_DonorAddress != "NULL" && item.LMB_DonorAddress != "" && item.LMB_DonorAddress != null && item.LMB_DonorAddress != "NA")
                                        {
                                            obj.LMB_DonorAddress = item.LMB_DonorAddress;
                                        }

                                        if (item.LMB_Remarks != "NULL" && item.LMB_Remarks != "" && item.LMB_Remarks != null && item.LMB_Remarks != "NA")
                                        {
                                            obj.LMB_Remarks = item.LMB_Remarks;
                                        }
                                        if (item.LMB_Keywords != "NULL" && item.LMB_Keywords != "" && item.LMB_Keywords != null && item.LMB_Keywords != "NA")
                                        {
                                            obj.LMB_Keywords = item.LMB_Keywords;
                                        }

                                        if (item.LMB_NoOfCopies != "NULL" && item.LMB_NoOfCopies != "" && item.LMB_NoOfCopies != null && item.LMB_NoOfCopies != "NA")
                                        {
                                            obj.LMB_NoOfCopies = Convert.ToInt64(item.LMB_NoOfCopies);
                                        }

                                        if (item.LMB_Keywords != "NULL" && item.LMB_Keywords != "" && item.LMB_Keywords != null && item.LMB_Keywords != "NA")
                                        {
                                            obj.LMB_Keywords = item.LMB_Keywords;
                                        }

                                        if (item.LMB_WithAccessories == "NULL" || item.LMB_WithAccessories == "" || item.LMB_WithAccessories == "1" || item.LMB_WithAccessories == "NA")
                                        {
                                            obj.LMB_WithAccessories = true;
                                        }
                                        else
                                        {
                                            obj.LMB_WithAccessories = false;
                                        }

                                        if (item.LMB_CurrenceyType != "NULL" && item.LMB_CurrenceyType != "" && item.LMB_CurrenceyType != null && item.LMB_CurrenceyType != "NA")
                                        {
                                            obj.LMB_CurrenceyType = item.LMB_CurrenceyType;
                                        }

                                        if (item.LMB_BookImage != "NULL" && item.LMB_BookImage != "" && item.LMB_BookImage != null && item.LMB_BookImage != "0x" && item.LMB_BookImage != "NA")
                                        {
                                            obj.LMB_BookImage = item.LMB_BookImage;
                                        }

                                        if (item.LMB_CurrenceyType != "NULL" && item.LMB_CurrenceyType != "" && item.LMB_CurrenceyType != null && item.LMB_CurrenceyType != "NA")
                                        {
                                            obj.LMB_CurrenceyType = item.LMB_CurrenceyType;
                                        }

                                        if (item.LMB_ActiveFlg == "NULL" || item.LMB_ActiveFlg == "" || item.LMB_ActiveFlg == "1" || item.LMB_ActiveFlg == "NA")
                                        {
                                            obj.LMB_ActiveFlg = true;
                                        }
                                        else
                                        {
                                            obj.LMB_ActiveFlg = false;
                                        }
                                        //obj.LMC_Id = 6;
                                        obj.CreatedDate = DateTime.Now;
                                        obj.UpdatedDate = DateTime.Now;
                                        _LibraryContext.Add(obj);
                                        int rowAffected = _LibraryContext.SaveChanges();

                                        if (rowAffected > 0)
                                        {
                                            LIB_Master_Book_Library_DMO lb = new LIB_Master_Book_Library_DMO();
                                            lb.LMAL_Id = data.LMAL_Id;
                                            lb.LMB_Id = obj.LMB_Id;
                                            lb.LMBL_ActiveFlg = true;
                                            lb.CreatedDate = DateTime.Now;
                                            lb.UpdatedDate = DateTime.Now;
                                            _LibraryContext.Add(lb);
                                            _LibraryContext.SaveChanges();
                                            data.suscnt += 1;

                                        }
                                        else
                                        {


                                        }
                                    }

                                }
                                data.returnval = true;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //data.failcnt += 1;
                data.returnval = false;
            }
            return data;
        }

        public ImportLibrarydataDTO getdetails(int id)
        {
            ImportLibrarydataDTO data = new ImportLibrarydataDTO();
            try
            {
                data.authorlist = _LibraryContext.MasterAuthorDMO.Distinct().ToArray();
                data.masterlibarary = _LibraryContext.LIB_Master_Library_DMO.Where(R => R.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ImportLibrarydataDTO deactiveY(ImportLibrarydataDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterAuthorDMO.Single(t => t.LMBA_Id == data.LMBA_Id);

                if (result.LMBA_ActiveFlg == true)
                {
                    result.LMBA_ActiveFlg = false;
                }
                else if (result.LMBA_ActiveFlg == false)
                {
                    result.LMBA_ActiveFlg = true;
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


    }
}


