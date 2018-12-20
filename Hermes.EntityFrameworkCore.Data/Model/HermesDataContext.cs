using Microsoft.EntityFrameworkCore;

namespace Hermes.EntityFrameworkCore.Data.Model
{
    public class HermesDataContext : DbContext
    {
        public HermesDataContext()
        {
        }

        public HermesDataContext(DbContextOptions<HermesDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomMarkers> CustomMarkers { get; set; }
        public virtual DbSet<EditInfo> EditInfo { get; set; }
        public virtual DbSet<EditedRoures> EditedRoures { get; set; }
        public virtual DbSet<EventVisits> EventVisits { get; set; }
        public virtual DbSet<LoadedData> LoadedData { get; set; }
        public virtual DbSet<ProcessedTracks> ProcessedTracks { get; set; }
        public virtual DbSet<ReadLog> ReadLog { get; set; }
        public virtual DbSet<SourceData> SourceData { get; set; }
        public virtual DbSet<SourceTracks> SourceTracks { get; set; }
        public virtual DbSet<TrackContent> TrackContent { get; set; }
        public virtual DbSet<TrackStatuses> TrackStatuses { get; set; }
        public virtual DbSet<TransportTypes> TransportTypes { get; set; }
        public virtual DbSet<UserStatuses> UserStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source= 127.0.0.1;Initial Catalog=hermes;Persist Security Info=True;User ID=sa;Password=Asdewq12#z");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<CustomMarkers>(entity =>
            {
                entity.ToTable("custom_markers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1024);

                entity.Property(e => e.ImageUri)
                    .HasColumnName("image_uri")
                    .HasMaxLength(1024);

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longtitude).HasColumnName("longtitude");
            });

            modelBuilder.Entity<EditInfo>(entity =>
            {
                entity.ToTable("edit_info");

                entity.HasIndex(e => e.ProcessedId)
                    .HasName("processed_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Editinfo1).HasColumnName("editinfo");

                entity.Property(e => e.Editor)
                    .HasColumnName("editor")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Edittingdate)
                    .HasColumnName("edittingdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProcessedId).HasColumnName("processed_id");

                entity.Property(e => e.Smoothinfo).HasColumnName("smoothinfo");

                entity.HasOne(d => d.Processed)
                    .WithMany(p => p.EditInfo)
                    .HasForeignKey(d => d.ProcessedId)
                    .HasConstraintName("FK_edit_info_processed_tracks");
            });

            modelBuilder.Entity<EditedRoures>(entity =>
            {
                entity.ToTable("edited_roures");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Jsonobject)
                    .HasColumnName("jsonobject")
                    .IsUnicode(false);

                entity.Property(e => e.ProcessedTrackId).HasColumnName("processed_track_id");

                entity.HasOne(d => d.ProcessedTrack)
                    .WithMany(p => p.EditedRoures)
                    .HasForeignKey(d => d.ProcessedTrackId)
                    .HasConstraintName("FK_edited_roures_processed_tracks");
            });

            modelBuilder.Entity<EventVisits>(entity =>
            {
                entity.ToTable("event_visits");

                entity.HasIndex(e => e.DateEvent)
                    .HasName("evnt_wiz_data");

                entity.HasIndex(e => e.FleetId)
                    .HasName("fi_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateEvent)
                    .HasColumnName("date_event")
                    .HasColumnType("datetime");

                entity.Property(e => e.Exceptedpoints).HasColumnName("exceptedpoints");

                entity.Property(e => e.FleetId)
                    .HasColumnName("fleet_id")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FleetType).HasColumnName("fleet_type");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.HasOne(d => d.FleetTypeNavigation)
                    .WithMany(p => p.EventVisits)
                    .HasForeignKey(d => d.FleetType)
                    .HasConstraintName("FK_event_visits_transport_types_id");
            });

            modelBuilder.Entity<LoadedData>(entity =>
            {
                entity.ToTable("loaded_data");

                entity.HasIndex(e => e.CarId)
                    .HasName("idx_car_id");

                entity.HasIndex(e => new { e.Id, e.DateEvent })
                    .HasName("idx_load_dada_date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId)
                    .HasColumnName("Car_ID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DateEvent).HasColumnType("datetime");

                entity.Property(e => e.DateFirst).HasColumnType("datetime");

                entity.Property(e => e.DateLast).HasColumnType("datetime");

                entity.Property(e => e.ReplId).HasColumnName("Repl_ID");

                entity.Property(e => e.TrackData)
                    .IsRequired()
                    .HasColumnType("image");
            });

            modelBuilder.Entity<ProcessedTracks>(entity =>
            {
                entity.ToTable("processed_tracks");

                entity.HasIndex(e => e.CarId)
                    .HasName("pt_carid");

                entity.HasIndex(e => e.DateTrack)
                    .HasName("pt_datetrack");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId)
                    .HasColumnName("Car_ID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DateTrack).HasColumnType("datetime");

                entity.Property(e => e.TransportType)
                    .HasColumnName("transport_type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ReadLog>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateEvent)
                    .HasColumnName("date_event")
                    .HasColumnType("datetime");

                entity.Property(e => e.FleetId)
                    .HasColumnName("fleet_id")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FleetType).HasColumnName("fleet_type");

                entity.Property(e => e.LodadedData).HasColumnName("lodaded_data");

                entity.HasOne(d => d.LodadedDataNavigation)
                    .WithMany(p => p.ReadLog)
                    .HasForeignKey(d => d.LodadedData)
                    .HasConstraintName("FK_ReadLog_loaded_data");
            });

            modelBuilder.Entity<SourceData>(entity =>
            {
                entity.ToTable("source_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FiterDataKey)
                    .HasColumnName("fiter_data_key")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gpx)
                    .HasColumnName("gpx")
                    .HasColumnType("image");

                entity.Property(e => e.LoadedDataId).HasColumnName("loaded_data_id");
            });

            modelBuilder.Entity<SourceTracks>(entity =>
            {
                entity.ToTable("source_tracks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId)
                    .HasColumnName("Car_ID")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TrackDate)
                    .HasColumnName("TRACK_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.TrackSource)
                    .HasColumnName("TRACK_SOURCE")
                    .HasColumnType("image");
            });

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.DiagramId)
                    .HasName("PK__sysdiagr__C2B05B61F07C8B05");

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<TrackContent>(entity =>
            {
                entity.ToTable("track_content");

                entity.HasIndex(e => e.ProcessedTrack)
                    .HasName("processed_trackcontent_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LoadedData)
                    .HasColumnName("loaded_data")
                    .HasMaxLength(500);

                entity.Property(e => e.ProcessedTrack).HasColumnName("processed_track");

                entity.HasOne(d => d.ProcessedTrackNavigation)
                    .WithMany(p => p.TrackContent)
                    .HasForeignKey(d => d.ProcessedTrack)
                    .HasConstraintName("FK_track_content_processed_tracks");
            });

            modelBuilder.Entity<TrackStatuses>(entity =>
            {
                entity.ToTable("track_statuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransportTypes>(entity =>
            {
                entity.ToTable("transport_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserStatuses>(entity =>
            {
                entity.ToTable("user_statuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TrackId).HasColumnName("track_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UserStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_statuses_track_statuses");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.UserStatuses)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_statuses_processed_tracks");
            });
        }
    }
}
