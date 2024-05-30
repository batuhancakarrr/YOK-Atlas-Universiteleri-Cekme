using Microsoft.EntityFrameworkCore;

namespace Agility;
public class UniversityDbContext : DbContext {
	public DbSet<University> Universities { get; set; }
	public DbSet<Department> Departments { get; set; }
	public DbSet<Type> Types { get; set; }
	public DbSet<UniversityDepartment> UniversityDepartments { get; set; }

	public UniversityDbContext() { }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<UniversityDepartment>()
			.HasKey(ud => new { ud.UniversityId, ud.DepartmentId });

		modelBuilder.Entity<UniversityDepartment>()
			.HasOne(ud => ud.University)
			.WithMany()
			.HasForeignKey(ud => ud.UniversityId);

		modelBuilder.Entity<UniversityDepartment>()
			.HasOne(ud => ud.Department)
			.WithMany()
			.HasForeignKey(ud => ud.DepartmentId);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder.UseSqlServer("Data Source=DESKTOP-05G29TQ;Initial Catalog=YokAtlas;Trusted_Connection=True;TrustServerCertificate=True");
	}
}
