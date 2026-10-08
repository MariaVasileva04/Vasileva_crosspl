using Microsoft.EntityFrameworkCore;
using Vasileva_crosspl.Models;
using Vasileva_crosspl.Dtos;

namespace Vasileva_crosspl.Services
{
    public class Service
    {
        private readonly AppDbContext _context;

        public Service(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Pipeline>> GetAllPipelines()
        {
            return await _context.Pipelines.ToListAsync();
        }

        public async Task<Pipeline> GetPipelineById(int id)
        {
            return await _context.Pipelines.FindAsync(id);
        }

        public async Task<Pipeline> CreatePipeline(Pipeline pipeline)
        {
            _context.Pipelines.Add(pipeline);
            await _context.SaveChangesAsync();
            return pipeline;
        }

        public async Task<bool> UpdatePipeline(int id, Pipeline pipeline)
        {
            if (id != pipeline.Id) return false;
            _context.Entry(pipeline).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePipeline(int id)
        {
            var pipeline = await _context.Pipelines.FindAsync(id);
            if (pipeline == null) return false;
            _context.Pipelines.Remove(pipeline);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<Contractor>> GetAllContractors()
        {
            return await _context.Contractors.ToListAsync();
        }

        public async Task<Contractor> GetContractorById(int id)
        {
            return await _context.Contractors.FindAsync(id);
        }

        public async Task<Contractor> CreateContractor(Contractor contractor)
        {
            _context.Contractors.Add(contractor);
            await _context.SaveChangesAsync();
            return contractor;
        }

        public async Task<bool> UpdateContractor(int id, Contractor contractor)
        {
            if (id != contractor.Id) return false;
            _context.Entry(contractor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContractor(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null) return false;
            _context.Contractors.Remove(contractor);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<MaintenanceRecord>> GetAllRecords()
        {
            return await _context.MaintenanceRecords
                .Include(r => r.Pipeline)
                .Include(r => r.Contractor)
                .ToListAsync();
        }

        public async Task<MaintenanceRecord> CreateRecord(MaintenanceRecord record)
        {
            _context.MaintenanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

       
        public async Task<MaintenanceRecord> GetRecordById(int id)
        {
            return await _context.MaintenanceRecords
                .Include(r => r.Pipeline)
                .Include(r => r.Contractor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> UpdateRecord(int id, MaintenanceRecord record)
        {
            if (id != record.Id) return false;
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRecord(int id)
        {
            var record = await _context.MaintenanceRecords.FindAsync(id);
            if (record == null) return false;
            _context.MaintenanceRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }


        public List<UrgentRepairDto> GetUrgentRepairs()
        {
            var allPipelines = _context.Pipelines.ToList();

            return allPipelines
                .Where(p => p.RequiresUrgentMaintenance())
                .Select(p => new UrgentRepairDto
                {
                    PipelineName = p.Name,
                    Wear = p.CalculateWearPercentage(),
                    RecommendedContractor = _context.Contractors
                        .Where(c => c.Rating >= 4)
                        .OrderBy(c => c.BasePricePerKm)
                        .Select(c => c.CompanyName)
                        .FirstOrDefault()
                }).ToList();

        public List<ContractorCostDto> GetContractorCosts()
        {
            return _context.MaintenanceRecords
                .Include(r => r.Contractor)
                .GroupBy(r => r.ContractorId)
                .Select(g => new ContractorCostDto
                {
                    ContractorId = g.Key,
                    ContractorName = g.First().Contractor!.CompanyName,
                    TotalSpent = g.Sum(r => r.ActualCost),
                    RecordsCount = g.Count()
                }).ToList();
        }

        public List<RecordDetailsDto> GetAllRecordsDetails()
        {
            var records = _context.MaintenanceRecords
                .Include(r => r.Pipeline)
                .Include(r => r.Contractor)
                .ToList();

            return records.Select(r => new RecordDetailsDto
            {
                RecordId = r.Id,
                Pipeline = r.Pipeline!.Name,
                Contractor = r.Contractor!.CompanyName,
                Date = r.Date,
                ActualCost = r.ActualCost,
                EstimatedCost = r.Contractor!.CalcCost(r.Pipeline),
                Notes = r.Notes
            }).ToList();
        }

        public List<object> GetMaintenanceSchedule()
        {
            var result = _context.MaintenanceRecords
                .Include(r => r.Pipeline)
                .Include(r => r.Contractor)
                .OrderByDescending(r => r.Date)
                .Select(r => new
                {
                    PipelineName = r.Pipeline!.Name,
                    LastMaintenanceDate = r.Date,
                    NextMaintenanceDate = r.Contractor!.CalcNextMainDate(r.Date),
                    Contractor = r.Contractor.CompanyName
                }).ToList();

            return result.Cast<object>().ToList();
        }
    }
}