# 🏥 MedCore Smart Hospital System

> **Note:** AI was used as an assistant in writing, formatting, and guiding some parts of this project due to time constraints. However, every line of code was fully understood and carefully reviewed to ensure the implementation of best practices and clean architecture.

## 💡 About the Project
**MedCore** is a comprehensive hospital management system built using **C#** and **Entity Framework Core**. The project aims to implement advanced concepts in database design, data protection, and complex business operations like medical appointments and concurrency handling. 

The system is designed for high performance and operational safety, utilizing a **Code-First** approach and **Fluent API** configurations.

---

## 🚀 Advanced EF Core Features

This project heavily utilizes advanced Entity Framework Core features, including:

1. **Inheritance (TPH - Table Per Hierarchy):**
   - Merging `Doctor` and `Patient` entities under a single `Users` table using a `Discriminator` to improve query performance and reduce the total table count.

2. **Safe Deletion (Soft Delete & Global Query Filters):**
   - Records are not physically deleted from the database; instead, an `IsDeleted` flag is set to `true`. 
   - A `Global Query Filter` is applied globally to automatically hide deleted data from the application layer.

3. **Tracking & Monitoring (Audit Trail - Shadow Properties):**
   - Automatically adding a `CreatedAt` shadow property to all tables to document creation time, without needing to define it explicitly in the C# domain models.

4. **Concurrency Control:**
   - Utilizing a `RowVersion` (Timestamp) token to prevent data conflicts if multiple users attempt to modify the exact same record simultaneously (handling `DbUpdateConcurrencyException`).

5. **Owned Entity Types:**
   - Encapsulating complex value objects like `Allergies` and `ChronicConditions` for patients directly within the `Users` table, avoiding unnecessary sub-tables and joins.

6. **High-Performance Queries (Compiled Queries):**
   - Utilizing EF Core Compiled Queries for high-frequency lookup operations (e.g., searching for a doctor by National ID) to guarantee maximum execution speed and minimum overhead.

7. **Database Constraints (Check Constraints):**
   - Applying database-level validation rules to protect data integrity, such as ensuring a schedule's `EndTime` is always strictly after its `StartTime`.

---

## 🗄️ Database Structure (Entities)

- **Specialties**: Medical departments and categories.
- **Users**: The main table containing both Doctors and Patients (TPH).
- **DoctorSchedules**: Doctor availability and booking time slots.
- **Appointments**: Patient bookings linked to specific doctor schedules.
- **Medications**: Available drugs and medicines in the system.
- **Prescriptions**: Medical prescriptions (Acting as a Many-to-Many bridge table between Appointments and Medications).

---

## 🛠️ Tech Stack
- **C# .NET 10.0**
- **Entity Framework Core**
- **Microsoft SQL Server**

---

## ⚙️ How to Run

1. **Clone the Repository:**
   Download the project and open the solution (`.sln`) using Visual Studio.

2. **Update the Database:**
   Open the `Package Manager Console` in Visual Studio and run the following command to generate the database:
   ```powershell
   Update-Database