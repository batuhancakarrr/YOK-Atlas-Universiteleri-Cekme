using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agility;
public class University {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; set; }
	[MaxLength(200), Required]
	public string Name { get; set; }
	[ForeignKey("Type")]
	public int TypeId { get; set; }
	public Type Type { get; set; }
}
