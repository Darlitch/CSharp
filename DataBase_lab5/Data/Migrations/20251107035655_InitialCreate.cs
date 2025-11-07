using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fork_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    index = table.Column<int>(type: "integer", nullable: false),
                    owner = table.Column<string>(type: "varchar", maxLength: 128, nullable: true),
                    state = table.Column<short>(type: "smallint", nullable: false),
                    timestamp_ms = table.Column<long>(type: "bigint", nullable: false),
                    run_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fork_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "philosopher_event",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    index = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    state = table.Column<short>(type: "smallint", nullable: false),
                    action = table.Column<short>(type: "smallint", nullable: false),
                    eaten = table.Column<int>(type: "integer", nullable: false),
                    waiting_time = table.Column<long>(type: "bigint", nullable: false),
                    timestamp_ms = table.Column<long>(type: "bigint", nullable: false),
                    run_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_philosopher_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "simulation_run",
                columns: table => new
                {
                    run_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    duration_ms = table.Column<long>(type: "bigint", nullable: false),
                    philosophers_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_simulation_run", x => x.run_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fork_events_run_id_timestamp_ms_index",
                table: "fork_events",
                columns: new[] { "run_id", "timestamp_ms", "index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_philosopher_event_run_id_timestamp_ms_index",
                table: "philosopher_event",
                columns: new[] { "run_id", "timestamp_ms", "index" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fork_events");

            migrationBuilder.DropTable(
                name: "philosopher_event");

            migrationBuilder.DropTable(
                name: "simulation_run");
        }
    }
}
