using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class MasterCompetitionCategoryImpl:Interfaces.MasterCompetitionCategoryInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterCompetitionCategoryImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterCompetitionCategoryDTO getDetails(MasterCompetitionCategoryDTO data)
        {
            try
            {
                var category = _context.MasterCompitionCategoryDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                if (category.Count > 0)
                {
                    data.competitionCategoryList = category.OrderByDescending(t=>t.SPCCMCC_Id).ToArray();
                    data.count = category.Count;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MasterCompetitionCategoryDTO saveRecord(MasterCompetitionCategoryDTO obj)
        {
            try
            {
                if (obj.SPCCMCC_Id == 0)
                {
                    var checkduplicate = _context.MasterCompitionCategoryDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMCC_CompitionCategory.Equals(obj.SPCCMCC_CompitionCategory)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterCompitionCategoryDMO>(obj);
                        mapp.SPCCMCC_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        _context.Add(mapp);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }
                else if (obj.SPCCMCC_Id > 0)
                {
                    var checkduplicate = _context.MasterCompitionCategoryDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMCC_CompitionCategory.Equals(obj.SPCCMCC_CompitionCategory) && d.SPCCMCC_Id != obj.SPCCMCC_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterCompitionCategoryDMO.Where(d => d.SPCCMCC_Id == obj.SPCCMCC_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterCompitionCategoryDMO.Single(d => d.SPCCMCC_Id == obj.SPCCMCC_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMCC_FromCCAgeDays = obj.SPCCMCC_FromCCAgeDays;
                            update.SPCCMCC_CCAgeFlag = obj.SPCCMCC_CCAgeFlag;
                            update.SPCCMCC_FromCCAgeMonth = obj.SPCCMCC_FromCCAgeMonth;
                            update.SPCCMCC_FromCCAgeYear = obj.SPCCMCC_FromCCAgeYear;
                            update.SPCCMCC_CCDesc = obj.SPCCMCC_CCDesc;
                            update.SPCCMCC_CCWeight = obj.SPCCMCC_CCWeight;
                            update.SPCCMCC_CCWeightFlag = obj.SPCCMCC_CCWeightFlag;
                            update.SPCCMCC_CompitionCategory = obj.SPCCMCC_CompitionCategory;
                            update.SPCCMCC_ToCCAgeYear = obj.SPCCMCC_ToCCAgeYear;
                            update.SPCCMCC_ToCCAgeMonth = obj.SPCCMCC_ToCCAgeMonth;
                            update.SPCCMCC_ToCCAgeDays = obj.SPCCMCC_ToCCAgeDays;
                            _context.Update(update);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "updated";
                            }
                            else
                            {
                                obj.returnVal = "updateFailed";
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public MasterCompetitionCategoryDTO EditDetails(int id)
        {
            MasterCompetitionCategoryDTO resp = new MasterCompetitionCategoryDTO();
            try
            {
                resp.editDetails = _context.MasterCompitionCategoryDMO.Where(d => d.SPCCMCC_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterCompetitionCategoryDTO deactivate(MasterCompetitionCategoryDTO obj)
        {
            try
            {
               
                    var update = _context.MasterCompitionCategoryDMO.Single(d => d.SPCCMCC_Id == obj.SPCCMCC_Id);
                  

                if (update.SPCCMCC_ActiveFlag == true)
                {
                    update.SPCCMCC_ActiveFlag = false;
                }
                else if (update.SPCCMCC_ActiveFlag == false)
                {
                    update.SPCCMCC_ActiveFlag = true;
                }
                update.UpdatedDate = DateTime.Now;
                _context.Update(update);
                    int s = _context.SaveChanges();
                    if (s > 0)
                    {
                        obj.retval = true;
                    }
                    else
                    {
                        obj.retval =false;
                    }
                }
         
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
