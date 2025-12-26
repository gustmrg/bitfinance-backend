using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Attributes;
using BitFinance.Application.DTOs;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Interfaces.Repositories;
using BitFinance.Domain.ValueObjects;
using BitFinance.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[OrganizationAuthorization]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations/{organizationId:guid}/bills")]
public class BillsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BillsController> _logger;
    private readonly IBillsRepository _billsRepository;
    private readonly IBillDocumentService _documentService;
    
    public BillsController(ApplicationDbContext context, 
        ILogger<BillsController> logger, 
        IBillsRepository billsRepository, 
        IBillDocumentService documentService)
    {
        _context = context;
        _logger = logger;
        _billsRepository = billsRepository;
        _documentService = documentService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<CreateBillResponse>> CreateBillAsync([FromRoute] Guid organizationId, [FromBody] CreateBillRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }
            
            var isValidCategory = Enum.TryParse(request.Category, true, out BillCategory category);
            var isValidStatus = Enum.TryParse(request.Status, true, out BillStatus status);
            
            if (!isValidCategory || !isValidStatus) return UnprocessableEntity();
            
            Bill bill = new()
            {
                Description = request.Description,
                Category = category,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                DueDate = DateOnly.FromDateTime(request.DueDate.ToUniversalTime()),
                PaymentDate = request.PaymentDate?.ToUniversalTime(),
                AmountDue = new Money(request.AmountDue),
                AmountPaid = request.AmountPaid.HasValue ? new Money(request.AmountPaid.Value) : null,
                OrganizationId = organizationId,
            };

            await _billsRepository.CreateAsync(bill);
            
            var response = new CreateBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                CreatedDate = bill.CreatedAt,
                DueDate = DateTime.Parse(bill.DueDate.ToString()),
                PaidDate = bill.PaymentDate,
                AmountDue = bill.AmountDue.Amount,
                AmountPaid = bill.AmountPaid?.Amount
            };
            
            return CreatedAtAction(nameof(GetBillById), new
            {
                billId = bill.Id, organizationId = bill.OrganizationId
            }, response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(CreateBillAsync), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResponse<Bill>>> GetBillsAsync(
        [FromRoute] Guid organizationId, 
        [FromQuery] int page = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
    {
        try
        {
            var totalRecords = await _billsRepository.GetEntriesCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var bills = await _billsRepository.GetAllByOrganizationAsync(organizationId, page, pageSize, from, to);

            var billsDto = bills.Select(bill => new GetBillResponse
                {
                    Id = bill.Id,
                    Description = bill.Description,
                    Category = bill.Category,
                    Status = bill.Status,
                    CreatedAt = bill.CreatedAt,
                    DueDate = DateTime.Parse(bill.DueDate.ToString()),
                    PaymentDate = bill.PaymentDate,
                    AmountDue = bill.AmountDue.Amount,
                    AmountPaid = bill.AmountPaid?.Amount,
                    Documents = bill.Documents.Select(doc => new DocumentSummary
                    {
                        Id = doc.Id,
                        FileName = doc.FileName,
                        ContentType = doc.ContentType,
                        DocumentType = doc.DocumentType
                    }).ToList()
                })
                .ToList();
            
            var response = new PagedResponse<GetBillResponse>(billsDto, page, pageSize, totalRecords, totalPages);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture),
                nameof(GetBillsAsync),
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{billId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GetBillResponse>> GetBillById([FromRoute] Guid billId)
    {
        try
        {
            Bill? bill = await _billsRepository.GetByIdAsync(billId);

            if (bill is null)
            {
                return NotFound();
            }

            var response = new GetBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                CreatedAt = bill.CreatedAt,
                DueDate = new DateTime(bill.DueDate, TimeOnly.MinValue),
                PaymentDate = bill.PaymentDate,
                AmountDue = bill.AmountDue.Amount,
                AmountPaid = bill.AmountPaid?.Amount,
                Documents = bill.Documents.Select(doc => new DocumentSummary
                {
                    Id = doc.Id,
                    FileName = doc.FileName,
                    ContentType = doc.ContentType,
                    DocumentType = doc.DocumentType
                }).ToList()
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(GetBillById), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPatch]
    [Route("{billId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UpdateBillResponse>> UpdateBill(Guid billId, [FromBody] UpdateBillRequest request)
    {
        try
        {
            bool isValidCategory = Enum.TryParse(request.Category, true, out BillCategory category);
            bool isValidStatus = Enum.TryParse(request.Status, true, out BillStatus status);
            
            if (!isValidCategory || !isValidStatus) return UnprocessableEntity();

            var bill = await _context.Bills.FirstOrDefaultAsync(b => b.Id == billId && b.DeletedAt == null);

            if (bill is null)
            {
                return NotFound();
            }
            
            bill.Description = request.Description;
            bill.Category = category;
            bill.Status = status;
            bill.DueDate = DateOnly.FromDateTime(request.DueDate.ToUniversalTime());
            bill.PaymentDate = request.PaymentDate?.ToUniversalTime();
            bill.AmountDue = new Money(request.AmountDue);
            bill.AmountPaid = request.AmountPaid.HasValue ? new Money(request.AmountPaid.Value) : null;

            await _billsRepository.UpdateAsync(bill, 
                b => b.Description, 
                b => b.Category, 
                b => b.Status, 
                b => b.DueDate, 
                b => b.PaymentDate,
                b => b.AmountDue,
                b => b.AmountPaid);

            var response = new UpdateBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                DueDate = new DateTime(bill.DueDate, TimeOnly.MinValue),
                PaidDate = bill.PaymentDate,
                AmountDue = bill.AmountDue.Amount,
                AmountPaid = bill.AmountPaid?.Amount
            };
        
            return Ok(response);
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(UpdateBill), 
                ex.Message);
            return BadRequest();
        }
    }
    
    [HttpDelete]
    [Route("{billId:guid}")]
    public async Task<ActionResult> DeleteBillById(Guid billId)
    {
        try
        {
            Bill? bill = await _billsRepository.GetByIdAsync(billId);

            if (bill is null)
            {
                return NotFound();
            }

            if (bill.DeletedAt is not null)
            {
                return BadRequest();
            }

            await _billsRepository.DeleteAsync(bill);

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error("{Timestamp} - Error on {MethodName} method request: {Message}",
                DateTime.Now.ToString("s", CultureInfo.InvariantCulture), 
                nameof(DeleteBillById), 
                ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("{billId:guid}/documents")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<BillDocument>> UploadDocumentAsync(
        [FromRoute] Guid billId,
        [FromForm] UploadBillDocumentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
            var userId = GetCurrentUserId();
        
            if (userId is null) 
                return Unauthorized("User is not authenticated.");
        
            using var stream = request.File.OpenReadStream();
            var document = await _documentService.UploadDocumentAsync(
                billId,
                stream,
                request.File.FileName,
                request.File.ContentType,
                request.DocumentType,
                userId
            );
        
            var response = new UploadDocumentResponse
            {
                Id = document.Id,
                BillId = document.BillId,
                FileName = document.FileName,
                ContentType = document.ContentType,
                DocumentType = document.DocumentType
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
    
    [HttpGet("{billId:guid}/documents/{documentId}")]
    public async Task<IActionResult> GetDocument(Guid billId, Guid documentId)
    {
        Bill? bill = await _billsRepository.GetByIdAsync(billId);

        if (bill is null)
        {
            return NotFound();
        }
        
        var (stream, fileName, contentType) = await _documentService.GetDocumentAsync(documentId);
        return File(stream, contentType, fileName);
    }
    
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
    }
}