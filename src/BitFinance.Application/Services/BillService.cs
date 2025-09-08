using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Bills;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BitFinance.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _repository;
    private readonly ILogger<BillService> _logger;

    public BillService(IBillRepository repository, ILogger<BillService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<BillDto>> CreateAsync(Guid organizationId, CreateBillRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.DescriptionIsRequired);
            }

            if (request.AmountDue <= 0)
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.AmountMustBePositive);
            }
            
            var isValidCategory = Enum.TryParse(request.Category, true, out BillCategory category);
            var isValidStatus = Enum.TryParse(request.Status, true, out BillStatus status);

            if (!isValidCategory)
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.InvalidCategory);
            }

            if (!isValidStatus)
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.InvalidStatus);
            }

            if (request.PaymentDate.HasValue && request.AmountPaid <= 0)
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.PaymentAmountMustBePositive);
            }

            var bill = new Bill
            {
                Description = request.Description.Trim(),
                Category = category,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                DueDate = request.DueDate.ToUniversalTime(),
                PaymentDate = request.PaymentDate?.ToUniversalTime(),
                AmountDue = request.AmountDue,
                AmountPaid = request.AmountPaid ?? 0,
                OrganizationId = organizationId
            };
        
            await _repository.CreateAsync(bill);

            var data = new BillDto
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                DueDate = bill.DueDate,
                PaymentDate = bill.PaymentDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };
        
            return Result<BillDto>.Success(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bill for organization {OrganizationId}", organizationId);
            return Result<BillDto>.Failure(Error.Infrastructure("Bills.Create.Failed", "An error occurred while creating the bill"));
        }
    }

    public async Task<Result<BillDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var bill =  await _repository.GetByIdAsync(id);

            if (bill == null)
            {
                return Result<BillDto>.Failure(DomainErrors.Bills.NotFound);
            }
            
            var data = new BillDto
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                DueDate = bill.DueDate,
                PaymentDate = bill.PaymentDate,
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
            };
        
            return Result<BillDto>.Success(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill with Id {Id}", id);
            return Result<BillDto>.Failure(Error.Infrastructure("Bills.Retrieve.Failed", "An error occurred while retrieving the bill"));
        }
    }

    public async Task<List<Bill>> GetAllBillsByStatusAsync(BillStatus status)
    {
        return await _repository.GetAllByStatusAsync(status);
    }

    public async Task UpdateAsync(Bill bill)
    {
        await _repository.UpdateAsync(bill);
    }
}