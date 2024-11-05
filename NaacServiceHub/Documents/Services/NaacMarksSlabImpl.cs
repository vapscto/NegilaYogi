using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Documents;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Services
{
    public class NaacMarksSlabImpl : Interface.NaacMarksSlabInterface
    {
        public GeneralContext _GeneralContext;
        public NaacMarksSlabImpl(GeneralContext _conte)
        {
            _GeneralContext = _conte;
        }

        public NAAC_AC_Criteria_MarksSlab_DTO Getdetails(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            try
            {
                data.criterialist = (from a in _GeneralContext.NaacDocumentUploadDMO
                                     where (a.NAACSL_ActiveFlag == true)
                                     select new NAAC_AC_Criteria_MarksSlab_DTO
                                     {
                                         NAACSL_Id = a.NAACSL_Id,
                                         NAACSL_SLNo = a.NAACSL_SLNo,
                                         NAACSL_SLNoDescription = a.NAACSL_SLNoDescription,
                                     }).Distinct().ToArray();

                data.griddata = (from a in _GeneralContext.NaacDocumentUploadDMO
                                 from b in _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO
                                 where (a.NAACSL_Id == b.NAACSL_Id && b.MI_Id == data.MI_Id)
                                 select new NAAC_AC_Criteria_MarksSlab_DTO
                                 {
                                     NAACSL_Id = a.NAACSL_Id,
                                     NAACSL_SLNo = a.NAACSL_SLNo,
                                     NAACSL_SLNoDescription = a.NAACSL_SLNoDescription,
                                     NCACCRMRSLB_Id = b.NCACCRMRSLB_Id,
                                     NCACCRMRSLB_FromSlab = b.NCACCRMRSLB_FromSlab,
                                     NCACCRMRSLB_ToSlab = b.NCACCRMRSLB_ToSlab,
                                     NCACCRMRSLB_Marks = b.NCACCRMRSLB_Marks,
                                     NCACCRMRSLB_ActiveFlg = b.NCACCRMRSLB_ActiveFlg,
                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Criteria_MarksSlab_DTO savedata(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            try
            {
                if (data.NCACCRMRSLB_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO.Where(t => t.MI_Id == data.MI_Id && t.NAACSL_Id == data.NAACSL_Id && t.NCACCRMRSLB_FromSlab == data.NCACCRMRSLB_FromSlab && t.NCACCRMRSLB_ToSlab == data.NCACCRMRSLB_ToSlab && t.NCACCRMRSLB_Marks == data.NCACCRMRSLB_Marks).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_Criteria_MarksSlab_DMO obj1 = new NAAC_AC_Criteria_MarksSlab_DMO();
                        obj1.NCACCRMRSLB_Id = data.NCACCRMRSLB_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.MT_Id = 0;
                        obj1.NAACSL_Id = data.NAACSL_Id;
                        obj1.NCACCRMRSLB_FromSlab = data.NCACCRMRSLB_FromSlab;
                        obj1.NCACCRMRSLB_ToSlab = data.NCACCRMRSLB_ToSlab;
                        obj1.NCACCRMRSLB_Marks = data.NCACCRMRSLB_Marks;
                        obj1.NCACCRMRSLB_ActiveFlg = true;
                        obj1.NCACCRMRSLB_CreatedBy = data.UserId;
                        obj1.NCACCRMRSLB_UpdatedBy = data.UserId;
                        obj1.NCACCRMRSLB_CreatedDate = DateTime.Now;
                        obj1.NCACCRMRSLB_UpdatedDate = DateTime.Now;

                        _GeneralContext.Add(obj1);
                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
                else if (data.NCACCRMRSLB_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCRMRSLB_Id != data.NCACCRMRSLB_Id && t.NAACSL_Id == data.NAACSL_Id && t.NCACCRMRSLB_FromSlab == data.NCACCRMRSLB_FromSlab && t.NCACCRMRSLB_ToSlab == data.NCACCRMRSLB_ToSlab && t.NCACCRMRSLB_Marks == data.NCACCRMRSLB_Marks).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO.Single(t => t.NCACCRMRSLB_Id == data.NCACCRMRSLB_Id && t.MI_Id == data.MI_Id);

                        update.NAACSL_Id = data.NAACSL_Id;
                        update.NCACCRMRSLB_FromSlab = data.NCACCRMRSLB_FromSlab;
                        update.NCACCRMRSLB_ToSlab = data.NCACCRMRSLB_ToSlab;
                        update.NCACCRMRSLB_Marks = data.NCACCRMRSLB_Marks;
                        update.NCACCRMRSLB_UpdatedBy = data.UserId;
                        update.NCACCRMRSLB_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);
                        int row = _GeneralContext.SaveChanges();

                        if (row > 0)
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
        public NAAC_AC_Criteria_MarksSlab_DTO editdata(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            try
            {
                data.editlist = (from a in _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO
                                 from b in _GeneralContext.NaacDocumentUploadDMO

                                 where (a.MI_Id == data.MI_Id && a.NCACCRMRSLB_Id == data.NCACCRMRSLB_Id && b.NAACSL_Id==a.NAACSL_Id)
                                 select new NAAC_AC_Criteria_MarksSlab_DTO {
                                     NCACCRMRSLB_Id=a.NCACCRMRSLB_Id,
                                     NAACSL_Id = a.NAACSL_Id,
                                     NCACCRMRSLB_FromSlab = a.NCACCRMRSLB_FromSlab,
                                     NCACCRMRSLB_ToSlab = a.NCACCRMRSLB_ToSlab,
                                     NCACCRMRSLB_Marks = a.NCACCRMRSLB_Marks,
                                     NCACCRMRSLB_ActiveFlg = a.NCACCRMRSLB_ActiveFlg,
                                     NAACSL_SLNo = b.NAACSL_SLNo,
                                     NAACSL_SLNoDescription = b.NAACSL_SLNoDescription,
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Criteria_MarksSlab_DTO deactive(NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_Criteria_MarksSlab_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCRMRSLB_Id == data.NCACCRMRSLB_Id).Distinct().Single();
                if (result.NCACCRMRSLB_ActiveFlg == true)
                {
                    result.NCACCRMRSLB_ActiveFlg = false;
                }
                else if (result.NCACCRMRSLB_ActiveFlg == false)
                {
                    result.NCACCRMRSLB_ActiveFlg = true;
                }
                result.NCACCRMRSLB_UpdatedDate = DateTime.Now;
                result.NCACCRMRSLB_UpdatedBy = data.UserId;
                _GeneralContext.Update(result);

                int row = _GeneralContext.SaveChanges();
                if (row > 0)
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
