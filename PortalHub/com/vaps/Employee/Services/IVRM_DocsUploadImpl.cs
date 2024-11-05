using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;

namespace PortalHub.com.vaps.Employee.Services
{
    public class IVRM_DocsUploadImpl : Interfaces.IVRM_DocsUploadInterface
    {
        public PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;
        ILogger<IVRM_DocsUploadImpl> _logPortal;
        public IVRM_DocsUploadImpl(PortalContext portalContext, DomainModelMsSqlServerContext db, ILogger<IVRM_DocsUploadImpl> log)
        {
            _PortalContext = portalContext;
            _db = db;
            _logPortal = log;
        }
        public IVRM_DocsUploadDTO Getdetails(IVRM_DocsUploadDTO data)
        {
            try
            {
                data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.Login_Id && c.MI_Id == data.MI_Id).Emp_Code;

                data.yearlist = _PortalContext.AcademicYearDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_ActiveFlag == 1).ToArray();

                data.classlist = _PortalContext.School_M_Section.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).ToArray();
               
                data.docsDetails = (from a in _PortalContext.IVRM_DocsUploadDMO
                                    from b in _PortalContext.School_M_Class
                                    from c in _PortalContext.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && a.Login_Id==data.Login_Id)
                                    select new IVRM_DocsUploadDTO
                                    {
                                        ASMCL_ClassName = b.ASMCL_ClassName,
                                        ASMC_SectionName = c.ASMC_SectionName,
                                        IDU_Type = a.IDU_Type,
                                        IDU_ActiveFlag = a.IDU_ActiveFlag,
                                        IDU_Attachment=a.IDU_Attachment,
                                        IDU_Id = a.IDU_Id
                                    }).OrderBy(d => d.IDU_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public IVRM_DocsUploadDTO savedetail(IVRM_DocsUploadDTO data)
        {
            try
            {
               if(data.IDU_Id>0)
                {
                    var Duplicate = _PortalContext.IVRM_DocsUploadDMO.Where(t =>t.IDU_Id!=data.IDU_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Login_Id == data.Login_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IDU_Type == data.IDU_Type && t.IDU_Attachment == data.IDU_Attachment).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _PortalContext.IVRM_DocsUploadDMO.Where(t => t.IDU_Id == data.IDU_Id).SingleOrDefault();

                        update.ASMCL_Id = data.ASMCL_Id;
                        update.ASMS_Id = data.ASMS_Id;
                        update.IDU_Type = data.IDU_Type;
                        update.IDU_Remarks = data.IDU_Remarks;
                        update.IDU_Attachment = data.IDU_Attachment;
                        update.IDU_FilePath = data.IDU_FilePath;
                        update.UpdatedDate = DateTime.Now;

                        _PortalContext.Update(update);

                        var contactExists = _PortalContext.SaveChanges();
                        if (contactExists > 0)
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
                    var Duplicate = _PortalContext.IVRM_DocsUploadDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Login_Id == data.Login_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.IDU_Type == data.IDU_Type && t.IDU_Attachment == data.IDU_Attachment).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_DocsUploadDMO obj = new IVRM_DocsUploadDMO();

                        obj.MI_Id = data.MI_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.Login_Id = data.Login_Id;
                        obj.ASMCL_Id = data.ASMCL_Id;
                        obj.ASMS_Id = data.ASMS_Id;
                        obj.IDU_Type = data.IDU_Type;
                        obj.IDU_Remarks = data.IDU_Remarks;
                        obj.IDU_Attachment = data.IDU_Attachment;
                        obj.IDU_FilePath = data.IDU_FilePath;
                        obj.IDU_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj);
                        var contactExists = _PortalContext.SaveChanges();
                        if (contactExists > 0)
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
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }
       
        public async Task<IVRM_DocsUploadDTO> get_classes(IVRM_DocsUploadDTO data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseClassStdata";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.classlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }               
            }
            catch (Exception ex)
            {
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }

        public IVRM_DocsUploadDTO getsectiondata(IVRM_DocsUploadDTO data)
        {
            try
            {
                data.sectionlist = (from d in _db.School_M_Section
                                    from e in _db.School_Adm_Y_StudentDMO
                                    where (d.ASMS_Id == e.ASMS_Id && d.ASMC_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && e.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && e.ASMCL_Id == data.ASMCL_Id)
                                    select new IVRM_DocsUploadDTO
                                    {
                                        ASMS_Id = d.ASMS_Id,
                                        ASMC_SectionName = d.ASMC_SectionName
                                    }
                          ).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                //data.sectionlist = (from d in _db.School_M_Section
                //                    from e in _db.Masterclasscategory
                //                    from f in _db.AdmSchoolMasterClassCatSec

                //                    where (d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == f.ASMS_Id && f.ASMCC_Id == e.ASMCC_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id)
                //                    select new IVRM_DocsUploadDTO
                //                    {
                //                        ASMS_Id = d.ASMS_Id,
                //                        ASMC_SectionName = d.ASMC_SectionName
                //                    }
                //            ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }


        public IVRM_DocsUploadDTO editData(IVRM_DocsUploadDTO data)
        {

            try
            {
                data.editlist = (from m in _PortalContext.IVRM_DocsUploadDMO
                                 from a in _PortalContext.AcademicYearDMO
                                 from n in _PortalContext.School_M_Class
                                 from o in _PortalContext.School_M_Section
                                 where (m.MI_Id == n.MI_Id && m.ASMAY_Id == a.ASMAY_Id && m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.IDU_Id == data.IDU_Id && m.MI_Id == data.MI_Id)
                                 select new IVRM_DocsUploadDTO
                                 {
                                     ASMCL_Id = n.ASMCL_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMS_Id = o.ASMS_Id,
                                     IDU_Id=m.IDU_Id,
                                     IDU_Type=m.IDU_Type,
                                     IDU_Remarks=m.IDU_Remarks,
                                     IDU_Attachment=m.IDU_Attachment,
                                     IDU_FilePath=m.IDU_FilePath
                                     
                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public IVRM_DocsUploadDTO deactivate(IVRM_DocsUploadDTO data)
        {
            try
            {
                var query = _PortalContext.IVRM_DocsUploadDMO.Single(s => s.MI_Id == data.MI_Id && s.IDU_Id == data.IDU_Id);

                if (query.IDU_ActiveFlag == true)
                {
                    query.IDU_ActiveFlag = false;
                }
                else
                {
                    query.IDU_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _PortalContext.Update(query);
                var contactExists = _PortalContext.SaveChanges();
                if (contactExists == 1)
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
                _logPortal.LogInformation("Portal  :" + ex.Message);
            }
            return data;
        }


    }
}
