using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterDocumentMappingImp : Interfaces.MasterDocumentMappingInterface
    {
        private static ConcurrentDictionary<string, MasterDocumentMappingDTO> _login =
      new ConcurrentDictionary<string, MasterDocumentMappingDTO>();

        private readonly MasterDocumentMappingContext _MasterDocumentMappingContext;


        public MasterDocumentMappingImp(MasterDocumentMappingContext MasterDocumentMappingContext)
        {
            _MasterDocumentMappingContext = MasterDocumentMappingContext;
        }
         
        public MasterDocumentMappingDTO Getdetails(MasterDocumentMappingDTO MasterDocumentMappingDTO)//int IVRMM_Id
        {

            List<MasterDocumentDMO> values = new List<MasterDocumentDMO>();
            values = _MasterDocumentMappingContext.MasterDocumentDMO.ToList();
            MasterDocumentMappingDTO.DocumentList = values.ToArray();

            List<MasterCategory> values1 = new List<MasterCategory>();
            values1 = _MasterDocumentMappingContext.MasterCategory.ToList();
            MasterDocumentMappingDTO.CategoryList = values1.ToArray();

            //MasterDocumentMappingDTO.GridviewDetails = (from DocCat in _MasterDocumentMappingContext.MasterDocumentMappingDMO
            //                                            from Doc in _MasterDocumentMappingContext.MasterDocumentDMO
            //                                            from Cat in _MasterDocumentMappingContext.MasterCategory

            //                                            where (DocCat.PASMD_Id == Doc.PASMD_Id && DocCat.AMC_Id == Cat.AMC_Id)
            //                                   select new MasterDocumentMappingDTO
            //                                   {
            //                                       CategoryName = Cat.AMC_Name,
            //                                       DocumentName = Doc.PASMD_DocumentName,
            //                                       PASCD_Id = DocCat.PASCD_Id,
            //                                       PASMD_Id = Doc.PASMD_Id,
            //                                       AMC_Id = Cat.AMC_Id
            //                                   }).ToArray();

            MasterDocumentMappingDTO.GridviewDetails = (from DocCat in _MasterDocumentMappingContext.MasterDocumentMappingDMO                                   
                                                        from Cat in _MasterDocumentMappingContext.MasterCategory

                                                        where (DocCat.AMC_Id == Cat.AMC_Id)  
                                                        select new MasterDocumentMappingDTO
                                                        {
                                                            CategoryName = Cat.AMC_Name,                                         
                                                            PASCD_Id = DocCat.PASCD_Id                  
                                                        }).ToArray();


            return MasterDocumentMappingDTO;
        }

        public MasterDocumentMappingDTO SaveData(MasterDocumentMappingDTO mas)
        {

       
            if (mas.PASCD_Id > 0)
            {

                var result = _MasterDocumentMappingContext.MasterDocumentMappingDMO.Single(t => t.AMSMD_Id.Equals(mas.PASMD_Id));

                for (int i = 0; i < mas.SelCategoryList.Count; i++)
                {
                    for (int j = 0; j < mas.SelDocumentList.Count; j++)
                    {
                        result.PASCD_Id = result.PASCD_Id;
                        result.AMSMD_Id = mas.SelDocumentList[j].AMSMD_Id;
                        result.MI_Id = result.MI_Id;
                        result.AMC_Id = mas.SelCategoryList[i].AMC_Id;

                        _MasterDocumentMappingContext.Update(result);
                        _MasterDocumentMappingContext.SaveChanges();
                    }
                }
            }
            else
            {
                for (int i = 0; i < mas.SelCategoryList.Count; i++)
                {
                    for (int j = 0; j < mas.SelDocumentList.Count; j++)
                    {
                        MasterDocumentMappingDMO MM2 = new MasterDocumentMappingDMO();

                        MM2.MI_Id = mas.MI_Id;
                        MM2.AMSMD_Id = mas.SelDocumentList[j].AMSMD_Id;
                        MM2.AMC_Id = mas.SelCategoryList[i].AMC_Id;

                        _MasterDocumentMappingContext.Add(MM2);
                        _MasterDocumentMappingContext.SaveChanges();
                    }
                }

            }

            return mas;
        }

        public MasterDocumentMappingDTO GetSelectedRowDetails(int ID)
        {
            MasterDocumentMappingDTO MasterDocumentMappingDTO = new MasterDocumentMappingDTO();

            //List<MasterDocumentMappingDMO> values = new List<MasterDocumentMappingDMO>();
            //values = _MasterDocumentMappingContext.MasterDocumentMappingDMO.Where(t => t.PASCD_Id.Equals(ID)).ToList().ToList();
            //MasterDocumentMappingDTO.SelectedRowDetails = values.ToArray();

            MasterDocumentMappingDTO.CategoryList = (from DocCat in _MasterDocumentMappingContext.MasterDocumentMappingDMO
                                                        from Cat in _MasterDocumentMappingContext.MasterCategory

                                                        where (DocCat.AMC_Id == Cat.AMC_Id)
                                                        select new MasterDocumentMappingDTO
                                                        {
                                                            CategoryName = Cat.AMC_Name,
                                                            AMC_Id = Cat.AMC_Id
                                                        }).ToArray();

            MasterDocumentMappingDTO.DocumentList = (from DocCat in _MasterDocumentMappingContext.MasterDocumentMappingDMO
                                                        from Doc in _MasterDocumentMappingContext.MasterDocumentDMO

                                                        where (DocCat.AMSMD_Id == Doc.AMSMD_Id)
                                                        select new MasterDocumentMappingDTO
                                                        {
                                                            DocumentName = Doc.AMSMD_DocumentName,
                                                            PASMD_Id = Doc.AMSMD_Id
                                                        }).ToArray();

            return MasterDocumentMappingDTO;
        }

        public MasterDocumentMappingDTO DeleteEntry(int ID)
        {
            MasterDocumentMappingDTO MasterDocumentMappingDTO = new MasterDocumentMappingDTO();

            List<MasterDocumentMappingDMO> masters1 = new List<MasterDocumentMappingDMO>();
            masters1 = _MasterDocumentMappingContext.MasterDocumentMappingDMO.Where(t => t.PASCD_Id.Equals(ID)).ToList();

            if (masters1.Any())
            {
                for (int i = 0; i < masters1.Count; i++)
                {
                    _MasterDocumentMappingContext.Remove(masters1.ElementAt(i));
                    _MasterDocumentMappingContext.SaveChanges();
                }
           
            }
            else
            {

            }

            return MasterDocumentMappingDTO;
        }

    }
}
