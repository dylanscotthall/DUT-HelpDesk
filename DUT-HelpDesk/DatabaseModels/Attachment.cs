namespace DUT_HelpDesk.DatabaseModels;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public int? TicketId { get; set; }

    public int? ReplyId { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileContent { get; set; }

    public string? ContentType { get; set; }

    public virtual Reply? Reply { get; set; }

    public virtual Ticket? Ticket { get; set; }

    override
    public string ToString()
    {
        return $"{FileName} /n {FileContent} /n {ContentType}";
    }
}
