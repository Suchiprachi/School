using Microsoft.EntityFrameworkCore;
using Secu_School_API.Models;
using YourNamespace.Models;

namespace Secu_School_API.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<StudentEnrollment> StudentEnrollments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamSubject> ExamSubjects { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassSectionTeacher> ClassSectionTeachers { get; set; }
        public DbSet<EventGallery> EventGalleries { get; set; }
        public DbSet<GalleryFile> GalleryFiles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<SalaryComponent> SalaryComponents { get; set; }
        public DbSet<UserLogin> Logins { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Role> Roles { get; set; }

        // Library
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FineMaster> FineMasters { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table mappings
            modelBuilder.Entity<Subject>().ToTable("subject");
            modelBuilder.Entity<School>().ToTable("school");
            modelBuilder.Entity<StudentEnrollment>().ToTable("student_enrollment");
            modelBuilder.Entity<Section>().ToTable("section");
            modelBuilder.Entity<Class>().ToTable("class");
            modelBuilder.Entity<Department>().ToTable("department");
            modelBuilder.Entity<Designation>().ToTable("designation");
            modelBuilder.Entity<EmployeeType>().ToTable("employee_type");
            modelBuilder.Entity<LeaveType>().ToTable("leave_type");
            modelBuilder.Entity<SalaryComponent>().ToTable("salary_component");
            modelBuilder.Entity<UserLogin>().ToTable("user_login");
            modelBuilder.Entity<Publisher>().ToTable("publisher");
            modelBuilder.Entity<Location>().ToTable("location");
            modelBuilder.Entity<Language>().ToTable("language");
            modelBuilder.Entity<Book>().ToTable("book");
            modelBuilder.Entity<BookCopy>().ToTable("book_copies");
            modelBuilder.Entity<Gallery>().ToTable("gallery");
            modelBuilder.Entity<ClassSectionTeacher>().ToTable("class_section_teacher");

            // Gallery enum conversion
            modelBuilder.Entity<Gallery>()
                .Property(g => g.MediaType)
                .HasConversion<string>();

            // =======================
            // Relationships
            // =======================
            modelBuilder.Entity<StudentEnrollment>()
                .HasOne(s => s.School)
                .WithMany(s => s.StudentEnrollments)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AcademicYear>()
                .HasOne(a => a.School)
                .WithMany(s => s.AcademicYears)
                .HasForeignKey(a => a.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.School)
                .WithMany(s => s.Departments)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Designation>()
                .HasOne(d => d.Department)
                .WithMany(dep => dep.Designations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Designation>()
                .HasOne(d => d.School)
                .WithMany(s => s.Designations)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.School)
                .WithMany(c => c.ClassSectionTeachers)
                .HasForeignKey(cst => cst.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.Class)
                .WithMany(c => c.ClassSectionTeachers)
                .HasForeignKey(cst => cst.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.Section)
                .WithMany(s => s.ClassSectionTeachers)
                .HasForeignKey(cst => cst.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.Staff)
                .WithMany(s => s.ClassSectionTeachers)
                .HasForeignKey(cst => cst.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.Subject)
                .WithMany(s => s.ClassSectionTeachers)
                .HasForeignKey(cst => cst.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSectionTeacher>()
                .HasOne(cst => cst.AcademicYear)
                .WithMany(a => a.ClassSectionTeachers)
                .HasForeignKey(cst => cst.AcademicYearId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeType>()
                .HasOne(et => et.School)
                .WithMany()
                .HasForeignKey(et => et.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLogin>()
                .HasOne(u => u.School)
                .WithMany()
                .HasForeignKey(u => u.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveType>()
                .HasOne(l => l.School)
                .WithMany()
                .HasForeignKey(l => l.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalaryComponent>()
                .HasOne(c => c.School)
                .WithMany(s => s.SalaryComponents)
                .HasForeignKey(c => c.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.School)
                .WithMany(s => s.Roles)
                .HasForeignKey(r => r.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FineMaster>()
                .HasOne(f => f.School)
                .WithMany(s => s.FineMasters)
                .HasForeignKey(f => f.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FineMaster>()
                .HasOne(f => f.Role)
                .WithMany(r => r.Fines)
                .HasForeignKey(f => f.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.School)
                .WithMany(sch => sch.Staffs)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Role)
                .WithMany(r => r.Staffs)
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Author>()
                .HasOne(e => e.School)
                .WithMany(s => s.Authors)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(e => e.School)
                .WithMany(s => s.Categories)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Publisher>()
                .HasOne(e => e.School)
                .WithMany(s => s.Publishers)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                .HasOne(e => e.School)
                .WithMany(s => s.Locations)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Language>()
                .HasOne(e => e.School)
                .WithMany(s => s.Languages)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(e => e.School)
                .WithMany(s => s.Books)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookCopy>()
                .HasOne(e => e.Location)
                .WithMany(s => s.BookCopies)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gallery>()
                .HasOne(g => g.School)
                .WithMany(s => s.Galleries)
                .HasForeignKey(g => g.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventGallery>()
               .HasMany(e => e.Files)
               .WithOne(f => f.EventGallery)
               .HasForeignKey(f => f.EventGalleryId)
               .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // Exam & ExamSubject mapping
            // =======================
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("exams");
                entity.HasKey(e => e.ExamId);

                entity.HasOne(e => e.School)
                      .WithMany(s => s.Exams)
                      .HasForeignKey(e => e.SchoolId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.AcademicYear)
                      .WithMany(s=>s.Exams)
                      .HasForeignKey(e => e.AcademicYearId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ExamSubjects)
                      .WithOne(es => es.Exam)
                      .HasForeignKey(es => es.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExamSubject>(entity =>
            {
                entity.ToTable("exam_subjects");
                entity.HasKey(e => e.ExamSubjectId);

                entity.HasOne(e => e.Class)
                      .WithMany(c => c.ExamSubjects)
                      .HasForeignKey(e => e.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Section)
                      .WithMany(s => s.ExamSubjects)
                      .HasForeignKey(e => e.SectionId)
                     .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Subject)
                      .WithMany(s => s.ExamSubjects)
                      .HasForeignKey(e => e.SubjectId);

                entity.HasOne(e => e.Staff)
                      .WithMany(s => s.ExamSubjects)
                      .HasForeignKey(e => e.StaffId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Exam)
                      .WithMany(s => s.ExamSubjects)
                      .HasForeignKey(e => e.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);


            });

        
        }
    }
}
