using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CLGLabImpl : Interfaces.CLGLabInterface
    {
        private static ConcurrentDictionary<string, CLGLabDTO> _login =
             new ConcurrentDictionary<string, CLGLabDTO>();

        public TTContext _TTContext;
        ILogger<CLGLabImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGLabImpl(TTContext academiccontext, ILogger<CLGLabImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGLabDTO getalldetails(CLGLabDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.catelist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.academiclist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();
                //   data.daydropdown = _TTContext.TT_Master_DayDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMD_ActiveFlag == true).Distinct().ToArray();

                data.sectionlist = _TTContext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();


                data.labdetailsarray = (from a in _TTContext.TT_LABLIB_DMO
                                            // from b in _TTContext.CLGLabDetailsDMO
                                        from c in _TTContext.AcademicYear
                                        from d in _TTContext.TTMasterCategoryDMO
                                        where a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == d.TTMC_Id
                                        select new CLGLabDTO
                                        {
                                            TTLAB_Id = a.TTLAB_Id,
                                            TTMC_CategoryName=d.TTMC_CategoryName,
                                            TTLAB_LABLIBName=a.TTLAB_LABLIBName,
                                            TTLAB_ActiveFlag=a.TTLAB_ActiveFlag,
                                            ASMAY_Year=c.ASMAY_Year,
                                            ASMAY_Order = c.ASMAY_Order,
                                            TTMC_Id=a.TTMC_Id,
                                            ASMAY_Id=a.ASMAY_Id
                                        }
                                      ).Distinct().OrderByDescending(tt=>tt.TTLAB_Id).ToArray();





            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        
        #region SAVE LAB
        public CLGLabDTO savedetail(CLGLabDTO data)
        {

            try
            {
                if (data.TTLAB_Id > 0)
                {
                    var resultw = _TTContext.TT_LABLIB_DMO.Where(t => t.MI_Id == data.MI_Id && t.TTLAB_LABLIBName.Trim()
                   == data.TTLAB_LABLIBName.Trim() && t.ASMAY_Id == data.ASMAY_Id && t.TTLAB_Id !=data.TTLAB_Id).ToList(); 
                    if (resultw.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _TTContext.TT_LABLIB_DMO.Single(t => t.MI_Id == data.MI_Id && t.TTLAB_Id==data.TTLAB_Id);

                        result.TTMC_Id = data.TTMC_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.TTLAB_LABLIBName = data.TTLAB_LABLIBName.Trim();
                        result.TTLAB_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _TTContext.Update(result);

                        var exist = _TTContext.CLGLabDetailsDMO.Where(t =>  t.TTLAB_Id == data.TTLAB_Id).ToList();
                        if (exist.Count>0)
                        {
                            foreach (var item in exist)
                            {
                                _TTContext.Remove(item);
                            }
                        }


                        if (data.TempararyArrayList.Length > 0)
                        {
                            foreach (var item in data.TempararyArrayList)
                            {
                                CLGLabDetailsDMO res = new CLGLabDetailsDMO();

                                res.TTLAB_Id = data.TTLAB_Id;
                                res.AMCO_Id = item.AMCO_Id;
                                res.AMB_Id = item.AMB_Id;
                                res.AMSE_Id = item.AMSE_Id;
                                res.ACMS_Id = item.ACMS_Id;
                                res.ISMS_Id = item.ISMS_Id;
                                res.TTLABDC_ActiveFlag = true;
                                res.CreatedDate = DateTime.Now;
                                res.UpdatedDate = DateTime.Now;
                                _TTContext.Add(res);
                            }
                            var contactExists = _TTContext.SaveChanges();
                            if (contactExists > 1)
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
                    var resultw = _TTContext.TT_LABLIB_DMO.Where(t =>t.MI_Id==data.MI_Id && t.TTLAB_LABLIBName.Trim()
                    ==data.TTLAB_LABLIBName.Trim() && t.ASMAY_Id==data.ASMAY_Id ).ToList();
                    if (resultw.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_LABLIB_DMO obj = new TT_LABLIB_DMO();
                        obj.MI_Id = data.MI_Id;
                        obj.TTMC_Id = data.TTMC_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.TTLAB_LABLIBName = data.TTLAB_LABLIBName.Trim();
                        obj.TTLAB_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _TTContext.Add(obj);
                        if (data.TempararyArrayList.Length>0)
                        {
                            foreach (var item in data.TempararyArrayList)
                            {
                                CLGLabDetailsDMO res = new CLGLabDetailsDMO();

                                res.TTLAB_Id = obj.TTLAB_Id;
                                res.AMCO_Id = item.AMCO_Id;
                                res.AMB_Id = item.AMB_Id;
                                res.AMSE_Id = item.AMSE_Id;
                                res.ACMS_Id = item.ACMS_Id;
                                res.ISMS_Id = item.ISMS_Id;
                                res.TTLABDC_ActiveFlag = true;
                                res.CreatedDate = DateTime.Now; 
                                res.UpdatedDate = DateTime.Now;
                                _TTContext.Add(res);
                            }
                            var contactExists = _TTContext.SaveChanges();
                            if (contactExists > 1)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion

        #region EDIT 
        public CLGLabDTO editlab(CLGLabDTO data)
        {

            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_EDIT_LAB_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTLAB_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTLAB_Id
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
                        data.labconsedit = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVE 
        public CLGLabDTO deactivate(CLGLabDTO data)
        {
            try
            {

                if (data.TTLAB_Id > 0)
                {
                    var result = _TTContext.TT_LABLIB_DMO.Single(t => t.TTLAB_Id == data.TTLAB_Id);

                    if (result.TTLAB_ActiveFlag == true)
                    {
                        result.TTLAB_ActiveFlag = false;
                        var resultdt = _TTContext.CLGLabDetailsDMO.Where(t => t.TTLAB_Id == data.TTLAB_Id).ToList();
                        if (resultdt.Count > 0)
                        {
                            foreach (var item in resultdt)
                            {
                                item.TTLABDC_ActiveFlag = false;
                                _TTContext.Update(item);
                            }
                        }
                        
                    }
                    else
                    {
                        result.TTLAB_ActiveFlag = true;
                        var resultdt = _TTContext.CLGLabDetailsDMO.Where(t => t.TTLAB_Id == data.TTLAB_Id).ToList();
                        if (resultdt.Count > 0)
                        {
                            foreach (var item in resultdt)
                            {
                                item.TTLABDC_ActiveFlag = true;
                                _TTContext.Update(item);
                            }
                        }
                    }
                    _TTContext.Update(result);
                    var flag = _TTContext.SaveChanges();
                    if (flag >1)
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
        #endregion
        
        #region VIEW LAB DETAILS 
        public CLGLabDTO viewrecordspopup(CLGLabDTO data)
        {
            try
            {


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_LAB_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTLAB_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.TTLAB_Id
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
                        data.labdetilspopuparray = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

      
    }
}
