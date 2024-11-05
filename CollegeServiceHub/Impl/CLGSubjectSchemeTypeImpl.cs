using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class CLGSubjectSchemeTypeImpl : Interface.CLGSubjectSchemeTypeInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        public CLGSubjectSchemeTypeImpl(ClgAdmissionContext ClgAdmissionContext)
        {
            _ClgAdmissionContext = ClgAdmissionContext;

        }
        public AdmCollegeSchemeTypeDTO savename(AdmCollegeSchemeTypeDTO data)
        {
            try
            {
                if (data.ACSS_Id > 0)
                {
                    var query = _ClgAdmissionContext.AdmCollegeSubjectSchemeDMO.Single(d => d.ACSS_Id == data.ACSS_Id);

                    query.ACSS_SchmeName = data.ACSS_SchmeName;
                    query.MI_Id = data.MI_Id;
                    query.ACST_ActiveFlg = query.ACST_ActiveFlg;
                    query.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
                else
                {
                    var query01 = _ClgAdmissionContext.AdmCollegeSubjectSchemeDMO.Where(q => q.ACSS_SchmeName == data.ACSS_SchmeName && q.MI_Id == data.MI_Id && q.ACST_ActiveFlg == data.ACST_ActiveFlg).ToArray();
                    if (query01.Length > 0)
                    {
                        data.returnval = false;
                        data.message = "Duplicate";
                    }
                    else
                    {
                        AdmCollegeSubjectSchemeDMO obj = new AdmCollegeSubjectSchemeDMO();
                        obj.ACSS_SchmeName = data.ACSS_SchmeName;
                        obj.ACST_ActiveFlg = true;
                        obj.MI_Id = data.MI_Id;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj);
                        _ClgAdmissionContext.SaveChanges();
                        data.returnval = true;
                        data.message = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmCollegeSchemeTypeDTO savetype(AdmCollegeSchemeTypeDTO data)
        {
            try
            {
                if (data.ACST_Id > 0)
                {
                    var checkquery = _ClgAdmissionContext.AdmCollegeSchemeTypeDMO.Where(q => q.ACST_SchmeType == data.ACST_SchmeType
                      && q.MI_Id == data.MI_Id && q.ACST_Id != data.ACST_Id).ToArray();


                    if (checkquery.Length > 0)
                    {
                        data.returnval = false;
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var query = _ClgAdmissionContext.AdmCollegeSchemeTypeDMO.Single(d => d.ACST_Id == data.ACST_Id);

                        query.ACST_SchmeType = data.ACST_SchmeType;
                        query.MI_Id = data.MI_Id;
                        query.ACST_ActiveFlg = query.ACST_ActiveFlg;
                        query.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(query);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.message = "Update";
                        }
                        else
                        {
                            data.message = "";
                        }
                    }
                }
                else
                {
                    var query01 = _ClgAdmissionContext.AdmCollegeSchemeTypeDMO.Where(q => q.ACST_SchmeType == data.ACST_SchmeType
                    && q.MI_Id == data.MI_Id).ToArray();
                    if (query01.Length > 0)
                    {
                        data.returnval = false;
                        data.message = "Duplicate";
                    }
                    else
                    {
                        AdmCollegeSchemeTypeDMO obj = new AdmCollegeSchemeTypeDMO();
                        obj.ACST_SchmeType = data.ACST_SchmeType;
                        obj.ACST_ActiveFlg = true;
                        obj.MI_Id = data.MI_Id;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj);
                        _ClgAdmissionContext.SaveChanges();
                        data.returnval = true;
                        data.message = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmCollegeSchemeTypeDTO getschema(AdmCollegeSchemeTypeDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.AdmCollegeSchemeTypeDMO.Where(q => q.MI_Id == data.MI_Id).ToArray();
                if (query.Length > 0)
                {
                    data.schelist = query;
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
        public AdmCollegeSchemeTypeDTO getsubject(AdmCollegeSchemeTypeDTO data)
        {
            var query = _ClgAdmissionContext.AdmCollegeSubjectSchemeDMO.Where(q => q.MI_Id == data.MI_Id).ToArray();
            if (query.Length > 0)
            {
                data.sublist = query;
                data.returnval = true;
            }
            else
            {
                data.returnval = false;
            }
            return data;
        }
        public AdmCollegeSchemeTypeDTO activedeactivebatch1(AdmCollegeSchemeTypeDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.AdmCollegeSubjectSchemeDMO.Single(t => t.MI_Id == data.MI_Id && t.ACSS_Id == data.ACSS_Id);
                if (query.ACST_ActiveFlg == true)
                {
                    query.ACST_ActiveFlg = false;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Scheme De-Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }
                }
                else
                {
                    query.ACST_ActiveFlg = true;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Scheme Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public AdmCollegeSchemeTypeDTO activedeactivebatch(AdmCollegeSchemeTypeDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.AdmCollegeSchemeTypeDMO.Single(t => t.MI_Id == data.MI_Id && t.ACST_Id == data.ACST_Id);
                if (query.ACST_ActiveFlg == true)
                {
                    query.ACST_ActiveFlg = false;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Scheme Type De-Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }
                }
                else
                {
                    query.ACST_ActiveFlg = true;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Scheme Type Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Adm_Prv_Sch_CombinationDTO getcombination(Adm_Prv_Sch_CombinationDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.Adm_Prv_Sch_CombinationDMO.Where(q => q.MI_Id == data.MI_Id).ToArray();
                if (query.Length > 0)
                {
                    data.getdetails = query;
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
        public Adm_Prv_Sch_CombinationDTO savecombination(Adm_Prv_Sch_CombinationDTO data)
        {
            try
            {
                if (data.ADMCB_ID > 0)
                {
                    var checkduplicate = _ClgAdmissionContext.Adm_Prv_Sch_CombinationDMO.Where(a => a.MI_Id == data.MI_Id && a.ADMCB_NAME == data.ADMCB_NAME && a.ADMCB_ID != data.ADMCB_ID).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _ClgAdmissionContext.Adm_Prv_Sch_CombinationDMO.Single(a => a.MI_Id == data.MI_Id && a.ADMCB_ID == data.ADMCB_ID);
                        result.ADMCB_NAME = data.ADMCB_NAME;
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var i = _ClgAdmissionContext.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Update";
                        }
                        else
                        {
                            data.message = "UpdateF";
                        }
                    }
                }
                else
                {
                    var checkduplicate = _ClgAdmissionContext.Adm_Prv_Sch_CombinationDMO.Where(a => a.MI_Id == data.MI_Id && a.ADMCB_NAME == data.ADMCB_NAME).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Adm_Prv_Sch_CombinationDMO prv = new Adm_Prv_Sch_CombinationDMO();
                        prv.MI_Id = data.MI_Id;
                        prv.ADMCB_NAME = data.ADMCB_NAME;
                        prv.ADMCB_Activeflag = true;
                        prv.CreatedDate = DateTime.Now;
                        prv.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(prv);
                        var i = _ClgAdmissionContext.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "AddF";
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
        public Adm_Prv_Sch_CombinationDTO activedeactivecomb(Adm_Prv_Sch_CombinationDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.Adm_Prv_Sch_CombinationDMO.Single(t => t.MI_Id == data.MI_Id && t.ADMCB_ID == data.ADMCB_ID);
                if (query.ADMCB_Activeflag == true)
                {
                    query.ADMCB_Activeflag = false;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Combination De-Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }
                }
                else
                {
                    query.ADMCB_Activeflag = true;
                    _ClgAdmissionContext.Update(query);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag > 0)
                    {
                        var msg = "Combination Activated Successfully";
                        data.message = msg;
                    }
                    else
                    {
                        var msg = "failed";
                        data.message = msg;
                    }

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
