using System.ComponentModel.DataAnnotations;
namespace Synapse.OrderStatus.Domain.Entities.Base;
public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public string CreateBy { get; set; } = string.Empty;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    
    public string UpdateBy { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
