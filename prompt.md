You are an expert C# desktop application engineer tasked with building a complete Windows Forms application for a university Final Year Project management system.

You will be given these attachment files:
1) A project description file containing the app requirements and workflow
2) A SQL script file containing the full MySQL schema, tables, keys, constraints, and seed/reference structure
3) An example DatabaseHelper.cs file
4) A .designer.cs file that shows the expected WinForms designer code style

Treat the SQL script as the single source of truth for the database. Do not change the schema. Do not use Entity Framework. All direct database access must be done only from the DAL layer using raw SQL queries with parameterized commands.

PRIMARY OBJECTIVE
Build a fully functional, maintainable, production-quality Windows Forms application in C# for managing final year project operations. The app must support the committee workflow for:
- Student Management
- Advisor Management
- Project Management
- Group Management
- Evaluation Management
- Report Management

The system should be usable by committee members to manage students, advisors, projects, group formation, project assignment, advisory board assignment, evaluations, and PDF reports.

TECHNOLOGY AND STACK REQUIREMENTS
- Language: C#
- UI Framework: Windows Forms
- Database: MySQL
- Database access: raw SQL only, no ORM, no Entity Framework
- Reports: PDF format using an appropriate library, preferably iText
- UI components: only standard Windows Forms controls available in the designer/UI builder
- No drawing library
- No WPF
- No custom visual frameworks unless absolutely necessary and clearly justified
- Use only safe, parameterized database queries
- Handle exceptions gracefully and show readable user-facing error messages

ARCHITECTURE REQUIREMENTS
Use a clean layered architecture and keep responsibilities strictly separated.

Required project structure:

UI/
- Forms
- UserControls
- Dialogs
- Any other presentation-layer files
- Each form/user control should follow standard WinForms file structure:
  - Name.cs
  - Name.Designer.cs
  - Name.resx

BL/
- Business rules and validation
- Example files:
  - StudentBL.cs
  - AdvisorBL.cs
  - ProjectBL.cs
  - GroupBL.cs
  - EvaluationBL.cs
  - ReportBL.cs
- BL must not contain SQL

DAL/
- All direct database operations
- Example files:
  - StudentDAL.cs
  - AdvisorDAL.cs
  - ProjectDAL.cs
  - GroupDAL.cs
  - EvaluationDAL.cs
  - ReportDAL.cs
- DAL is the only layer allowed to talk directly to the database

Models/
- Database entity models
- Example:
  - Student.cs
  - Advisor.cs
  - Project.cs
  - Group.cs
  - Evaluation.cs
  - Lookup.cs
- Model types must reflect database nullability correctly
- Foreign-key-linked values should be represented properly
- Composite-key relationship entities should also be modeled explicitly where needed

Utilities/
- Shared helpers
- Example:
  - DatabaseHelper.cs
  - ValidationHelper.cs
  - MappingHelper.cs
  - PdfHelper.cs
  - Any reusable utilities needed

DATABASE RULES
- Use the SQL file exactly as provided
- Do not rename tables, columns, keys, or constraints
- Do not add, remove, or modify schema objects unless the SQL file explicitly requires it
- Respect all primary keys, foreign keys, composite keys, defaults, unique constraints, and nullable columns
- All insert/update/delete logic must preserve database integrity
- Any lookup/reference data must be handled based on the schema and lookup categories defined in the script
- Assume the schema is authoritative even if it differs from the naming you might prefer

SYSTEM DOMAIN
The app manages final year project workflows in a Computer Science department.

Main operational areas:

1) Student Management
- Add, edit, delete, search, and view students
- Maintain student identity and person details
- Respect registration number handling and any uniqueness constraints
- Validate required fields, formats, and DB constraints
- Show user-friendly messages for validation and database errors

2) Advisor Management
- Add, edit, delete, search, and view advisors
- Advisors are linked to person data plus advisor-specific fields
- Handle designations through lookup values from the database
- Maintain salary and any other advisor-specific properties
- Keep data consistent with schema rules

3) Project Management
- Add, edit, delete, search, and view projects
- Handle title, description, and any project metadata defined in schema
- Prevent duplicate or invalid entries
- Provide clear forms for managing project records

4) Group Management
- Create and manage student groups
- Add students to groups and remove them
- Use assignment dates and statuses where applicable
- Prevent duplicate membership entries
- Enforce reasonable business rules around group composition based on schema and domain logic
- Display group members clearly

5) Project Assignment
- Assign a project to a group
- Prevent duplicate group-project assignment rows
- Show assignment date
- Ensure a group has a valid project assignment state
- Provide a clear UI for assignment and reassignment operations

6) Advisory Board Assignment
- Assign multiple advisors to a project
- Support main advisor, co-advisor, and industry advisor roles
- Advisor role must come from the relevant lookup category in the database
- Prevent invalid or duplicate role assignment combinations
- Show the advisory board for each project in a clear, structured way

7) Evaluation Management
- Define and manage evaluation records
- Assign evaluation marks to groups
- Capture obtained marks and evaluation dates
- Validate marks against total marks where appropriate
- Show evaluation histories by group, project, and/or student where useful
- Ensure composite key constraints are honored

8) Report Management
- Generate professional PDF reports using iText or an equivalent suitable PDF library
- Reports must be print-ready, cleanly formatted, and professional
- Minimum reports:
  - Project list with advisory board and student list
  - Marks sheet showing evaluation-wise marks for each student and project
- Additional useful reports are encouraged if they help committee workflow
- Reports should have headers, footers, table formatting, titles, and readable layout
- Include export/save-to-PDF functionality

UI/UX REQUIREMENTS
- Design a modern, polished, professional Windows Forms interface
- Use a consistent modern color theme
- Create a sidebar navigation menu for the six major sections
- Use a top header and a content area
- Keep screens uncluttered and task-focused
- Use standard WinForms controls such as panels, buttons, labels, text boxes, combo boxes, data grids, tabs, group boxes, and dialogs
- Prefer a clean, responsive layout with proper spacing and alignment
- Make the UI intuitive for non-technical committee users
- Every major screen should support CRUD and navigation cleanly
- Show clear success, warning, and error messages

WINFORMS CODE STYLE REQUIREMENTS
- Keep the project structure close to what Visual Studio generates for WinForms
- Forms and user controls should be split into:
  - .cs
  - .Designer.cs
  - .resx
- Do not collapse everything into one file
- Use partial classes where appropriate
- Keep designer-generated style intact where possible
- Follow the attached .designer.cs file as a style reference
- Organize screens and controls into meaningful folders

BUSINESS LOGIC REQUIREMENTS
- BL must contain all validation and business rules
- BL should orchestrate workflows and enforce consistency before calling DAL
- BL must not know SQL syntax
- BL should handle domain validation such as:
  - required fields
  - field format validation
  - logical validation
  - duplicate checks
  - key constraint safety
  - cross-entity consistency
- BL should expose clear methods for UI consumption

DAL REQUIREMENTS
- DAL must contain raw SQL only
- Use parameterized queries everywhere
- Handle connections, commands, readers, and transactions correctly
- Dispose resources properly
- Support CRUD and query operations cleanly
- DAL must not show UI messages
- DAL must not contain business rules beyond data access concerns
- Where transactional consistency is needed, use proper transaction handling

MODEL REQUIREMENTS
Build model classes that match the database tables and relationships.
- Use correct types for all fields
- Use nullable types where the database allows nulls
- Include relationship fields and helper properties where needed
- Represent composite-key tables properly
- Keep models simple, clean, and serialization-friendly

REPORT REQUIREMENTS
Use iText as the preferred PDF generation library.
The reporting layer should:
- Generate PDF files directly from the application
- Support clean styling and tabular layouts
- Include report title, date, and contextual metadata
- Be able to export reports to a chosen file path
- Handle empty datasets gracefully
- Handle long tables, multiple pages, and page headers/footers if needed

FUNCTIONAL EXPECTATIONS
The application should include:
- Master data management screens for students, advisors, projects, and evaluations
- Group creation and management
- Assignment workflow for group-project and project-advisor relationships
- Evaluation marking workflow
- Search/filter capabilities where useful
- Validation for all inputs
- Reusable helper logic
- Error-safe database interactions
- A report module for PDF generation

QUALITY BAR
The final solution should be:
- Compile-ready
- Modular
- Readable
- Maintainable
- Easy to extend
- Friendly for end users
- Safe against SQL injection
- Fully aligned with the provided schema and constraints

IMPLEMENTATION GUIDANCE
Before coding:
1) Read the SQL script carefully
2) Identify all tables, constraints, relationships, and lookup categories
3) Map the schema into C# model classes
4) Determine the app workflows and the screens required
5) Design navigation and form structure

During implementation:
- Prefer small, reusable classes and methods
- Keep each feature isolated in its own files
- Use transactions where multiple linked inserts/updates must succeed together
- Validate data before hitting the database
- Return clear results from BL methods to the UI
- Keep report generation separate from CRUD logic
- Use iText for PDF generation

EXPECTED DELIVERABLES
Provide the complete application source code with:
- UI forms and controls
- Designer files
- Models
- BL classes
- DAL classes
- Utilities including DB helper and PDF helper
- Full PDF report generation
- A professional WinForms UI with sidebar navigation
- All required CRUD, assignment, evaluation, and reporting functionality

IMPORTANT CONSTRAINTS
- Do not use Entity Framework
- Do not modify the schema
- Do not use a drawing library for the UI
- Do not mix database code into the UI
- Do not skip validation
- Do not hardcode lookup values when the schema provides lookup tables
- Do not produce placeholder-only code; the application should be truly functional

OUTPUT EXPECTATION
Deliver the solution as a well-organized Visual Studio-style WinForms project structure with properly separated files and production-quality code.