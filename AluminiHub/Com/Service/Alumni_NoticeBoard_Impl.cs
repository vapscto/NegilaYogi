using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class Alumni_NoticeBoard_Impl : Interface.Alumni_NoticeBoard_Interface
    {
        public AlumniContext _AlumniContext;
        public Alumni_NoticeBoard_Impl(AlumniContext AlumniContext)
        {
            _AlumniContext = AlumniContext;
        }
        public Alumni_NoticeBoard_DTO loaddata(Alumni_NoticeBoard_DTO dto)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "alumni_noticeboard";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });

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
                            dto.alumninoticeboardlist = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Alumni_NoticeBoard_DTO savedetail(Alumni_NoticeBoard_DTO dto)
        {
            try
            {
                if (dto.ALNTB_Id > 0)
                {
                    var nd = _AlumniContext.Alumni_NoticeBoard_DMO_con.Single(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id);
                    nd.MI_Id = dto.MI_Id;
                    nd.ALNTB_Title = dto.ALNTB_Title;
                    nd.ALNTB_Description = dto.ALNTB_Description;
                    nd.ALNTB_DisplayDate = dto.ALNTB_DisplayDate;
                    nd.ALNTB_StartDate = dto.ALNTB_StartDate;
                    nd.ALNTB_EndDate = dto.ALNTB_EndDate;
                    nd.ALNTB_UpdatedDate = DateTime.Today;
                    _AlumniContext.Update(nd);

                    var allfile = _AlumniContext.Alumni_NoticeBoard_Files_DMO_con.Where(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id);
                    foreach (var item in allfile)
                    {
                        var result = _AlumniContext.Alumni_NoticeBoard_Files_DMO_con.Single(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id && a.ALNTBFL_Id == item.ALNTBFL_Id);
                        result.ALNTBFL_ActiveFlag = false;
                        _AlumniContext.Update(result);
                    }
                    if (dto.Attachment_Array.Length > 0)
                    {

                        foreach (var item in dto.Attachment_Array)
                        {
                            Alumni_NoticeBoard_Files_DMO dd = new Alumni_NoticeBoard_Files_DMO();

                            dd.MI_Id = dto.MI_Id;
                            dd.ALNTB_Id = dto.ALNTB_Id;
                            dd.ALNTBFL_FileName = item.ALNTBFL_FileName;
                            dd.ALNTBFL_FilePath = item.ALNTBFL_FilePath;
                            dd.ALNTBFL_ActiveFlag = true;
                            dd.ALNTBFL_CreatedDate = DateTime.Today;
                            dd.ALNTBFL_CreatedBy = dto.UserId;
                            _AlumniContext.Add(dd);

                        }
                    }

                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "Update";
                    }
                    else
                    {
                        dto.message = "Error";
                    }


                }
                else
                {
                    Alumni_NoticeBoard_DMO nd = new Alumni_NoticeBoard_DMO();

                    nd.MI_Id = dto.MI_Id;
                    nd.ALNTB_Title = dto.ALNTB_Title;
                    nd.ALNTB_Description = dto.ALNTB_Description;
                    nd.ALNTB_DisplayDate = dto.ALNTB_DisplayDate;
                    nd.ALNTB_StartDate = dto.ALNTB_StartDate;
                    nd.ALNTB_EndDate = dto.ALNTB_EndDate;
                    nd.ALNTB_TTSylabusFlg = "O";
                    nd.ALNTB_ActiveFlag = true;
                    nd.ALNTB_CreatedDate = DateTime.Today;
                    _AlumniContext.Add(nd);
                    if (dto.Attachment_Array.Length > 0)
                    {
                        foreach (var item in dto.Attachment_Array)
                        {
                            Alumni_NoticeBoard_Files_DMO dd = new Alumni_NoticeBoard_Files_DMO();

                            dd.MI_Id = dto.MI_Id;
                            dd.ALNTB_Id = nd.ALNTB_Id;
                            dd.ALNTBFL_FileName = item.ALNTBFL_FileName;
                            dd.ALNTBFL_FilePath = item.ALNTBFL_FilePath;
                            dd.ALNTBFL_ActiveFlag = true;
                            dd.ALNTBFL_CreatedDate = DateTime.Today;
                            dd.ALNTBFL_CreatedBy = dto.UserId;
                            _AlumniContext.Add(dd);

                        }
                    }
                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "Add";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Alumni_NoticeBoard_DTO viewData(Alumni_NoticeBoard_DTO dto)
        {
            try
            {
                dto.attachementlist = _AlumniContext.Alumni_NoticeBoard_Files_DMO_con.Where(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id && a.ALNTBFL_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Alumni_NoticeBoard_DTO deactivate(Alumni_NoticeBoard_DTO dto)
        {
            try
            {
                var result = _AlumniContext.Alumni_NoticeBoard_DMO_con.Single(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id);
                if (dto.ALNTB_ActiveFlag == true)
                {
                    result.ALNTB_ActiveFlag = false;
                    _AlumniContext.Update(result);
                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "false";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }
                else
                {
                    result.ALNTB_ActiveFlag = true;
                    _AlumniContext.Update(result);
                    var cnt = _AlumniContext.SaveChanges();
                    if (cnt > 0)
                    {
                        dto.message = "true";
                    }
                    else
                    {
                        dto.message = "Error";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public Alumni_NoticeBoard_DTO editdetails(Alumni_NoticeBoard_DTO dto)
        {
            try
            {
                dto.editdetails = _AlumniContext.Alumni_NoticeBoard_DMO_con.Where(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id).ToArray();
                dto.editdetailsfiles = _AlumniContext.Alumni_NoticeBoard_Files_DMO_con.Where(a => a.ALNTB_Id == dto.ALNTB_Id && a.MI_Id == dto.MI_Id && a.ALNTBFL_ActiveFlag == true).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
