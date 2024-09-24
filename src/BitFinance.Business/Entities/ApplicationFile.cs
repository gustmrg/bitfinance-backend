namespace BitFinance.Business.Entities;

public class ApplicationFile
{
    public ApplicationFile(byte[] content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }

    public Guid Id { get; set; }
    public byte[] Content { get; set; }
}