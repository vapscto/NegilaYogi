using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MasterBoardandSchoolTypeImpl : Interfaces.MasterBoardandSchoolTypeInterface
    {
        private static ConcurrentDictionary<string, MasterBoardDTO> _login =
            new ConcurrentDictionary<string, MasterBoardDTO>();
        private static ConcurrentDictionary<string, MasterSchoolTypeDTO> _login1 =
           new ConcurrentDictionary<string, MasterSchoolTypeDTO>();

        public MasterBoardandSchoolTypeContext _masterboardandSchoolTypeContext;
        public AdmissionFormContext _db;

        public MasterBoardandSchoolTypeImpl(MasterBoardandSchoolTypeContext masterboardandSchoolTypeContext, AdmissionFormContext db)
        {
            _masterboardandSchoolTypeContext = masterboardandSchoolTypeContext;
            _db = db;
        }


        public MasterBoardDTO getAllDetails(int id)
        {
            MasterBoardDTO mstcat = new MasterBoardDTO();
            try
            {
                List<MasterBorad> allboard = new List<MasterBorad>();
                allboard = _masterboardandSchoolTypeContext.masterborad.Where(d => d.MI_Id == id).OrderByDescending(d => d.IVRMMB_Id).ToList();
                mstcat.boardList = allboard.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return mstcat;
        }
        public MasterSchoolTypeDTO getAllSchoolTypeDetails(MasterSchoolTypeDTO mstcat)
        {
            try
            {
                List<MasterSchoolType> allschoolType = new List<MasterSchoolType>();
                allschoolType = _masterboardandSchoolTypeContext.masterschoolType.OrderByDescending(d => d.IVRMMTYP_Id).ToList();
                mstcat.schoolTypeList = allschoolType.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return mstcat;
        }
        public MasterBoardDTO getdetails(int id)
        {
            MasterBoardDTO cate = new MasterBoardDTO();
            try
            {
                List<MasterBorad> lorg = new List<MasterBorad>();
                lorg = _masterboardandSchoolTypeContext.masterborad.AsNoTracking().Where(t => t.IVRMMB_Id == id).ToList();
                cate.boardList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return cate;
        }
        public MasterSchoolTypeDTO getSchoolTypedetails(int id)
        {
            MasterSchoolTypeDTO cate = new MasterSchoolTypeDTO();
            try
            {
                List<MasterSchoolType> lorg1 = new List<MasterSchoolType>();
                lorg1 = _masterboardandSchoolTypeContext.masterschoolType.AsNoTracking().Where(t => t.IVRMMTYP_Id.Equals(id)).ToList();
                cate.schoolTypeList = lorg1.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return cate;
        }
        public MasterBoardDTO savedet(MasterBoardDTO catgry)
        {
            try
            {
                MasterBorad ctgry = Mapper.Map<MasterBorad>(catgry);

                if (ctgry.IVRMMB_Id > 0)
                {
                    var Duplicateresult = _masterboardandSchoolTypeContext.masterborad.Where(t => t.IVRMMB_Name == ctgry.IVRMMB_Name && t.IVRMMB_Id != ctgry.IVRMMB_Id && t.MI_Id == catgry.MI_Id).Count();
                    if (Duplicateresult > 0)
                    {
                        catgry.returnval = "Duplicate";
                        return catgry;
                    }
                    else
                    {
                        var result = _masterboardandSchoolTypeContext.masterborad.Single(t => t.IVRMMB_Id == ctgry.IVRMMB_Id);


                        result.IVRMMB_Description = ctgry.IVRMMB_Description;
                        result.IVRMMB_Name = ctgry.IVRMMB_Name;
                        //added by 02/02/2017
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _masterboardandSchoolTypeContext.Update(result);
                        var flag = _masterboardandSchoolTypeContext.SaveChanges();
                        if (flag == 1)
                        {
                            catgry.returnval = "Update";
                        }
                        else
                        {
                            catgry.returnval = "false";
                        }
                    }
                }
                else
                {
                    var Duplicateresult = _masterboardandSchoolTypeContext.masterborad.Where(t => t.IVRMMB_Name == ctgry.IVRMMB_Name && t.MI_Id==ctgry.MI_Id).Count();
                    if (Duplicateresult > 0)
                    {
                        catgry.returnval = "Duplicate";
                        return catgry;
                    }

                    //added by 02/02/2017
                    ctgry.CreatedDate = DateTime.Now;
                    ctgry.UpdatedDate = DateTime.Now;
                    ctgry.Is_Active = true;
                    ctgry.MI_Id = catgry.MI_Id;
                    _masterboardandSchoolTypeContext.Add(ctgry);
                    var flag = _masterboardandSchoolTypeContext.SaveChanges();
                    if (flag == 1)
                    {
                        catgry.returnval = "Add";
                    }
                    else
                    {
                        catgry.returnval = "false";
                    }
                }

                List<MasterBorad> allBoard = new List<MasterBorad>();
                allBoard = _masterboardandSchoolTypeContext.masterborad.Where(d => d.MI_Id == catgry.MI_Id).OrderByDescending(d => d.IVRMMB_Id).ToList();
                catgry.boardList = allBoard.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return catgry;
        }
        public MasterSchoolTypeDTO saveSchoolTypeDet(MasterSchoolTypeDTO catgry)
        {
            try
            {
                MasterSchoolType ctgry = Mapper.Map<MasterSchoolType>(catgry);

                if (ctgry.IVRMMTYP_Id > 0)
                {
                    var checkduplicate = _masterboardandSchoolTypeContext.masterschoolType.Where(d => d.IVRMMTYP_Type == catgry.IVRMMTYP_Type && d.IVRMMTYP_Id != ctgry.IVRMMTYP_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        catgry.message = catgry.IVRMMTYP_Type + " already exist.....! ";
                    }
                    else
                    {


                        var result = _masterboardandSchoolTypeContext.masterschoolType.Single(t => t.IVRMMTYP_Id == ctgry.IVRMMTYP_Id);

                        result.Is_Active = true;
                        result.IVRMMTYP_Description = ctgry.IVRMMTYP_Description;
                        result.IVRMMTYP_Type = ctgry.IVRMMTYP_Type;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _masterboardandSchoolTypeContext.Update(result);
                        var flag = _masterboardandSchoolTypeContext.SaveChanges();
                        if (flag == 1)
                        {
                            catgry.message = "u";
                            catgry.returnval = true;
                        }
                        else
                        {
                            catgry.message = "u";
                            catgry.returnval = false;
                        }
                    }
                }
                else
                {
                    //added by 02/02/2017
                    var checkduplicate = _masterboardandSchoolTypeContext.masterschoolType.Where(d => d.IVRMMTYP_Type == catgry.IVRMMTYP_Type).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        catgry.message = catgry.IVRMMTYP_Type + " already exist.....! ";
                    }
                    else
                    {
                        ctgry.CreatedDate = DateTime.Now;
                        ctgry.UpdatedDate = DateTime.Now;
                        ctgry.Is_Active = true;
                        _masterboardandSchoolTypeContext.Add(ctgry);
                        var flag = _masterboardandSchoolTypeContext.SaveChanges();
                        if (flag == 1)
                        {
                            catgry.message = "a";
                            catgry.returnval = true;
                        }
                        else
                        {
                            catgry.message = "a";
                            catgry.returnval = false;
                        }
                    }

                }

                List<MasterSchoolType> allSchoolType = new List<MasterSchoolType>();
                allSchoolType = _masterboardandSchoolTypeContext.masterschoolType.OrderByDescending(d => d.IVRMMTYP_Id).ToList();
                catgry.schoolTypeList = allSchoolType.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return catgry;
        }

        public MasterBoardDTO deleterec(int id)
        {
            MasterBoardDTO org = new MasterBoardDTO();
            List<MasterBorad> lorg = new List<MasterBorad>();

            try
            {
                lorg = _masterboardandSchoolTypeContext.masterborad.Where(t => t.IVRMMB_Id == id).ToList();

                var check = _db.StudentPrevSchoolDMO.Where(d => d.MI_Id == lorg.FirstOrDefault().MI_Id && d.AMSTPS_PreSchoolBoard.Contains(lorg.FirstOrDefault().IVRMMB_Name)).ToList();
                if (check.Count > 0)
                {
                    org.returnval = "mapped";
                }
                else
                {
                    if (lorg.Any())
                    {
                        _masterboardandSchoolTypeContext.Remove(lorg.ElementAt(0));

                        var flag = _masterboardandSchoolTypeContext.SaveChanges();
                        if (flag == 1)
                        {
                            org.returnval = "true";
                        }
                        else
                        {
                            org.returnval = "false";
                        }
                    }
                }



                List<MasterBorad> allBoard = new List<MasterBorad>();
                allBoard = _masterboardandSchoolTypeContext.masterborad.Where(d => d.MI_Id == lorg.FirstOrDefault().MI_Id).OrderByDescending(d => d.IVRMMB_Id).ToList();
                org.boardList = allBoard.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        public MasterSchoolTypeDTO deleteSchoolTyperec(int id)
        {

            MasterSchoolTypeDTO org = new MasterSchoolTypeDTO();
            List<MasterSchoolType> lorg = new List<MasterSchoolType>();

            try
            {
                lorg = _masterboardandSchoolTypeContext.masterschoolType.Where(t => t.IVRMMTYP_Id.Equals(id)).ToList();

                if (lorg.Any())
                {
                    _masterboardandSchoolTypeContext.Remove(lorg.ElementAt(0));

                    var flag = _masterboardandSchoolTypeContext.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = true;
                    }
                    else
                    {
                        org.returnval = false;
                    }
                }
                List<MasterSchoolType> allSchoolType = new List<MasterSchoolType>();
                allSchoolType = _masterboardandSchoolTypeContext.masterschoolType.OrderByDescending(d => d.IVRMMTYP_Id).ToList();
                org.schoolTypeList = allSchoolType.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }

    }
}
