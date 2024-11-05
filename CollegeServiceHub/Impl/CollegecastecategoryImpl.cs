using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegecastecategoryImpl : Interface.CollegecastecategoryInterface
    {
        private static ConcurrentDictionary<string, CollegecastecategoryDTO> _login =
      new ConcurrentDictionary<string, CollegecastecategoryDTO>();

        public ClgAdmissionContext _castecategoryContext;
        public CollegecastecategoryImpl(ClgAdmissionContext castecategoryContext)
        {
            _castecategoryContext = castecategoryContext;
        }

        public CollegecastecategoryDTO GetcastecategoryData(CollegecastecategoryDTO CollegecastecategoryDTO)//int IVRMM_Id
        {

            Array[] showdata = new Array[50];
            List<CollegecastecaegoryDMO> Allname = new List<CollegecastecaegoryDMO>();
            Allname = _castecategoryContext.CasteCategory.OrderByDescending(a => a.CreatedDate).ToList();
            CollegecastecategoryDTO.castecategoryname = Allname.ToArray();
            if (CollegecastecategoryDTO.castecategoryname.Length > 0)
            {
                CollegecastecategoryDTO.count = CollegecastecategoryDTO.castecategoryname.Length;
            }
            else
            {
                CollegecastecategoryDTO.count = 0;
            }
            return CollegecastecategoryDTO;
        }

        public CollegecastecategoryDTO GetSelectedRowDetails(int ID)
        {
            CollegecastecategoryDTO CollegecastecategoryDTO = new CollegecastecategoryDTO();
            List<CollegecastecaegoryDMO> lorg = new List<CollegecastecaegoryDMO>();
            lorg = _castecategoryContext.CasteCategory.Where(t => t.IMCC_Id.Equals(ID)).OrderByDescending(a => a.CreatedDate).ToList();
            CollegecastecategoryDTO.castecategoryname = lorg.ToArray();
            return CollegecastecategoryDTO;
        }

        public CollegecastecategoryDTO MasterDeleteModulesData(int ID)
        {
            CollegecastecategoryDTO CollegecastecategoryDTO = new CollegecastecategoryDTO();
            List<CollegecastecaegoryDMO> masters = new List<CollegecastecaegoryDMO>();
            try
            {
                var check_castecategory_used = _castecategoryContext.Adm_Master_College_StudentDMO.Where(t => t.IMCC_Id == ID).ToList();
                var check_castecategory_used1 = _castecategoryContext.Caste.Where(t => t.IMCC_Id == ID).ToList();

                if (check_castecategory_used.Count == 0 && check_castecategory_used1.Count == 0)
                {
                    masters = _castecategoryContext.CasteCategory.Where(t => t.IMCC_Id.Equals(ID)).ToList();
                    if (masters.Any())
                    {
                        _castecategoryContext.Remove(masters.ElementAt(0));
                        int n = _castecategoryContext.SaveChanges();
                        if (n > 0)
                        {
                            CollegecastecategoryDTO.returnVal = true;
                        }
                        else
                        {
                            CollegecastecategoryDTO.returnVal = false;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    CollegecastecategoryDTO.message = "Delete";
                }

                List<CollegecastecaegoryDMO> Allname = new List<CollegecastecaegoryDMO>();
                Allname = _castecategoryContext.CasteCategory.OrderByDescending(a => a.CreatedDate).ToList();
                CollegecastecategoryDTO.castecategoryname = Allname.ToArray();
                if (CollegecastecategoryDTO.castecategoryname.Length > 0)
                {
                    CollegecastecategoryDTO.count = CollegecastecategoryDTO.castecategoryname.Length;
                }
                else
                {
                    CollegecastecategoryDTO.count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CollegecastecategoryDTO.message = "Delete";
                //CollegecastecategoryDTO.message = "Sorry You Can Not Delete This Record. Because It Is Mapped To Student";
                List<CollegecastecaegoryDMO> Allname = new List<CollegecastecaegoryDMO>();
                Allname = _castecategoryContext.CasteCategory.OrderByDescending(a => a.CreatedDate).ToList();
                CollegecastecategoryDTO.castecategoryname = Allname.ToArray();
            }

            return CollegecastecategoryDTO;
        }

        public CollegecastecategoryDTO castecategoryData(CollegecastecategoryDTO mas)
        {

            CollegecastecaegoryDMO MM = Mapper.Map<CollegecastecaegoryDMO>(mas);
            if (mas.IMCC_Id != 0)
            {
                var checkDuplicates1 = _castecategoryContext.CasteCategory.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName) && d.IMCC_Id != mas.IMCC_Id).ToList();
                if (checkDuplicates1.Count > 0)
                {
                    var result = _castecategoryContext.CasteCategory.Single(t => t.IMCC_Id == mas.IMCC_Id);
                    //result.IVRMM_Id = mas.IVRMM_Id;
                    if (result.IMCC_CategoryDesc != mas.IMCC_CategoryDesc)
                    {
                        if (result.IMCC_CategoryName != mas.IMCC_CategoryName)
                        {
                            var dupCatName = _castecategoryContext.CasteCategory.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName) && d.IMCC_Id != mas.IMCC_Id).ToList();
                            if (dupCatName.Count > 0)
                            {
                                mas.message = "Caste Category Already Exists";
                            }
                            else
                            {
                                result.IMCC_CategoryName = mas.IMCC_CategoryName;
                                result.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                                result.CreatedDate = result.CreatedDate;
                                result.UpdatedDate = DateTime.Now;
                                _castecategoryContext.Update(result);
                                int n = _castecategoryContext.SaveChanges();
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
                        else
                        {
                            result.IMCC_CategoryName = mas.IMCC_CategoryName;
                            result.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                            result.CreatedDate = result.CreatedDate;
                            result.UpdatedDate = DateTime.Now;
                            _castecategoryContext.Update(result);
                            int n = _castecategoryContext.SaveChanges();
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
                    else
                    {
                        mas.message = "Caste Category Already Exists";
                    }
                }
                else
                {
                    var result = _castecategoryContext.CasteCategory.Single(t => t.IMCC_Id == mas.IMCC_Id);
                    //result.IVRMM_Id = mas.IVRMM_Id;

                    result.IMCC_CategoryName = mas.IMCC_CategoryName;
                    result.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _castecategoryContext.Update(result);
                    int n = _castecategoryContext.SaveChanges();
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
            else
            {
                CollegecastecaegoryDMO MM3 = new CollegecastecaegoryDMO();
                var checkDuplicates = _castecategoryContext.CasteCategory.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName)).ToList();
                if (checkDuplicates.Count > 0)
                {
                    mas.message = "Caste Category Already Exists";
                }
                else
                {
                    MM3.IMCC_CategoryName = mas.IMCC_CategoryName;
                    MM3.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _castecategoryContext.Add(MM3);
                    int n = _castecategoryContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = false;
                    }
                }
            }
            List<CollegecastecaegoryDMO> Allname = new List<CollegecastecaegoryDMO>();
            Allname = _castecategoryContext.CasteCategory.OrderByDescending(a => a.CreatedDate).ToList();
            mas.castecategoryname = Allname.ToArray();
            if (mas.castecategoryname.Length > 0)
            {
                mas.count = mas.castecategoryname.Length;
            }
            else
            {
                mas.count = 0;
            }
            // _MasterActivityContext.SaveChanges();

            return mas;
        }
    }
}
