using DataAccessMsSqlServerProvider.NAAC.Documents;
using DomainModel.Model.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Services
{
    public class NaacDocumentUploadImpl : Interface.NaacDocumentUploadInterface
    {
        public DocumentsContext _context;
        public NaacDocumentUploadImpl(DocumentsContext _conte)
        {
            _context = _conte;
        }
        public NaacDocumentUploadDTO onload(NaacDocumentUploadDTO data)
        {
            try
            {
                var getinstitution = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";

                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

                    if (NAACSL_InstitutionTypeFlg.ToUpper() == "DEEMED")
                    {
                        NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_SubInstitutionTypeFlg.ToUpper();
                    }
                }

                data.getparentidzero = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true && a.NAACSL_ParentId == 0
                && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getalldata = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true
                && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getsavealldata = (from a in _context.NaacDocumentUploadDetailsDMO
                                       from b in _context.NaacDocumentUploadDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && a.MI_Id == data.MI_Id && a.NAACMSL_ActiveFlag == true
                                       && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select a).Distinct().ToArray();

                var getapprovedlist = (from a in _context.NaacDocumentUploadDetailsDMO
                                       from b in _context.NaacDocumentUploadDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && a.MI_Id == data.MI_Id && a.NAACMSL_ActiveFlag == true && a.NAACMSL_Status == "Approved"
                                       && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadDTO
                                       {
                                           NAACSL_Id = a.NAACSL_Id,
                                           NAACSL_Percentage = b.NAACSL_Percentage
                                       }).Distinct().ToList();

                decimal percentage = 0;

                if (getapprovedlist.Count > 0)
                {
                    foreach (var c in getapprovedlist)
                    {
                        percentage += Convert.ToDecimal(c.NAACSL_Percentage);
                    }

                    data.percentage = percentage;
                }
                else
                {
                    data.percentage = 0;
                }


                var getmaxmarkslist = (from b in _context.NaacDocumentUploadDMO
                                       where (b.NAACSL_ActiveFlag == true && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadDTO
                                       {
                                           NAACSL_Percentage = b.NAACSL_Percentage
                                       }).Distinct().ToList();

                decimal maxpercentage = 0;

                if (getmaxmarkslist.Count > 0)
                {
                    foreach (var c in getmaxmarkslist)
                    {
                        maxpercentage += Convert.ToDecimal(c.NAACSL_Percentage);
                    }
                }
                else
                {
                    maxpercentage = 0;
                }
                if (percentage > 0 && maxpercentage > 0)
                {
                    var totalpercentage = (percentage / maxpercentage) * 100;
                    data.percentage = Convert.ToDecimal(String.Format("{0:0.00}", totalpercentage));
                }
                else
                {
                    data.percentage = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NaacDocumentUploadDTO save(NaacDocumentUploadDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long NAACMSL_Id = 0;
                NaacDocumentUploadDetailsDMO dmo = new NaacDocumentUploadDetailsDMO();
                var checksaveddetails = _context.NaacDocumentUploadDetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id).ToList();
                if (checksaveddetails.Count() > 0)
                {
                    NAACMSL_Id = checksaveddetails.FirstOrDefault().NAACMSL_Id;

                    var checksaveddetailsresult = _context.NaacDocumentUploadDetailsDMO.Single(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id);
                    checksaveddetailsresult.NAACMSL_Uploadpath = data.document_Path;
                    checksaveddetailsresult.NAACMSL_Details = data.comments;
                    checksaveddetailsresult.NAACMSL_UserRemarks = "";
                    checksaveddetailsresult.NAACMSL_Status = "Upload";
                    checksaveddetailsresult.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    checksaveddetailsresult.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    _context.Update(checksaveddetailsresult);

                }
                else
                {
                    dmo.NAACSL_Id = data.NAACSL_Id;
                    dmo.MI_Id = data.MI_Id;
                    dmo.NAACMSL_Status = "Upload";
                    dmo.NAACMSL_Uploadpath = data.document_Path;
                    dmo.NAACMSL_UserRemarks = "";
                    dmo.NAACMSL_Details = data.comments;
                    dmo.NAACMSL_ConsultantRemarks = "";
                    dmo.NAACMSL_ActiveFlag = true;
                    dmo.NAACMSL_CreatedBy = data.NAACMSL_CreatedBy;
                    dmo.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    dmo.NAACMSL_CreatedDate = DateTime.UtcNow;
                    dmo.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    _context.Add(dmo);

                    NAACMSL_Id = dmo.NAACMSL_Id;
                }

                // Master SL Files
                if (data.document_Path != null && data.document_Path != "")
                {
                    NAAC_Master_SL_FileDMO masterslfiledmo = new NAAC_Master_SL_FileDMO();

                    if (data.NAACMSLF_Id > 0)
                    {
                        var resultmasterfiledmo = _context.NAAC_Master_SL_FileDMO.Single(a => a.NAACMSLF_Id == data.NAACMSLF_Id);
                        resultmasterfiledmo.NAACMSLF_FileRemarks = data.comments;
                        resultmasterfiledmo.NAACMSLF_UpdatedDate = DateTime.UtcNow;
                        resultmasterfiledmo.NAACMSLF_UpdatedBy = data.NAACMSL_CreatedBy;
                        _context.Update(resultmasterfiledmo);
                    }
                    else
                    {
                        masterslfiledmo.NAACMSL_Id = NAACMSL_Id;
                        masterslfiledmo.NAACMSLF_FileName = data.file_name;
                        masterslfiledmo.NAACMSLF_FilePath = data.document_Path;
                        masterslfiledmo.NAACMSLF_UploadDate = indianTime;
                        masterslfiledmo.NAACMSLF_FileRemarks = "";
                        masterslfiledmo.NAACMSLF_FileStatus = "Upload";
                        masterslfiledmo.NAACMSLF_ActiveFlag = true;
                        masterslfiledmo.NAACMSLF_CreatedBy = data.NAACMSL_CreatedBy;
                        masterslfiledmo.NAACMSLF_CreatedDate = DateTime.UtcNow;
                        masterslfiledmo.NAACMSLF_UpdatedBy = data.NAACMSL_CreatedBy;
                        masterslfiledmo.NAACMSLF_UpdatedDate = DateTime.UtcNow;

                        _context.Add(masterslfiledmo);
                    }

                    //NAAC_Master_SL_File_CommentsDMO masterslfilecomments = new NAAC_Master_SL_File_CommentsDMO();
                    //masterslfilecomments.NAACMSLF_Id = masterslfiledmo.NAACMSLF_Id;
                    //masterslfilecomments.NAACMSLFCO_Remarks = data.comments;
                    //masterslfilecomments.NAACMSLFCO_RemarksBy = data.NAACMSL_CreatedBy;
                    //masterslfilecomments.NAACMSLFCO_ActiveFlag = true;
                    //masterslfilecomments.NAACMSLFCO_CreatedBy = data.NAACMSL_CreatedBy;
                    //masterslfilecomments.NAACMSLFCO_CreatedDate = DateTime.UtcNow;
                    //masterslfilecomments.NAACMSLFCO_UpdatedBy = data.NAACMSL_CreatedBy;
                    //masterslfilecomments.NAACMSLFCO_UpdatedDate = DateTime.UtcNow;
                    //_context.Add(masterslfilecomments);
                }

                var i = _context.SaveChanges();
                if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // consultancy Saving  
        public NaacDocumentUploadDTO saveapproved(NaacDocumentUploadDTO data)
        {
            try
            {
                var checksaveddetails = _context.NaacDocumentUploadDetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id).ToList();
                if (checksaveddetails.Count() > 0)
                {
                    var checksaveddetailsresult = _context.NaacDocumentUploadDetailsDMO.Single(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id);
                    checksaveddetailsresult.NAACMSL_Status = data.status;
                    checksaveddetailsresult.NAACMSL_ConsultantRemarks = data.coordinatorcomments;
                    checksaveddetailsresult.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    checksaveddetailsresult.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    _context.Update(checksaveddetailsresult);
                }
                else
                {
                    NaacDocumentUploadDetailsDMO dmo = new NaacDocumentUploadDetailsDMO();
                    dmo.NAACSL_Id = data.NAACSL_Id;
                    dmo.MI_Id = data.MI_Id;
                    dmo.NAACMSL_Status = data.status;
                    dmo.NAACMSL_Uploadpath = data.document_Path;
                    dmo.NAACMSL_ConsultantRemarks = "";
                    dmo.NAACMSL_UserRemarks = "";
                    dmo.NAACMSL_ActiveFlag = true;
                    dmo.NAACMSL_CreatedBy = data.NAACMSL_CreatedBy;
                    dmo.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    dmo.NAACMSL_CreatedDate = DateTime.UtcNow;
                    dmo.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    _context.Add(dmo);
                }

                var i = _context.SaveChanges();
                if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ********* View The Uploded Files ********* //
        public NaacDocumentUploadDTO getuploaddoc(NaacDocumentUploadDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_Get_Document_Upload_List_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@NAACSL_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.NAACSL_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@NAACMSLF_Id", SqlDbType.VarChar) { Value = 0 });

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
                        data.getdocumentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ********* Save The File Wise Comments ********* //
        public NaacDocumentUploadDTO savecomments(NaacDocumentUploadDTO data)
        {
            try
            {
                if (data.temp_dto.Length > 0)
                {
                    foreach (var c in data.temp_dto)
                    {
                        NAAC_Master_SL_File_CommentsDMO filecommentsdmo = new NAAC_Master_SL_File_CommentsDMO();

                        filecommentsdmo.NAACMSLF_Id = c.NAACMSLF_Id;
                        filecommentsdmo.NAACMSLFCO_Remarks = c.usercomments;
                        filecommentsdmo.NAACMSLFCO_RemarksBy = data.NAACMSL_CreatedBy;
                        filecommentsdmo.NAACMSLFCO_ActiveFlag = true;
                        filecommentsdmo.NAACMSLFCO_CreatedBy = data.NAACMSL_CreatedBy;
                        filecommentsdmo.NAACMSLFCO_CreatedDate = DateTime.UtcNow;
                        filecommentsdmo.NAACMSLFCO_UpdatedBy = data.NAACMSL_CreatedBy;
                        filecommentsdmo.NAACMSLFCO_UpdatedDate = DateTime.UtcNow;
                        _context.Add(filecommentsdmo);
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NaacDocumentUploadDTO savecommentscons(NaacDocumentUploadDTO data)
        {
            try
            {
                if (data.temp_dto.Length > 0)
                {
                    foreach (var c in data.temp_dto)
                    {
                        var result = _context.NAAC_Master_SL_FileDMO.Single(a => a.NAACMSLF_Id == c.NAACMSLF_Id);
                        result.NAACMSLF_FileStatus = c.filestatus;
                        result.NAACMSLF_UpdatedBy = data.NAACMSL_CreatedBy;
                        result.NAACMSLF_UpdatedDate = DateTime.UtcNow;
                        _context.Update(result);

                        if (c.usercomments != "" && c.usercomments != null)
                        {
                            NAAC_Master_SL_File_CommentsDMO filecommentsdmo = new NAAC_Master_SL_File_CommentsDMO();
                            filecommentsdmo.NAACMSLF_Id = c.NAACMSLF_Id;
                            filecommentsdmo.NAACMSLFCO_Remarks = c.usercomments;
                            filecommentsdmo.NAACMSLFCO_RemarksBy = data.NAACMSL_CreatedBy;
                            filecommentsdmo.NAACMSLFCO_ActiveFlag = true;
                            filecommentsdmo.NAACMSLFCO_CreatedBy = data.NAACMSL_CreatedBy;
                            filecommentsdmo.NAACMSLFCO_CreatedDate = DateTime.UtcNow;
                            filecommentsdmo.NAACMSLFCO_UpdatedBy = data.NAACMSL_CreatedBy;
                            filecommentsdmo.NAACMSLFCO_UpdatedDate = DateTime.UtcNow;
                            _context.Add(filecommentsdmo);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ********* View The File Wise Comments ********* //
        public NaacDocumentUploadDTO viewcomments(NaacDocumentUploadDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_Get_Document_Upload_List_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@NAACSL_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.NAACSL_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@NAACMSLF_Id", SqlDbType.VarChar) { Value = data.NAACMSLF_Id });

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
                        data.getcommentslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ********* Save The General Comments ********* //
        public NaacDocumentUploadDTO savegeneralcommetns(NaacDocumentUploadDTO data)
        {
            try
            {
                NAAC_Master_SL_CommentsDMO slcommentsdmo = new NAAC_Master_SL_CommentsDMO();
                slcommentsdmo.NAACSL_Id = data.NAACSL_Id;
                slcommentsdmo.NAACMSLCO_Remarks = data.NAACMSLCO_Remarks;
                slcommentsdmo.MI_Id = data.MI_Id;
                slcommentsdmo.NAACMSLCO_RemarksBy = data.NAACMSL_CreatedBy;
                slcommentsdmo.NAACMSLCO_ActiveFlag = true;
                slcommentsdmo.NAACMSLCO_CreatedBy = data.NAACMSL_CreatedBy;
                slcommentsdmo.NAACMSLCO_CreatedDate = DateTime.UtcNow;
                slcommentsdmo.NAACMSLCO_UpdatedBy = data.NAACMSL_CreatedBy;
                slcommentsdmo.NAACMSLCO_UpdatedDate = DateTime.UtcNow;

                _context.Add(slcommentsdmo);
                var i = _context.SaveChanges();

                if (i > 0)
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

        // ********* View The General Comments ********* //
        public NaacDocumentUploadDTO getcomments(NaacDocumentUploadDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_Get_Document_Upload_List_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@NAACSL_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.NAACSL_Id) });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@NAACMSLF_Id", SqlDbType.VarChar) { Value = 0 });

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
                        data.getgeneralcommentslist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ********* Save Hyperlinks ************ // 
        public NaacDocumentUploadDTO savehyperlinks(NaacDocumentUploadDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                long NAACMSL_Id = 0;
                NaacDocumentUploadDetailsDMO dmo = new NaacDocumentUploadDetailsDMO();
                var checksaveddetails = _context.NaacDocumentUploadDetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id).ToList();
                if (checksaveddetails.Count() > 0)
                {
                    NAACMSL_Id = checksaveddetails.FirstOrDefault().NAACMSL_Id;

                    var checksaveddetailsresult = _context.NaacDocumentUploadDetailsDMO.Single(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id);
                    checksaveddetailsresult.NAACMSL_Uploadpath = data.document_Path;
                    checksaveddetailsresult.NAACMSL_Details = data.comments;
                    checksaveddetailsresult.NAACMSL_UserRemarks = "";
                    checksaveddetailsresult.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    checksaveddetailsresult.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    _context.Update(checksaveddetailsresult);
                }
                else
                {
                    dmo.NAACSL_Id = data.NAACSL_Id;
                    dmo.MI_Id = data.MI_Id;
                    dmo.NAACMSL_Status = "Upload";
                    dmo.NAACMSL_Uploadpath = data.document_Path;
                    dmo.NAACMSL_UserRemarks = "";
                    dmo.NAACMSL_Details = data.comments;
                    dmo.NAACMSL_ConsultantRemarks = "";
                    dmo.NAACMSL_ActiveFlag = true;
                    dmo.NAACMSL_CreatedBy = data.NAACMSL_CreatedBy;
                    dmo.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    dmo.NAACMSL_CreatedDate = DateTime.UtcNow;
                    dmo.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    _context.Add(dmo);

                    NAACMSL_Id = dmo.NAACMSL_Id;
                }


                if (data.temp_hyperlink_dto.Count() > 0)
                {
                    foreach (var d in data.temp_hyperlink_dto)
                    {
                        NAAC_Master_SL_LinkDMO dmolinks = new NAAC_Master_SL_LinkDMO();

                        dmolinks.NAACMSL_Id = NAACMSL_Id;
                        dmolinks.NAACMSLLK_LinkName = d.NAACMSLLK_LinkName;
                        dmolinks.NAACMSLLK_LinkRemarks = d.NAACMSLLK_LinkRemarks;
                        dmolinks.NAACMSLLK_ActiveFlag = true;
                        dmolinks.NAACMSLLK_CreatedBy = data.NAACMSL_CreatedBy;
                        dmolinks.NAACMSLLK_UpdatedBy = data.NAACMSL_UpdatedBy;
                        dmolinks.NAACMSLLK_CreatedDate = DateTime.UtcNow;
                        dmolinks.NAACMSLLK_UpdatedDate = DateTime.UtcNow;

                        _context.Add(dmolinks);
                    }
                }
                var i = _context.SaveChanges();
                if (i > 0)
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

        // ******** View Added Hyper Links *************** //
        public NaacDocumentUploadDTO viewaddedhyperlink(NaacDocumentUploadDTO data)
        {
            try
            {
                data.viewhyperlinks = (from a in _context.NaacDocumentUploadDMO
                                       from b in _context.NaacDocumentUploadDetailsDMO
                                       from c in _context.NAAC_Master_SL_LinkDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && b.NAACMSL_Id == c.NAACMSL_Id && b.MI_Id == data.MI_Id && b.NAACSL_Id == data.NAACSL_Id)
                                       select new NaacDocumentUploadDTO
                                       {
                                           NAACMSLLK_Id = c.NAACMSLLK_Id,
                                           NAACMSLLK_LinkName = c.NAACMSLLK_LinkName,
                                           NAACMSLLK_LinkRemarks = c.NAACMSLLK_LinkRemarks,
                                           NAACMSLLK_ActiveFlag = c.NAACMSLLK_ActiveFlag
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // ******** Deleting The Hyper Links ********* //
        public NaacDocumentUploadDTO deletehyperlink(NaacDocumentUploadDTO data)
        {
            try
            {
                var checkrecord = _context.NAAC_Master_SL_LinkDMO.Where(a => a.NAACMSLLK_Id == data.NAACMSLLK_Id).ToList();

                if (checkrecord.Count() > 0)
                {
                    var checkrecordresult = _context.NAAC_Master_SL_LinkDMO.Single(a => a.NAACMSLLK_Id == data.NAACMSLLK_Id);
                    _context.Remove(checkrecordresult);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // *********** Deleting The Upload Documents ********* //
        public NaacDocumentUploadDTO deleteuploadfile(NaacDocumentUploadDTO data)
        {
            try
            {
                var checkrecord = _context.NAAC_Master_SL_FileDMO.Where(a => a.NAACMSLF_Id == data.NAACMSLF_Id).ToList();

                if (checkrecord.Count() > 0)
                {
                    var checkrecordresult = _context.NAAC_Master_SL_FileDMO.Single(a => a.NAACMSLF_Id == data.NAACMSLF_Id);
                    _context.Remove(checkrecordresult);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // *********** Adding CGP *************** //
        public NaacDocumentUploadDTO saveCGPA(NaacDocumentUploadDTO data)
        {
            try
            {
                var checkdata = _context.NaacDocumentUploadDetailsDMO.Where(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id).ToList();
                if (checkdata.Count() > 0)
                {
                    var checksaveddetailsresult = _context.NaacDocumentUploadDetailsDMO.Single(a => a.MI_Id == data.MI_Id && a.NAACSL_Id == data.NAACSL_Id);
                    checksaveddetailsresult.NAACMSL_CGPA = data.NAACMSL_CGPA;
                    checksaveddetailsresult.NAACMSL_UpdatedBy = data.NAACMSL_UpdatedBy;
                    checksaveddetailsresult.NAACMSL_UpdatedDate = DateTime.UtcNow;
                    _context.Update(checksaveddetailsresult);
                }

                var i = _context.SaveChanges();
                if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
