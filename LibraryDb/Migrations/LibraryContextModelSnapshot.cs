﻿// <auto-generated />
using System;
using LibraryDb.Model.LibraryContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryDb.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibraryDb.Model.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookInfoId")
                        .HasColumnType("int");

                    b.Property<int>("Edition")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookInfoId");

                    b.HasIndex("Isbn")
                        .IsUnique();

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(400)");

                    b.Property<decimal>("Rating")
                        .HasPrecision(3, 1)
                        .HasColumnType("decimal(3,1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("BookInfos");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookInfoAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("BookInfoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookInfoId");

                    b.ToTable("BookInfoAuthors");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookLoanCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("LoanCardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("LoanCardId");

                    b.ToTable("BookLoanCards");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("ActualReturnDate")
                        .HasColumnType("date");

                    b.Property<int>("BookLoanCardId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ExpectedReturnDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("LoanDate")
                        .HasColumnType("date");

                    b.Property<bool>("Returned")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BookLoanCardId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.LoanCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("LoanCardNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("LoanCards");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Book", b =>
                {
                    b.HasOne("LibraryDb.Model.Entities.BookInfo", "BookInfo")
                        .WithMany("Books")
                        .HasForeignKey("BookInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookInfo");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookInfoAuthor", b =>
                {
                    b.HasOne("LibraryDb.Model.Entities.Author", "Author")
                        .WithMany("BookInfoAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryDb.Model.Entities.BookInfo", "BookInfo")
                        .WithMany("BookInfoAuthors")
                        .HasForeignKey("BookInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("BookInfo");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookLoanCard", b =>
                {
                    b.HasOne("LibraryDb.Model.Entities.Book", "Book")
                        .WithMany("BookLoanCards")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryDb.Model.Entities.LoanCard", "LoanCard")
                        .WithMany("BookLoanCard")
                        .HasForeignKey("LoanCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("LoanCard");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Loan", b =>
                {
                    b.HasOne("LibraryDb.Model.Entities.BookLoanCard", "BookLoanCard")
                        .WithMany()
                        .HasForeignKey("BookLoanCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookLoanCard");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.LoanCard", b =>
                {
                    b.HasOne("LibraryDb.Model.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Author", b =>
                {
                    b.Navigation("BookInfoAuthors");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.Book", b =>
                {
                    b.Navigation("BookLoanCards");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.BookInfo", b =>
                {
                    b.Navigation("BookInfoAuthors");

                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryDb.Model.Entities.LoanCard", b =>
                {
                    b.Navigation("BookLoanCard");
                });
#pragma warning restore 612, 618
        }
    }
}
