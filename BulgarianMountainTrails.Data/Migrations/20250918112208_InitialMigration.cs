using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulgarianMountainTrails.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Huts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mountain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Altitude = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointsOfInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Mountain = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsOfInterest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mountain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    LengthKm = table.Column<double>(type: "float", nullable: false),
                    DurationHours = table.Column<double>(type: "float", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Caves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthM = table.Column<double>(type: "float", nullable: false),
                    IsTouristAccessible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caves_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaKm2 = table.Column<double>(type: "float", nullable: false),
                    DepthM = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lakes_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Monasteries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoundedYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monasteries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monasteries_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Peaks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ElevationM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peaks_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthKm = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rivers_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Waterfalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeightM = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waterfalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waterfalls_PointsOfInterest_Id",
                        column: x => x.Id,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrailHuts",
                columns: table => new
                {
                    TrailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailHuts", x => new { x.TrailId, x.HutId });
                    table.ForeignKey(
                        name: "FK_TrailHuts_Huts_HutId",
                        column: x => x.HutId,
                        principalTable: "Huts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrailHuts_Trails_TrailId",
                        column: x => x.TrailId,
                        principalTable: "Trails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrailPOIs",
                columns: table => new
                {
                    TrailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointOfInterestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailPOIs", x => new { x.TrailId, x.PointOfInterestId });
                    table.ForeignKey(
                        name: "FK_TrailPOIs_PointsOfInterest_PointOfInterestId",
                        column: x => x.PointOfInterestId,
                        principalTable: "PointsOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrailPOIs_Trails_TrailId",
                        column: x => x.TrailId,
                        principalTable: "Trails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrailHuts_HutId",
                table: "TrailHuts",
                column: "HutId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailPOIs_PointOfInterestId",
                table: "TrailPOIs",
                column: "PointOfInterestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caves");

            migrationBuilder.DropTable(
                name: "Lakes");

            migrationBuilder.DropTable(
                name: "Monasteries");

            migrationBuilder.DropTable(
                name: "Peaks");

            migrationBuilder.DropTable(
                name: "Rivers");

            migrationBuilder.DropTable(
                name: "TrailHuts");

            migrationBuilder.DropTable(
                name: "TrailPOIs");

            migrationBuilder.DropTable(
                name: "Waterfalls");

            migrationBuilder.DropTable(
                name: "Huts");

            migrationBuilder.DropTable(
                name: "Trails");

            migrationBuilder.DropTable(
                name: "PointsOfInterest");
        }
    }
}
