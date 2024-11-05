using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class MasterDayImpl : Interfaces.MasterDayInterface
    {
        private static ConcurrentDictionary<string, TT_Master_DayDTO> _login =
        new ConcurrentDictionary<string, TT_Master_DayDTO>();

        private readonly TTContext _contextobj;
        private readonly DomainModelMsSqlServerContext _db;

        public MasterDayImpl(TTContext masterday, DomainModelMsSqlServerContext db)
        {
            _contextobj = masterday;
            _db = db;
        }
        public TT_Master_DayDTO savedetail(TT_Master_DayDTO data)
        {
            try
            {
                if (data.TTMD_Id > 0)
                {
                    var res = _contextobj.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower()) && t.TTMD_Id != data.TTMD_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _contextobj.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id);
                        result.TTMD_DayCode = data.TTMD_DayCode.ToUpper();
                        result.TTMD_DayName = data.TTMD_DayName.ToUpper();
                        result.UpdatedDate = DateTime.Now;
                        result.TTMD_ActiveFlag = true;
                        _contextobj.Update(result);
                        var contactExists = _contextobj.SaveChanges();
                        if (contactExists == 1)
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
                    var res = _contextobj.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower())).ToList();
                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        long OID = 0;
                        var orderId = _contextobj.TT_Master_DayDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                        if (orderId.Count == 0)
                        {
                            OID = 1;
                        }
                        else
                        {
                            long ooid = orderId.Select(r => r.Order_Id).Max();
                            OID = ooid + 1;
                        }

                        TT_Master_DayDMO obj = new TT_Master_DayDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.TTMD_DayName = data.TTMD_DayName.Trim().ToUpper();
                        obj.TTMD_DayCode = data.TTMD_DayCode.Trim().ToUpper(); ;
                        obj.Order_Id = OID;
                        obj.TTMD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _contextobj.Add(obj);
                        var contactExists = _contextobj.SaveChanges();
                        if (contactExists == 1)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TT_Master_DayDTO getdetails(int id)
        {
            TT_Master_DayDTO TTMD = new TT_Master_DayDTO();
            try
            {
                TTMD.Daylist = _contextobj.TT_Master_DayDMO.Where(a => a.MI_Id.Equals(id)).OrderBy(a => a.Order_Id).ToArray();

                TTMD.Daylisttwo = _contextobj.TT_Master_DayDMO.Where(a => a.MI_Id.Equals(id) && a.TTMD_ActiveFlag == true).OrderBy(a => a.Order_Id).ToArray();

                TTMD.Daylistdetail = (from TT_Master_Day_Classwise in _contextobj.TT_Master_Day_ClasswiseDMO
                                      from AcademicYear in _contextobj.AcademicYear
                                      from School_M_Class in _contextobj.School_M_Class
                                      from TT_Master_DayDMO in _contextobj.TT_Master_DayDMO
                                          //from TTMasterCategoryDMO in _contextobj.TTMasterCategoryDMO
                                          //  from TT_Category_Class_DMO in _contextobj.TT_Category_Class_DMO
                                      where (TT_Master_Day_Classwise.ASMAY_Id == AcademicYear.ASMAY_Id && TT_Master_Day_Classwise.ASMCL_Id == School_M_Class.ASMCL_Id && TT_Master_Day_Classwise.TTMD_Id == TT_Master_DayDMO.TTMD_Id && TT_Master_DayDMO.MI_Id == id
                                      //&& TTMasterCategoryDMO.TTMC_Id == TT_Category_Class_DMO.TTMC_Id && TT_Category_Class_DMO.ASMCL_Id == School_M_Class.ASMCL_Id
                                      )
                                      select new TT_Master_DayDTO
                                      {

                                          TTMDC_Id = TT_Master_Day_Classwise.TTMDC_Id,
                                          classname = School_M_Class.ASMCL_ClassName,
                                          academicyr = AcademicYear.ASMAY_Year,
                                          TTMD_DayName = TT_Master_DayDMO.TTMD_DayName,
                                          TTMDC_ActiveFlag = TT_Master_Day_Classwise.TTMDC_ActiveFlag,
                                          // catgname = TTMasterCategoryDMO.TTMC_CategoryName
                                      }).ToArray();
            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return TTMD;

        }
        public TT_Master_DayDTO getpageedit(int id)
        {
            TT_Master_DayDTO page = new TT_Master_DayDTO();
            try
            {
                List<TT_Master_DayDMO> lorg = new List<TT_Master_DayDMO>();
                lorg = _contextobj.TT_Master_DayDMO.AsNoTracking().Where(t => t.TTMD_Id.Equals(id)).ToList();
                page.Daylistedit = lorg.ToArray();
                // page.day_count = _contextobj.TT_Master_DayDMO.AsNoTracking().Where(t => t.ASMCL_Id.Equals(id) && t.TTMD_ActiveFlag == true).Count();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_Master_DayDTO getdayedit(int id)
        {
            TT_Master_DayDTO page = new TT_Master_DayDTO();
            try
            {
                page.Daydetailedit = (from TT_Master_Day_Classwise in _contextobj.TT_Master_Day_ClasswiseDMO

                                      from TTMasterCategoryDMO in _contextobj.TTMasterCategoryDMO
                                      from TT_Category_Class_DMO in _contextobj.TT_Category_Class_DMO
                                      where (TTMasterCategoryDMO.TTMC_Id == TT_Category_Class_DMO.TTMC_Id && TT_Master_Day_Classwise.ASMCL_Id == TT_Category_Class_DMO.ASMCL_Id && TT_Master_Day_Classwise.TTMDC_Id == id)
                                      select new TT_Master_DayDTO
                                      {

                                          TTMDC_Id = TT_Master_Day_Classwise.TTMDC_Id,
                                          ASMAY_Id = TT_Master_Day_Classwise.ASMAY_Id,
                                          ASMCL_Id = TT_Master_Day_Classwise.ASMCL_Id,
                                          TTMD_Id = TT_Master_Day_Classwise.TTMD_Id,
                                          TTMC_Id = TTMasterCategoryDMO.TTMC_Id
                                      }
                                  ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_Master_DayDTO deleterec(int id)
        {
            TT_Master_DayDTO page = new TT_Master_DayDTO();
            try
            {
                List<TT_Master_DayDMO> lorg = new List<TT_Master_DayDMO>();
                lorg = _contextobj.TT_Master_DayDMO.Where(t => t.TTMD_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _contextobj.Remove(lorg.ElementAt(0));
                    var contactExists = _contextobj.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_Master_DayDTO deactivate(TT_Master_DayDTO acd)
        {
            try
            {
                TT_Master_DayDMO pge = Mapper.Map<TT_Master_DayDMO>(acd);
                if (pge.TTMD_Id > 0)
                {
                    var result = _contextobj.TT_Master_DayDMO.Single(t => t.TTMD_Id.Equals(pge.TTMD_Id));
                    if (result.TTMD_ActiveFlag.Equals(true))
                    {
                        result.TTMD_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMD_ActiveFlag = true;
                    }
                    _contextobj.Update(result);
                    var flag = _contextobj.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public TT_Master_DayDTO getorder(TT_Master_DayDTO data)
        {
            try
            {
                data.dayorderlist = _contextobj.TT_Master_DayDMO.Where(i => i.MI_Id == data.MI_Id).OrderBy(a => a.Order_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        public TT_Master_DayDTO saveorder(TT_Master_DayDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.ordeidss.Count(); i++)
                {
                    var reult = _contextobj.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.ordeidss[i].TTMD_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.Order_Id = id;
                    }
                    else
                    {
                        reult.Order_Id = id;
                    }
                    _contextobj.Update(reult);
                    var flag = _contextobj.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        public TT_Master_DayDTO deactivate1(TT_Master_DayDTO acd)
        {
            try
            {
                TT_Master_Day_ClasswiseDMO pge = Mapper.Map<TT_Master_Day_ClasswiseDMO>(acd);
                if (pge.TTMDC_Id > 0)
                {
                    var result = _contextobj.TT_Master_Day_ClasswiseDMO.Single(t => t.TTMDC_Id.Equals(pge.TTMDC_Id));
                    if (result.TTMDC_ActiveFlag.Equals(true))
                    {
                        result.TTMDC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMDC_ActiveFlag = true;
                    }
                    _contextobj.Update(result);
                    var flag = _contextobj.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public TT_Master_DayDTO savedaydetail(TT_Master_DayDTO _category)
        {
            TT_Master_Day_ClasswiseDMO objpgee = Mapper.Map<TT_Master_Day_ClasswiseDMO>(_category);
            try
            {
                for (int i = 0; i < _category.Temp_class_Array.Count(); i++)
                {
                    var a = 0;
                    for (int j = 0; j < _category.temp_day_Array.Count(); j++)
                    {
                        if (a == 0)
                        {
                            List<TT_Master_Day_ClasswiseDMO> lorg = new List<TT_Master_Day_ClasswiseDMO>();
                            lorg = _contextobj.TT_Master_Day_ClasswiseDMO.Where(t => t.ASMCL_Id.Equals(_category.Temp_class_Array[i].ASMCL_Id) && t.ASMAY_Id.Equals(_category.ASMAY_Id)).ToList();
                            if (lorg.Any())
                            {
                                for (int h = 0; h < lorg.Count; h++)
                                {
                                    _contextobj.Remove(lorg.ElementAt(h));
                                    _contextobj.SaveChanges();
                                }
                                a = 1;
                            }
                        }
                        TT_Master_Day_ClasswiseDMO objpge = new TT_Master_Day_ClasswiseDMO();
                        objpge.ASMAY_Id = _category.ASMAY_Id;
                        objpge.ASMCL_Id = _category.Temp_class_Array[i].ASMCL_Id;
                        objpge.TTMD_Id = _category.temp_day_Array[j].TTMD_Id;
                        objpge.TTMDC_ActiveFlag = true;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _contextobj.Add(objpge);
                        var contactExists = _contextobj.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        #region Load Available Periods page Details
        public TT_Master_DayDTO getavdata(TT_Master_DayDTO data)
        {
            try
            {
                data.yearlist = _contextobj.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(w => w.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        #region Load Available Periods page Details
        public TT_Master_DayDTO getPeriods(TT_Master_DayDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_AVAILABLE_PERIODS";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.periodlist = retObject.ToArray();
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
        #endregion 
        #region Allocate Periods 
        public TT_Master_DayDTO allocateperiod(TT_Master_DayDTO data)
        {
            try
            {


                if (data.alocateids != null)
                {
                    foreach (var item in data.alocateids)
                    {
                        var confirmstatus1 = _db.Database.ExecuteSqlCommand("SCHOOL_AVAILABLE_PERIODS_INSERT @p0,@p1,@p2,@p3,@p4,@p4,@p5",
                            data.MI_Id, data.ASMAY_Id, item.ASMCL_Id, item.ASMS_Id, item.HRME_Id, item.ISMS_Id);

                    }
                }



            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion


    }
}
