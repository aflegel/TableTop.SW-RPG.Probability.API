namespace DiceCalculator.DataContext
{
	using System;
	using System.Linq;
	using Microsoft.EntityFrameworkCore;

	public class ProbabilityContext : DbContext
	{
		// Your context has been configured to use a 'ProbabilityContext' connection string from your application's
		// configuration file (App.config or Web.config). By default, this connection string targets the
		// 'DiceCalculator.DataContext.ProbabilityContext' database on your LocalDb instance.
		//
		// If you wish to target a different database and/or database provider, modify the 'ProbabilityContext'
		// connection string in the application configuration file.
		public ProbabilityContext(DbContextOptions<ProbabilityContext> options) : base(options)
		{
		}

		// Add a DbSet for each entity type that you want to include in your model. For more information
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Die> Dice { get; set; }
		public virtual DbSet<Face> Faces { get; set; }
		public virtual DbSet<FaceSymbol> FaceSymbols { get; set; }
		public virtual DbSet<Symbol> Symbols { get; set; }
		public virtual DbSet<Pool> Pools { get; set; }
		public virtual DbSet<PoolResult> PoolResults { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Die>().ToTable(nameof(Die));
			modelBuilder.Entity<Face>().ToTable(nameof(Face));
			modelBuilder.Entity<FaceSymbol>().ToTable(nameof(FaceSymbol));
			modelBuilder.Entity<Pool>().ToTable(nameof(Pool));
			modelBuilder.Entity<PoolResult>().ToTable(nameof(PoolResult));
			modelBuilder.Entity<Symbol>().ToTable(nameof(Symbol));

			modelBuilder.Entity<PoolResult>().HasKey(composite => new { composite.FaceId, composite.PoolId });

			modelBuilder.Entity<PoolResult>()
				.HasOne(e => e.Face)
				.WithMany(c => c.PoolResults);

			modelBuilder.Entity<PoolResult>()
				.HasOne(e => e.Pool)
				.WithMany(c => c.PoolResults);

			modelBuilder.Entity<Face>()
				.HasOne(e => e.Die)
				.WithMany(c => c.Faces);

			modelBuilder.Entity<FaceSymbol>().HasKey(composite => new { composite.FaceId, composite.SymbolId });

			modelBuilder.Entity<FaceSymbol>()
				.HasOne(e => e.Face)
				.WithMany(c => c.FaceSymbols);

			modelBuilder.Entity<FaceSymbol>()
				.HasOne(e => e.Symbol)
				.WithMany(c => c.FaceSymbols);
		}
	}

	//public class MyEntity
	//{
	//    public int Id { get; set; }
	//    public string Name { get; set; }
	//}
}