﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KontrolaNawykow.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public Gender? Plec { get; set; }

        public int? Wiek { get; set; }
        public double? Waga { get; set; }
        public double? Wzrost { get; set; }

        [MaxLength(50)]
        public string? AktywnoscFizyczna { get; set; }

        [MaxLength(50)]
        public string? RodzajPracy { get; set; }

        public UserGoal? Cel { get; set; }
        public double? CustomBmi { get; set; }
        public int? CustomCaloriesDeficit { get; set; }
        public int? CustomProteinGrams { get; set; }
        public int? CustomCarbsGrams { get; set; }
        public int? CustomFatGrams { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public List<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
        public List<CustomFood> CustomFoods { get; set; } = new List<CustomFood>();
        public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
        public List<ToDo> ToDos { get; set; } = new List<ToDo>();
        public List<Statystyki> Statystyki { get; set; } = new List<Statystyki>();
        public List<NawykWPlanie> NawykiWPlanie { get; set; } = new List<NawykWPlanie>();
        public List<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();
    }

    public enum Gender
    {
        Mezczyzna,
        Kobieta
    }

    public enum UserGoal
    {
        ZdroweNawyki,
        Schudniecie,
        PrzybranieMasy
    }

    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int Calories { get; set; }
        public float Protein { get; set; }
        public float Fat { get; set; }
        public float Carbs { get; set; }
        public string Instructions { get; set; } = null!;
        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsPublic { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
        public List<PlanPosilkowPrzepis> PlanPosilkowPrzepisy { get; set; } = new List<PlanPosilkowPrzepis>();
        public List<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();
    }

    public class RecipeRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }

    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public float? Amount { get; set; }
    }

    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int Calories { get; set; }
        public float Protein { get; set; }
        public float Fat { get; set; }
        public float Carbs { get; set; }

        public string Unit { get; set; } = "g";

        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
    }

    public class MealPlan
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public MealType MealType { get; set; }
        public DateTime? Date { get; set; }

        public int? RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }

        public string CustomEntry { get; set; } = null!;
        public bool Eaten { get; set; }
        public float? Gramature { get; set; } = 100;

        public List<PlanPosilkowPrzepis> PlanPosilkowPrzepisy { get; set; } = new List<PlanPosilkowPrzepis>();
        public List<ListaZakupow> ListaZakupow { get; set; } = new List<ListaZakupow>();
        public int? CustomCalories { get; set; }
        public float? CustomProtein { get; set; }
        public float? CustomCarbs { get; set; }
        public float? CustomFat { get; set; }
    }

    public class PlanPosilkowPrzepis
    {
        [Key]
        public int Id { get; set; }

        public int PlanPosilkowId { get; set; }
        public MealPlan PlanPosilkow { get; set; } = null!;

        public int PrzepisId { get; set; }
        public Recipe Przepis { get; set; } = null!;
    }

    public enum MealType
    {
        Sniadanie,
        Obiad,
        Kolacja,
        Przekaska
    }

    public class CustomFood
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Name { get; set; } = null!;
        public int Calories { get; set; }
        public float Protein { get; set; }
        public float Fat { get; set; }
        public float Carbs { get; set; }
    }

    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public float Amount { get; set; }
        public bool Status { get; set; }
    }

    public class ListaZakupow
    {
        [Key]
        public int Id { get; set; }

        public int PlanPosilkowId { get; set; }
        public MealPlan PlanPosilkow { get; set; } = null!;

        public StatusRodzaj Status { get; set; }
    }

    public enum StatusRodzaj
    {
        Niezakupione,
        Zakupione
    }

    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Task { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsTemplate { get; set; } = false;
    }

    public class Nawyk
    {
        [Key]
        public int Id { get; set; }

        public string Nazwa { get; set; } = null!;
        public bool Cykliczny { get; set; }
        public bool Wykonany { get; set; }
        public DateTime DataUtworzenia { get; set; } = DateTime.UtcNow;

        public List<NawykWPlanie> NawykiWPlanie { get; set; } = new List<NawykWPlanie>();
    }

    public class NawykWPlanie
    {
        [Key]
        public int Id { get; set; }

        public int NawykId { get; set; }
        [ForeignKey("NawykId")]
        public Nawyk Nawyk { get; set; } = null!;

        public DzienTygodnia Dzien { get; set; }

        public int UzytkownikId { get; set; }
        [ForeignKey("UzytkownikId")]
        public User Uzytkownik { get; set; } = null!;
    }

    public enum DzienTygodnia
    {
        Poniedzialek,
        Wtorek,
        Sroda,
        Czwartek,
        Piatek,
        Sobota,
        Niedziela
    }

    public class Statystyki
    {
        [Key]
        public int Id { get; set; }

        public int UzytkownikId { get; set; }
        [ForeignKey("UzytkownikId")]
        public User Uzytkownik { get; set; } = null!;

        public DateTime Data { get; set; }
        public double Waga { get; set; }
        public int DniDiety { get; set; }
        public double ZmianaWagi { get; set; }
    }

    public class UserStatisticsViewModel
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public int TotalRecipes { get; set; }
        public int TotalMealPlans { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
    }

    public class FridgeItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Podaj ilosc")]
        public int Amount { get; set; }

        [Required]
        public string Unit { get; set; } = "g";

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}