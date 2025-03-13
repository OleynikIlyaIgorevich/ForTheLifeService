using ForTheLife.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForTheLife.DbContexts;

public partial class ForTheLifeDbContext : DbContext
{
    public ForTheLifeDbContext()
    {
    }

    public ForTheLifeDbContext(DbContextOptions<ForTheLifeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<OrdersStatus> OrdersStatuses { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductName> ProductNames { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=for_the_life_db;user=root;password=12032003", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.OrdersStatusId, "orders_status_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrdersStatusId).HasColumnName("orders_status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OrdersStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrdersStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_ibfk_1");
        });

        modelBuilder.Entity<OrdersProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders_products");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.HasIndex(e => e.ProductId, "product_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_products_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_products_ibfk_2");
        });

        modelBuilder.Entity<OrdersStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders_statuses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producers");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.Article, "article").IsUnique();

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.HasIndex(e => e.ProducerId, "producer_id");

            entity.HasIndex(e => e.ProductNameId, "product_name_id");

            entity.HasIndex(e => e.SupplierId, "supplier_id");

            entity.HasIndex(e => e.UnitId, "unit_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Article)
                .HasMaxLength(6)
                .HasColumnName("article");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.CurrentSale).HasColumnName("current_sale");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(128)
                .HasColumnName("image_url");
            entity.Property(e => e.MaxSale).HasColumnName("max_sale");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProducerId).HasColumnName("producer_id");
            entity.Property(e => e.ProductNameId).HasColumnName("product_name_id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_1");

            entity.HasOne(d => d.Producer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_3");

            entity.HasOne(d => d.ProductName).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_4");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_2");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_5");
        });

        modelBuilder.Entity<ProductName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product_names");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("units");

            entity.HasIndex(e => e.Title, "title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "role_id");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname)
                .HasMaxLength(128)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(128)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(128)
                .HasColumnName("middlename");
            entity.Property(e => e.Pass)
                .HasMaxLength(256)
                .HasColumnName("pass");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(32)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
