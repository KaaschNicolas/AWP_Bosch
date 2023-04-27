﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class ChangeToEngAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pcbs_Diagnoses_EnddiagnoseId",
                table: "Pcbs");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Pcbs_LeiterplatteId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_StorageLocations_NachId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_VerbuchtVonId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "VerbuchtVonId",
                table: "Transfers",
                newName: "StorageLocationId");

            migrationBuilder.RenameColumn(
                name: "NachId",
                table: "Transfers",
                newName: "NotedById");

            migrationBuilder.RenameColumn(
                name: "LeiterplatteId",
                table: "Transfers",
                newName: "PcbId");

            migrationBuilder.RenameColumn(
                name: "Anmerkung",
                table: "Transfers",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_VerbuchtVonId",
                table: "Transfers",
                newName: "IX_Transfers_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_NachId",
                table: "Transfers",
                newName: "IX_Transfers_NotedById");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_LeiterplatteId",
                table: "Transfers",
                newName: "IX_Transfers_PcbId");

            migrationBuilder.RenameColumn(
                name: "VerweildauerRot",
                table: "StorageLocations",
                newName: "DwellTimeYellow");

            migrationBuilder.RenameColumn(
                name: "VerweildauerGelb",
                table: "StorageLocations",
                newName: "DwellTimeRed");

            migrationBuilder.RenameColumn(
                name: "LagerName",
                table: "StorageLocations",
                newName: "StorageName");

            migrationBuilder.RenameColumn(
                name: "EnddiagnoseId",
                table: "Pcbs",
                newName: "DiagnosisId");

            migrationBuilder.RenameIndex(
                name: "IX_Pcbs_EnddiagnoseId",
                table: "Pcbs",
                newName: "IX_Pcbs_DiagnosisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pcbs_Diagnoses_DiagnosisId",
                table: "Pcbs",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers",
                column: "PcbId",
                principalTable: "Pcbs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_StorageLocations_StorageLocationId",
                table: "Transfers",
                column: "StorageLocationId",
                principalTable: "StorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_NotedById",
                table: "Transfers",
                column: "NotedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pcbs_Diagnoses_DiagnosisId",
                table: "Pcbs");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_StorageLocations_StorageLocationId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_NotedById",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "StorageLocationId",
                table: "Transfers",
                newName: "VerbuchtVonId");

            migrationBuilder.RenameColumn(
                name: "PcbId",
                table: "Transfers",
                newName: "LeiterplatteId");

            migrationBuilder.RenameColumn(
                name: "NotedById",
                table: "Transfers",
                newName: "NachId");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Transfers",
                newName: "Anmerkung");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_StorageLocationId",
                table: "Transfers",
                newName: "IX_Transfers_VerbuchtVonId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_PcbId",
                table: "Transfers",
                newName: "IX_Transfers_LeiterplatteId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_NotedById",
                table: "Transfers",
                newName: "IX_Transfers_NachId");

            migrationBuilder.RenameColumn(
                name: "StorageName",
                table: "StorageLocations",
                newName: "LagerName");

            migrationBuilder.RenameColumn(
                name: "DwellTimeYellow",
                table: "StorageLocations",
                newName: "VerweildauerRot");

            migrationBuilder.RenameColumn(
                name: "DwellTimeRed",
                table: "StorageLocations",
                newName: "VerweildauerGelb");

            migrationBuilder.RenameColumn(
                name: "DiagnosisId",
                table: "Pcbs",
                newName: "EnddiagnoseId");

            migrationBuilder.RenameIndex(
                name: "IX_Pcbs_DiagnosisId",
                table: "Pcbs",
                newName: "IX_Pcbs_EnddiagnoseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pcbs_Diagnoses_EnddiagnoseId",
                table: "Pcbs",
                column: "EnddiagnoseId",
                principalTable: "Diagnoses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Pcbs_LeiterplatteId",
                table: "Transfers",
                column: "LeiterplatteId",
                principalTable: "Pcbs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_StorageLocations_NachId",
                table: "Transfers",
                column: "NachId",
                principalTable: "StorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_VerbuchtVonId",
                table: "Transfers",
                column: "VerbuchtVonId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
