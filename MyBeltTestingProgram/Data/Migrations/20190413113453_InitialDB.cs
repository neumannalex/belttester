using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBeltTestingProgram.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combinations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combinations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stances",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stances", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Techniques",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Annotation = table.Column<string>(nullable: true),
                    Purpose = table.Column<int>(nullable: false),
                    Weapon = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Techniques", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Motions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StanceID = table.Column<int>(nullable: true),
                    MoveID = table.Column<int>(nullable: true),
                    TechniqueID = table.Column<int>(nullable: true),
                    CombinationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Motions_Combinations_CombinationID",
                        column: x => x.CombinationID,
                        principalTable: "Combinations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motions_Moves_MoveID",
                        column: x => x.MoveID,
                        principalTable: "Moves",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motions_Stances_StanceID",
                        column: x => x.StanceID,
                        principalTable: "Stances",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motions_Techniques_TechniqueID",
                        column: x => x.TechniqueID,
                        principalTable: "Techniques",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motions_CombinationID",
                table: "Motions",
                column: "CombinationID");

            migrationBuilder.CreateIndex(
                name: "IX_Motions_MoveID",
                table: "Motions",
                column: "MoveID");

            migrationBuilder.CreateIndex(
                name: "IX_Motions_StanceID",
                table: "Motions",
                column: "StanceID");

            migrationBuilder.CreateIndex(
                name: "IX_Motions_TechniqueID",
                table: "Motions",
                column: "TechniqueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motions");

            migrationBuilder.DropTable(
                name: "Combinations");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Stances");

            migrationBuilder.DropTable(
                name: "Techniques");
        }
    }
}
