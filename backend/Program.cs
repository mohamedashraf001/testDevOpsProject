using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // ده بيأكد إن الجداول بتتكريت لو مش موجودة
    db.Database.EnsureCreated();
}

// 1. جلب كل الموظفين (Read)
app.MapGet("/api/employees", async (AppDbContext db) =>
{
    return await db.Employees.ToListAsync();
});

// 2. إضافة موظف جديد (Create)
app.MapPost("/api/employees", async (AppDbContext db, Employee emp) =>
{
    db.Employees.Add(emp);
    await db.SaveChangesAsync();
    return Results.Created($"/api/employees/{emp.Id}", emp);
});

// 3. تعديل بيانات موظف (Update)
app.MapPut("/api/employees/{id}", async (int id, AppDbContext db, Employee inputEmp) =>
{
    var emp = await db.Employees.FindAsync(id);
    if (emp is null) return Results.NotFound();

    emp.Name = inputEmp.Name;
    emp.Position = inputEmp.Position;
    emp.Department = inputEmp.Department;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 4. حذف موظف (Delete)
app.MapDelete("/api/employees/{id}", async (int id, AppDbContext db) =>
{
    var emp = await db.Employees.FindAsync(id);
    if (emp is null) return Results.NotFound();

    db.Employees.Remove(emp);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

// ---------------------------------
// شكل قاعدة البيانات الجديدة (3 أعمدة)
// ---------------------------------
class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Employee> Employees { get; set; }
}

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}