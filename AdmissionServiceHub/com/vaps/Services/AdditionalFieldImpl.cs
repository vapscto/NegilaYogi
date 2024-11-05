using System;
using System.Collections.Generic;
using System.Linq;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdditionalFieldImpl : AdditionalFieldInterface
    {
        public DomainModelMsSqlServerContext _db;
        public AdditionalFieldImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        public AdditionalFieldDTO Save(AdditionalFieldDTO cdto)
        {
            try
            {
                //var checklabelname = _db.additional_field_context.Where(t => t.IPAF_Name == cdto.IPAF_Name).ToList();
                //var checkdisplayname= _db.additional_field_context.Where(t => t.IPAF_Display_Name == cdto.IPAF_Display_Name).ToList();
                AdditionalField enq = Mapper.Map<AdditionalField>(cdto);
                if (enq.IPAF_Id == 0)
                {
                    var checklabelname = _db.additional_field_context.Where(t => t.IPAF_Name == cdto.IPAF_Name && t.MI_Id==cdto.MI_Id && t.Page_Id==cdto.Page_Id).ToList();
                    var checkdisplayname = _db.additional_field_context.Where(t => t.IPAF_Display_Name == cdto.IPAF_Display_Name && t.MI_Id == cdto.MI_Id && t.Page_Id == cdto.Page_Id).ToList();
                    if (checkdisplayname.Count == 0 && checklabelname.Count == 0)
                    {
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        _db.Add(enq);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            cdto.message = "Save";
                            cdto.returnval = true;
                        }
                        else
                        {
                            cdto.message = "Save";
                            cdto.returnval = false;
                        }
                    }
                    else
                    {
                        cdto.message = "Duplicate";
                        //cdto.returnval = false;
                    }
                }
                else
                {
                    var checklabelname1 = _db.additional_field_context.Where(t => t.IPAF_Name == cdto.IPAF_Name &&  t.IPAF_Id != cdto.IPAF_Id && t.MI_Id == cdto.MI_Id && t.Page_Id == cdto.Page_Id).ToList();
                    var checkdisplayname1 = _db.additional_field_context.Where(t => t.IPAF_Display_Name == cdto.IPAF_Display_Name && t.IPAF_Id != cdto.IPAF_Id && t.MI_Id == cdto.MI_Id && t.Page_Id == cdto.Page_Id).ToList();

                    if (checklabelname1.Count == 0 && checkdisplayname1.Count == 0)
                    {
                        var upd = _db.additional_field_context.Single(d => d.IPAF_Id == cdto.IPAF_Id);
                        upd.UpdatedDate = DateTime.Now;
                        if (upd.IPAF_Active_Flag == 0)
                        {
                            upd.IPAF_Active_Flag = 0;
                        }
                        else
                        {
                            upd.IPAF_Active_Flag = 1;
                        }
                        upd.IPAF_Apl_Report = cdto.IPAF_Apl_Report;
                        upd.IPAF_Display_Name = cdto.IPAF_Display_Name;
                        upd.IPAF_Flag = cdto.IPAF_Flag;
                        upd.IPAF_Name = cdto.IPAF_Name;
                        upd.IPAF_Scale = cdto.IPAF_Scale;
                        upd.IPAF_Size = cdto.IPAF_Size;
                        upd.IPAF_Type = cdto.IPAF_Type;
                        upd.Page_Id = cdto.Page_Id;

                        _db.Update(upd);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            cdto.message = "Update";
                            cdto.returnval = true;
                        }
                        else
                        {
                            cdto.message = "Update";
                            cdto.returnval = false;
                        }
                    }
                    else
                    {
                        cdto.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }


        public AdditionalFieldDTO getBasicData(int id)
        {
            AdditionalFieldDTO addf = new AdditionalFieldDTO();
            try
            {

                List<AdditionalField> allacademic = new List<AdditionalField>();
                allacademic = _db.additional_field_context.Where(t=>t.MI_Id==id).ToList();
                addf.AdditionalList = allacademic.ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return addf;
        }


        public AdditionalFieldDTO editData(int edit)
        {
            AdditionalFieldDTO edit_obj = new AdditionalFieldDTO();
            try
            {
                //List<AdditionalField> edit_list = new List<AdditionalField>();
                //var result = _db.additional_field_context.AsNoTracking().Where(t => t.IPAF_Id.Equals(edit)).ToList(); ;
                //edit_obj = Mapper.Map<AdditionalFieldDTO>(result);
                //edit_list.editingList = edit_obj.ToArray();


                List<AdditionalField> lorg = new List<AdditionalField>();
                lorg = _db.additional_field_context.AsNoTracking().Where(t => t.IPAF_Id.Equals(edit)).ToList();
                edit_obj.editingList = lorg.ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return edit_obj;
        }


        public void deactivate(int id)
        {
            try
            {

                if (id > 0)
                {
                    var result = _db.additional_field_context.Single(t => t.IPAF_Id == id);
                    if (result.IPAF_Active_Flag == 1)
                    {
                        result.IPAF_Active_Flag = 0;
                    }
                    else
                    {
                        result.IPAF_Active_Flag = 1;
                    }
                    _db.Update(result);
                    _db.SaveChanges();



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

        }
    }
}
