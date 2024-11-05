using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;

namespace DataAccessMsSqlServerProvider
{
    public class DataAccessMsSqlServerProvider  : IDataAccessProvider
    {
        private readonly DomainModelMsSqlServerContext _context;
        private readonly Enquirycontext _enqContext;
        private readonly ILogger _logger;
        private ApplicationDBContext _appDBContex;

        public DataAccessMsSqlServerProvider(DomainModelMsSqlServerContext context, Enquirycontext enqContext,ILoggerFactory loggerFactory, ApplicationDBContext appDBContext)
        {
            _context = context;
            _enqContext = enqContext;
            _logger = loggerFactory.CreateLogger("DataAccessMsSqlServerProvider");
            _appDBContex = appDBContext;
        }

        public void AddDataEventRecord(DataEventRecord dataEventRecord)
        {
            if (dataEventRecord.SourceInfo != null && dataEventRecord.SourceInfoId == 0)
            {
                _context.SourceInfos.Add(dataEventRecord.SourceInfo);
            }
            _context.DataEventRecords.Add(dataEventRecord);
            _context.SaveChanges();
        }

        public void UpdateDataEventRecord(long dataEventRecordId, DataEventRecord dataEventRecord)
        {
            _context.DataEventRecords.Update(dataEventRecord);
            _context.SaveChanges();
        }

        public void DeleteDataEventRecord(long dataEventRecordId)
        {
            var entity = _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
            _context.DataEventRecords.Remove(entity);
            _context.SaveChanges();
        }

        public DataEventRecord GetDataEventRecord(long dataEventRecordId)
        {
            return _context.DataEventRecords.First(t => t.DataEventRecordId == dataEventRecordId);
        }

        public List<DataEventRecord> GetDataEventRecords()
        {
            // Using the shadow property EF.Property<DateTime>(dataEventRecord)
            return _context.DataEventRecords.OrderByDescending(dataEventRecord => EF.Property<DateTime>(dataEventRecord, "UpdatedTimestamp")).ToList();
        }

        public List<SourceInfo> GetSourceInfos(bool withChildren)
        {
            // Using the shadow property EF.Property<DateTime>(srcInfo)
            if (withChildren)
            {
                return _context.SourceInfos.Include(s => s.DataEventRecords).OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToList();
            }

            return _context.SourceInfos.OrderByDescending(srcInfo => EF.Property<DateTime>(srcInfo, "UpdatedTimestamp")).ToList();
        }
      
    }
}
