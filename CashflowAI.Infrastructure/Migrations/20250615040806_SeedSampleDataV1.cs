using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashflowAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleDataV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Generate 1000 sample transactions for 2024-2025
            var rand = new Random(42);
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2025, 6, 30);
            var descriptions = new[] { "Salary", "Freelance", "Groceries", "Bus pass", "Movie", "Gift", "Electricity", "Doctor", "Course", "Trip", "Bonus", "Shopping", "Dining", "Internet", "Fuel", "Rent", "Insurance", "Gym", "Concert", "Book" };
            var notes = new[] { "", "Paid by card", "Paid by cash", "Online payment", "Reimbursed", "Split bill", "Recurring", "One-time", "Urgent", "Planned" };
            int[] userIds = { 1, 2, 3 };
            int[] categoryIds = new int[24];
            for (int i = 0; i < 24; i++) categoryIds[i] = i + 1;

            for (int i = 0; i < 5000; i++)
            {
                var userId = userIds[rand.Next(userIds.Length)];
                var categoryId = categoryIds[rand.Next(categoryIds.Length)];
                var isIncome = rand.NextDouble() < 0.25; // 25% là income
                var amount = isIncome ? rand.Next(500, 10000) : -rand.Next(10, 2000);
                var type = isIncome ? "Income" : "Expense";
                var desc = descriptions[rand.Next(descriptions.Length)] + (isIncome ? " income" : " expense");
                var note = notes[rand.Next(notes.Length)];
                var daysRange = (endDate - startDate).Days;
                var date = startDate.AddDays(rand.Next(daysRange));
                migrationBuilder.InsertData(
                    table: "Transactions",
                    columns: new[] { "Amount", "Description", "TransactionDate", "Type", "CategoryId", "UserId", "CreatedAt", "Notes" },
                    values: new object[]
                    {
                        (decimal)amount,
                        desc,
                        date,
                        type,
                        categoryId,
                        userId,
                        date,
                        note
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
