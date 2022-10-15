using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attracties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attracties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DateTimeBereik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Begin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Eind = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateTimeBereik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GastInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaatstBezochteURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gebruikers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruikers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Onderhoud",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataId = table.Column<int>(type: "int", nullable: true),
                    reparatieAttractieId = table.Column<int>(type: "int", nullable: false),
                    Probleem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onderhoud", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Onderhoud_Attracties_reparatieAttractieId",
                        column: x => x.reparatieAttractieId,
                        principalTable: "Attracties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Onderhoud_DateTimeBereik_DataId",
                        column: x => x.DataId,
                        principalTable: "DateTimeBereik",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Gasten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EersteBezoek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BegeleiderId = table.Column<int>(type: "int", nullable: true),
                    FavorietId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gasten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gasten_Attracties_FavorietId",
                        column: x => x.FavorietId,
                        principalTable: "Attracties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gasten_Gasten_BegeleiderId",
                        column: x => x.BegeleiderId,
                        principalTable: "Gasten",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gasten_Gebruikers_Id",
                        column: x => x.Id,
                        principalTable: "Gebruikers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medewerkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medewerkers_Gebruikers_Id",
                        column: x => x.Id,
                        principalTable: "Gebruikers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reserveringen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GastId = table.Column<int>(type: "int", nullable: true),
                    AttractieId = table.Column<int>(type: "int", nullable: true),
                    DataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserveringen_Attracties_AttractieId",
                        column: x => x.AttractieId,
                        principalTable: "Attracties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reserveringen_DateTimeBereik_DataId",
                        column: x => x.DataId,
                        principalTable: "DateTimeBereik",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reserveringen_Gasten_GastId",
                        column: x => x.GastId,
                        principalTable: "Gasten",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedewerkerOnderhoud",
                columns: table => new
                {
                    DoetId = table.Column<int>(type: "int", nullable: false),
                    WordtGedaanDoorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedewerkerOnderhoud", x => new { x.DoetId, x.WordtGedaanDoorId });
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud_Medewerkers_WordtGedaanDoorId",
                        column: x => x.WordtGedaanDoorId,
                        principalTable: "Medewerkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud_Onderhoud_DoetId",
                        column: x => x.DoetId,
                        principalTable: "Onderhoud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedewerkerOnderhoud1",
                columns: table => new
                {
                    CoordineertId = table.Column<int>(type: "int", nullable: false),
                    WordtGecoordineerdDoorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedewerkerOnderhoud1", x => new { x.CoordineertId, x.WordtGecoordineerdDoorId });
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud1_Medewerkers_WordtGecoordineerdDoorId",
                        column: x => x.WordtGecoordineerdDoorId,
                        principalTable: "Medewerkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedewerkerOnderhoud1_Onderhoud_CoordineertId",
                        column: x => x.CoordineertId,
                        principalTable: "Onderhoud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gasten_BegeleiderId",
                table: "Gasten",
                column: "BegeleiderId",
                unique: true,
                filter: "[BegeleiderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gasten_FavorietId",
                table: "Gasten",
                column: "FavorietId");

            migrationBuilder.CreateIndex(
                name: "IX_MedewerkerOnderhoud_WordtGedaanDoorId",
                table: "MedewerkerOnderhoud",
                column: "WordtGedaanDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedewerkerOnderhoud1_WordtGecoordineerdDoorId",
                table: "MedewerkerOnderhoud1",
                column: "WordtGecoordineerdDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_Onderhoud_DataId",
                table: "Onderhoud",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Onderhoud_reparatieAttractieId",
                table: "Onderhoud",
                column: "reparatieAttractieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_AttractieId",
                table: "Reserveringen",
                column: "AttractieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_DataId",
                table: "Reserveringen",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserveringen_GastId",
                table: "Reserveringen",
                column: "GastId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GastInfo");

            migrationBuilder.DropTable(
                name: "MedewerkerOnderhoud");

            migrationBuilder.DropTable(
                name: "MedewerkerOnderhoud1");

            migrationBuilder.DropTable(
                name: "Reserveringen");

            migrationBuilder.DropTable(
                name: "Medewerkers");

            migrationBuilder.DropTable(
                name: "Onderhoud");

            migrationBuilder.DropTable(
                name: "Gasten");

            migrationBuilder.DropTable(
                name: "DateTimeBereik");

            migrationBuilder.DropTable(
                name: "Attracties");

            migrationBuilder.DropTable(
                name: "Gebruikers");
        }
    }
}
