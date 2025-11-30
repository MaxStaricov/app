using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    beginning = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    venue = table.Column<string>(type: "text", nullable: false),
                    base_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_events_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_ratings", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_ratings_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Rock" },
                    { 2, "Pop" },
                    { 3, "Jazz" }
                });

            migrationBuilder.InsertData(
                table: "events",
                columns: new[] { "id", "base_price", "beginning", "genre_id", "name", "venue" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 50m, new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Rock Concert", "Stadium A" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 50m, new DateTime(2025, 12, 10, 18, 0, 0, 0, DateTimeKind.Utc), 2, "Pop Festival", "Arena B" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 40m, new DateTime(2025, 12, 15, 20, 0, 0, 0, DateTimeKind.Utc), 3, "Jazz Night", "Club C" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 50m, new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Rock Concert", "Stadium A" }
                });

            migrationBuilder.InsertData(
                table: "event_ratings",
                columns: new[] { "id", "event_id", "rating" },
                values: new object[,]
                {
                    { 1, new Guid("11111111-1111-1111-1111-111111111111"), 4.5 },
                    { 2, new Guid("11111111-1111-1111-1111-111111111111"), 3.7999999999999998 },
                    { 3, new Guid("22222222-2222-2222-2222-222222222222"), 4.9000000000000004 },
                    { 4, new Guid("33333333-3333-3333-3333-333333333333"), 4.2000000000000002 },
                    { 5, new Guid("55555555-5555-5555-5555-555555555555"), 4.5 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_event_ratings_event_id",
                table: "event_ratings",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_genre_id",
                table: "events",
                column: "genre_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_ratings");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "genres");
        }
    }
}
