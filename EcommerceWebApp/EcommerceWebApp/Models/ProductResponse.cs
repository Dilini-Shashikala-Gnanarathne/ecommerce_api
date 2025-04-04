
public class ProductResponse
{
    internal DateTime CreatedAt;
    internal DateTime? UpdatedAt;
    internal string CreatedBy;

    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new List<string>();
    public string? UserNic { get; internal set; }
}