using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using projectX.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Screening> Screenings { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieActors> MovieActors { get; set; }
    public DbSet<ScreeningMovies> ScreeningMovies { get; set; }
    public DbSet<ScreeningCinemas> ScreeningCinemas { get; set; }
    public DbSet<MovieGenres> MovieGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieActors>()                    //MovieActors - mejdinna        M:N
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActors>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(a => a.ActorId);

        modelBuilder.Entity<MovieActors>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(m => m.MovieId);


        modelBuilder.Entity<ScreeningMovies>()                //ScreeningMovies - mejdinna    M:N
            .HasKey(ma => new { ma.ScreeningId, ma.MovieId });

        modelBuilder.Entity<ScreeningMovies>()
            .HasOne(sm => sm.Movie)
            .WithMany(s => s.ScreeningMovies)
            .HasForeignKey(m => m.MovieId);

        modelBuilder.Entity<ScreeningMovies>()
            .HasOne(sm => sm.Screening)
            .WithMany(s => s.ScreeningMovies)
            .HasForeignKey(sm => sm.ScreeningId);

        modelBuilder.Entity<MovieGenres>()                    //MovieGenres - mejdinna        M:N
            .HasKey(ma => new { ma.MovieId, ma.GenreId });

        modelBuilder.Entity<MovieGenres>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        modelBuilder.Entity<MovieGenres>()
            .HasOne(mg => mg.Genre)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);

        modelBuilder.Entity<ScreeningCinemas>()               //ScreeningCinemas - mejdinna   M:N
            .HasKey(ma => new { ma.ScreeningId, ma.CinemaId });

        modelBuilder.Entity<ScreeningCinemas>()
            .HasOne(mg => mg.Screening)
            .WithMany(m => m.ScreeningCinemas)
            .HasForeignKey(mg => mg.ScreeningId);

        modelBuilder.Entity<ScreeningCinemas>()
            .HasOne(mg => mg.Cinema)
            .WithMany(m => m.ScreeningCinemas)
            .HasForeignKey(mg => mg.CinemaId);


        modelBuilder.Entity<Hall>()    //Halls sus Kino         1:M   1
            .HasOne(t => t.Cinema)
            .WithMany(c => c.Halls)
            .HasForeignKey(c => c.CinemaId);

        modelBuilder.Entity<Ticket>()   //Ticket sus Screening   1:M   2
            .HasOne(s => s.Screening)
            .WithMany(t => t.Tickets)
            .HasForeignKey(t => t.ScreeningId);

        modelBuilder.Entity<Ticket>()   //Ticket sus User        1:M   3
            .HasOne(s => s.User)
            .WithMany(t => t.Tickets)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Review>()   //Review sus User        1:M   4
            .HasOne(u => u.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<Review>()   //Review sus Movie       1:M   5
            .HasOne(u => u.Movie)
            .WithMany(u => u.Reviews)
            .HasForeignKey(u => u.MovieId);

    }
}
