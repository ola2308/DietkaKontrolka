using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace KontrolaNawykow.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<CustomFood> CustomFoods { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Nawyk> Nawyki { get; set; }
        public DbSet<NawykWPlanie> NawykiWPlanie { get; set; }
        public DbSet<Statystyki> Statystyki { get; set; }
        public DbSet<PlanPosilkowPrzepis> PlanPosilkowPrzepisy { get; set; }
        public DbSet<ListaZakupow> ListyZakupow { get; set; }
        public DbSet<FridgeItem> FridgeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RecipeIngredient relationships
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe relationships
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RecipeRating relationships
            modelBuilder.Entity<RecipeRating>()
                .HasOne(rr => rr.Recipe)
                .WithMany(r => r.Ratings)
                .HasForeignKey(rr => rr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeRating>()
                .HasOne(rr => rr.User)
                .WithMany(u => u.RecipeRatings)
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Ensure one user can rate a recipe only once
            modelBuilder.Entity<RecipeRating>()
                .HasIndex(rr => new { rr.RecipeId, rr.UserId })
                .IsUnique();

            // MealPlan relationships
            modelBuilder.Entity<MealPlan>()
                .HasOne(mp => mp.User)
                .WithMany(u => u.MealPlans)
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MealPlan>()
                .HasOne(mp => mp.Recipe)
                .WithMany(r => r.MealPlans)
                .HasForeignKey(mp => mp.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);

            // CustomFood relationships
            modelBuilder.Entity<CustomFood>()
                .HasOne(cf => cf.User)
                .WithMany(u => u.CustomFoods)
                .HasForeignKey(cf => cf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShoppingList relationships
            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.ShoppingLists)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingList>()
                .HasOne(sl => sl.Ingredient)
                .WithMany(i => i.ShoppingLists)
                .HasForeignKey(sl => sl.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            // ToDo relationships
            modelBuilder.Entity<ToDo>()
                .HasOne(td => td.User)
                .WithMany(u => u.ToDos)
                .HasForeignKey(td => td.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // FridgeItem relationships
            modelBuilder.Entity<FridgeItem>()
                .HasOne(fi => fi.User)
                .WithMany()
                .HasForeignKey(fi => fi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // NawykWPlanie relationships
            modelBuilder.Entity<NawykWPlanie>()
                .HasOne(nwp => nwp.Nawyk)
                .WithMany(n => n.NawykiWPlanie)
                .HasForeignKey(nwp => nwp.NawykId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NawykWPlanie>()
                .HasOne(nwp => nwp.Uzytkownik)
                .WithMany(u => u.NawykiWPlanie)
                .HasForeignKey(nwp => nwp.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);

            // Statystyki relationships
            modelBuilder.Entity<Statystyki>()
                .HasOne(s => s.Uzytkownik)
                .WithMany(u => u.Statystyki)
                .HasForeignKey(s => s.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);

            // PlanPosilkowPrzepis relationships
            modelBuilder.Entity<PlanPosilkowPrzepis>()
                .HasOne(ppp => ppp.PlanPosilkow)
                .WithMany(pp => pp.PlanPosilkowPrzepisy)
                .HasForeignKey(ppp => ppp.PlanPosilkowId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanPosilkowPrzepis>()
                .HasOne(ppp => ppp.Przepis)
                .WithMany(r => r.PlanPosilkowPrzepisy)
                .HasForeignKey(ppp => ppp.PrzepisId)
                .OnDelete(DeleteBehavior.Restrict);

            // ListaZakupow relationships
            modelBuilder.Entity<ListaZakupow>()
                .HasOne(lz => lz.PlanPosilkow)
                .WithMany(pp => pp.ListaZakupow)
                .HasForeignKey(lz => lz.PlanPosilkowId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for weight and BMI fields (using decimal instead of double)
            modelBuilder.Entity<User>()
                .Property(u => u.Waga)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<User>()
                .Property(u => u.Wzrost)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<User>()
                .Property(u => u.CustomBmi)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Statystyki>()
                .Property(s => s.Waga)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Statystyki>()
                .Property(s => s.ZmianaWagi)
                .HasColumnType("decimal(5,2)");

            // Configure nutritional values as float (real in SQL Server) - NO PRECISION for float/real
            // Float/real types in SQL Server don't support precision specification

            // For amounts that need precision, use decimal instead
            modelBuilder.Entity<RecipeIngredient>()
                .Property(ri => ri.Amount)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<MealPlan>()
                .Property(mp => mp.Gramature)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<MealPlan>()
                .Property(mp => mp.CustomProtein)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<MealPlan>()
                .Property(mp => mp.CustomCarbs)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<MealPlan>()
                .Property(mp => mp.CustomFat)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<CustomFood>()
                .Property(cf => cf.Protein)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<CustomFood>()
                .Property(cf => cf.Fat)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<CustomFood>()
                .Property(cf => cf.Carbs)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<ShoppingList>()
                .Property(sl => sl.Amount)
                .HasColumnType("decimal(7,2)");

            // Add indexes for better query performance
            modelBuilder.Entity<Recipe>()
                .HasIndex(r => r.IsPublic);

            modelBuilder.Entity<Recipe>()
                .HasIndex(r => r.Name);

            modelBuilder.Entity<MealPlan>()
                .HasIndex(mp => mp.Date);

            modelBuilder.Entity<MealPlan>()
                .HasIndex(mp => new { mp.UserId, mp.Date });

            modelBuilder.Entity<ToDo>()
                .HasIndex(td => new { td.UserId, td.IsCompleted });

            modelBuilder.Entity<RecipeRating>()
                .HasIndex(rr => rr.CreatedAt);

            modelBuilder.Entity<Statystyki>()
                .HasIndex(s => new { s.UzytkownikId, s.Data });
        }
    }
}