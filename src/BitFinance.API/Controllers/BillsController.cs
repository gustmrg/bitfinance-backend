using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;
using Asp.Versioning;
using BitFinance.API.Attributes;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.API.Services.Interfaces;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;
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
    [EndpointSummary("Create a bill")]
    [EndpointDescription("Creates a new bill within the specified organization.")]
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
                AmountDue = request.AmountDue,
                AmountPaid = request.AmountPaid,
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
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
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
    [EndpointSummary("List bills")]
    [EndpointDescription("Returns a paginated list of bills for the organization. Supports optional date range filtering.")]
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
                    AmountDue = bill.AmountDue,
                    AmountPaid = bill.AmountPaid,
                    Documents = bill.Documents.Select(doc => new DocumentResponseModel
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
    [EndpointSummary("Get bill details")]
    [EndpointDescription("Returns the details and attached documents of a specific bill.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GetBillResponse>> GetBillById([FromRoute] Guid organizationId, [FromRoute] Guid billId)
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
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid,
                Documents = bill.Documents.Select(doc => new DocumentResponseModel
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
    [EndpointSummary("Update a bill")]
    [EndpointDescription("Updates the details of an existing bill.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<UpdateBillResponse>> UpdateBill([FromRoute] Guid organizationId, Guid billId, [FromBody] UpdateBillRequest request)
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
            bill.AmountDue = request.AmountDue;
            bill.AmountPaid = request.AmountPaid;

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
                AmountDue = bill.AmountDue,
                AmountPaid = bill.AmountPaid
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
    [EndpointSummary("Delete a bill")]
    [EndpointDescription("Soft-deletes a bill by setting its deleted timestamp.")]
    public async Task<ActionResult> DeleteBillById([FromRoute] Guid organizationId, Guid billId)
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
    [EndpointSummary("Upload a bill document")]
    [EndpointDescription("Uploads a file attachment to an existing bill.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<BillDocument>> UploadDocumentAsync(
        [FromRoute] Guid organizationId,
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
    [EndpointSummary("Download a bill document")]
    [EndpointDescription("Downloads a specific document attached to a bill.")]
    public async Task<IActionResult> GetDocument([FromRoute] Guid organizationId, Guid billId, Guid documentId)
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