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
    public class Alumni_Gallery_Impl : Interface.Alumni_Gallery_Interface
    {
        public AlumniContext _AlumniContext;
        public Alumni_Gallery_Impl(AlumniContext AlumniContext)
        {
            _AlumniContext = AlumniContext;
        }
        public Alumni_GalleryDTO getloaddata(Alumni_GalleryDTO data)
        {
            try
            {
                if (data.Role_flag != "" && data.Role_flag != null)
                {
                    data.roletype = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToArray();
                }
                var rolet = _AlumniContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.roleflg = rolet.FirstOrDefault().IVRMRT_Role;
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Alumni_GalleryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = data.IVRMRT_Id });
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

                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Alumni_GalleryDTO savedata(Alumni_GalleryDTO data)
        {
            try
            {
                Alumni_Gallery_DMO img = new Alumni_Gallery_DMO();
                img.MI_Id = data.MI_Id;
                img.ALGA_GalleryName = data.ALGA_GalleryName;
                img.ALGA_Date = data.ALGA_Date;
                img.ALGA_CreatedBy = data.UserId;
                img.ALGA_UpdatedBy = data.UserId;
                img.ALGA_Time = data.ALGA_Time;
                img.ALGA_CommonGalleryFlg = "Alumni";
                img.ALGA_ActiveFlag = true;

                _AlumniContext.Add(img);

                if (data.images_list.Length > 0)
                {
                    if (data.mediatype == "I")
                    {
                        foreach (var ig in data.images_list)
                        {
                            Alumni_Gallery_Photos_DMO imgfile = new Alumni_Gallery_Photos_DMO();
                            imgfile.ALGA_Id = img.ALGA_Id;
                            imgfile.ALGAP_Photos = ig.FilePath;
                            imgfile.ALGAP_CoverPhotoFlag = "0";
                            imgfile.ALGAP_ActiveFlag = true;
                            _AlumniContext.Add(imgfile);
                        }
                    }
                    else
                    {
                        foreach (var ig in data.images_list)
                        {
                            Alumni_Gallery_Videos_DMO vidfile = new Alumni_Gallery_Videos_DMO();
                            vidfile.ALGA_Id = img.ALGA_Id;
                            vidfile.ALGAV_Videos = ig.FilePath;
                            vidfile.ALGAV_ActiveFlag = true;
                            _AlumniContext.Add(vidfile);
                        }
                    }
                }

                var contexttrans = _AlumniContext.SaveChanges();
                if (contexttrans > 0)
                {
                    data.ALGA_Id = img.ALGA_Id;
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
        public Alumni_GalleryDTO savecover(Alumni_GalleryDTO data)
        {
            try
            {
                //var coverflag = _AlumniContext.Alumni_Gallery_DMO_con.Where(u => u.ALGA_Id == data.ALGA_Id).ToList();
                //foreach (var item in coverflag)
                //{
                //    if (item.ALGAP_CoverPhotoFlag == true)
                //    {
                //        var coverflag1 = _AlumniContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGA_Id == item.IGA_Id && u.IGAP_Id == item.IGAP_Id);
                //        coverflag1.IGAP_CoverPhotoFlag = false;
                //        _AlumniContext.Update(coverflag1);
                //    }
                //}
                //var coverflag2 = _AlumniContext.IVRM_Gallery_PhotosDMO.Single(u => u.IGA_Id == data.IGA_Id && u.IGAP_Id == data.IGAP_Id);
                //coverflag2.IGAP_CoverPhotoFlag = true;
                //_AlumniContext.Update(coverflag2);
                //var contextupdate = _AlumniContext.SaveChanges();
                //if (contextupdate > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ex)
            {
                data.message = "Error";
            }
            return data;
        }
        public Alumni_GalleryDTO getcovermodel(Alumni_GalleryDTO data)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Alumni_GalleryFilesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ALGA_Id", SqlDbType.BigInt) { Value = data.ALGA_Id });
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
                            data.view_galleryimg = retObject.Distinct().ToArray();
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
        public Alumni_GalleryDTO deactive(Alumni_GalleryDTO data)
        {
            try
            {
                var result = _AlumniContext.Alumni_Gallery_DMO_con.Single(t => t.ALGA_Id == data.ALGA_Id);
                if (result.ALGA_ActiveFlag == true)
                {
                    result.ALGA_ActiveFlag = false;
                }
                else if (result.ALGA_ActiveFlag == false)
                {
                    result.ALGA_ActiveFlag = true;
                }
              
                _AlumniContext.Update(result);

               
                var resultIGP = _AlumniContext.Alumni_Gallery_Photos_DMO_con.Where(t => t.ALGAP_Id == data.ALGAP_Id).ToList();
                var resultIGV = _AlumniContext.Alumni_Gallery_Videos_DMO_con.Where(t => t.ALGAV_Id == data.ALGAV_Id).ToList();
              
                foreach (var igp in resultIGP)
                {
                    if (result.ALGA_ActiveFlag == true)
                    {
                        igp.ALGAP_ActiveFlag = true;
                    }
                    else if (result.ALGA_ActiveFlag == false)
                    {
                        igp.ALGAP_ActiveFlag = false;
                    }

                    _AlumniContext.Update(igp);
                }
                foreach (var igv in resultIGV)
                {
                    if (result.ALGA_ActiveFlag == true)
                    {
                        igv.ALGAV_ActiveFlag = true;
                    }
                    else if (result.ALGA_ActiveFlag == false)
                    {
                        igv.ALGAV_ActiveFlag = false;
                    }
                    
                    _AlumniContext.Update(igv);
                }


                int returnval = _AlumniContext.SaveChanges();
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
