using Microsoft.EntityFrameworkCore;
using OrganizacnaStrukturaFirmy.Models;

namespace YourProjectName.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Company -> Manager
        modelBuilder.Entity<Company>()
            .HasOne(c => c.Manager)
            .WithMany()
            .HasForeignKey(c => c.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Division -> Company
        modelBuilder.Entity<Division>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Divisions)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Division -> Manager
        modelBuilder.Entity<Division>()
            .HasOne(d => d.Manager)
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Project -> Division
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Division)
            .WithMany(d => d.Projects)
            .HasForeignKey(p => p.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Project -> Manager
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Manager)
            .WithMany()
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Department -> Project
        modelBuilder.Entity<Department>()
            .HasOne(d => d.Project)
            .WithMany(p => p.Departments)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        // Department -> Manager
        modelBuilder.Entity<Department>()
            .HasOne(d => d.Manager)
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Company
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Company)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Department
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}