using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.PDA;
using DataAccessMsSqlServerProvider.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.PDA;
using DomainModel.Model.com.vaps.admission;
using PDAServiceHub.com.vaps.interfaces;

namespace PDAServiceHub.com.vaps.services
{
    public class PDAClassSectionReportImpl : PDAClassSectionReportInterface
    {

        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
      new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDAClassSectionReportImpl> _logger;
        public PDAClassSectionReportImpl(PDAContext frgContext, ILogger<PDAClassSectionReportImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getalldetails(PDATransactionDTO data)
        {


            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_Id == data.ASMAY_Id).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classlst = new List<School_M_Class>();
                classlst = _PdaheadContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMCL_Id).ToList();
                data.classlist = classlst.ToArray();

                List<School_M_Section> sectionlst = new List<School_M_Section>();
                sectionlst = _PdaheadContext.school_M_Section.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMS_Id).ToList();
                data.searcharray = sectionlst.ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }
    }
}
