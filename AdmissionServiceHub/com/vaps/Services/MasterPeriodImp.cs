using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterPeriodImp : Interfaces.MasterPeriodInterface
    {
        private readonly MasterPeriodContext _MasterPeriodContext;
        public MasterPeriodImp(MasterPeriodContext MasterPeriodContext)
        {
            _MasterPeriodContext = MasterPeriodContext;
        }
        public MasterPeriodDTO GetMasterPeriodData(MasterPeriodDTO MasterPeriodDTO)//int IVRMM_Id
        {
            List<MasterCategory> allcategory = new List<MasterCategory>();
            allcategory = _MasterPeriodContext.MasterCategory.Where(d => d.MI_Id == MasterPeriodDTO.MI_Id && d.AMC_ActiveFlag == 1).ToList();
            MasterPeriodDTO.categorydropDown = allcategory.ToArray();

            MasterPeriodDTO.GridviewDetails = (from sp in _MasterPeriodContext.MasterPeriodDMO
                                               from cp in _MasterPeriodContext.MasterPeriodCategoryDMO
                                               from cn in _MasterPeriodContext.MasterCategory
                                               where (sp.IMP_Id == cp.IMP_Id && cp.AMC_Id == cn.AMC_Id && sp.MI_Id == MasterPeriodDTO.MI_Id)
                                               select new MasterPeriodDTO
                                               {
                                                   CategoryName = cn.AMC_Name,
                                                   IMP_PeriodName = sp.IMP_PeriodName,
                                                   IMP_PeriodOrder = sp.IMP_PeriodOrder,
                                                   Half = cp.IMPCM_PeriodFlag,
                                                   IMP_Id = sp.IMP_Id,
                                                   IMPCM_Id = cp.IMPCM_Id
                                               }).ToArray();
            return MasterPeriodDTO;
        }

        public MasterPeriodDTO SaveData(MasterPeriodDTO mas)
        {
            try
            {
                List<MasterPeriodDMO> Allname1 = new List<MasterPeriodDMO>();
                Allname1 = _MasterPeriodContext.MasterPeriodDMO.Where(t => t.MI_Id == mas.MI_Id && t.IMP_PeriodName.Equals(mas.IMP_PeriodName) && t.IMP_PeriodOrder == mas.IMP_PeriodOrder).ToList();
                if (Allname1.Count > 0)
                {

                    for (int i = 0; i < mas.SelectedCategoryDetails.Count; i++)
                    {
                        List<MasterPeriodCategoryDMO> Allname2 = new List<MasterPeriodCategoryDMO>();
                        Allname2 = _MasterPeriodContext.MasterPeriodCategoryDMO.Where(t => t.IMP_Id == Allname1[0].IMP_Id && t.AMC_Id == mas.SelectedCategoryDetails[i].AMC_Id && t.IMPCM_PeriodFlag.Equals(mas.Half)).ToList();
                        if (Allname2.Count > 0)
                        {
                            mas.message = "Some Duplicate record exist......!!";
                        }
                        else
                        {
                            var result = _MasterPeriodContext.MasterPeriodCategoryDMO.Single(t => t.IMP_Id == Allname1[0].IMP_Id &&
                                t.AMC_Id == mas.SelectedCategoryDetails[i].AMC_Id);
                            result.IMPCM_Id = result.IMPCM_Id;
                            result.IMP_Id = result.IMP_Id;
                            result.AMC_Id = mas.SelectedCategoryDetails[i].AMC_Id;
                            result.IMPCM_PeriodFlag = mas.Half;
                            result.UpdatedDate = DateTime.Now;
                            _MasterPeriodContext.Update(result);
                            int n = _MasterPeriodContext.SaveChanges();
                            if (n > 0)
                            {
                                mas.messageupdate = "Update";
                                mas.returnVal = true;
                            }
                            else
                            {
                                mas.messageupdate = "Update";
                                mas.returnVal = false;
                            }
                        }
                    }
                }
                else
                {
                    MasterPeriodDMO MM2 = new MasterPeriodDMO
                    {
                        MI_Id = mas.MI_Id,
                        IMP_PeriodName = mas.IMP_PeriodName,
                        IMP_PeriodOrder = mas.IMP_PeriodOrder,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    _MasterPeriodContext.Add(MM2);
                    _MasterPeriodContext.SaveChanges();

                    for (int i = 0; i < mas.SelectedCategoryDetails.Count; i++)
                    {
                        MasterPeriodCategoryDMO MM3 = new MasterPeriodCategoryDMO();

                        MM3.IMP_Id = MM2.IMP_Id;
                        MM3.AMC_Id = mas.SelectedCategoryDetails[i].AMC_Id;
                        MM3.IMPCM_PeriodFlag = mas.Half;
                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        _MasterPeriodContext.Add(MM3);
                        _MasterPeriodContext.SaveChanges();
                    }

                }

                return mas;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mas.message = " Failed To Save.Already exist.....!";
            }
            return mas;
        }
        public MasterPeriodDTO GetSelectedRowDetails(MasterPeriodDTO dto)
        {

            MasterPeriodDTO MasterPeriodDTO = new MasterPeriodDTO();
            var amcid = _MasterPeriodContext.MasterPeriodCategoryDMO.Where(d => d.IMPCM_Id == dto.IMPCM_Id).ToList();
            List<MasterCategory> allcategory = new List<MasterCategory>();
            allcategory = _MasterPeriodContext.MasterCategory.Where(s => s.AMC_Id == amcid.FirstOrDefault().AMC_Id).ToList();
            MasterPeriodDTO.categorydropDown = allcategory.ToArray();

            MasterPeriodDTO.GridviewDetails = (from sp in _MasterPeriodContext.MasterPeriodDMO
                                               from cp in _MasterPeriodContext.MasterPeriodCategoryDMO
                                               from cn in _MasterPeriodContext.MasterCategory
                                               where (sp.IMP_Id == cp.IMP_Id && cp.AMC_Id == cn.AMC_Id && sp.IMP_Id == dto.IMP_Id)
                                               select new MasterPeriodDTO
                                               {

                                                   CategoryName = cn.AMC_Name,
                                                   IMP_PeriodName = sp.IMP_PeriodName,
                                                   IMP_PeriodOrder = sp.IMP_PeriodOrder,
                                                   Half = cp.IMPCM_PeriodFlag,
                                                   IMP_Id = sp.IMP_Id,
                                                   AMC_Id = cn.AMC_Id
                                               }).ToArray();
            return MasterPeriodDTO;
        }
        public MasterPeriodDTO DeleteEntry(int ID)
        {
            //MasterPeriodCategoryDTO MasterPeriodCategoryDTO = new MasterPeriodCategoryDTO();
            List<MasterPeriodCategoryDMO> masters = new List<MasterPeriodCategoryDMO>();
            masters = _MasterPeriodContext.MasterPeriodCategoryDMO.Where(t => t.IMP_Id.Equals(ID)).ToList();

            if (masters.Any())
            {
                for (int i = 0; i < masters.Count; i++)
                {
                    _MasterPeriodContext.Remove(masters.ElementAt(i));
                    _MasterPeriodContext.SaveChanges();
                }
            }
            MasterPeriodDTO MasterPeriodDTO = new MasterPeriodDTO();
            List<MasterPeriodDMO> masters1 = new List<MasterPeriodDMO>();
            masters1 = _MasterPeriodContext.MasterPeriodDMO.Where(t => t.IMP_Id == ID).ToList();

            if (masters.Any())
            {
                _MasterPeriodContext.Remove(masters1.ElementAt(0));
                _MasterPeriodContext.SaveChanges();
            }
            return MasterPeriodDTO;
        }
    }
}