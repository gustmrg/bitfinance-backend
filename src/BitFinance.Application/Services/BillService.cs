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

    public async Task<Result<List<BillDto>>> GetAllByOrganizationAsync(
        Guid organizationId, 
        int page = 1, int pageSize = 100, 
        DateTime? from = null, DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1)
            {
                return Result<List<BillDto>>.Failure(Error.Validation("Bills.Page.Invalid", "Page must be greater than 0"));
            }
            
            if (pageSize < 1 || pageSize > 500)
            {
                return Result<List<BillDto>>.Failure(Error.Validation("Bills.PageSize.Invalid", "Page size must be between 1 and 500"));
            }

            // Validate date range - both must be provided or both must be null
            if ((from.HasValue && !to.HasValue) || (!from.HasValue && to.HasValue))
            {
                return Result<List<BillDto>>.Failure(DomainErrors.Bills.InvalidDateRange);
            }
            
            // If both dates are provided, validate that from is before to
            if (from.HasValue && to.HasValue && from.Value > to.Value)
            {
                return Result<List<BillDto>>.Failure(DomainErrors.Bills.InvalidDateRange);
            }
            
            var bills = await _repository.GetAllByOrganizationAsync(
                organizationId, 
                page, pageSize, 
                from, to);

            var data = bills.Select(x => new BillDto
            {
                Id = x.Id,
                Description = x.Description,
                Category = x.Category,
                Status = x.Status,
                DueDate = x.DueDate,
                PaymentDate = x.PaymentDate,
                AmountDue = x.AmountDue,
                AmountPaid = x.AmountPaid
            }).ToList();
            
            return Result<List<BillDto>>.Success(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bills for organization {OrganizationId}, page {Page}, pageSize {PageSize}", 
                organizationId, page, pageSize);
            return Result<List<BillDto>>.Failure(Error.Infrastructure("Bills.Retrieve.Failed", "An error occurred while retrieving bills"));
        }
    }

    public async Task UpdateAsync(Bill bill)
    {
        await _repository.UpdateAsync(bill);
    }

}