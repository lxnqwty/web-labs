using backend.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202611060001_InitialEfCore")]
public sealed class InitialEfCore : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_categories", x => x.id));

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                full_name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                login = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                role = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.id);
                table.CheckConstraint("users_role_check", "role IN ('Student', 'Reviewer', 'Manager')");
            });

        migrationBuilder.CreateTable(
            name: "activities",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                number = table.Column<int>(type: "integer", nullable: false).Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                title = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                category_id = table.Column<int>(type: "integer", nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                status = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                created_by_user_id = table.Column<int>(type: "integer", nullable: false),
                assignee_user_id = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_activities", x => x.id);
                table.CheckConstraint("activities_title_check", "char_length(title) BETWEEN 5 AND 80");
                table.CheckConstraint("activities_description_check", "char_length(description) BETWEEN 10 AND 500");
                table.CheckConstraint("activities_status_check", "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");
                table.ForeignKey("FK_activities_categories_category_id", x => x.category_id, "categories", "id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_activities_users_created_by_user_id", x => x.created_by_user_id, "users", "id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_activities_users_assignee_user_id", x => x.assignee_user_id, "users", "id", onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateIndex("IX_categories_name", "categories", "name", unique: true);
        migrationBuilder.CreateIndex("IX_users_login", "users", "login", unique: true);
        migrationBuilder.CreateIndex("activities_category_id_idx", "activities", "category_id");
        migrationBuilder.CreateIndex("activities_status_idx", "activities", "status");
        migrationBuilder.CreateIndex("IX_activities_created_by_user_id", "activities", "created_by_user_id");
        migrationBuilder.CreateIndex("IX_activities_assignee_user_id", "activities", "assignee_user_id");

        migrationBuilder.InsertData("users", new[] { "id", "full_name", "login", "role" }, new object[,]
        {
            { 1, "Андрей Карабаев", "andrey.student", "Student" },
            { 2, "Иван Петров", "ivan.reviewer", "Reviewer" },
            { 3, "Мария Сидорова", "maria.manager", "Manager" }
        });

        migrationBuilder.InsertData("categories", new[] { "id", "name" }, new object[,]
        {
            { 1, "Учебная деятельность" },
            { 2, "Научная деятельность" },
            { 3, "Проектная деятельность" }
        });

        // Explicit seed IDs must advance PostgreSQL identity sequences.
        migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('users', 'id'), (SELECT MAX(id) FROM users));");
        migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('categories', 'id'), (SELECT MAX(id) FROM categories));");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "activities");
        migrationBuilder.DropTable(name: "categories");
        migrationBuilder.DropTable(name: "users");
    }
}
