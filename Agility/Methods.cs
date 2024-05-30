using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace Agility;
public class Methods {
	public async Task<List<University>> GetAllUniversitiesAsync() {
		List<University> universities = [];

		string url = "https://yokatlas.yok.gov.tr/lisans-anasayfa.php";
		HtmlWeb web = new();
		HtmlDocument doc = await web.LoadFromWebAsync(url);

		HtmlNodeCollection optgroupNodes = doc.DocumentNode.SelectNodes("//select[@id='univ']//optgroup");

		if (optgroupNodes != null) {
			foreach (HtmlNode optgroupNode in optgroupNodes) {
				string label = optgroupNode.GetAttributeValue("label", "");

				HtmlNodeCollection optionNodes = optgroupNode.SelectNodes(".//option");

				if (optionNodes != null) {
					foreach (HtmlNode optionNode in optionNodes) {
						string name = optionNode.InnerText.Trim();
						string value = optionNode.GetAttributeValue("value", "");

						universities.Add(new University {
							Id = Convert.ToInt32(value),
							Name = name,
							TypeId = GetGroupLabel(label)
						});
					}
				}
				else {
					Console.WriteLine("No options found.");
				}
			}
		}
		else {
			Console.WriteLine("No optgroups found.");
		}

		return universities;
	}
	private int GetGroupLabel(string label) {
		return label.ToLower() switch {
			"devlet üniversiteleri" => 1,
			"vakıf üniversiteleri" => 2,
			"kktc üniversiteleri" => 3,
			"yurtdışı üniversiteleri" => 4,
			_ => 0,
		};
	}
	public async Task<List<Department>> GetAllDepartmentsAsync() {
		List<Department> departments = [];

		string url = "https://yokatlas.yok.gov.tr/lisans-anasayfa.php";
		HtmlWeb web = new();
		HtmlDocument doc = await web.LoadFromWebAsync(url);

		HtmlNodeCollection optionNodes = doc.DocumentNode.SelectNodes("//select[@id='bolum']//option");

		if (optionNodes != null) {
			foreach (HtmlNode optionNode in optionNodes) {
				string name = optionNode.InnerText.Trim();
				string value = optionNode.GetAttributeValue("value", "");

				departments.Add(new Department {
					Name = name,
					Id = Convert.ToInt32(value)
				});
			}
		}
		else {
			Console.WriteLine("No options found.");
		}

		return departments;
	}
	public async Task<List<UniversityDepartment>> GetDepartmentsForUniversityAsync(UniversityDbContext context, int universityId) {
		List<UniversityDepartment> universityDepartments = [];

		HtmlWeb web = new();
		HtmlDocument doc = await web.LoadFromWebAsync("https://yokatlas.yok.gov.tr/lisans-univ.php?u=" + universityId);
		HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//h4[@class='panel-title']//a//div");

		if (collection != null) {
			foreach (HtmlNode divNode in collection) {
				string departmentName = divNode.InnerText.Trim();

				Department dbDepartment = context.Departments.AsNoTracking().FirstOrDefault(x => x.Name == departmentName);
				if (dbDepartment == null) {
					dbDepartment = new() { Name = departmentName };
					context.Departments.Add(dbDepartment);
					context.SaveChanges();
				}

				UniversityDepartment universityDepartment = new() {
					UniversityId = universityId,
					DepartmentId = dbDepartment.Id
				};
				universityDepartments.Add(universityDepartment);
			}
		}
		return universityDepartments.DistinctBy(x => new { x.UniversityId, x.DepartmentId }).ToList();
	}
}
