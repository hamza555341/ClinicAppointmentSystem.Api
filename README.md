🏥 Clinic Management System API

A production-ready Clinic Management System API built using ASP.NET Core Web API and Clean Architecture. This system is designed to help clinic administrators and reception staff manage clinic operations through a centralized, scalable, and maintainable backend.

The project focuses on real-world backend engineering practices including role-based system design, background job processing using Hangfire, and global exception handling.

📌 Project Purpose

The system provides a structured backend that allows administrators to manage doctors, patients, specializations, and appointments efficiently. Patients can also interact with the system to browse doctors and book appointments.

This project was built with production mindset, scalability, and maintainability as core priorities.

🧱 Architecture Overview

The project follows Clean Architecture and is divided into four layers:

Domain Layer
Contains core entities, base entity structure, and business rules.

Application Layer
Contains interfaces, DTOs, specifications, and abstraction for business logic.

Infrastructure Layer
Contains Entity Framework Core implementation, DbContext, Identity configuration, repository implementations, and Hangfire integration.

Presentation Layer (API)
Contains controllers, endpoints, middleware, and Swagger configuration.

This separation ensures scalability, maintainability, and testability.

⚙️ Technologies Used

• ASP.NET Core Web API
• Entity Framework Core
• SQL Server
• Clean Architecture
• Repository Pattern
• Specification Pattern
• ASP.NET Core Identity
• Role-based system preparation
• Hangfire for Background Jobs
• Global Exception Handling Middleware
• Dependency Injection
• LINQ
• Async / Await Programming
• Soft Delete Pattern
• Swagger / OpenAPI Documentation

🔐 Authentication Endpoints

Authentication module provides secure user management.

Available endpoints include:

• Register Patient
• Register Doctor
• Login
• Get Current User
• Check Email Exists

The system is designed and prepared for role-based authorization.

👤 Patient Features

Patients can:

• Register and login
• View doctors
• View doctor details
• View specializations
• Book appointments
• View their appointments
• Cancel appointments

This simulates real clinic interaction from the patient perspective.

🛠 Admin Features

Admins have full control over clinic operations.

Doctor Management

• Add doctor
• Update doctor
• View doctors
• Activate / Deactivate doctor
• Soft delete doctor

Specialization Management

• Add specialization
• Update specialization
• Delete specialization

Appointment Management

• View appointments
• Monitor and control clinic workflow

🔄 Background Jobs using Hangfire

Hangfire is integrated to support background job processing.

This allows the system to handle scalable background tasks such as:

• Future email notifications
• Appointment reminders
• Scheduled operations

Hangfire ensures non-blocking and production-ready background processing.

⚠️ Global Exception Handling Middleware

A custom exception handling middleware was implemented to:

• Handle exceptions globally
• Prevent application crashes
• Return consistent error responses
• Improve debugging and maintainability

This is essential in production-level systems.

🧠 Design Patterns Used

Repository Pattern
Provides abstraction between application and database layers.

Specification Pattern
Provides reusable and flexible query logic.

Soft Delete Pattern
Prevents permanent data deletion and ensures data safety.

📖 API Documentation

Swagger is integrated for API documentation and testing.

Swagger provides:

• Clear endpoint visualization
• Request and response models
• Easy API testing

Swagger endpoint:

/swagger
📂 API Modules

The system includes the following modules:

• Authentication
• AdminDoctors
• AdminSpecializations
• AdminAppointments
• PatientDoctors
• PatientSpecializations
• PatientAppointments

🚀 Engineering Focus

This project was built to apply real backend engineering practices including:

• Clean Architecture implementation
• Role-based system design
• Hangfire background job integration
• Global exception handling middleware
• Scalable and maintainable structure
• Professional Git workflow with structured commits

🔮 Future Improvements

Planned improvements include:

• JWT Authentication
• Full role-based authorization
• Email notifications
• Deployment to production
• Frontend integration

💡 Learning Outcomes

This project helped strengthen my skills in:

• Backend architecture design
• Clean Architecture
• Role-based systems
• Hangfire integration
• Exception handling middleware
• Building production-ready APIs
