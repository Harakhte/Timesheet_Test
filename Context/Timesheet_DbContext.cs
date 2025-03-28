using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TImesheet_TEST.Models;
using Task = TImesheet_TEST.Models.Task;

namespace TImesheet_TEST.Context;

public partial class Timesheet_DbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public Timesheet_DbContext(DbContextOptions<Timesheet_DbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskProject> TaskProjects { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }
    public DbSet<Timesheet> Timesheets { get; set; }
    
    public Timesheet_DbContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=DESKTOP-LVPTRQ8;Database=Timesheet_Test;Trusted_Connection=True;TrustServerCertificate=True;");
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Projects)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.TasksProjects)
            .WithOne(tp => tp.Project)
            .HasForeignKey(tp => tp.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Project)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Team>()
            .HasMany(t => t.UserTeams)
            .WithOne(ut => ut.Team)
            .HasForeignKey(ut => ut.TeamId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserTeams)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Timesheets)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction); 
        
        modelBuilder.Entity<Task>()
            .HasMany(t => t.TasksProjects)
            .WithOne(tp => tp.Task)
            .HasForeignKey(tp => tp.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<TaskProject>()
            .HasMany(tp => tp.Timesheets)
            .WithOne(t => t.TaskProject)
            .HasForeignKey(t => t.TaskProjectId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction);    
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
