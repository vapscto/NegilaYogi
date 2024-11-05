using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.admission;

namespace PortalHub.com.vaps.IVRM.Services
{
    public class IVRM_GalleryImpl : Interfaces.IVRM_GalleryInterface
    {
        private static ConcurrentDictionary<string, IVRM_GalleryDTO> _login =
           new ConcurrentDictionary<string, IVRM_GalleryDTO>();
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;
        public ClgAdmissionContext _ClgAdmissionContext;

        public IVRM_GalleryImpl(PortalContext PortalContext, DomainModelMsSqlServerContext context, ClgAdmissionContext clgAdmissionContext)
        {
            _PortalContext = PortalContext;
            _context = context;
            _ClgAdmissionContext = clgAdmissionContext;
        }

        public IVRM_GalleryDTO getloaddata(IVRM_GalleryDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_GalleryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
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
                            data.get_galleryimg = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.classlist = (from a in _PortalContext.School_M_Class
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.AcademicYearDMO
                                      from d in _PortalContext.School_M_Section
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.AMST_Id == data.AMST_Id)
                                      select new IVRM_GalleryDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          ASMS_Id = b.ASMS_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classlist = (from a in _PortalContext.School_M_Class
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      from c in _PortalContext.AcademicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_ActiveFlag == true)
                                      select new IVRM_GalleryDTO
                                      {
                                          ASMCL_Id = b.ASMCL_Id,
                                          //ASMS_Id = b.ASMS_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMCL_Order = a.ASMCL_Order
                                      }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_GalleryDTO get_section(IVRM_GalleryDTO data)
        {
            try
            {

                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.get_galleryimg = _PortalContext.IVRM_GalleryDMO.Where(g => g.MI_Id == data.MI_Id && g.IGA_ActiveFlag == true).ToArray();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.sectionlist = (from a in _PortalContext.School_M_Class
                                        from b in _PortalContext.School_Adm_Y_StudentDMO
                                        from c in _PortalContext.AcademicYearDMO
                                        from d in _PortalContext.School_M_Section
                                        where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMCL_ActiveFlag == true)
                                        select new IVRM_GalleryDTO
                                        {
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = d.ASMC_SectionName,
                                            ASMCL_ClassName = a.ASMCL_ClassName,                                            
                                            ASMC_Order = d.ASMC_Order
                                        }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
                else
                {

                    if (data.Classlst.Length > 0)
                    {
                        List<IVRM_GalleryDTO> aggregatedSections = new List<IVRM_GalleryDTO>(); // Create a list to aggregate the sections

                        foreach (var ue in data.Classlst)
                        {
                            long asmclId = Convert.ToInt64(ue.ASMCL_Id);
                            var sections = (from a in _PortalContext.School_M_Class
                                            from b in _PortalContext.School_Adm_Y_StudentDMO
                                            from c in _PortalContext.AcademicYearDMO
                                            from d in _PortalContext.School_M_Section
                                            where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == asmclId && a.ASMCL_ActiveFlag == true)
                                            select new IVRM_GalleryDTO
                                            {
                                                ASMCL_Id = b.ASMCL_Id,
                                                ASMS_Id = b.ASMS_Id,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                ASMCL_ClassName = a.ASMCL_ClassName,
                                                ASMC_Order = d.ASMC_Order
                                            }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

                            aggregatedSections.AddRange(sections); 
                        }

                        data.sectionlist = aggregatedSections.ToArray(); 
                    }

                    //data.sectionlist = (from a in _PortalContext.School_M_Class
                    //                    from b in _PortalContext.School_Adm_Y_StudentDMO
                    //                    from c in _PortalContext.AcademicYearDMO
                    //                    from d in _PortalContext.School_M_Section
                    //                    where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMCL_ActiveFlag == true)
                    //                    select new IVRM_GalleryDTO
                    //                    {
                    //                        ASMCL_Id = b.ASMCL_Id,
                    //                        ASMS_Id = b.ASMS_Id,
                    //                        ASMC_SectionName = d.ASMC_SectionName,
                    //                        ASMC_Order = d.ASMC_Order
                    //                    }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public IVRM_GalleryDTO savedata(IVRM_GalleryDTO data)
        {
            try
            {
                IVRM_GalleryDMO img = new IVRM_GalleryDMO();
                img.MI_Id = data.MI_Id;
                img.IGA_GalleryName = data.IGA_GalleryName;
                img.IGA_Date = data.IGA_Date;
                img.IGA_CreatedBy = data.UserId;
                img.IGA_UpdatedBy = data.UserId;
                img.IGA_Time = data.IGA_Time;
                img.IGA_CommonGalleryFlg = data.IGA_CommonGalleryFlg;
                img.IGA_ActiveFlag = true;
                img.CreatedDate = DateTime.Now;
                img.UpdatedDate = DateTime.Now;
                _PortalContext.Add(img);

                if (data.images_list.Length > 0)
                {
                    if (data.mediatype == "I")
                    {
                        foreach (var ig in data.images_list)
                        {
                            IVRM_Gallery_PhotosDMO imgfile = new IVRM_Gallery_PhotosDMO();
                            imgfile.IGA_Id = img.IGA_Id;
                            imgfile.IGAP_Photos = ig;
                            imgfile.IGAP_CoverPhotoFlag = false;
                            imgfile.IGAP_ActiveFlag = true;
                            imgfile.CreatedDate = DateTime.Now;
                            imgfile.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(imgfile);
                        }
                    }
                    else
                    {
                        foreach (var ig in data.images_list)
                        {
                            IVRM_Gallery_VideosDMO vidfile = new IVRM_Gallery_VideosDMO();
                            vidfile.IGA_Id = img.IGA_Id;
                            vidfile.IGAV_Videos = ig;
                            vidfile.IGAV_ActiveFlag = true;
                            vidfile.CreatedDate = DateTime.Now;
                            vidfile.UpdatedDate = DateTime.Now;
                            _PortalContext.Add(vidfile);
                        }
                    }
                }
                if (data.arraySection.Length > 0)
                {
                    foreach (var igcls in data.arraySection)
                    {
                        IVRM_Gallery_ClassDMO imgcls = new IVRM_Gallery_ClassDMO();
                        imgcls.IGA_Id = img.IGA_Id;
                        imgcls.ASMCL_Id = igcls.ASMCL_Id;
                        imgcls.ASMS_Id = igcls.ASMS_Id;
                        imgcls.IGACL_ActiveFlag = true;
                        imgcls.IGACL_CreatedBy = data.UserId;
                        imgcls.IGACL_UpdatedBy = data.UserId;
                        imgcls.CreatedDate = DateTime.Now;
                        imgcls.UpdatedDate = DateTime.Now;
                        _PortalContext.Add(imgcls);
                    }
                }
                var contexttrans = _PortalContext.SaveChanges();
                if (contexttrans > 0)
                {
                    data.igaId = img.IGA_Id;
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
            }
            return data;
        }
        public IVRM_GalleryDTO savecover(IVRM_GalleryDTO data)
        {
            try
            {
                var coverflag = _PortalContext.IVRM_Gallery_PhotosDMO.Where(u => u.IGA_Id==data.IGA_Id).ToList();
                foreach (var item in coverflag)
                {
                    if (item.IGAP_CoverPhotoFlag == true)
                    {
                        var coverflag1 = _PortalContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGA_Id == item.IGA_Id && u.IGAP_Id==item.IGAP_Id);
                        coverflag1.IGAP_CoverPhotoFlag = false;
                        _PortalContext.Update(coverflag1);
                    }
                }
                var coverflag2 = _PortalContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGA_Id == data.IGA_Id && u.IGAP_Id == data.IGAP_Id);
                coverflag2.IGAP_CoverPhotoFlag = true;
                _PortalContext.Update(coverflag2);
                var contextupdate = _PortalContext.SaveChanges();
                if (contextupdate > 0)
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
                data.message = "Error";
            }
            return data;
        }
        public IVRM_GalleryDTO getcovermodel(IVRM_GalleryDTO data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_GalleryFilesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IGA_Id", SqlDbType.BigInt) { Value = data.IGA_Id });
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
                            data.covermodel = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_GalleryFilesDetails1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
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
                            data.covermodel1 = retObject.Distinct().ToArray();
                        }
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


        //kiosk added by adarsh
        public IVRM_GalleryDTO kioskvideo(IVRM_GalleryDTO data)
        {
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_GalleryFilesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IGA_Id", SqlDbType.BigInt) { Value = "0" });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt) { Value = data.roleflg });
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
                            data.classlist = retObject.Distinct().ToArray();
                        }
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
        public IVRM_GalleryDTO deactive(IVRM_GalleryDTO data)
        {
            try
            {
                var result = _PortalContext.IVRM_GalleryDMO.Single(t => t.IGA_Id == data.IGA_Id);
                if (result.IGA_ActiveFlag == true)
                {
                    result.IGA_ActiveFlag = false;
                }
                else if (result.IGA_ActiveFlag == false)
                {
                    result.IGA_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);

                var resultIGC = _PortalContext.IVRM_Gallery_ClassDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                var resultIGP = _PortalContext.IVRM_Gallery_PhotosDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                var resultIGV = _PortalContext.IVRM_Gallery_VideosDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                foreach (var igc in resultIGC)
                {
                    if (result.IGA_ActiveFlag == true)
                    {
                        igc.IGACL_ActiveFlag = true;
                    }
                    else if (result.IGA_ActiveFlag == false)
                    {
                        igc.IGACL_ActiveFlag = false;
                    }
                    igc.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(igc);
                }
                foreach (var igp in resultIGP)
                {
                    if (result.IGA_ActiveFlag == true)
                    {
                        igp.IGAP_ActiveFlag = true;
                    }
                    else if (result.IGA_ActiveFlag == false)
                    {
                        igp.IGAP_ActiveFlag = false;
                    }

                    igp.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(igp);
                }
                foreach (var igv in resultIGV)
                {
                    if (result.IGA_ActiveFlag == true)
                    {
                        igv.IGAV_ActiveFlag = true;
                    }
                    else if (result.IGA_ActiveFlag == false)
                    {
                        igv.IGAV_ActiveFlag = false;
                    }

                    igv.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(igv);
                }


                int returnval = _PortalContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        //edit
        public IVRM_GalleryDTO Editdetails(IVRM_GalleryDTO data)
        {
            try
            {

                data.editdata = _PortalContext.IVRM_GalleryDMO.Where(t => t.IGA_Id == data.IGA_Id).Distinct().ToArray();

                data.attachementlist = (from a in _PortalContext.IVRM_Gallery_PhotosDMO
                                        from b in _PortalContext.IVRM_Gallery_VideosDMO
                                        where a.IGA_Id == data.IGA_Id && a.IGA_Id == b.IGA_Id
                                        select new IVRM_GalleryDTO
                                        {
                                            IGAP_Photos = a.IGAP_Photos,
                                            IGAV_Videos = b.IGAV_Videos,

                                        }).ToArray();
                data.editclass= _PortalContext.IVRM_Gallery_ClassDMO.Where(t => t.IGA_Id == data.IGA_Id ).ToArray();
                


                //data.editclass = (from a in _PortalContext.IVRM_Gallery_ClassDMO
                //                  from b in _PortalContext.IVRM_GalleryDMO
                //                        where a.IGA_Id == data.IGA_Id && a.IGA_Id == b.IGA_Id
                //                        select new IVRM_GalleryDTO
                //                        {
                //                            ASMCL_Id = a.ASMCL_Id,
                //                            ASMS_Id = a.ASMS_Id,

                //                        }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return data;
        }
    }
}
