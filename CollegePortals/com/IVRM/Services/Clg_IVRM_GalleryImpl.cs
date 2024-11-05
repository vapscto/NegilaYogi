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
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;

namespace CollegePortals.com.vaps.IVRM.Services
{
    public class Clg_IVRM_GalleryImpl : Interfaces.Clg_IVRM_GalleryInterface
    {
        private static ConcurrentDictionary<string, ClgIVRMGalleryDTO> _login = new ConcurrentDictionary<string, ClgIVRMGalleryDTO>();
        private CollegeportalContext _PortalContext;

        public Clg_IVRM_GalleryImpl(CollegeportalContext PortalContext)
        {
            _PortalContext = PortalContext;
        }

        public ClgIVRMGalleryDTO getloaddata(ClgIVRMGalleryDTO data)
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
                    data.courselist = (from a in _PortalContext.Adm_College_Yearly_StudentDMO  
                                      from b in _PortalContext.MasterCourseDMO
                                      from c in _PortalContext.ClgMasterBranchDMO
                                      from d in _PortalContext.CLG_Adm_Master_SemesterDMO
                                      from e in _PortalContext.Adm_College_Master_SectionDMO
                                      from f in _PortalContext.academicYearDMO
                                      where (b.MI_Id == f.MI_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.ASMAY_Id == f.ASMAY_Id && b.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && a.AMSE_Id == d.AMSE_Id && a.ACMS_Id == e.ACMS_Id && b.AMCO_ActiveFlag == true && a.AMCST_Id == data.AMCST_Id)
                                      select new ClgIVRMGalleryDTO
                                      {
                                          AMCO_Id = b.AMCO_Id,
                                          AMCO_CourseName = b.AMCO_CourseName,
                                          AMCO_Order = b.AMCO_Order,
                                          AMB_Id = c.AMB_Id,
                                          AMB_BranchName = c.AMB_BranchName,
                                          AMSE_Id = d.AMSE_Id,
                                          AMSE_SEMName = d.AMSE_SEMName,
                                          ACMS_Id = e.ACMS_Id,
                                          ACMS_SectionName = e.ACMS_SectionName
                                      }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                }
                else
                {
                    data.courselist = (from a in _PortalContext.MasterCourseDMO
                                      from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                      from c in _PortalContext.academicYearDMO
                                      where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCO_ActiveFlag == true)
                                      select new ClgIVRMGalleryDTO
                                      {
                                          AMCO_Id = b.AMCO_Id,
                                          AMCO_CourseName = a.AMCO_CourseName,
                                          AMCO_Order = a.AMCO_Order
                                      }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgIVRMGalleryDTO get_branch(ClgIVRMGalleryDTO data)
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
                    data.branchlist = (from a in _PortalContext.MasterCourseDMO
                                        from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                        from c in _PortalContext.academicYearDMO
                                        from d in _PortalContext.ClgMasterBranchDMO
                                        where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true)
                                        select new ClgIVRMGalleryDTO
                                        {
                                            AMCO_Id = b.AMCO_Id,
                                            AMB_Id = b.AMB_Id,
                                            AMB_BranchName = d.AMB_BranchName,
                                            AMB_Order = d.AMB_Order
                                        }).Distinct().OrderBy(t => t.AMB_Order).ToArray();
                }
                else
                {
                    data.branchlist = (from a in _PortalContext.MasterCourseDMO
                                        from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                        from c in _PortalContext.academicYearDMO
                                        from d in _PortalContext.ClgMasterBranchDMO
                                        where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true)
                                        select new ClgIVRMGalleryDTO
                                        {
                                            AMCO_Id = b.AMCO_Id,
                                            AMB_Id = b.AMB_Id,
                                            AMB_BranchName = d.AMB_BranchName,
                                            AMB_Order = d.AMB_Order
                                        }).Distinct().OrderBy(t => t.AMB_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgIVRMGalleryDTO get_semester(ClgIVRMGalleryDTO data)
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
                    data.semesterlist = (from a in _PortalContext.MasterCourseDMO
                                       from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                       from c in _PortalContext.academicYearDMO
                                       from d in _PortalContext.ClgMasterBranchDMO
                                       from e in _PortalContext.CLG_Adm_Master_SemesterDMO
                                       where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id && e.AMSE_ActiveFlg == true && e.AMSE_Id == b.AMSE_Id)
                                       select new ClgIVRMGalleryDTO
                                       {
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id,
                                           AMSE_SEMName = e.AMSE_SEMName,
                                           AMSE_SEMOrder = e.AMSE_SEMOrder
                                       }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    data.semesterlist = (from a in _PortalContext.MasterCourseDMO
                                       from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                       from c in _PortalContext.academicYearDMO
                                       from d in _PortalContext.ClgMasterBranchDMO
                                       from e in _PortalContext.CLG_Adm_Master_SemesterDMO
                                         where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id && e.AMSE_ActiveFlg == true && e.AMSE_Id == b.AMSE_Id)
                                       select new ClgIVRMGalleryDTO
                                       {
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id,
                                           AMSE_SEMName = e.AMSE_SEMName,
                                           AMSE_SEMOrder = e.AMSE_SEMOrder
                                       }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgIVRMGalleryDTO get_Section(ClgIVRMGalleryDTO data)
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
                    data.sectionlist = (from a in _PortalContext.MasterCourseDMO
                                       from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                       from c in _PortalContext.academicYearDMO
                                       from d in _PortalContext.ClgMasterBranchDMO
                                       from e in _PortalContext.CLG_Adm_Master_SemesterDMO
                                       from f in _PortalContext.Adm_College_Master_SectionDMO
                                        where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == data.AMSE_Id && f.ACMS_ActiveFlag == true && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id)
                                       select new ClgIVRMGalleryDTO
                                       {
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = e.AMSE_Id,
                                           ACMS_Id = f.ACMS_Id,
                                           ACMS_SectionName = f.ACMS_SectionName,
                                           ACMS_Order = f.ACMS_Order
                                       }).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
                }
                else
                {
                    data.sectionlist = (from a in _PortalContext.MasterCourseDMO
                                       from b in _PortalContext.Adm_College_Yearly_StudentDMO
                                       from c in _PortalContext.academicYearDMO
                                       from d in _PortalContext.ClgMasterBranchDMO
                                       from e in _PortalContext.CLG_Adm_Master_SemesterDMO
                                       from f in _PortalContext.Adm_College_Master_SectionDMO
                                        where (a.MI_Id == c.MI_Id && a.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == data.AMSE_Id && f.ACMS_ActiveFlag == true && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id)
                                       select new ClgIVRMGalleryDTO
                                       {
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = e.AMSE_Id,
                                           ACMS_Id = f.ACMS_Id,
                                           ACMS_SectionName = f.ACMS_SectionName,
                                           ACMS_Order = f.ACMS_Order
                                       }).Distinct().OrderBy(t => t.ACMS_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgIVRMGalleryDTO savedata(ClgIVRMGalleryDTO data)
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
                _PortalContext.SaveChanges();
                //long IGAId = img.IGA_Id;

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
                        IVRM_Gallery_ProgramsDMO imgcls = new IVRM_Gallery_ProgramsDMO();
                        imgcls.IGA_Id = img.IGA_Id;
                        imgcls.AMCO_Id = data.AMCO_Id;
                        imgcls.AMB_Id = data.AMB_Id;
                        imgcls.AMSE_Id = data.AMSE_Id;
                        imgcls.ACMS_Id = igcls.ACMS_Id;
                        imgcls.IGAPRG_ActiveFlag = true;
                        imgcls.IGAPRG_CreatedBy = data.UserId;
                        imgcls.IGAPRG_UpdatedBy = data.UserId;
                        imgcls.IGAPRG_CreatedDate = DateTime.Now;
                        imgcls.IGAPRG_UpdatedDate = DateTime.Now;
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
        public ClgIVRMGalleryDTO savecover(ClgIVRMGalleryDTO data)
        {
            try
            {
                var coverflag = _PortalContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGAP_Id == data.IGAP_Id);
                coverflag.IGAP_CoverPhotoFlag = true;
                _PortalContext.Update(coverflag);
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
        public ClgIVRMGalleryDTO getcovermodel(ClgIVRMGalleryDTO data)
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgIVRMGalleryDTO deactive(ClgIVRMGalleryDTO data)
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

                var resultIGC = _PortalContext.IVRM_Gallery_ProgramsDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                var resultIGP = _PortalContext.IVRM_Gallery_PhotosDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                var resultIGV = _PortalContext.IVRM_Gallery_VideosDMO.Where(t => t.IGA_Id == data.IGA_Id).ToList();
                foreach (var igc in resultIGC)
                {
                    if (result.IGA_ActiveFlag == true)
                    {
                        igc.IGAPRG_ActiveFlag = true;
                    }
                    else if (result.IGA_ActiveFlag == false)
                    {
                        igc.IGAPRG_ActiveFlag = false;
                    }
                    igc.IGAPRG_UpdatedDate = DateTime.Now;
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

    }
}
