using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class Adm_School_Master_StreamImpl : Interfaces.Adm_School_Master_StreamInterface
    {
        public AdmissionFormContext _context;
        public Adm_School_Master_StreamImpl(AdmissionFormContext rr)
        {
            _context = rr;
        }
        public Adm_School_Master_Stream_DTO getdata(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                data.streamlist = _context.Adm_School_Master_Stream.Where(t => t.MI_Id == data.MI_Id&&t.ASMST_ActiveFlag==true).Distinct().ToArray();

                data.classlist = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id&&t.ASMCL_ActiveFlag==true).Distinct().OrderBy(a=>a.ASMCL_Order).ToArray();

                data.sectionlist = _context.AdmSection.Where(t => t.MI_Id == data.MI_Id&&t.ASMC_ActiveFlag==1).Distinct().OrderBy(a=>a.ASMC_Order).ToArray();

                data.mastervehicle = _context.Adm_School_Master_Stream.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.mastervehicle2 = (from a in _context.Adm_School_Stream_Class
                                       from b in _context.Adm_School_Master_Stream
                                       from c in _context.School_M_Class
                                       where (a.ASMST_Id == b.ASMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                       select new Adm_School_Master_Stream_DTO
                                       {
                                           ASMST_Id = a.ASMST_Id,
                                           //ASSTCL_Id = a.ASSTCL_Id,
                                           MI_Id = a.MI_Id,
                                           ASMCL_Id = a.ASMCL_Id,
                                           ASMST_StreamName = b.ASMST_StreamName,
                                           ASMCL_ClassName = c.ASMCL_ClassName,

                                       }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO savedata(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                if (data.ASMST_Id == 0)
                {

                    var milist = _context.Institution.Where(t => t.MI_Id == data.MI_Id).ToList();
                    data.masterinsti = (from a in _context.Institution
                                        from b in _context.Adm_School_Master_Stream
                                        where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                        select new Adm_School_Master_Stream_DTO
                                        {
                                            ASMST_Id = b.ASMST_Id,
                                            ASMST_StreamName = b.ASMST_StreamName,
                                        }).ToArray();
                    data.order = data.masterinsti.Length + 1;
                    var duplicate = _context.Adm_School_Master_Stream.Where(t => t.ASMST_StreamName == data.ASMST_StreamName && t.MI_Id==data.MI_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Adm_School_Master_Stream DMO1 = new Adm_School_Master_Stream();
                        DMO1.MI_Id = data.MI_Id;
                        DMO1.ASMST_StreamName = data.ASMST_StreamName;
                        DMO1.ASMST_StreamCode = data.ASMST_StreamCode;
                        DMO1.ASMST_Order = data.order;
                        DMO1.ASMST_ActiveFlag = true;
                        DMO1.CreatedDate = DateTime.Now;
                        DMO1.UpdatedDate = DateTime.Now;
                        _context.Add(DMO1);
                        int rowAffected = _context.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.message = "saved";
                        }
                        else
                        {
                            data.message = "notsaved";
                        }
                    }

                }
                else if (data.ASMST_Id > 0)
                {


                    var DMO2 = _context.Adm_School_Master_Stream.Where(t => t.MI_Id == data.MI_Id && t.ASMST_Id == data.ASMST_Id).SingleOrDefault();

                    DMO2.MI_Id = data.MI_Id;
                    DMO2.ASMST_StreamName = data.ASMST_StreamName;
                    DMO2.ASMST_StreamCode = data.ASMST_StreamCode;

                    DMO2.UpdatedDate = DateTime.Now;
                    _context.Update(DMO2);
                    int rowAffected1 = _context.SaveChanges();
                    if (rowAffected1 > 0)
                    {
                        data.message = "updated";
                    }
                    else
                    {
                        data.message = "notupdated";
                    }


                }


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO savedata2(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                if (data.editedit == true && data.ASSTCL_Id == 0)
                {

                    for (int i = 0; i < data.selectedsectionlist.Length; i++)
                    {
                        var tempdata = data.selectedsectionlist[i].ASMS_Id;

                        var clsstream = _context.Adm_School_Stream_Class.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMST_Id == data.ASMST_Id).Distinct().ToArray();
                         
                        List<long> year1_ids = new List<long>();
                        foreach (var item in data.selectedsectionlist)
                        {
                            year1_ids.Add(item.ASMS_Id);
                        }
                        for (int j = 0; j < clsstream.Length; j++)
                        {
                            if (!year1_ids.Contains(clsstream[j].ASMS_Id))
                            {
                                data.secid = clsstream[j].ASMS_Id;

                                var CountRemoveFiles = _context.Adm_School_Stream_Class.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMST_Id == data.ASMST_Id && t.ASMS_Id == data.secid).ToList();
                                if (CountRemoveFiles.Count > 0)
                                {
                                    foreach (var RemoveFiles in CountRemoveFiles)
                                    {
                                        _context.Remove(RemoveFiles);
                                    }
                                    int row = _context.SaveChanges();
                                    if (row > 0)
                                    {
                                        data.del = true;
                                    }
                                    else
                                    {
                                        data.del = false;
                                    }

                                }
                            }
                        }










                        //if (!myList.Contains("name"))
                        //{
                        //    myList.Add("name");
                        //}

                        //if (!data.selectedsectionlist.Contains(clsstream[].ASMS_Id))
                        //{
                        //    return 
                        //}

                        //foreach (Adm_School_Master_Stream_DTO[] items in data.selectedsectionlist)
                        //{
                        //    Console.WriteLine(items);
                        //}




                        //if (data.selectedsectionlist[].ASMS_Id != tempdata)
                        //{

                        //}








                        var check_fee_master_concession_update3 = _context.Adm_School_Stream_Class.Where(a => a.ASMS_Id == tempdata && a.ASMCL_Id == data.ASMCL_Id && a.ASMST_Id == data.ASMST_Id).ToList();
                        if (check_fee_master_concession_update3.Count == 0)
                        {

                            Adm_School_Stream_Class DMO5 = new Adm_School_Stream_Class();
                            DMO5.ASMST_Id = data.ASMST_Id;
                            DMO5.ASMCL_Id = data.ASMCL_Id;
                            DMO5.MI_Id = data.MI_Id;
                            DMO5.ASMS_Id = tempdata;
                            DMO5.UpdatedDate = DateTime.Now;
                            DMO5.CreatedDate = DateTime.Now;
                            DMO5.ASSTCL_ActiveFlag = true;
                            _context.Add(DMO5);
                            int n = _context.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Update2";

                            }
                            else
                            {
                                data.message = "notUpdate2";
                                data.retrunval2 = false;
                            }
                        }
                        else
                        {
                            data.message = "Update2";
                            data.retrunval2 = true;
                        }
                    }
                }
                if (data.ASSTCL_Id == 0 && data.editedit == false)
                {
                    for (int i = 0; i < data.selectedsectionlist.Length; i++)
                    {
                        var tempdata = data.selectedsectionlist[i].ASMS_Id;
                        var check_fee_master_concession3 = _context.Adm_School_Stream_Class.Where(a => a.ASMS_Id == tempdata && a.ASMST_Id == data.ASMST_Id && a.ASMCL_Id == data.ASMCL_Id).ToList();

                        //check_fee_master_concession3[0].MI_Id=data.MI_Id;


                        if (check_fee_master_concession3.Count == 0)
                        {
                            Adm_School_Stream_Class feemastDMO = new Adm_School_Stream_Class();
                            feemastDMO.ASSTCL_Id = data.ASSTCL_Id;
                            feemastDMO.MI_Id = data.MI_Id;
                            feemastDMO.ASMST_Id = data.ASMST_Id;
                            feemastDMO.ASMS_Id = tempdata;
                            feemastDMO.ASMCL_Id = data.ASMCL_Id;
                            feemastDMO.CreatedDate = DateTime.Now;
                            feemastDMO.UpdatedDate = DateTime.Now;
                            feemastDMO.ASSTCL_ActiveFlag = true;
                            _context.Add(feemastDMO);
                            int n = _context.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Add";
                                data.retrunval2 = true;
                            }
                            else
                            {
                                data.message = "notAdd";
                                data.retrunval2 = false;
                            }
                        }
                        else
                        {
                            data.message = "Duplicate";
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO editdata(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                data.editdata = _context.Adm_School_Master_Stream.Where(a => a.MI_Id == data.MI_Id && a.ASMST_Id == data.ASMST_Id).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO activedeactive(Adm_School_Master_Stream_DTO data)
        {
            try
            {

                //var deactivatecount = (from a in _context.Adm_School_Master_Stream
                //                       from b in _context.Adm_School_Stream_Class
                //                       where (a.ASMST_Id == b.ASMST_Id && b.ASMST_Id == data.ASMST_Id && a.ASMST_ActiveFlag == true)
                //                       select b).Distinct().ToList();

                var deactivatecount = (from a in _context.Adm_School_Master_Stream
                                       from b in _context.Adm_School_Stream_Class
                                       where (a.ASMST_Id == b.ASMST_Id && b.ASMST_Id == data.ASMST_Id && a.ASMST_ActiveFlag == true)
                                       select b).Distinct().ToList();
                if (deactivatecount.Count() > 0)
                {
                    data.message = "Mapped";
                }
                else
                {
                    var w = _context.Adm_School_Master_Stream.Where(t => t.ASMST_Id == data.ASMST_Id).SingleOrDefault();
                    if (w.ASMST_ActiveFlag == false)
                    {
                        w.ASMST_ActiveFlag = true;
                    }
                    else
                    {
                        w.ASMST_ActiveFlag = false;
                    }

                    w.UpdatedDate = DateTime.Now;
                    w.MI_Id = data.MI_Id;
                    _context.Update(w);
                    int y = _context.SaveChanges();
                    if (y > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    
                }


                //if (deactivatecount.Count() > 0)
                //{
                //    var w = _context.Adm_School_Master_Stream.Where(t => t.ASMST_Id == data.ASMST_Id).SingleOrDefault();
                //    if (w.ASMST_ActiveFlag == false)
                //    {
                //        w.ASMST_ActiveFlag = true;
                //    }
                //    else
                //    {
                //        w.ASMST_ActiveFlag = false;
                //    }

                //    w.UpdatedDate = DateTime.Now;
                //    w.MI_Id = data.MI_Id;
                //    _context.Update(w);
                //    int y = _context.SaveChanges();
                //    if (y > 0)
                //    {
                //        data.returnval = true;
                //    }
                //    else
                //    {
                //        data.returnval = false;
                //    }
                //}
                //else
                //{
                //    data.message = "Mapped";
                //}
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO getdetails(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                data.streamdetails = (from a in _context.Adm_School_Stream_Class
                                      from b in _context.Adm_School_Master_Stream
                                      from c in _context.School_M_Class
                                      from d in _context.AdmSection
                                      where (a.ASMST_Id == b.ASMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMST_Id == data.ASMST_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id)
                                      select new Adm_School_Master_Stream_DTO
                                      {
                                          ASMST_Id = a.ASMST_Id,
                                          ASSTCL_Id = a.ASSTCL_Id,
                                          ASMCL_Id = a.ASMCL_Id,
                                          ASMS_Id = a.ASMS_Id,
                                          ASMST_StreamName = b.ASMST_StreamName,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASSTCL_ActiveFlag = a.ASSTCL_ActiveFlag,

                                      }).Distinct().ToArray();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO deactive2(Adm_School_Master_Stream_DTO data)
        {
            try
            {
                var w = _context.Adm_School_Stream_Class.Where(t => t.ASSTCL_Id == data.ASSTCL_Id).SingleOrDefault();
                if (w.ASSTCL_ActiveFlag == false)
                {
                    w.ASSTCL_ActiveFlag = true;
                }
                else
                {
                    w.ASSTCL_ActiveFlag = false;
                }

                w.UpdatedDate = DateTime.Now;
                w.MI_Id = data.MI_Id;
                _context.Update(w);
                int y = _context.SaveChanges();
                if (y > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.streamdetails = (from a in _context.Adm_School_Stream_Class
                                      from b in _context.Adm_School_Master_Stream
                                      from c in _context.School_M_Class
                                      from d in _context.AdmSection
                                      where (a.ASMST_Id == b.ASMST_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMST_Id == data.ASMST_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id)
                                      select new Adm_School_Master_Stream_DTO
                                      {
                                          ASMST_Id = a.ASMST_Id,
                                          ASSTCL_Id = a.ASSTCL_Id,
                                          ASMCL_Id = a.ASMCL_Id,
                                          ASMS_Id = a.ASMS_Id,
                                          ASMST_StreamName = b.ASMST_StreamName,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          ASSTCL_ActiveFlag = a.ASSTCL_ActiveFlag,

                                      }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_Stream_DTO edit2(Adm_School_Master_Stream_DTO data)
        {
            try
            {

                // data.sectionlistedit=_context.Adm_School_Stream_Class.Whe)
                data.sectionlistedit = (from a in _context.Adm_School_Stream_Class
                                        from b in _context.AdmSection
                                        where (a.ASMCL_Id == data.ASMCL_Id && a.ASMST_Id == data.ASMST_Id && a.ASMS_Id == b.ASMS_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                        select new Adm_School_Master_Stream_DTO
                                        {
                                            ASMS_Id = a.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                        }).Distinct().ToArray();
                data.editedit = true;


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
