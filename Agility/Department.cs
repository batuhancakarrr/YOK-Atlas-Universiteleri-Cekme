using System.ComponentModel.DataAnnotations;

namespace Agility;
public class Department {
	[Key]
	public int Id { get; set; }
	[MaxLength(200), Required]
	public string Name { get; set; }
}
