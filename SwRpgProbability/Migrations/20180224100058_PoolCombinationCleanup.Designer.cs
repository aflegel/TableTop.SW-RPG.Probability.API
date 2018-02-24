﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SwRpgProbability.Models.DataContext;
using System;

namespace SwRpgProbability.Migrations
{
    [DbContext(typeof(ProbabilityContext))]
    [Migration("20180224100058_PoolCombinationCleanup")]
    partial class PoolCombinationCleanup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.Die", b =>
                {
                    b.Property<int>("DieId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("DieId");

                    b.ToTable("Die");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.DieFace", b =>
                {
                    b.Property<int>("DieFaceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DieId");

                    b.HasKey("DieFaceId");

                    b.HasIndex("DieId");

                    b.ToTable("DieFace");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.DieFaceSymbol", b =>
                {
                    b.Property<int>("DieFaceId");

                    b.Property<int>("Symbol");

                    b.Property<int>("Quantity");

                    b.HasKey("DieFaceId", "Symbol");

                    b.ToTable("DieFaceSymbol");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.Pool", b =>
                {
                    b.Property<long>("PoolId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long>("TotalOutcomes");

                    b.Property<long>("UniqueOutcomes");

                    b.HasKey("PoolId");

                    b.ToTable("Pool");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolCombination", b =>
                {
                    b.Property<long>("PositivePoolId");

                    b.Property<long>("NegativePoolId");

                    b.Property<long>("AdvantageOutcomes");

                    b.Property<long>("DespairOutcomes");

                    b.Property<long>("SuccessOutcomes");

                    b.Property<long>("ThreatOutcomes");

                    b.Property<long>("TriumphOutcomes");

                    b.HasKey("PositivePoolId", "NegativePoolId");

                    b.HasIndex("NegativePoolId");

                    b.ToTable("PoolCombination");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolDie", b =>
                {
                    b.Property<long>("PoolId");

                    b.Property<int>("DieId");

                    b.Property<int>("Quantity");

                    b.HasKey("PoolId", "DieId");

                    b.HasIndex("DieId");

                    b.ToTable("PoolDie");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolResult", b =>
                {
                    b.Property<long>("PoolResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("PoolId");

                    b.Property<long>("Quantity");

                    b.HasKey("PoolResultId");

                    b.HasIndex("PoolId");

                    b.ToTable("PoolResult");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolResultSymbol", b =>
                {
                    b.Property<long>("PoolResultId");

                    b.Property<int>("Symbol");

                    b.Property<int>("Quantity");

                    b.HasKey("PoolResultId", "Symbol");

                    b.ToTable("PoolResultSymbol");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.DieFace", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.Die", "Die")
                        .WithMany("DieFaces")
                        .HasForeignKey("DieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.DieFaceSymbol", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.DieFace", "DieFace")
                        .WithMany("DieFaceSymbols")
                        .HasForeignKey("DieFaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolCombination", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.Pool", "NegativePool")
                        .WithMany("NegativePoolCombinations")
                        .HasForeignKey("NegativePoolId");

                    b.HasOne("SwRpgProbability.Models.DataContext.Pool", "PositivePool")
                        .WithMany("PositivePoolCombinations")
                        .HasForeignKey("PositivePoolId");
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolDie", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.Die", "Die")
                        .WithMany("PoolDice")
                        .HasForeignKey("DieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SwRpgProbability.Models.DataContext.Pool", "Pool")
                        .WithMany("PoolDice")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolResult", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.Pool", "Pool")
                        .WithMany("PoolResults")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.Models.DataContext.PoolResultSymbol", b =>
                {
                    b.HasOne("SwRpgProbability.Models.DataContext.PoolResult", "PoolResult")
                        .WithMany("PoolResultSymbols")
                        .HasForeignKey("PoolResultId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
