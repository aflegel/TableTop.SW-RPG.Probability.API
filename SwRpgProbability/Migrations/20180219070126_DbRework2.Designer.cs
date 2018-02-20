﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SwRpgProbability.DataContext;
using SwRpgProbability.Dice;
using System;

namespace SwRpgProbability.Migrations
{
    [DbContext(typeof(ProbabilityContext))]
    [Migration("20180219070126_DbRework2")]
    partial class DbRework2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SwRpgProbability.DataContext.Die", b =>
                {
                    b.Property<int>("DieId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("DieId");

                    b.ToTable("Die");
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.Pool", b =>
                {
                    b.Property<long>("PoolId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AdvantageOutcomes");

                    b.Property<long>("DespairOutcomes");

                    b.Property<long>("FailureOutcomes");

                    b.Property<string>("Name");

                    b.Property<long>("StalemateOutcomes");

                    b.Property<long>("SuccessOutcomes");

                    b.Property<long>("ThreatOutcomes");

                    b.Property<long>("TotalOutcomes");

                    b.Property<long>("TriumphOutcomes");

                    b.Property<long>("UniqueOutcomes");

                    b.HasKey("PoolId");

                    b.ToTable("Pool");
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolDie", b =>
                {
                    b.Property<long>("PoolId");

                    b.Property<int>("DieId");

                    b.Property<int>("Quantity");

                    b.HasKey("PoolId", "DieId");

                    b.HasIndex("DieId");

                    b.ToTable("PoolDie");
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolResult", b =>
                {
                    b.Property<long>("PoolResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("PoolId");

                    b.HasKey("PoolResultId");

                    b.HasIndex("PoolId");

                    b.ToTable("PoolResult");
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolResultSymbol", b =>
                {
                    b.Property<long>("PoolResultId");

                    b.Property<int>("Symbol");

                    b.Property<int>("Quantity");

                    b.HasKey("PoolResultId", "Symbol");

                    b.ToTable("PoolResultSymbol");
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolDie", b =>
                {
                    b.HasOne("SwRpgProbability.DataContext.Die", "Die")
                        .WithMany("PoolDice")
                        .HasForeignKey("DieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SwRpgProbability.DataContext.Pool", "Pool")
                        .WithMany("PoolDice")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolResult", b =>
                {
                    b.HasOne("SwRpgProbability.DataContext.Pool", "Pool")
                        .WithMany("PoolResults")
                        .HasForeignKey("PoolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SwRpgProbability.DataContext.PoolResultSymbol", b =>
                {
                    b.HasOne("SwRpgProbability.DataContext.PoolResult", "PoolResult")
                        .WithMany("PoolResultSymbols")
                        .HasForeignKey("PoolResultId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
