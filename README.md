Cinema Management System
Project Overview

A comprehensive ASP.NET Core MVC application for managing cinema operations, including movie screenings, ticket sales, user management, and AI-powered assistance. The system implements modern software design patterns, features a beautiful glassmorphism UI design, and integrates with local AI models for enhanced functionality.
 Architecture
MVC Pattern

This project follows the Model-View-Controller (MVC) architectural pattern, providing clear separation of concerns and maintainable code structure.
Controllers (10 Total)

The application includes the following controllers:

    HomeController - Main landing page and privacy views

    AiController - AI-powered chat functionality using local Ollama models

    TicketController - Comprehensive ticket management with pricing strategies

    MoviesController - Movie catalog management with actor relationships

    HallsController - Cinema hall management

    UsersController - User account management

    ScreeningsController - Movie screening scheduling

    ReviewsController - User review system

    CinemasController - Cinema location management

    ActorsController - Actor information management

 Modern UI Design
Glassmorphism Design System

The application features a modern glassmorphism UI with:

    Gradient backgrounds and glass-like card effects

    Backdrop blur filters for frosted glass appearance

    Smooth animations and hover effects

    Responsive design that works on all devices

    Modern color schemes with gradient accents

Key UI Features:

    Glass cards with backdrop blur effects

    Gradient headers and buttons

    Smooth transitions and micro-interactions

    Responsive grid layouts

    Modern form designs with floating labels

    Interactive elements with hover states

Design Patterns Implemented
1. Strategy Pattern

    Location: PricingStrategies.cs

    Purpose: Dynamic pricing calculation based on user type

    Strategies:

        StandardPricingStrategy - Regular pricing

        StudentPricingStrategy - 20% discount

        SeniorPricingStrategy - 30% discount

        PremiumPricingStrategy - 50% premium for VIP services

2. Factory Pattern

    Location: NotificationFactory.cs

    Purpose: Create different notification types

    Notification Types:

        EmailNotification - Email notifications

        SMSNotification - SMS messaging

        PushNotification - Push notifications

3. Singleton Pattern

    Location: LoggerService.cs

    Purpose: Global logging service with single instance

    Features: Thread-safe implementation with information, warning, and error logging

 AI Integration
Local AI Model Support

    Service: OllamaService.cs

    Model: LLaMA 3 (local deployment)

    Endpoint: http://localhost:11434/api/generate

    Features:

        AI-powered chat assistance

        Local processing for privacy

        Stream and non-stream response modes

        Modern chat interface with typing indicators

AI Controller Features

    Endpoint: /Ai/Ask

    Functionality: Process natural language prompts

    Modern Chat UI: Glassmorphism design with real-time messaging

    Error Handling: Connection validation and graceful failure

 Database & Models
Entity Framework Core

    DbContext: AppDbContext.cs with comprehensive relationship configuration

    Relationships: One-to-Many, Many-to-Many with junction tables

Core Models (12 Total)

    Actor - Actor information with movie relationships

    Cinema - Cinema locations with halls

    Genre - Movie genres and categories

    Hall - Screening halls with capacity

    Movie - Movie information with actors and genres

    Review - User reviews and ratings

    Screening - Movie showtimes and scheduling

    Ticket - Booking system with pricing strategies

    User - Customer management system

    MovieActors - Many-to-many junction table

    MovieGenres - Many-to-many junction table

    ScreeningCinemas - Many-to-many junction table

Database Relationships

    One-to-Many: Cinema→Halls, Screening→Tickets, User→Tickets, User→Reviews, Movie→Reviews

    Many-to-Many: Movies↔Actors, Movies↔Genres, Screenings↔Cinemas, Screenings↔Movies

 CRUD Operations

The application implements full CRUD (Create, Read, Update, Delete) operations for all major entities with modern UI:
Standard Operations per Controller:

    Index() - List all records with modern card layouts

    Create() - Display creation form with glassmorphism design

    Create([Bind]) - Process creation with validation

    Edit(id) - Display edit form with current values

    Edit(id, entity) - Process updates with error handling

    Delete(id) - Confirm deletion with warning UI

    DeleteConfirmed(id) - Execute deletion

Advanced Features:

    Data Validation: ModelState validation with modern error displays

    Relationship Management: Many-to-many with junction entities

    Dropdown Population: Dynamic form element population

    Anti-Forgery Protection: Secure form submissions

    Search & Filter: Real-time search functionality

 Business Logic
Ticket Pricing System

    Dynamic price calculation based on user type

    Quantity-based pricing with strategy pattern

    User type categorization (Standard, Student, Senior, Premium)

Notification System

    Factory-based notification creation

    Multiple delivery channels (Email, SMS, Push)

    Logging of all notification activities

Screening Management

    Multi-cinema screening support

    Time-based scheduling with calendar integration

    Capacity planning through hall management

 Actor Management System
Modern UI Features for Actors:

    Glass Card Layouts with actor avatars

    Statistics Dashboard with real-time metrics

    Search & Filter functionality

    Responsive Design for all screen sizes

    Interactive Forms with validation

    Age Calculation and display

Actor CRUD Operations:

    Create: Modern form with validation and icons

    Read: Card-based listing with statistics

    Update: Pre-populated forms with current data

    Delete: Confirmation dialogs with warning UI

 Technical Features
Logging System

    Singleton logger service with thread safety

    Information, warning, and error logging levels

    Log viewing interface via /Ticket/ViewLogs

Error Handling

    Global error handling with user-friendly messages

    Try-catch blocks with proper exception management

    Model validation with modern UI feedback

Security Features

    Anti-forgery token validation

    Input sanitization and validation

    Entity existence validation

    Secure form submissions