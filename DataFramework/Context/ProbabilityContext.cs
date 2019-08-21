namespace DataFramework.Context
{
	using System;
	using System.Linq;
	using DataFramework.Models;
	using Microsoft.EntityFrameworkCore;

	public class ProbabilityContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=ALEXANDER-HP-85;Database=TableTop.Utility.StarWarsRPGProbability;integrated security=True;MultipleActiveResultSets=true");
		//optionsBuilder.UseSqlServer(@"Server=Alex-Desktop;Database=TableTop.Utility.StarWarsRPGProbability;integrated security=True;MultipleActiveResultSets=true");

		// Add a DbSet for each entity type that you want to include in your model. For more information
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Die> Dice { get; set; }
		public virtual DbSet<Pool> Pools { get; set; }
		public virtual DbSet<PoolCombination> PoolCombinations { get; set; }
		public virtual DbSet<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }
		public virtual DbSet<PoolDie> PoolDice { get; set; }
		public virtual DbSet<PoolResult> PoolResults { get; set; }
		public virtual DbSet<PoolResultSymbol> PoolResultSymbols { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{


			modelBuilder.Entity<Die>().ToTable(nameof(Die));
			modelBuilder.Entity<Pool>().ToTable(nameof(Pool));
			modelBuilder.Entity<PoolCombination>().ToTable(nameof(PoolCombination));
			modelBuilder.Entity<PoolCombinationStatistic>().ToTable(nameof(PoolCombinationStatistic));
			modelBuilder.Entity<PoolDie>().ToTable(nameof(PoolDie));
			modelBuilder.Entity<PoolResult>().ToTable(nameof(PoolResult));
			modelBuilder.Entity<PoolResultSymbol>().ToTable(nameof(PoolResultSymbol));

			modelBuilder.Entity<PoolCombination>().HasKey(composite => new { composite.PositivePoolId, composite.NegativePoolId });
			modelBuilder.Entity<PoolCombinationStatistic>().HasKey(composite => new { composite.PositivePoolId, composite.NegativePoolId, composite.Symbol, composite.Quantity });

			modelBuilder.Entity<PoolDie>().HasKey(composite => new { composite.PoolId, composite.DieId });
			modelBuilder.Entity<DieFaceSymbol>().HasKey(composite => new { composite.DieFaceId, composite.Symbol });
			modelBuilder.Entity<PoolResultSymbol>().HasKey(composite => new { composite.PoolResultId, composite.Symbol });

			modelBuilder.Entity<PoolCombination>()
				.HasOne(e => e.PositivePool)
				.WithMany(c => c.PositivePoolCombinations)
				.HasForeignKey(f => f.PositivePoolId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			modelBuilder.Entity<PoolCombination>()
				.HasOne(e => e.NegativePool)
				.WithMany(c => c.NegativePoolCombinations)
				.HasForeignKey(f => f.NegativePoolId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			modelBuilder.Entity<PoolCombinationStatistic>()
				.HasOne(e => e.PoolCombination)
				.WithMany(c => c.PoolCombinationStatistics);

			modelBuilder.Entity<PoolResult>()
				.HasOne(e => e.Pool)
				.WithMany(c => c.PoolResults);

			modelBuilder.Entity<PoolResultSymbol>()
				.HasOne(e => e.PoolResult)
				.WithMany(c => c.PoolResultSymbols);

			modelBuilder.Entity<PoolDie>()
				.HasOne(e => e.Pool)
				.WithMany(c => c.PoolDice);

			modelBuilder.Entity<PoolDie>()
				.HasOne(e => e.Die)
				.WithMany(c => c.PoolDice);

			modelBuilder.Entity<DieFaceSymbol>()
				.HasOne(e => e.DieFace)
				.WithMany(c => c.DieFaceSymbols);

			modelBuilder.Entity<DieFace>()
				.HasOne(e => e.Die)
				.WithMany(c => c.DieFaces);

			foreach (var property in modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetProperties())
				.Where(p => p.ClrType == typeof(decimal)))
			{
				property.Relational().ColumnType = "decimal(24, 0)";// 100,000,000,000,000,000,000
			}

		}
	}
}
