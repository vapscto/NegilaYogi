using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;



namespace AdmissionServiceHub.com.vaps.Services
{
    public class castecategoryImpl : Interfaces.castecategoryInterface
    {
        private static ConcurrentDictionary<string, castecategoryDTO> _login =
        new ConcurrentDictionary<string, castecategoryDTO>();

        private readonly castecategoryContext _castecategoryContext;


        public castecategoryImpl(castecategoryContext castecategoryContext)
        {
            _castecategoryContext = castecategoryContext;
        }

        public castecategoryDTO GetcastecategoryData(castecategoryDTO castecategoryDTO)//int IVRMM_Id
        {

            Array[] showdata = new Array[50];
            List<castecategoryDMO> Allname = new List<castecategoryDMO>();
            Allname = _castecategoryContext.castecategoryDMO.OrderByDescending(a => a.CreatedDate).ToList();
            castecategoryDTO.castecategoryname = Allname.ToArray();
            if (castecategoryDTO.castecategoryname.Length > 0)
            {
                castecategoryDTO.count = castecategoryDTO.castecategoryname.Length;
            }
            else
            {
                castecategoryDTO.count = 0;
            }
            return castecategoryDTO;
        }

        public castecategoryDTO GetSelectedRowDetails(int ID)
        {
            castecategoryDTO castecategoryDTO = new castecategoryDTO();
            List<castecategoryDMO> lorg = new List<castecategoryDMO>();
            lorg = _castecategoryContext.castecategoryDMO.Where(t => t.IMCC_Id.Equals(ID)).OrderByDescending(a => a.CreatedDate).ToList();
            castecategoryDTO.castecategoryname = lorg.ToArray();
            return castecategoryDTO;
        }

        public castecategoryDTO MasterDeleteModulesData(int ID)
        {
            castecategoryDTO castecategoryDTO = new castecategoryDTO();
            List<castecategoryDMO> masters = new List<castecategoryDMO>();
            try
            {
                var check_castecategory_used = _castecategoryContext.studentdmo.Where(t => t.IMCC_Id == ID).ToList();
                var check_castecategory_used1 = _castecategoryContext.mastercasteDMO.Where(t => t.IMCC_Id == ID).ToList();

                if (check_castecategory_used.Count == 0 && check_castecategory_used1.Count == 0)
                {
                    masters = _castecategoryContext.castecategoryDMO.Where(t => t.IMCC_Id.Equals(ID)).ToList();
                    if (masters.Any())
                    {
                        _castecategoryContext.Remove(masters.ElementAt(0));
                        int n = _castecategoryContext.SaveChanges();
                        if (n > 0)
                        {
                            castecategoryDTO.returnVal = true;
                        }
                        else
                        {
                            castecategoryDTO.returnVal = false;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    castecategoryDTO.message = "Delete";
                }

                List<castecategoryDMO> Allname = new List<castecategoryDMO>();
                Allname = _castecategoryContext.castecategoryDMO.OrderByDescending(a => a.CreatedDate).ToList();
                castecategoryDTO.castecategoryname = Allname.ToArray();
                if (castecategoryDTO.castecategoryname.Length > 0)
                {
                    castecategoryDTO.count = castecategoryDTO.castecategoryname.Length;
                }
                else
                {
                    castecategoryDTO.count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                castecategoryDTO.message = "Delete";
                castecategoryDTO.message = "Sorry You Can Not Delete This Record. Because It Is Mapped To Student";
                List<castecategoryDMO> Allname = new List<castecategoryDMO>();
                Allname = _castecategoryContext.castecategoryDMO.OrderByDescending(a => a.CreatedDate).ToList();
                castecategoryDTO.castecategoryname = Allname.ToArray();
            }

            return castecategoryDTO;
        }

        public castecategoryDTO castecategoryData(castecategoryDTO mas)
        {

            castecategoryDMO MM = Mapper.Map<castecategoryDMO>(mas);
            if (mas.IMCC_Id != 0)
            {
                var checkDuplicates1 = _castecategoryContext.castecategoryDMO.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName) && d.IMCC_Id != mas.IMCC_Id).ToList();
                if (checkDuplicates1.Count > 0)
                {
                    var result = _castecategoryContext.castecategoryDMO.Single(t => t.IMCC_Id == mas.IMCC_Id);                    
                    if (result.IMCC_CategoryDesc != mas.IMCC_CategoryDesc)
                    {
                        if (result.IMCC_CategoryName != mas.IMCC_CategoryName)
                        {
                            var dupCatName = _castecategoryContext.castecategoryDMO.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName) && d.IMCC_Id != mas.IMCC_Id).ToList();
                            if (dupCatName.Count > 0)
                            {
                                mas.message = "Caste Category Already Exists";
                            }
                            else
                            {
                                result.IMCC_CategoryName = mas.IMCC_CategoryName;
                                result.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                                result.IMCC_CategoryCode = mas.IMCC_CategoryCode;
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
                            result.IMCC_CategoryCode = mas.IMCC_CategoryCode;
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
                    var result = _castecategoryContext.castecategoryDMO.Single(t => t.IMCC_Id == mas.IMCC_Id);
                    //result.IVRMM_Id = mas.IVRMM_Id;

                    result.IMCC_CategoryName = mas.IMCC_CategoryName;
                    result.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                    result.IMCC_CategoryCode = mas.IMCC_CategoryCode;
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
                castecategoryDMO MM3 = new castecategoryDMO();
                var checkDuplicates = _castecategoryContext.castecategoryDMO.Where(d => d.IMCC_CategoryName.Equals(mas.IMCC_CategoryName)).ToList();
                if (checkDuplicates.Count > 0)
                {
                    mas.message = "Caste Category Already Exists";
                }
                else
                {
                    MM3.IMCC_CategoryName = mas.IMCC_CategoryName;
                    MM3.IMCC_CategoryDesc = mas.IMCC_CategoryDesc;
                    MM3.IMCC_CategoryCode = mas.IMCC_CategoryCode;
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
            List<castecategoryDMO> Allname = new List<castecategoryDMO>();
            Allname = _castecategoryContext.castecategoryDMO.OrderByDescending(a => a.CreatedDate).ToList();
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
