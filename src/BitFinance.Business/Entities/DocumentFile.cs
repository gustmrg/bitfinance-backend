namespace BitFinance.Business.Entities;

public class DocumentFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid BillId { get; set; }
}