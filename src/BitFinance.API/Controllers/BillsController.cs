using System.Globalization;
using System.Security.Claims;
using BitFinance.API.Attributes;
using BitFinance.API.Extensions;
using BitFinance.API.Models;
using BitFinance.API.Models.Request;
using BitFinance.API.Models.Response;
using BitFinance.Application.Common;
using BitFinance.Application.DTOs.Bills;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BitFinance.API.Controllers;

[ApiController]
[Authorize]
[OrganizationAuthorization]
[Route("api/organizations/{organizationId:guid}/bills")]
public class BillsController : ControllerBase
{
    private readonly IBillService _billService;
    private readonly IBillDocumentService _documentService;
    private readonly ILogger<BillsController> _logger;
    
    public BillsController(
        IBillService billService,
        IBillDocumentService documentService,
        ILogger<BillsController> logger)
    {
        _billService = billService;
        _documentService = documentService;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBillAsync(
        [FromRoute] Guid organizationId,
        [FromBody] CreateBillRequestDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var billResult = await _billService.CreateAsync(organizationId, request, cancellationToken);
            
            if (!billResult.IsSuccess) return billResult.ToActionResult();

            return billResult.ToCreatedResult(nameof(GetBillById), "Bills", new
            {
                organizationId = organizationId,
                billId = billResult.Data!.Id
            });
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
    public async Task<IActionResult> GetBillsAsync(
        [FromRoute] Guid organizationId, 
        [FromQuery] int page = 1, int pageSize = 100, DateTime? from = null, DateTime? to = null)
    {
        try
        {
            var totalRecords = 100; // create method of retrieving records
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var bills = await _billService.GetAllByOrganizationAsync(organizationId, page, pageSize, from, to);

            var billsDto = bills.Select(bill => new GetBillResponse
                {
                    Id = bill.Id,
                    Description = bill.Description,
                    Category = bill.Category,
                    Status = bill.Status,
                    CreatedAt = bill.CreatedAt,
                    DueDate = bill.DueDate,
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
    /*
    
    [HttpGet]
    [Route("{billId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetBillById([FromRoute] Guid organizationId, [FromRoute] Guid billId)
    {
        try
        {
            var billResult = await _billService.GetByIdAsync(billId);

            return billResult.ToActionResult();

            // if (bill is null)
            // {
            //     return NotFound();
            // }
            //
            // var response = new GetBillResponse
            // {
            //     Id = bill.Id,
            //     Description = bill.Description,
            //     Category = bill.Category,
            //     Status = bill.Status,
            //     CreatedAt = bill.CreatedAt,
            //     DueDate = bill.DueDate,
            //     PaymentDate = bill.PaymentDate,
            //     AmountDue = bill.AmountDue,
            //     AmountPaid = bill.AmountPaid,
            //     Documents = bill.Documents.Select(doc => new DocumentResponseModel
            //     {
            //         Id = doc.Id,
            //         FileName = doc.FileName,
            //         ContentType = doc.ContentType,
            //         DocumentType = doc.DocumentType
            //     }).ToList()
            // };
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
    
    /*
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
            bill.DueDate = request.DueDate.ToUniversalTime();
            bill.PaymentDate = request.PaymentDate?.ToUniversalTime();
            bill.AmountDue = request.AmountDue;
            bill.AmountPaid = request.AmountPaid;

            await _billService.UpdateAsync(bill);

            var response = new UpdateBillResponse
            {
                Id = bill.Id,
                Description = bill.Description,
                Category = bill.Category,
                Status = bill.Status,
                DueDate = bill.DueDate,
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
    public async Task<ActionResult> DeleteBillById(Guid billId)
    {
        try
        {
            Bill? bill = await _billService.GetByIdAsync(billId);

            if (bill is null)
            {
                return NotFound();
            }

            if (bill.DeletedAt is not null)
            {
                return BadRequest();
            }

            await _billService.DeleteAsync(billId);

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
        Bill? bill = await _billService.GetByIdAsync(billId);

        if (bill is null)
        {
            return NotFound();
        }
        
        var (stream, fileName, contentType) = await _documentService.GetDocumentAsync(documentId);
        return File(stream, contentType, fileName);
    }
    
    */
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
    }
}