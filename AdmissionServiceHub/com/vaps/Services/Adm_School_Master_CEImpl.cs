using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class Adm_School_Master_CEImpl:Interfaces.Adm_School_Master_CEInterface
    {
        public AdmissionFormContext _context;
        public Adm_School_Master_CEImpl(AdmissionFormContext rr)
        {
            _context = rr;
        }
        public Adm_School_Master_CE_DTO getdata(Adm_School_Master_CE_DTO data)
        {
            try
            {
                data.streamlist = _context.Adm_School_Master_Stream.Where(t => t.MI_Id == data.MI_Id&&t.ASMST_ActiveFlag==true).Distinct().ToArray();

                data.classlist = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id&&t.ASMCL_ActiveFlag==true).Distinct().ToArray();

                data.cexamlist = _context.Adm_School_Master_CE.Where(t => t.MI_Id == data.MI_Id&&t.ASMCE_ActiveFlag==true).Distinct().ToArray();

                data.mastervehicle = _context.Adm_School_Master_CE.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();

                data.mastervehicle2 = (from a in _context.Adm_School_Stream_Class_CE
                                       from b in _context.Adm_School_Master_CE
                                       from c in _context.Adm_School_Master_Stream
                                       from d in _context.School_M_Class
                                       where (a.ASMCE_Id == b.ASMCE_Id && a.ASMST_Id == c.ASMST_Id && a.ASMCL_Id == d.ASMCL_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id)
                                       select new Adm_School_Master_CE_DTO
                                       {
                                           ASSTCLCE_Id = a.ASSTCLCE_Id,
                                           ASMCE_Id = a.ASMCE_Id,
                                           ASMCL_Id = a.ASMCL_Id,
                                           ASMST_Id = a.ASMST_Id,
                                           ASMCE_CEName = b.ASMCE_CEName,
                                           ASMST_StreamName = c.ASMST_StreamName,
                                           ASMCL_ClassName= d.ASMCL_ClassName,
                                           ASSTCLCE_ActiveFlag = a.ASSTCLCE_ActiveFlag,
                                       }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_CE_DTO savedata(Adm_School_Master_CE_DTO data)
        {
            try
            {

                
                if (data.ASMCE_Id == 0)
                {

                    var milist = _context.Institution.Where(t => t.MI_Id == data.MI_Id).ToList();
                    data.masterinsti = (from a in _context.Institution
                                        from b in _context.Adm_School_Master_CE
                                        where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                        select new Adm_School_Master_CE_DTO
                                        {
                                            ASMCE_Id = b.ASMCE_Id,
                                            ASMCE_CEName = b.ASMCE_CEName,
                                        }).ToArray();
                    data.order = data.masterinsti.Length + 1;

                    var duplicate = _context.Adm_School_Master_CE.Where(t => t.ASMCE_CEName == data.ASMCE_CEName).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Adm_School_Master_CE DMO1 = new Adm_School_Master_CE();
                        DMO1.MI_Id = data.MI_Id;
                        DMO1.ASMCE_CEName = data.ASMCE_CEName;
                        DMO1.ASMCE_CECode = data.ASMCE_CECode;
                        DMO1.ASMCE_Order = data.order;
                        DMO1.ASMCE_ActiveFlag = true;
                     
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
                else if (data.ASMCE_Id > 0)
                {
                    var DMO2 = _context.Adm_School_Master_CE.Where(t => t.MI_Id == data.MI_Id && t.ASMCE_Id == data.ASMCE_Id).SingleOrDefault();                   
                        DMO2.MI_Id = data.MI_Id;
                        DMO2.ASMCE_CEName = data.ASMCE_CEName;
                        DMO2.ASMCE_CECode = data.ASMCE_CECode;                                               
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
        public Adm_School_Master_CE_DTO savedata2(Adm_School_Master_CE_DTO data)
        {
            try
            {

                if (data.ASSTCLCE_Id == 0)
                {
                    var duplicate = _context.Adm_School_Stream_Class_CE.Where(t => t.ASMCE_Id == data.ASMCE_Id&&t.ASMCL_Id==data.ASMCL_Id&&t.ASMST_Id==data.ASMST_Id).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Adm_School_Stream_Class_CE DMO1 = new Adm_School_Stream_Class_CE();
                        DMO1.MI_Id = data.MI_Id;
                        DMO1.ASMST_Id = data.ASMST_Id;
                        DMO1.ASMCL_Id = data.ASMCL_Id;
                        DMO1.ASMCE_Id = data.ASMCE_Id;
                        DMO1.ASSTCLCE_CompulsoryFlg = data.ASSTCLCE_CompulsoryFlg;
                        
                        DMO1.ASSTCLCE_ActiveFlag = true;

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
                else if (data.ASSTCLCE_Id > 0)
                {

                    var DMO2 = _context.Adm_School_Stream_Class_CE.Where(t => t.MI_Id == data.MI_Id && t.ASSTCLCE_Id == data.ASSTCLCE_Id).SingleOrDefault();
               
                        DMO2.MI_Id = data.MI_Id;
                        DMO2.ASMCE_Id = data.ASMCE_Id;
                        DMO2.ASMCL_Id = data.ASMCL_Id;
                        DMO2.ASMST_Id = data.ASMST_Id;
                        DMO2.ASSTCLCE_CompulsoryFlg = data.ASSTCLCE_CompulsoryFlg;

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
        public Adm_School_Master_CE_DTO editdata(Adm_School_Master_CE_DTO data)
        {
            try
            {
                data.editdata = _context.Adm_School_Master_CE.Where(a => a.MI_Id == data.MI_Id && a.ASMCE_Id == data.ASMCE_Id).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_CE_DTO activedeactive(Adm_School_Master_CE_DTO data)
        {
            try
            {
                var deactivatecount = (from a in _context.Adm_School_Master_CE
                                       from b in _context.Adm_School_Stream_Class_CE
                                       where (a.ASMCE_Id == b.ASMCE_Id && b.ASMCE_Id == data.ASMCE_Id && a.ASMCE_ActiveFlag == true)
                                       select b).Distinct().ToList();
                if (deactivatecount.Count() > 0)
                {
                    data.message = "Mapped";
                }
                else
                {
                    var w = _context.Adm_School_Master_CE.Where(t => t.ASMCE_Id == data.ASMCE_Id).SingleOrDefault();
                    if (w.ASMCE_ActiveFlag == false)
                    {
                        w.ASMCE_ActiveFlag = true;
                    }
                    else
                    {
                        w.ASMCE_ActiveFlag = false;
                    }                    
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
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }    
        public Adm_School_Master_CE_DTO deactive2(Adm_School_Master_CE_DTO data)
        {
            try
            {
                var w = _context.Adm_School_Stream_Class_CE.Where(t => t.ASSTCLCE_Id == data.ASSTCLCE_Id).SingleOrDefault();
                if (w.ASSTCLCE_ActiveFlag == false)
                {
                    w.ASSTCLCE_ActiveFlag = true;
                }
                else
                {
                    w.ASSTCLCE_ActiveFlag = false;
                }
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
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Adm_School_Master_CE_DTO edit2(Adm_School_Master_CE_DTO data)
        {
            try
            {

                data.editdata2 = (from a in _context.Adm_School_Stream_Class_CE
                              from b in _context.Adm_School_Master_CE
                              from c in _context.Adm_School_Master_Stream
                              from d in _context.School_M_Class
                              where (a.ASMCE_Id == b.ASMCE_Id && a.ASMST_Id == c.ASMST_Id && a.ASMCL_Id == d.ASMCL_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ASSTCLCE_Id == data.ASSTCLCE_Id)
                              select new Adm_School_Master_CE_DTO
                              {
                                  ASSTCLCE_Id = a.ASSTCLCE_Id,
                                  ASMCE_Id = a.ASMCE_Id,
                                  ASMCL_Id = a.ASMCL_Id,
                                  ASMST_Id = a.ASMST_Id,
                                  ASMCE_CEName = b.ASMCE_CEName,
                                  ASMST_StreamName = c.ASMST_StreamName,
                                  ASMCL_ClassName = d.ASMCL_ClassName,
                                  ASSTCLCE_CompulsoryFlg = a.ASSTCLCE_CompulsoryFlg,
                              }).Distinct().ToArray();

                


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
