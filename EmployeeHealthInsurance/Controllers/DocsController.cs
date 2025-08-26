using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

[Authorize(Roles = "Admin,HRManager")]
public class DocsController : Controller
{
	[HttpGet]
	public IActionResult Glossary()
	{
		using var stream = new MemoryStream();

		Document.Create(container =>
		{
			container.Page(page =>
			{
				page.Margin(30);
				page.Size(PageSizes.A4);
				page.PageColor(Colors.White);

				page.Header().Text("Employee Group Health Insurance System — Technical Glossary").SemiBold().FontSize(18);

				page.Content().Column(col =>
				{
					col.Spacing(8);

					col.Item().Text("Architecture").SemiBold().FontSize(14);
					col.Item().Text("• ASP.NET Core 8 MVC: Web framework with MVC pattern and Razor Views.");
					col.Item().Text("• Layered Architecture: Controllers → Services → Data (EF Core).");
					col.Item().Text("• Dependency Injection (DI): Built-in IoC container for services.");
					col.Item().Text("• Middleware Pipeline: HTTPS, Static Files, Routing, Auth, Authorization.");

					col.Item().Text("Security").SemiBold().FontSize(14);
					col.Item().Text("• ASP.NET Core Identity: Cookie-based authentication and user management.");
					col.Item().Text("• Role-based Authorization: Admin, HRManager, Employee via [Authorize(Roles=...)].");
					col.Item().Text("• Anti-forgery: [ValidateAntiForgeryToken] on POST actions.");

					col.Item().Text("Data & Persistence").SemiBold().FontSize(14);
					col.Item().Text("• Entity Framework Core: ORM for SQL Server.");
					col.Item().Text("• DbContext & DbSet: Unit of work and table abstractions.");
					col.Item().Text("• Migrations: Code-first DB schema evolution; auto-applied on startup.");
					col.Item().Text("• Eager Loading: Include/ThenInclude to load related entities.");
					col.Item().Text("• Enum Conversions: Store enums as int/string for readability.");

					col.Item().Text("Domain").SemiBold().FontSize(14);
					col.Item().Text("• Organization, Employee, Policy, Enrollment, Claim, EnrollmentDependent.");
					col.Item().Text("• Relationships: Organization 1-* Employee; Policy 1-* Enrollment; Enrollment 1-* Claim.");
					col.Item().Text("• Statuses: EnrollmentStatus (ACTIVE/CANCELLED), ClaimStatus (SUBMITTED/APPROVED/REJECTED).");

					col.Item().Text("Features & Workflows").SemiBold().FontSize(14);
					col.Item().Text("• Authentication: Login via Identity; cookie auth.");
					col.Item().Text("• Employee Management: HR registers employees; Identity user created with role.");
					col.Item().Text("• Enrollments: Employees enroll in policies; cancel to set CANCELLED.");
					col.Item().Text("• Claims: Employees submit; HR updates status.");
					col.Item().Text("• Premium Calculation: Based on base premium, policy type, age, dependents.");
					col.Item().Text("• Reporting: QuestPDF (PDF), ClosedXML (Excel) downloadable files.");

					col.Item().Text("Tooling & Ops").SemiBold().FontSize(14);
					col.Item().Text("• Swagger/Swashbuckle: API docs enabled in Development.");
					col.Item().Text("• Configuration: ConnectionStrings in appsettings.json; cookie paths for login/denied.");
				});

				page.Footer().AlignRight().Text(txt =>
				{
					txt.DefaultTextStyle(t => t.FontSize(10));
					txt.Span("Generated on: ");
					txt.Span(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
				});
			});
		}).GeneratePdf(stream);

		stream.Position = 0;
		return File(stream, "application/pdf", "TechnicalGlossary.pdf");
	}
}