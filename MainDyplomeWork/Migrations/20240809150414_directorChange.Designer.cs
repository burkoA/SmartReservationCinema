﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartReservationCinema.FilmContext;

namespace SmartReservationCinema.Migrations
{
    [DbContext(typeof(FilmDbContext))]
    [Migration("20240809150414_directorChange")]
    partial class directorChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Cinema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CinemaName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CinemaRating")
                        .HasColumnType("float");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LatCoordinate")
                        .HasColumnType("float");

                    b.Property<string>("Localisation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LongCoordinate")
                        .HasColumnType("float");

                    b.Property<int>("TownId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TownId");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<int?>("IdFilm")
                        .HasColumnType("int");

                    b.Property<int?>("IdUser")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdFilm");

                    b.HasIndex("IdUser");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkExperience")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Director");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.FailedLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email", "Time");

                    b.HasIndex("IPAddress", "Time");

                    b.ToTable("FailedLogins");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.FavouriteFilm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("FavouriteFilms");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Film", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DirectorId")
                        .HasColumnType("int");

                    b.Property<string>("FilmName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Image")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<DateTime>("Realese")
                        .HasColumnType("datetime2");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.FilmMark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.Property<DateTime>("MarkDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("FilmMarks");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Film_Actor", b =>
                {
                    b.Property<int>("Id_Film_Actor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_Actor")
                        .HasColumnType("int");

                    b.Property<int>("Id_Film")
                        .HasColumnType("int");

                    b.HasKey("Id_Film_Actor");

                    b.HasIndex("Id_Actor");

                    b.HasIndex("Id_Film");

                    b.ToTable("FilmActor");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GenreName = "Action"
                        },
                        new
                        {
                            Id = 2,
                            GenreName = "Adventure"
                        },
                        new
                        {
                            Id = 3,
                            GenreName = "Comedy"
                        },
                        new
                        {
                            Id = 4,
                            GenreName = "Drama"
                        },
                        new
                        {
                            Id = 5,
                            GenreName = "Crime"
                        },
                        new
                        {
                            Id = 6,
                            GenreName = "Horror"
                        },
                        new
                        {
                            Id = 7,
                            GenreName = "Fantasy"
                        },
                        new
                        {
                            Id = 8,
                            GenreName = "Romance"
                        },
                        new
                        {
                            Id = 9,
                            GenreName = "Thriller"
                        },
                        new
                        {
                            Id = 10,
                            GenreName = "Animation"
                        },
                        new
                        {
                            Id = 11,
                            GenreName = "Family"
                        },
                        new
                        {
                            Id = 12,
                            GenreName = "War"
                        },
                        new
                        {
                            Id = 13,
                            GenreName = "Documentary"
                        },
                        new
                        {
                            Id = 14,
                            GenreName = "Biography"
                        },
                        new
                        {
                            Id = 15,
                            GenreName = "Sci-fi"
                        },
                        new
                        {
                            Id = 16,
                            GenreName = "Western"
                        },
                        new
                        {
                            Id = 17,
                            GenreName = "Post-apocalyptic"
                        });
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Genre_Film", b =>
                {
                    b.Property<int>("Id_Genre_Film")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_Film")
                        .HasColumnType("int");

                    b.Property<int>("Id_Genre")
                        .HasColumnType("int");

                    b.HasKey("Id_Genre_Film");

                    b.HasIndex("Id_Film");

                    b.HasIndex("Id_Genre");

                    b.ToTable("GenresFilmes");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Hall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CinemaId")
                        .HasColumnType("int");

                    b.Property<string>("HallName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.HallSector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.Property<string>("SectorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.ToTable("HallSectors");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CinemaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<int?>("HallId")
                        .HasColumnType("int");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubtitleLanguageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CinemaId");

                    b.HasIndex("FilmId");

                    b.HasIndex("HallId");

                    b.HasIndex("LanguageId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Subtitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSubtitle")
                        .HasColumnType("bit");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("LanguageId");

                    b.ToTable("Subtitles");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.TicketPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SectorId")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.HasIndex("SessionId");

                    b.ToTable("TicketPrices");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Town", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TownName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Towns");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Region = "Masovian Voivodeship",
                            TownName = "Warsawa"
                        },
                        new
                        {
                            Id = 2,
                            Region = "Lesser Poland Voivodeship",
                            TownName = "Krakow"
                        },
                        new
                        {
                            Id = 3,
                            Region = "Lesser Poland Voivodeship",
                            TownName = "Zakopane"
                        },
                        new
                        {
                            Id = 4,
                            Region = "Lower Silesian Voivodeship",
                            TownName = "Wroclaw"
                        },
                        new
                        {
                            Id = 5,
                            Region = "Subcarpathian Voivodeship",
                            TownName = "Rzeszow"
                        },
                        new
                        {
                            Id = 6,
                            Region = "Lodz Voivodeship",
                            TownName = "Lodz"
                        },
                        new
                        {
                            Id = 7,
                            Region = "Greater Poland Voivodeship",
                            TownName = "Poznan"
                        },
                        new
                        {
                            Id = 8,
                            Region = "Pomeranian Voivodeship",
                            TownName = "Gdansk"
                        },
                        new
                        {
                            Id = 9,
                            Region = "Lublin Voivodeship",
                            TownName = "Lublin"
                        },
                        new
                        {
                            Id = 10,
                            Region = "Podlaskie Voivodeship",
                            TownName = "Bialystock"
                        });
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            FirstName = "Arsen",
                            LastName = "",
                            Password = "12345",
                            RegisterDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "manager@gmail.com",
                            FirstName = "Andrzej",
                            LastName = "",
                            Password = "123",
                            RegisterDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "manager"
                        },
                        new
                        {
                            Id = 3,
                            Email = "user@gmail.com",
                            FirstName = "Kuba",
                            LastName = "",
                            Password = "321",
                            RegisterDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "user"
                        });
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Cinema", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Town", "Town")
                        .WithMany("Cinemas")
                        .HasForeignKey("TownId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Town");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Comment", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Film", "Film")
                        .WithMany("Comments")
                        .HasForeignKey("IdFilm");

                    b.HasOne("SmartReservationCinema.FilmContext.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser");

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.FavouriteFilm", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Film", "Film")
                        .WithMany("FavouriteFilms")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Film", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Director", "Director")
                        .WithMany("Films")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Director");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.FilmMark", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Film", "Film")
                        .WithMany("Marks")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Film_Actor", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Actor", "actor")
                        .WithMany("Films")
                        .HasForeignKey("Id_Actor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Film", "film")
                        .WithMany("Actors")
                        .HasForeignKey("Id_Film")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("actor");

                    b.Navigation("film");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Genre_Film", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Film", "film")
                        .WithMany("Genres")
                        .HasForeignKey("Id_Film")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Genre", "genre")
                        .WithMany("films")
                        .HasForeignKey("Id_Genre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("film");

                    b.Navigation("genre");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Hall", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Cinema", "Cinema")
                        .WithMany()
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cinema");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.HallSector", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Hall", "Hall")
                        .WithMany("HallSectors")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Session", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Cinema", "cinema")
                        .WithMany("Sessions")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Film", "film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Hall", "hall")
                        .WithMany()
                        .HasForeignKey("HallId");

                    b.HasOne("SmartReservationCinema.FilmContext.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("cinema");

                    b.Navigation("film");

                    b.Navigation("hall");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Subtitle", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.Film", "Film")
                        .WithMany("Subtitles")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.TicketPrice", b =>
                {
                    b.HasOne("SmartReservationCinema.FilmContext.HallSector", "HallSector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartReservationCinema.FilmContext.Session", "Session")
                        .WithMany("TicketPrices")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HallSector");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Actor", b =>
                {
                    b.Navigation("Films");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Cinema", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Director", b =>
                {
                    b.Navigation("Films");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Film", b =>
                {
                    b.Navigation("Actors");

                    b.Navigation("Comments");

                    b.Navigation("FavouriteFilms");

                    b.Navigation("Genres");

                    b.Navigation("Marks");

                    b.Navigation("Subtitles");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Genre", b =>
                {
                    b.Navigation("films");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Hall", b =>
                {
                    b.Navigation("HallSectors");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Session", b =>
                {
                    b.Navigation("TicketPrices");
                });

            modelBuilder.Entity("SmartReservationCinema.FilmContext.Town", b =>
                {
                    b.Navigation("Cinemas");
                });
#pragma warning restore 612, 618
        }
    }
}
