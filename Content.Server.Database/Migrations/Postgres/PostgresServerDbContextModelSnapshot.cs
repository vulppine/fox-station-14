﻿// <auto-generated />
using System;
using System.Net;
using Content.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Content.Server.Database.Migrations.Postgres
{
    [DbContext(typeof(PostgresServerDbContext))]
    partial class PostgresServerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<int?>("AdminRankId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("UserId");

                    b.HasIndex("AdminRankId");

                    b.ToTable("admin");
                });

            modelBuilder.Entity("Content.Server.Database.AdminFlag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_flag_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid")
                        .HasColumnName("admin_id");

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("flag");

                    b.Property<bool>("Negative")
                        .HasColumnType("boolean")
                        .HasColumnName("negative");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("Flag", "AdminId")
                        .IsUnique();

                    b.ToTable("admin_flag");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("admin_rank");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRankFlag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_flag_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AdminRankId")
                        .HasColumnType("integer")
                        .HasColumnName("admin_rank_id");

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("flag");

                    b.HasKey("Id");

                    b.HasIndex("AdminRankId");

                    b.HasIndex("Flag", "AdminRankId")
                        .IsUnique();

                    b.ToTable("admin_rank_flag");
                });

            modelBuilder.Entity("Content.Server.Database.Antag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("antag_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AntagName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("antag_name");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId", "AntagName")
                        .IsUnique();

                    b.ToTable("antag");
                });

            modelBuilder.Entity("Content.Server.Database.AnthroSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("anthro_system_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Markings")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("markings");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.Property<string>("SpeciesBase")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("species_base");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId")
                        .IsUnique();

                    b.ToTable("anthro_system");
                });

            modelBuilder.Entity("Content.Server.Database.AssignedUserId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("assigned_user_id_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("assigned_user_id");
                });

            modelBuilder.Entity("Content.Server.Database.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("job_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_name");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.HasIndex("ProfileId", "JobName")
                        .IsUnique();

                    b.HasIndex(new[] { "ProfileId" }, "IX_job_one_high_priority")
                        .IsUnique()
                        .HasFilter("priority = 3");

                    b.ToTable("job");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresConnectionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("connection_log_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<IPAddress>("Address")
                        .IsRequired()
                        .HasColumnType("inet")
                        .HasColumnName("address");

                    b.Property<byte[]>("HWId")
                        .HasColumnType("bytea")
                        .HasColumnName("hwid");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("connection_log");

                    b.HasCheckConstraint("AddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= address");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("player_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("FirstSeenTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("first_seen_time");

                    b.Property<IPAddress>("LastSeenAddress")
                        .IsRequired()
                        .HasColumnType("inet")
                        .HasColumnName("last_seen_address");

                    b.Property<byte[]>("LastSeenHWId")
                        .HasColumnType("bytea")
                        .HasColumnName("last_seen_hwid");

                    b.Property<DateTime>("LastSeenTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_seen_time");

                    b.Property<string>("LastSeenUserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_seen_user_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("LastSeenUserName");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("player");

                    b.HasCheckConstraint("LastSeenAddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= last_seen_address");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresServerBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("server_ban_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<ValueTuple<IPAddress, int>?>("Address")
                        .HasColumnType("inet")
                        .HasColumnName("address");

                    b.Property<DateTime>("BanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ban_time");

                    b.Property<Guid?>("BanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("banning_admin");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<byte[]>("HWId")
                        .HasColumnType("bytea")
                        .HasColumnName("hwid");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("Address");

                    b.HasIndex("UserId");

                    b.ToTable("server_ban");

                    b.HasCheckConstraint("AddressNotIPv6MappedIPv4", "NOT inet '::ffff:0.0.0.0/96' >>= address");

                    b.HasCheckConstraint("HaveEitherAddressOrUserIdOrHWId", "address IS NOT NULL OR user_id IS NOT NULL OR hwid IS NOT NULL");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresServerUnban", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("unban_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BanId")
                        .HasColumnType("integer")
                        .HasColumnName("ban_id");

                    b.Property<DateTime>("UnbanTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("unban_time");

                    b.Property<Guid?>("UnbanningAdmin")
                        .HasColumnType("uuid")
                        .HasColumnName("unbanning_admin");

                    b.HasKey("Id");

                    b.HasIndex("BanId")
                        .IsUnique();

                    b.ToTable("server_unban");
                });

            modelBuilder.Entity("Content.Server.Database.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("preference_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AdminOOCColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("admin_ooc_color");

                    b.Property<int>("SelectedCharacterSlot")
                        .HasColumnType("integer")
                        .HasColumnName("selected_character_slot");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("preference");
                });

            modelBuilder.Entity("Content.Server.Database.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("profile_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Age")
                        .HasColumnType("integer")
                        .HasColumnName("age");

                    b.Property<string>("Backpack")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("backpack");

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("char_name");

                    b.Property<string>("Clothing")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("clothing");

                    b.Property<string>("EyeColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("eye_color");

                    b.Property<string>("FacialHairColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("facial_hair_color");

                    b.Property<string>("FacialHairName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("facial_hair_name");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("gender");

                    b.Property<string>("HairColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hair_color");

                    b.Property<string>("HairName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hair_name");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("integer")
                        .HasColumnName("preference_id");

                    b.Property<int>("PreferenceUnavailable")
                        .HasColumnType("integer")
                        .HasColumnName("pref_unavailable");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<string>("SkinColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("skin_color");

                    b.Property<int>("Slot")
                        .HasColumnType("integer")
                        .HasColumnName("slot");

                    b.HasKey("Id");

                    b.HasIndex("PreferenceId");

                    b.HasIndex("Slot", "PreferenceId")
                        .IsUnique();

                    b.ToTable("profile");
                });

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.HasOne("Content.Server.Database.AdminRank", "AdminRank")
                        .WithMany("Admins")
                        .HasForeignKey("AdminRankId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AdminRank");
                });

            modelBuilder.Entity("Content.Server.Database.AdminFlag", b =>
                {
                    b.HasOne("Content.Server.Database.Admin", "Admin")
                        .WithMany("Flags")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRankFlag", b =>
                {
                    b.HasOne("Content.Server.Database.AdminRank", "Rank")
                        .WithMany("Flags")
                        .HasForeignKey("AdminRankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rank");
                });

            modelBuilder.Entity("Content.Server.Database.Antag", b =>
                {
                    b.HasOne("Content.Server.Database.Profile", "Profile")
                        .WithMany("Antags")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Content.Server.Database.AnthroSystem", b =>
                {
                    b.HasOne("Content.Server.Database.Profile", "Profile")
                        .WithOne("AnthroSystem")
                        .HasForeignKey("Content.Server.Database.AnthroSystem", "ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Content.Server.Database.Job", b =>
                {
                    b.HasOne("Content.Server.Database.Profile", "Profile")
                        .WithMany("Jobs")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresServerUnban", b =>
                {
                    b.HasOne("Content.Server.Database.PostgresServerBan", "Ban")
                        .WithOne("Unban")
                        .HasForeignKey("Content.Server.Database.PostgresServerUnban", "BanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ban");
                });

            modelBuilder.Entity("Content.Server.Database.Profile", b =>
                {
                    b.HasOne("Content.Server.Database.Preference", "Preference")
                        .WithMany("Profiles")
                        .HasForeignKey("PreferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preference");
                });

            modelBuilder.Entity("Content.Server.Database.Admin", b =>
                {
                    b.Navigation("Flags");
                });

            modelBuilder.Entity("Content.Server.Database.AdminRank", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Flags");
                });

            modelBuilder.Entity("Content.Server.Database.PostgresServerBan", b =>
                {
                    b.Navigation("Unban");
                });

            modelBuilder.Entity("Content.Server.Database.Preference", b =>
                {
                    b.Navigation("Profiles");
                });

            modelBuilder.Entity("Content.Server.Database.Profile", b =>
                {
                    b.Navigation("Antags");

                    b.Navigation("AnthroSystem")
                        .IsRequired();

                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
