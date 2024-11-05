using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class CirculationParameterImpl : Interfaces.CirculationParameterInterface
    {
        public LibraryContext _LibraryContext;
        public CirculationParameterImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public async Task<CirculationParameterDTO> Savedata(CirculationParameterDTO data)
        {
            try
            {
                string COL_SCHFLG = "";
                var SCHCOLFLAG = (from a in _LibraryContext.Institute
                                  where a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1
                                  select new CirculationParameterDTO
                                  {
                                      MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                  }).Distinct().ToList();
                if (SCHCOLFLAG.Count>0)
                {
                    COL_SCHFLG = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                    data.MI_SchoolCollegeFlag = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                }

                if (data.BOOKFLAG == "BP")
                {
                    if (data.issuertype1 == "STUDENT")
                    {
                        if (COL_SCHFLG == "S")
                        {
                            if (data.LBCPA_Id > 0)
                            {
                              
                                var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                 where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_Id != data.LBCPA_Id && a.LBCPA_ActiveFlg==true && b.LBCPAS_ActiveFlg==true)
                                                 select b).Distinct().ToArray();

                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    var update = _LibraryContext.LIB_Circulation_Parameter_StudentDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id).SingleOrDefault();

                                    update.LBCPA_Id = data.LBCPA_Id;
                                    update.ASMCL_Id = data.ASMCL_Id;
                                    update.LBCPAS_NoOfItems = data.Max_Issue_Items;
                                    update.LBCPAS_IssueDays = data.Max_Issue_Days;
                                    update.LBCPAS_NoOfRenewals = data.Max_No_Renewals;
                                    update.LBCPAS_ActiveFlg = true;
                                    update.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Update(update);
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
                            else
                            {

                                if (data.ASMCL_Id != 0)
                                {

                                    var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                                     where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_ActiveFlg == true && b.LBCPAS_ActiveFlg == true)
                                                     select b).Distinct().ToArray();
                                    if (Duplicate.Count() > 0)
                                    {
                                        data.duplicate = true;
                                    }

                                    else
                                    {
                                        LIB_Book_Circulation_ParameterDMO obj1 = new LIB_Book_Circulation_ParameterDMO();
                                        obj1.LBCPA_IssueRefFlg = data.Catgname;
                                        obj1.LBCPA_Flg = data.issuertype1.Trim();
                                        obj1.MI_Id = data.MI_Id;
                                        obj1.LBCPA_ExcludeHolidayFlg = data.LBCPA_ExcludeHolidayFlg;
                                        obj1.LBCPA_ActiveFlg = true;
                                        obj1.UpdatedDate = DateTime.Now;
                                        obj1.CreatedDate = DateTime.Now; ;
                                        _LibraryContext.Add(obj1);
                                        LIB_Circulation_Parameter_StudentDMO obj = new LIB_Circulation_Parameter_StudentDMO();

                                        obj.LBCPA_Id = obj1.LBCPA_Id;
                                        obj.ASMCL_Id = data.ASMCL_Id;
                                        obj.LBCPAS_NoOfItems = data.Max_Issue_Items;
                                        obj.LBCPAS_IssueDays = data.Max_Issue_Days;
                                        obj.LBCPAS_NoOfRenewals = data.Max_No_Renewals;
                                        obj.LBCPAS_ActiveFlg = true;
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
                                //else
                                //{

                                //    var category = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                                //    var cls = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();

                                //    for (int i = 0; i < cls.Count; i++)
                                //    {
                                //        var Duplicate1 = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                //                          from b in _LibraryContext.LIB_Circulation_Parameter_StudentDMO
                                //                          where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == cls[i].ASMCL_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname))
                                //                          select b).Distinct().ToArray();
                                //        if (Duplicate1.Count() > 0)
                                //        {
                                //            //  data.duplicate = true;
                                //        }

                                //        else
                                //        {
                                //            LIB_Circulation_Parameter_StudentDMO obj = new LIB_Circulation_Parameter_StudentDMO();
                                //            obj.LBCPA_Id = data.LBCPA_Id;
                                //            obj.ASMCL_Id = cls[i].ASMCL_Id;
                                //            obj.LBCPAS_NoOfItems = data.Max_Issue_Items;
                                //            obj.LBCPAS_IssueDays = data.Max_Issue_Days;
                                //            obj.LBCPAS_NoOfRenewals = data.Max_No_Renewals;
                                //            obj.LBCPAS_ActiveFlg = true;
                                //            obj.CreatedDate = DateTime.Now;
                                //            obj.UpdatedDate = DateTime.Now;
                                //            _LibraryContext.Add(obj);

                                //            int rowAffected = _LibraryContext.SaveChanges();
                                //            if (rowAffected > 0)
                                //            {
                                //                data.returnval = true;
                                //            }
                                //            else
                                //            {
                                //                data.returnval = false;
                                //            }
                                //        }

                                //    }
                                //}

                            }
                           
                        }
                        else
                        if (COL_SCHFLG == "C" || COL_SCHFLG == "U")
                        {
                            if (data.LBCPA_Id > 0)
                            {

                                var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                                                 where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id  && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_Id != data.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPASC_ActiveFlg == true)
                                                 select b).Distinct().ToArray();

                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    var update = _LibraryContext.LIBCirculationParameterStudentCollegeDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id).SingleOrDefault();

                                    update.LBCPA_Id = data.LBCPA_Id;
                                    update.LBCPASC_NoOfItems =Convert.ToInt32(data.Max_Issue_Items);
                                    update.LBCPASC_IssueDays = Convert.ToInt32(data.Max_Issue_Days);
                                    update.LBCPASC_NoOfRenewals = Convert.ToInt32(data.Max_No_Renewals);
                                    update.LBCPASC_ActiveFlg = true;
                                    update.LBCPASC_UpadtedBy = data.IVRMUL_Id;
                                    update.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Update(update);
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
                            else
                            {


                                    var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                                     from b in _LibraryContext.LIBCirculationParameterStudentCollegeDMO
                                                     where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id  && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_ActiveFlg == true && b.LBCPASC_ActiveFlg == true)
                                                     select b).Distinct().ToArray();
                                    if (Duplicate.Count() > 0)
                                    {
                                        data.duplicate = true;
                                    }

                                    else
                                    {
                                        LIB_Book_Circulation_ParameterDMO obj1 = new LIB_Book_Circulation_ParameterDMO();
                                        obj1.LBCPA_IssueRefFlg = data.Catgname;
                                        obj1.LBCPA_Flg = data.issuertype1.Trim();
                                        obj1.MI_Id = data.MI_Id;
                                        obj1.LBCPA_ActiveFlg = true;
                                        obj1.UpdatedDate = DateTime.Now;
                                        obj1.CreatedDate = DateTime.Now;
                                        obj1.LBCPA_ExcludeHolidayFlg = data.LBCPA_ExcludeHolidayFlg;
                                    _LibraryContext.Add(obj1);
                                        LIB_Circulation_Parameter_Student_CollegeDMO obj = new LIB_Circulation_Parameter_Student_CollegeDMO();

                                        obj.LBCPA_Id = obj1.LBCPA_Id;
                                        obj.LBCPASC_NoOfItems = Convert.ToInt32(data.Max_Issue_Items);
                                        obj.LBCPASC_IssueDays = Convert.ToInt32(data.Max_Issue_Days);
                                        obj.LBCPASC_NoOfRenewals = Convert.ToInt32(data.Max_No_Renewals);
                                        obj.LBCPASC_CreatedBy = data.IVRMUL_Id;
                                        obj.LBCPASC_UpadtedBy = data.IVRMUL_Id;
                                        obj.LBCPASC_ActiveFlg = true;
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

                    }
                    else if (data.issuertype1 == "STAFF")
                    {
                        if (data.LBCPA_Id > 0)
                        {
                            //var Duplicate = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id && t.HRMGT_Id == data.HRMGT_Id).ToList();

                            var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                             where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_Id != data.LBCPA_Id && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true)
                                             select b).Distinct().ToArray();
                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {


                                var update = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id && t.HRMGT_Id == data.HRMGT_Id).SingleOrDefault();
                                update.LBCPA_Id = data.LBCPA_Id;
                                update.HRMGT_Id = data.HRMGT_Id;
                                update.LBCPAST_NoOfItems = data.Max_Issue_Items;
                                update.LBCPAST_IssueDays = data.Max_Issue_Days;
                                update.LBCPAST_NoOfRenewals = data.Max_No_Renewals;
                                update.LBCPAST_ActiveFlg = true;
                                update.UpdatedDate = DateTime.Now;

                                _LibraryContext.Update(update);
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
                        else
                        {
                            //var Duplicate = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id && t.HRMGT_Id == data.HRMGT_Id).ToList();

                            var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_Circulation_Parameter_StaffDMO
                                             where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && b.HRMGT_Id == data.HRMGT_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_ActiveFlg == true && b.LBCPAST_ActiveFlg == true)
                                             select b).Distinct().ToArray();


                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {

                                LIB_Book_Circulation_ParameterDMO obj1 = new LIB_Book_Circulation_ParameterDMO();
                                obj1.LBCPA_IssueRefFlg = data.Catgname;
                                obj1.LBCPA_Flg = data.issuertype1.Trim();
                                obj1.MI_Id = data.MI_Id;
                                obj1.LBCPA_ActiveFlg = true;
                                obj1.UpdatedDate = DateTime.Now;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.LBCPA_ExcludeHolidayFlg = data.LBCPA_ExcludeHolidayFlg;
                                _LibraryContext.Add(obj1);

                                LIB_Circulation_Parameter_StaffDMO obj = new LIB_Circulation_Parameter_StaffDMO();

                                obj.LBCPA_Id = obj1.LBCPA_Id;
                                obj.HRMGT_Id = data.HRMGT_Id;
                                obj.LBCPAST_NoOfItems = data.Max_Issue_Items;
                                obj.LBCPAST_IssueDays = data.Max_Issue_Days;
                                obj.LBCPAST_NoOfRenewals = data.Max_No_Renewals;
                                obj.LBCPAST_ActiveFlg = true;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedDate = DateTime.Now;
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
                    else if (data.issuertype1 == "GUEST" || data.issuertype1 == "DEPARTMENT")
                    {
                        if (data.LBCPA_Id > 0)
                        {
                            //var Duplicate = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id).ToList();

                            var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                             where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id && a.LBCPA_Id != data.LBCPA_Id && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_Flg.Trim()== data.issuertype1.Trim() && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true)
                                             select b).Distinct().ToArray();


                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var update = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id).SingleOrDefault();
                               
                                update.LBCPAO_NoOfItems = data.Max_Issue_Items;
                                update.LBCPAO_IssueDays = data.Max_Issue_Days;
                                update.LBCPAO_NoOfRenewals = data.Max_No_Renewals;
                                update.LBCPAO_ActiveFlg = true;
                                update.UpdatedDate = DateTime.Now;
                           

                                _LibraryContext.Update(update);
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
                        else
                        {
                          //  var Duplicate = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Where(t => t.LBCPA_Id == data.LBCPA_Id).ToList();

                            var Duplicate = (from a in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_Circulation_Parameter_OthersDMO
                                             where (a.LBCPA_Id == b.LBCPA_Id && a.MI_Id == data.MI_Id  && a.LBCPA_IssueRefFlg.Contains(data.Catgname) && a.LBCPA_Flg.Trim() == data.issuertype1.Trim() && a.LBCPA_ActiveFlg == true && b.LBCPAO_ActiveFlg == true)
                                             select b).Distinct().ToArray();


                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {

                                LIB_Book_Circulation_ParameterDMO obj1 = new LIB_Book_Circulation_ParameterDMO();
                                obj1.LBCPA_IssueRefFlg = data.Catgname;
                                obj1.LBCPA_Flg = data.issuertype1.Trim();
                                obj1.MI_Id = data.MI_Id;
                                obj1.LBCPA_ActiveFlg = true;
                                obj1.UpdatedDate = DateTime.Now;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.LBCPA_ExcludeHolidayFlg = data.LBCPA_ExcludeHolidayFlg;
                                _LibraryContext.Add(obj1);
                                LIB_Circulation_Parameter_OthersDMO obj = new LIB_Circulation_Parameter_OthersDMO();

                                obj.LBCPA_Id = obj1.LBCPA_Id;
                                obj.LBCPAO_NoOfItems = data.Max_Issue_Items;
                                obj.LBCPAO_IssueDays = data.Max_Issue_Days;
                                obj.LBCPAO_NoOfRenewals = data.Max_No_Renewals;
                                obj.LBCPAO_ActiveFlg = true;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedDate = DateTime.Now;
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
                }
                else
                {
                    if (data.issuertype1 == "STUDENT")
                    {
                        if (COL_SCHFLG=="S")
                        {
                            if (data.LNBCPA_Id > 0)
                            {
                                var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO
                                                 where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && a.LNBCPA_Id == data.LNBCPA_Id && b.ASMCL_Id == data.ASMCL_Id && b.LMC_Id == data.LMC_Id && a.LNBCPA_Id != data.LNBCPA_Id)
                                                 select b).Distinct().ToArray();

                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    var update = _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO.Where(t =>  t.LNBCPA_Id == data.LNBCPA_Id).SingleOrDefault();
                                    update.LMC_Id = data.LMC_Id;
                                    update.LNBCPA_Id = data.LNBCPA_Id;
                                    update.ASMCL_Id = data.ASMCL_Id;
                                    update.LNBCPAS_NoOfItems = data.Max_Issue_Items;
                                    update.LNBCPAS_IssueDays = data.Max_Issue_Days;
                                    update.LNBCPAS_NoOfRenewals = data.Max_No_Renewals;
                                    update.LNBCPAS_ActiveFlg = true;
                                    update.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Update(update);
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
                            else
                            {
                                var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO
                                                 where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.LMC_Id == data.LMC_Id)
                                                 select b).Distinct().ToArray();


                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    LIB_NonBook_Circulation_ParameterDMO obj1 = new LIB_NonBook_Circulation_ParameterDMO();

                                        obj1.MI_Id = data.MI_Id;
                                    obj1.LNBCPA_Flg = data.issuertype1.Trim();
                                    obj1.LNBCPA_ActiveFlg = true;
                                    obj1.CreatedDate = DateTime.Now; 
                                    obj1.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Add(obj1);
                                    LIB_NonBook_Circulation_Parameter_StudentDMO obj = new LIB_NonBook_Circulation_Parameter_StudentDMO();

                                    obj.LMC_Id = data.LMC_Id;
                                    obj.LNBCPA_Id = obj1.LNBCPA_Id;
                                    obj.ASMCL_Id = data.ASMCL_Id;
                                    obj.LNBCPAS_NoOfItems = data.Max_Issue_Items;
                                    obj.LNBCPAS_IssueDays = data.Max_Issue_Days;
                                    obj.LNBCPAS_NoOfRenewals = data.Max_No_Renewals;
                                    obj.LNBCPAS_ActiveFlg = true;
                                    obj.UpdatedDate = DateTime.Now;
                                    obj.CreatedDate = DateTime.Now;

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
                            if (data.LNBCPA_Id > 0)
                            {
                                var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO
                                                 where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && a.LNBCPA_Id == data.LNBCPA_Id && b.LMC_Id == data.LMC_Id && a.LNBCPA_Id != data.LNBCPA_Id)
                                                 select b).Distinct().ToArray();

                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    var update = _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO.Where(t => t.LNBCPA_Id == data.LNBCPA_Id).SingleOrDefault();
                                    update.LMC_Id = data.LMC_Id;
                                    update.LNBCPA_Id = data.LNBCPA_Id;
                                   
                                    update.LNBCPASC_NoOfItems = Convert.ToInt32(data.Max_Issue_Items);
                                    update.LNBCPASC_IssueDays = Convert.ToInt32(data.Max_Issue_Days);
                                    update.LNBCPASC_NoOfRenewals = Convert.ToInt32(data.Max_No_Renewals);
                                    update.LNBCPASC_ActiveFlg = true;
                                    update.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Update(update);
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
                            else
                            {
                                var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                                 from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO
                                                 where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id  && b.LMC_Id == data.LMC_Id)
                                                 select b).Distinct().ToArray();


                                if (Duplicate.Count() > 0)
                                {
                                    data.duplicate = true;
                                }
                                else
                                {
                                    LIB_NonBook_Circulation_ParameterDMO obj1 = new LIB_NonBook_Circulation_ParameterDMO();

                                    obj1.MI_Id = data.MI_Id;
                                    obj1.LNBCPA_Flg = data.issuertype1.Trim();
                                    obj1.LNBCPA_ActiveFlg = true;
                                    obj1.CreatedDate = DateTime.Now;
                                    obj1.UpdatedDate = DateTime.Now;

                                    _LibraryContext.Add(obj1);
                                    LIB_NonBook_Circulation_Parameter_Student_CollegeDMO obj = new LIB_NonBook_Circulation_Parameter_Student_CollegeDMO();

                                    obj.LMC_Id = data.LMC_Id;
                                    obj.LNBCPA_Id = obj1.LNBCPA_Id;
                                    obj.LNBCPASC_NoOfItems = Convert.ToInt32(data.Max_Issue_Items);
                                    obj.LNBCPASC_IssueDays = Convert.ToInt32(data.Max_Issue_Days);
                                    obj.LNBCPASC_NoOfRenewals = Convert.ToInt32(data.Max_No_Renewals);
                                    obj.LNBCPASC_ActiveFlg = true;
                                    obj.UpdatedDate = DateTime.Now;
                                    obj.CreatedDate = DateTime.Now;

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


                        
                    }
                    else if (data.issuertype1 == "STAFF")
                    {
                        if (data.LNBCPA_Id > 0)
                        {

                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                             where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && a.LNBCPA_Id != data.LNBCPA_Id && b.HRMGT_Id == data.HRMGT_Id && b.LMC_Id == data.LMC_Id)
                                             select b).Distinct().ToArray();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var update = _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO.Where(t => t.LNBCPA_Id == data.LNBCPA_Id).SingleOrDefault();
                               
                                update.HRMGT_Id = data.HRMGT_Id;
                                update.LMC_Id = data.LMC_Id;
                                update.LNBCPAST_NoOfItems = data.Max_Issue_Items;
                                update.LNBCPAST_IssueDays = data.Max_Issue_Days;
                                update.LNBCPAST_NoOfRenewals = data.Max_No_Renewals;
                                update.LNBCPAST_ActiveFlg = true;
                                update.UpdatedDate = DateTime.Now;

                                _LibraryContext.Update(update);
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
                        else
                        {
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO
                                             where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id  && b.HRMGT_Id == data.HRMGT_Id && b.LMC_Id == data.LMC_Id)
                                             select b).Distinct().ToArray();

                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {

                                LIB_NonBook_Circulation_ParameterDMO obj1 = new LIB_NonBook_Circulation_ParameterDMO();
                                obj1.LNBCPA_Flg = data.issuertype1.Trim();
                                obj1.MI_Id = data.MI_Id;
                                obj1.LNBCPA_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);

                                LIB_NonBook_Circulation_Parameter_StaffDMO obj = new LIB_NonBook_Circulation_Parameter_StaffDMO();
                                obj.LNBCPA_Id = obj1.LNBCPA_Id;
                                obj.HRMGT_Id = data.HRMGT_Id;
                                obj.LMC_Id = data.LMC_Id;
                                obj.LNBCPAST_NoOfItems = data.Max_Issue_Items;
                                obj.LNBCPAST_IssueDays = data.Max_Issue_Days;
                                obj.LNBCPAST_NoOfRenewals = data.Max_No_Renewals;
                                obj.LNBCPAST_ActiveFlg = true;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedDate = DateTime.Now;
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
                    else if (data.issuertype1 == "GUEST" || data.issuertype1 == "DEPARTMENT")
                    {
                        if (data.LNBCPA_Id > 0)
                        {
                          
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                             where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && a.LNBCPA_Id != data.LNBCPA_Id && b.LMC_Id==data.LMC_Id && a.LNBCPA_Flg.Trim()== data.issuertype1.Trim())
                                             select b).Distinct().ToArray();


                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var update = _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO.Where(t => t.LNBCPA_Id == data.LNBCPA_Id).SingleOrDefault();
                             
                                update.LMC_Id = data.LMC_Id;
                                update.LNBCPAO_NoOfItems = data.Max_Issue_Items;
                                update.LNBCPAO_IssueDays = data.Max_Issue_Days;
                                update.LNBCPAO_NoOfRenewals = data.Max_No_Renewals;
                                update.LNBCPAO_ActiveFlg = true;
                                update.UpdatedDate = DateTime.Now;
                                _LibraryContext.Update(update);
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
                        else
                        {
                        
                            var Duplicate = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                             from b in _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO
                                             where (a.LNBCPA_Id == b.LNBCPA_Id && a.MI_Id == data.MI_Id && a.LNBCPA_Flg.Trim() == data.issuertype1.Trim() && b.LMC_Id==data.LMC_Id)
                                             select b).Distinct().ToArray();
                            if (Duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                LIB_NonBook_Circulation_ParameterDMO obj1 = new LIB_NonBook_Circulation_ParameterDMO();
                                obj1.LNBCPA_Flg = data.issuertype1.Trim();
                                obj1.MI_Id = data.MI_Id;
                                obj1.LNBCPA_ActiveFlg = true;
                                obj1.CreatedDate = DateTime.Now;
                                obj1.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj1);

                                LIB_NonBook_Circulation_Parameter_OthersDMO obj = new LIB_NonBook_Circulation_Parameter_OthersDMO();

                                obj.LNBCPA_Id = obj1.LNBCPA_Id;
                                obj.LMC_Id = data.LMC_Id;
                                obj.LNBCPAO_NoOfItems = data.Max_Issue_Items;
                                obj.LNBCPAO_IssueDays = data.Max_Issue_Days;
                                obj.LNBCPAO_NoOfRenewals = data.Max_No_Renewals;
                                obj.LNBCPAO_ActiveFlg = true;
                                obj.UpdatedDate = DateTime.Now;
                                obj.CreatedDate = DateTime.Now;
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
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CirculationParameterDTO> getdetails(CirculationParameterDTO data)
        {
           
            try
            {
                string COL_SCHFLG = "";
                var SCHCOLFLAG = (from a in _LibraryContext.Institute
                                  where a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1
                                  select new CirculationParameterDTO
                                  {
                                      MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                  }).Distinct().ToList();
                if (SCHCOLFLAG.Count > 0)
                {
                    COL_SCHFLG = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                    data.MI_SchoolCollegeFlag = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                }

                data.categorylist = _LibraryContext.MasterCategoryDMO.Where(a => a.LMC_BNBFlg == "Non-Book" && a.MI_Id == data.MI_Id).ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.ToArray();

                List<HR_Master_GroupTypeDMO> hremp = new List<HR_Master_GroupTypeDMO>();
                hremp = _LibraryContext.HR_Master_GroupTypeDMO.Where(t => t.MI_Id == data.MI_Id && t.HRMGT_ActiveFlag==true).ToList();
                data.fillemp = hremp.ToArray();

                try
                {
                    var retObject1 = new List<dynamic>();
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_GET_CIRCULATION_PARAMETER_DATA";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                           SqlDbType.VarChar)
                        {
                            Value = data.BOOKFLAG
                        });
                        cmd.Parameters.Add(new SqlParameter("@issuertype",
                         SqlDbType.VarChar)
                        {
                            Value = data.issuertype1
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
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CirculationParameterDTO> getdata(CirculationParameterDTO data)
        {
          
            try
            {

                if (data.LMC_CategoryName== "BP")
                {
                    data.categorylist = _LibraryContext.MasterCategoryDMO.Where(a => a.LMC_BNBFlg == "Book" && a.MI_Id == data.MI_Id).ToArray();
                }
                else
                {
                    data.categorylist = _LibraryContext.MasterCategoryDMO.Where(a => a.LMC_BNBFlg == "Non-Book" && a.MI_Id == data.MI_Id).ToArray();
                }

                try
                {
                    var retObject1 = new List<dynamic>();
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "circulation_data";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                           SqlDbType.VarChar)
                        {
                            Value = data.LMC_CategoryName
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public CirculationParameterDTO gettype(CirculationParameterDTO data)
        {
            try
            {
                //data.categorylist = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Where(a => a.LMC_BNBFlg == "Book" && a.MI_Id == data.MI_Id).ToArray();
                //&& a.LMC_CategoryName == data.Catgname


                if (data.LMC_CategoryName == "BP")
                {
                    data.issuetype = (from a in _LibraryContext.MasterCategoryDMO
                                      from b in _LibraryContext.LIB_Book_Circulation_ParameterDMO
                                      where (b.MI_Id == data.MI_Id && b.LBCPA_IssueRefFlg.Contains(data.Catgname))
                                      select new CirculationParameterDTO
                                      {
                                          LBCPA_Id = b.LBCPA_Id,
                                          LBCPA_Flg = b.LBCPA_Flg
                                      }).Distinct().ToArray();
                }
                else
                {
                    data.issuetype = (from a in _LibraryContext.LIB_NonBook_Circulation_ParameterDMO
                                      where (a.MI_Id == data.MI_Id)
                                      select new CirculationParameterDTO
                                      {
                                          LBCPA_Id = a.LNBCPA_Id,
                                          LBCPA_Flg = a.LNBCPA_Flg
                                      }).Distinct().ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CirculationParameterDTO deactiveY(CirculationParameterDTO data)
        {
            try
            {
                
                
                if (data.BOOKFLAG == "BP")
                {
                    if(data.issuertype1=="STUDENT")
                    {
                        var result1 = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);
                        if (result1.LBCPA_ActiveFlg == true)
                        {
                            result1.LBCPA_ActiveFlg = false;
                        }
                        else if (result1.LBCPA_ActiveFlg == false)
                        {
                            result1.LBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(result1);
                        if (data.MI_SchoolCollegeFlag=="S")
                        {
                            var result = _LibraryContext.LIB_Circulation_Parameter_StudentDMO.Single(t => t.LBCPAS_Id == data.Parmeter_Id);
                            
                           // var result = _LibraryContext.LIB_Circulation_Parameter_StudentDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);
                            if (result.LBCPAS_ActiveFlg == true)
                            {
                                result.LBCPAS_ActiveFlg = false;
                            }
                            else if (result.LBCPAS_ActiveFlg == false)
                            {
                                result.LBCPAS_ActiveFlg = true;
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
                        else if (data.MI_SchoolCollegeFlag == "C")
                        {
                          //  var result = _LibraryContext.LIBCirculationParameterStudentCollegeDMO.Single(t => t.LBCPASC_Id == data.Parmeter_Id);

                           var result = _LibraryContext.LIBCirculationParameterStudentCollegeDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);
                            if (result.LBCPASC_ActiveFlg == true)
                            {
                                result.LBCPASC_ActiveFlg = false;
                            }
                            else if (result.LBCPASC_ActiveFlg == false)
                            {
                                result.LBCPASC_ActiveFlg = true;
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
                      
                    }
                    else if(data.issuertype1 == "STAFF")
                    {
                      
                        var result1 = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);
                           if (result1.LBCPA_ActiveFlg == true)
                        {
                            result1.LBCPA_ActiveFlg = false;
                        }
                        else if (result1.LBCPA_ActiveFlg == false)
                        {
                            result1.LBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(result1);
                        //LBCPAST_Id
                       // var result = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Single(t => t.LBCPAST_Id == data.Parmeter_Id);                     
                      var result = _LibraryContext.LIB_Circulation_Parameter_StaffDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);

                        if (result.LBCPAST_ActiveFlg == true)
                        {
                            result.LBCPAST_ActiveFlg = false;
                        }
                        else if (result.LBCPAST_ActiveFlg == false)
                        {
                            result.LBCPAST_ActiveFlg = true;
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
                    else
                    {
                        var result1 = _LibraryContext.LIB_Book_Circulation_ParameterDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);
                        if (result1.LBCPA_ActiveFlg == true)
                        {
                            result1.LBCPA_ActiveFlg = false;
                        }
                        else if (result1.LBCPA_ActiveFlg == false)
                        {
                            result1.LBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now; 
                        _LibraryContext.Update(result1);
                        var result = _LibraryContext.LIB_Circulation_Parameter_OthersDMO.Single(t => t.LBCPA_Id == data.LBCPA_Id);

                        if (result.LBCPAO_ActiveFlg == true)
                        {
                            result.LBCPAO_ActiveFlg = false;
                        }
                        else if (result.LBCPAO_ActiveFlg == false)
                        {
                            result.LBCPAO_ActiveFlg = true;
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
                }

                else
                {
                    if (data.issuertype1 == "STUDENT")
                    {
                        var result1 = _LibraryContext.LIB_NonBook_Circulation_ParameterDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);
                        if (result1.LNBCPA_ActiveFlg == true)
                        {
                            result1.LNBCPA_ActiveFlg = false;
                        }
                        else if (result1.LNBCPA_ActiveFlg == false)
                        {
                            result1.LNBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(result1);

                        if (data.MI_SchoolCollegeFlag=="S")
                        {
                            var result = _LibraryContext.LIB_NonBook_Circulation_Parameter_StudentDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);
                            if (result.LNBCPAS_ActiveFlg == true)
                            {
                                result.LNBCPAS_ActiveFlg = false;
                            }
                            else if (result.LNBCPAS_ActiveFlg == false)
                            {
                                result.LNBCPAS_ActiveFlg = true;
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
                        else
                        {
                            var result = _LibraryContext.LIB_NonBook_Circulation_Parameter_Student_CollegeDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);
                            if (result.LNBCPASC_ActiveFlg == true)
                            {
                                result.LNBCPASC_ActiveFlg = false;
                            }
                            else if (result.LNBCPASC_ActiveFlg == false)
                            {
                                result.LNBCPASC_ActiveFlg = true;
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

                       


                    }
                    else if (data.issuertype1 == "STAFF")
                    {

                        var result1 = _LibraryContext.LIB_NonBook_Circulation_ParameterDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);
                        if (result1.LNBCPA_ActiveFlg == true)
                        {
                            result1.LNBCPA_ActiveFlg = false;
                        }
                        else if (result1.LNBCPA_ActiveFlg == false)
                        {
                            result1.LNBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(result1);
                        var result = _LibraryContext.LIB_NonBook_Circulation_Parameter_StaffDMO.Single(t =>  t.LNBCPA_Id == data.LNBCPA_Id);

                        if (result.LNBCPAST_ActiveFlg == true)
                        {
                            result.LNBCPAST_ActiveFlg = false;
                        }
                        else if (result.LNBCPAST_ActiveFlg == false)
                        {
                            result.LNBCPAST_ActiveFlg = true;
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
                    else
                    {
                        var result1 = _LibraryContext.LIB_NonBook_Circulation_ParameterDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);
                        if (result1.LNBCPA_ActiveFlg == true)
                        {
                            result1.LNBCPA_ActiveFlg = false;
                        }
                        else if (result1.LNBCPA_ActiveFlg == false)
                        {
                            result1.LNBCPA_ActiveFlg = true;
                        }
                        result1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(result1);

                        var result = _LibraryContext.LIB_NonBook_Circulation_Parameter_OthersDMO.Single(t => t.LNBCPA_Id == data.LNBCPA_Id);


                        if (result.LNBCPAO_ActiveFlg == true)
                        {
                            result.LNBCPAO_ActiveFlg = false;
                        }
                        else if (result.LNBCPAO_ActiveFlg == false)
                        {
                            result.LNBCPAO_ActiveFlg = true;
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
                }
                
                   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public  CirculationParameterDTO editdata(CirculationParameterDTO data)
        {
            try
            {
                string COL_SCHFLG = "";
                var SCHCOLFLAG = (from a in _LibraryContext.Institute
                                  where a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1
                                  select new CirculationParameterDTO
                                  {
                                      MI_SchoolCollegeFlag = a.MI_SchoolCollegeFlag
                                  }).Distinct().ToList();
                if (SCHCOLFLAG.Count > 0)
                {
                    COL_SCHFLG = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                    data.MI_SchoolCollegeFlag = SCHCOLFLAG[0].MI_SchoolCollegeFlag;
                }

                if (COL_SCHFLG=="S")
                {
                    if (data.issuertype1=="STUDENT")
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

    }
}
