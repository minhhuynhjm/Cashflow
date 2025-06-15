using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashflowAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert sample users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserName", "Email", "PasswordHash", "FullName", "CreatedAt", "LastLoginAt", "IsActive" },
                values: new object[,]
                {
                    {
                        "john.doe",
                        "john.doe@example.com",
                        "AQAAAAEAACcQAAAAELbXp1QrHhX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5Q==",
                        "John Doe",
                        DateTime.UtcNow.AddMonths(-3),
                        DateTime.UtcNow.AddDays(-1),
                        true
                    },
                    {
                        "jane.smith",
                        "jane.smith@example.com",
                        "AQAAAAEAACcQAAAAELbXp1QrHhX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5Q==",
                        "Jane Smith",
                        DateTime.UtcNow.AddMonths(-2),
                        DateTime.UtcNow.AddDays(-2),
                        true
                    },
                    {
                        "bob.wilson",
                        "bob.wilson@example.com",
                        "AQAAAAEAACcQAAAAELbXp1QrHhX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5YzX5Q==",
                        "Bob Wilson",
                        DateTime.UtcNow.AddMonths(-1),
                        DateTime.UtcNow.AddDays(-3),
                        true
                    }
                }
            );

            // Insert sample categories for each user
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name", "Description", "Type", "Icon", "Color", "UserId", "CreatedAt", "IsDefault" },
                values: new object[,]
                {
                    // Income Categories
                    { "Salary", "Monthly salary income", "Income", "money-bill", "#4CAF50", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Freelance", "Freelance work income", "Income", "laptop-code", "#8BC34A", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Investments", "Investment returns", "Income", "chart-line", "#009688", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Salary", "Monthly salary income", "Income", "money-bill", "#4CAF50", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Side Business", "Side business income", "Income", "store", "#8BC34A", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Salary", "Monthly salary income", "Income", "money-bill", "#4CAF50", 3, DateTime.UtcNow.AddMonths(-1), true },

                    // Expense Categories
                    { "Food & Dining", "Groceries and restaurant expenses", "Expense", "utensils", "#F44336", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Transportation", "Public transport and fuel costs", "Expense", "car", "#2196F3", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Shopping", "Clothing and personal items", "Expense", "shopping-cart", "#9C27B0", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Entertainment", "Movies, games, and leisure activities", "Expense", "film", "#FF9800", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Bills & Utilities", "Monthly bills and utility payments", "Expense", "file-invoice", "#607D8B", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Health & Medical", "Healthcare and medical expenses", "Expense", "heartbeat", "#E91E63", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Education", "Tuition and educational expenses", "Expense", "graduation-cap", "#795548", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Travel", "Travel and vacation expenses", "Expense", "plane", "#00BCD4", 1, DateTime.UtcNow.AddMonths(-3), true },
                    { "Gifts", "Gifts and donations", "Expense", "gift", "#FFC107", 1, DateTime.UtcNow.AddMonths(-3), true },

                    { "Food & Dining", "Groceries and restaurant expenses", "Expense", "utensils", "#F44336", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Transportation", "Public transport and fuel costs", "Expense", "car", "#2196F3", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Shopping", "Clothing and personal items", "Expense", "shopping-cart", "#9C27B0", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Entertainment", "Movies, games, and leisure activities", "Expense", "film", "#FF9800", 2, DateTime.UtcNow.AddMonths(-2), true },
                    { "Bills & Utilities", "Monthly bills and utility payments", "Expense", "file-invoice", "#607D8B", 2, DateTime.UtcNow.AddMonths(-2), true },

                    { "Food & Dining", "Groceries and restaurant expenses", "Expense", "utensils", "#F44336", 3, DateTime.UtcNow.AddMonths(-1), true },
                    { "Transportation", "Public transport and fuel costs", "Expense", "car", "#2196F3", 3, DateTime.UtcNow.AddMonths(-1), true },
                    { "Shopping", "Clothing and personal items", "Expense", "shopping-cart", "#9C27B0", 3, DateTime.UtcNow.AddMonths(-1), true },
                    { "Entertainment", "Movies, games, and leisure activities", "Expense", "film", "#FF9800", 3, DateTime.UtcNow.AddMonths(-1), true }
                }
            );

            // Insert sample transactions for each user
            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Amount", "Description", "TransactionDate", "Type", "CategoryId", "UserId", "CreatedAt", "Notes" },
                values: new object[,]
                {
                    // John Doe's Transactions (User 1)
                    { 5000.00m, "Monthly salary", DateTime.UtcNow.AddDays(-5), "Income", 1, 1, DateTime.UtcNow, "Regular monthly salary payment" },
                    { 1500.00m, "Freelance project", DateTime.UtcNow.AddDays(-10), "Income", 2, 1, DateTime.UtcNow, "Website development project" },
                    { 300.00m, "Stock dividends", DateTime.UtcNow.AddDays(-15), "Income", 3, 1, DateTime.UtcNow, "Quarterly stock dividends" },
                    { -150.00m, "Grocery shopping", DateTime.UtcNow.AddDays(-3), "Expense", 7, 1, DateTime.UtcNow, "Weekly groceries at supermarket" },
                    { -50.00m, "Bus pass", DateTime.UtcNow.AddDays(-2), "Expense", 8, 1, DateTime.UtcNow, "Monthly bus pass" },
                    { -200.00m, "New clothes", DateTime.UtcNow.AddDays(-1), "Expense", 9, 1, DateTime.UtcNow, "New winter jacket" },
                    { -30.00m, "Movie tickets", DateTime.UtcNow, "Expense", 10, 1, DateTime.UtcNow, "Weekend movie with friends" },
                    { -120.00m, "Electricity bill", DateTime.UtcNow.AddDays(-4), "Expense", 11, 1, DateTime.UtcNow, "Monthly electricity payment" },
                    { -80.00m, "Doctor visit", DateTime.UtcNow.AddDays(-6), "Expense", 12, 1, DateTime.UtcNow, "Regular checkup" },
                    { -300.00m, "Online course", DateTime.UtcNow.AddDays(-7), "Expense", 13, 1, DateTime.UtcNow, "Advanced programming course" },
                    { -500.00m, "Weekend trip", DateTime.UtcNow.AddDays(-8), "Expense", 14, 1, DateTime.UtcNow, "Weekend getaway" },
                    { -100.00m, "Birthday gift", DateTime.UtcNow.AddDays(-9), "Expense", 15, 1, DateTime.UtcNow, "Friend's birthday gift" },

                    // Jane Smith's Transactions (User 2)
                    { 4500.00m, "Monthly salary", DateTime.UtcNow.AddDays(-5), "Income", 4, 2, DateTime.UtcNow, "Regular monthly salary payment" },
                    { 800.00m, "Side business income", DateTime.UtcNow.AddDays(-12), "Income", 5, 2, DateTime.UtcNow, "Online store sales" },
                    { -120.00m, "Grocery shopping", DateTime.UtcNow.AddDays(-3), "Expense", 16, 2, DateTime.UtcNow, "Weekly groceries" },
                    { -40.00m, "Train ticket", DateTime.UtcNow.AddDays(-2), "Expense", 17, 2, DateTime.UtcNow, "Monthly train pass" },
                    { -150.00m, "New shoes", DateTime.UtcNow.AddDays(-1), "Expense", 18, 2, DateTime.UtcNow, "Running shoes" },
                    { -25.00m, "Concert tickets", DateTime.UtcNow, "Expense", 19, 2, DateTime.UtcNow, "Local band concert" },
                    { -90.00m, "Internet bill", DateTime.UtcNow.AddDays(-4), "Expense", 20, 2, DateTime.UtcNow, "Monthly internet payment" },

                    // Bob Wilson's Transactions (User 3)
                    { 4000.00m, "Monthly salary", DateTime.UtcNow.AddDays(-5), "Income", 6, 3, DateTime.UtcNow, "Regular monthly salary payment" },
                    { -100.00m, "Grocery shopping", DateTime.UtcNow.AddDays(-3), "Expense", 21, 3, DateTime.UtcNow, "Weekly groceries" },
                    { -35.00m, "Bus ticket", DateTime.UtcNow.AddDays(-2), "Expense", 22, 3, DateTime.UtcNow, "Monthly bus pass" },
                    { -180.00m, "New phone case", DateTime.UtcNow.AddDays(-1), "Expense", 23, 3, DateTime.UtcNow, "Phone accessories" },
                    { -40.00m, "Game subscription", DateTime.UtcNow, "Expense", 24, 3, DateTime.UtcNow, "Monthly game pass" }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}