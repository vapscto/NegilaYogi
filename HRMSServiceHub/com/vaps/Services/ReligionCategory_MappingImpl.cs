using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class ReligionCategory_MappingImpl:Interfaces.ReligionCategory_MappingInterface
    {
        public AdmissionFormContext _AdmissionFormContext;
        public ReligionCategory_MappingImpl(AdmissionFormContext hh)
        {
            _AdmissionFormContext = hh;
        }
        public ReligionCategory_MappingDTO loaddata(ReligionCategory_MappingDTO data)
        {
            try
            {
                data.religion_list = _AdmissionFormContext.Religion.Where(t => t.Is_Active == true).Distinct().ToArray();

                data.caste_list = _AdmissionFormContext.CasteCategory.ToArray();

                data.get_masterlist = (from a in _AdmissionFormContext.Religion
                                       from b in _AdmissionFormContext.CasteCategory
                                       from c in _AdmissionFormContext.ReligionCategory_MappingDMO
                                       where (a.IVRMMR_Id == c.IVRMMR_Id && b.IMCC_Id == c.IMCC_Id)
                                       select new ReligionCategory_MappingDTO
                                       {
                                           IRCC_Id = c.IRCC_Id,
                                           IVRMMR_Id = a.IVRMMR_Id,
                                           IVRMMR_Name = a.IVRMMR_Name,
                                           IMCC_Id = b.IMCC_Id,
                                           IMCC_CategoryName = b.IMCC_CategoryName,
                                           IRCC_ActiveFlg = c.IRCC_ActiveFlg
                                       }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }        
        public ReligionCategory_MappingDTO savedata(ReligionCategory_MappingDTO data)
        {
            try
            {
                if (data.IRCC_Id == 0)
                {
                    for (int i = 0; i < data.castid.Length; i++)
                    {
                        var tempdata = data.castid[i].IMCC_Id;
                        {                           
                            var duplicate = _AdmissionFormContext.ReligionCategory_MappingDMO.Where(t => t.IRCC_Id == data.castid[i].IMCC_Id && t.IVRMMR_Id == data.IVRMMR_Id).ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                ReligionCategory_MappingDMO rrr = new ReligionCategory_MappingDMO();
                                rrr.IRCC_Id = data.IRCC_Id;                              
                                rrr.IVRMMR_Id = data.IVRMMR_Id;                               
                                rrr.IMCC_Id = tempdata;
                                rrr.IRCC_CreatedDate = DateTime.Now;
                                rrr.IRCC_UpdatedDate = DateTime.Now;
                                rrr.IRCC_CreatedBy = data.UserId;
                                rrr.IRCC_UpdatedBy = data.UserId;
                                rrr.IRCC_ActiveFlg = true;
                                _AdmissionFormContext.Add(rrr);
                                int y = _AdmissionFormContext.SaveChanges();
                                if (y > 0)
                                {
                                    data.msg = "Saved";
                                }
                                else
                                {
                                    data.msg = "Failed";
                                }
                            }
                        }
                    }
                }
                else if (data.IRCC_Id > 0)
                {
                    var duplicate = _AdmissionFormContext.ReligionCategory_MappingDMO.Where(t => t.IRCC_Id != data.IRCC_Id &&  t.IVRMMR_Id == data.IVRMMR_Id && t.IMCC_Id==data.IMCC_Id).ToArray();

                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _AdmissionFormContext.ReligionCategory_MappingDMO.Where(t => t.IRCC_Id == data.IRCC_Id).SingleOrDefault();
                                         
                        yy.IVRMMR_Id = data.IVRMMR_Id;
                        yy.IMCC_Id = data.castid[0].IMCC_Id;                       
                        yy.IRCC_UpdatedDate = DateTime.Now;                       
                        yy.IRCC_UpdatedBy = data.UserId;
                        _AdmissionFormContext.Update(yy);
                        int r = _AdmissionFormContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "Updated";

                        }
                        else
                        {
                            data.msg = "Update Failed";
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
        //===================Edit
        public ReligionCategory_MappingDTO Editdata(ReligionCategory_MappingDTO data)
        {
            try
            {
               
                var editlist = (from a in _AdmissionFormContext.CasteCategory
                                from b in _AdmissionFormContext.ReligionCategory_MappingDMO
                                where (b.IRCC_Id == data.IRCC_Id && a.IMCC_Id == b.IMCC_Id)
                                select new ReligionCategory_MappingDTO
                                {
                                    IRCC_Id = b.IRCC_Id,
                                    IVRMMR_Id = b.IVRMMR_Id,
                                    IMCC_Id = b.IMCC_Id
                                }).Distinct().ToList();

                data.editlist = editlist.ToArray();
               
                data.caste_list = (from a in _AdmissionFormContext.CasteCategory
                                    from b in _AdmissionFormContext.ReligionCategory_MappingDMO                                  
                                    where (a.IMCC_Id == b.IMCC_Id && b.IRCC_Id == data.IRCC_Id)
                                    select new ReligionCategory_MappingDTO
                                    {
                                        IMCC_Id = b.IMCC_Id,
                                        IMCC_CategoryName = a.IMCC_CategoryName,
                                    }).Distinct().OrderBy(b => b.IMCC_Id).ToArray();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //=========================active/deactive()===========================//
        public ReligionCategory_MappingDTO masterDecative(ReligionCategory_MappingDTO data)
        {
            try
            {
                var u = _AdmissionFormContext.ReligionCategory_MappingDMO.Where(t => t.IRCC_Id == data.IRCC_Id).SingleOrDefault();
                if (u.IRCC_ActiveFlg == true)
                {
                    u.IRCC_ActiveFlg = false;
                }
                else if (u.IRCC_ActiveFlg == false)
                {
                    u.IRCC_ActiveFlg = true;
                }

                _AdmissionFormContext.Update(u);
                int o = _AdmissionFormContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
