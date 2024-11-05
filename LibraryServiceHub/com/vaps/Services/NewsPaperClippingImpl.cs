using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class NewsPaperClippingImpl:Interfaces.NewsPaperClippingInterface
    {
        public LibraryContext _libContext;
        public DomainModelMsSqlServerContext _db;
        //public DomainModelMsSqlServerContext _db;
        public NewsPaperClippingImpl(LibraryContext pContext, DomainModelMsSqlServerContext db)
        {
            _libContext = pContext;
            _db = db;
        }

        public ImageClipping_DTO Getdetails(ImageClipping_DTO data)
        {
            try
            {
                data.alldata = _libContext.ImageClipping_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ImageClipping_DTO savedetail(ImageClipping_DTO data)
        {
            try
            {
                if (data.LNPCL_Id > 0)
                {
                    var ChkDuplicate = _libContext.ImageClipping_DMO.Where(t => t.LNPCL_Id!=data.LNPCL_Id && t.LNPCL_ClipName.Equals(data.LNPCL_ClipName) && t.MI_Id==data.MI_Id && t.LNPCL_ClipImage==data.LNPCL_ClipImage && t.LNPCL_ClipDetails==data.LNPCL_ClipDetails).ToList();
                    if (ChkDuplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var resultobj = _libContext.ImageClipping_DMO.Single(t => t.LNPCL_Id.Equals(data.LNPCL_Id) && t.MI_Id.Equals(data.MI_Id));

                        resultobj.MI_Id = data.MI_Id;
                        resultobj.LNPCL_ClipName = data.LNPCL_ClipName;
                        resultobj.LNPCL_ClipImage = data.LNPCL_ClipImage;
                        resultobj.LNPCL_ClipDetails = data.LNPCL_ClipDetails;                        
                        resultobj.UpdatedDate = DateTime.Now;
                        resultobj.UpdatedBy = data.UserId;

                        _libContext.Update(resultobj);
                        int returnval = _libContext.SaveChanges();
                        if (returnval > 0)
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
                    var ChkDuplicate = _libContext.ImageClipping_DMO.Where(t => t.LNPCL_ClipName.Equals(data.LNPCL_ClipName) && t.MI_Id == data.MI_Id && t.LNPCL_ClipImage == data.LNPCL_ClipImage && t.LNPCL_ClipDetails == data.LNPCL_ClipDetails).ToList();
                    if (ChkDuplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        ImageClipping_DMO obj = new ImageClipping_DMO();

                        obj.MI_Id = data.MI_Id;                        
                        obj.LNPCL_ClipName = data.LNPCL_ClipName;
                        obj.LNPCL_ClipImage = data.LNPCL_ClipImage;
                        obj.LNPCL_FilePath = data.LNPCL_FilePath;
                        obj.LNPCL_ClipDetails = data.LNPCL_ClipDetails;                    
                        obj.LNPCL_ActiveFlg = true;
                        obj.UpdatedDate = DateTime.Now;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedBy = data.UserId;
                        obj.CreatedBy = data.UserId;
                        _libContext.Add(obj);

                        int returnval = _libContext.SaveChanges();

                        if (returnval > 0)
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

        public ImageClipping_DTO editdetails(ImageClipping_DTO data)
        {
            try
            {
                var edit = (from a in _libContext.ImageClipping_DMO
                            where (a.LNPCL_Id == data.LNPCL_Id && a.MI_Id == data.MI_Id)
                            select new ImageClipping_DTO
                            {
                                LNPCL_Id = a.LNPCL_Id,
                                LNPCL_ClipName = a.LNPCL_ClipName,
                                LNPCL_ClipDetails = a.LNPCL_ClipDetails,
                                LNPCL_ClipImage = a.LNPCL_ClipImage,
                                LNPCL_ActiveFlg = a.LNPCL_ActiveFlg,
                                LNPCL_FilePath = a.LNPCL_FilePath,

                            }).Distinct().OrderBy(t => t.LNPCL_Id).ToArray();
                if (edit.Length > 0)
                {
                    data.editdetails = edit;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ImageClipping_DTO deactivate(ImageClipping_DTO data)
        {
            try
            {
                var query = _libContext.ImageClipping_DMO.Single(s => s.MI_Id == data.MI_Id && s.LNPCL_Id == data.LNPCL_Id);

                if (query.LNPCL_ActiveFlg == true)
                {
                    query.LNPCL_ActiveFlg = false;
                }
                else
                {
                    query.LNPCL_ActiveFlg = true;
                }
                query.UpdatedDate = DateTime.Now;
                _libContext.Update(query);
                var contactExists = _libContext.SaveChanges();
                if (contactExists > 0)
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
