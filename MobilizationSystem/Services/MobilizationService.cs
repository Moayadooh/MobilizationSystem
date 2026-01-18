using Microsoft.EntityFrameworkCore;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Services
{
    public class MobilizationService : IMobilizationService
    {
        private readonly MobilizationDbContext _context;

        public MobilizationService(MobilizationDbContext context)
        {
            _context = context;
        }

        public async Task ApproveAsync(int requestId, string approvedBy, bool isFinalApproval)
        {
            var request = await _context.MobilizationRequests
                .Include(r => r.MobilizationRequestPersons)
                .Include(r => r.MobilizationRequestResources)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Request not found");

            // Officer approval → Approved
            request.Status = isFinalApproval
                ? RequestStatus.Activated
                : RequestStatus.Approved;

            _context.Approvals.Add(new Approval
            {
                MobilizationRequestId = requestId,
                ApprovedBy = approvedBy,
                ApprovalDate = DateTime.UtcNow,
                IsApproved = true
            });

            // 🔒 LOCK only on final approval
            if (isFinalApproval)
            {
                await LockResourcesAsync(request);
            }

            await _context.SaveChangesAsync();
        }

        public async Task CompleteAsync(int requestId)
        {
            var request = await _context.MobilizationRequests
                .Include(r => r.MobilizationRequestPersons)
                .Include(r => r.MobilizationRequestResources)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Request not found");

            request.Status = RequestStatus.Completed;

            // 🔓 UNLOCK
            await UnlockResourcesAsync(request);

            await _context.SaveChangesAsync();
        }

        private async Task LockResourcesAsync(MobilizationRequest request)
        {
            foreach (var p in request.MobilizationRequestPersons)
            {
                var person = await _context.Persons.FindAsync(p.PersonId);
                if (person != null)
                    person.IsAvailable = false;
            }

            foreach (var r in request.MobilizationRequestResources)
            {
                var resource = await _context.Resources.FindAsync(r.ResourceId);
                if (resource != null)
                    resource.IsAvailable = false;
            }
        }

        private async Task UnlockResourcesAsync(MobilizationRequest request)
        {
            foreach (var p in request.MobilizationRequestPersons)
            {
                var person = await _context.Persons.FindAsync(p.PersonId);
                if (person != null)
                    person.IsAvailable = true;
            }

            foreach (var r in request.MobilizationRequestResources)
            {
                var resource = await _context.Resources.FindAsync(r.ResourceId);
                if (resource != null)
                    resource.IsAvailable = true;
            }
        }

        public async Task CancelAsync(int requestId, string cancelledBy)
        {
            var request = await _context.MobilizationRequests
                .Include(r => r.MobilizationRequestPersons)
                .Include(r => r.MobilizationRequestResources)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status == RequestStatus.Completed ||
                request.Status == RequestStatus.Rejected)
                throw new InvalidOperationException("This request cannot be cancelled");

            request.Status = RequestStatus.Cancelled;

            // 🔓 RELEASE persons
            foreach (var p in request.MobilizationRequestPersons)
            {
                var person = await _context.Persons.FindAsync(p.PersonId);
                if (person != null)
                    person.IsAvailable = true;
            }

            // 🔓 RELEASE resources
            foreach (var r in request.MobilizationRequestResources)
            {
                var resource = await _context.Resources.FindAsync(r.ResourceId);
                if (resource != null)
                    resource.IsAvailable = true;
            }

            // 📝 Audit
            _context.AuditLogs.Add(new AuditLog
            {
                UserName = cancelledBy,
                Action = "Cancel Mobilization Request",
                TableName = nameof(MobilizationRequest),
                RecordId = request.Id,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}
