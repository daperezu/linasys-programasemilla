# LinaSys

## 📌 Overview

LinaSys is a platform designed to facilitate the management of projects, user participation, and data-driven evaluations. It includes functionalities for administrators and participants to create, manage, and analyze forms, surveys, and diagnostics.

## 📂 Project Structure

The platform consists of different roles and functionalities:

### 🔹 **Administrator Features**

- **Create and Manage Forms**: Define form name, design cover, set start/end dates, and edit questions with scoring and recommendations.
- **Control Form Settings**: Assign question segments, define response types (single/multiple selection, text, numeric), and enforce required responses.
- **Dashboard & Data Management**: Modify questions, duplicate forms/projects, edit participant responses, and review/download results.
- **Access Control**: Generate unique access codes for participants and share form links.
- **Bulk Participant Upload**: Import participant lists from Excel and send automated registration emails.
- **Response Analysis**: View aggregated results, compare individual/group responses, and generate graphical reports.
- **Evaluation Process**: Conduct surveys with various question types and allow anonymous responses.

### 🔹 **Participant Features**

- **User Authentication & Access**: Seamless login/register via the homepage.
- **Restricted Access to Projects**: Participants only see relevant projects they are registered in.
- **Form Submission**: Clearly visible start buttons, deadline reminders, and support for multiple submissions when applicable.
- **Mentor Assessments**: Ability for mentors to complete multiple evaluations with structured response order.

### 🔹 **Platform Features**

- **Super Admin Dashboard**: Displays overall statistics (total projects, diagnostics, archived projects).
- **Admin Controls**: Create, modify, duplicate, and delete projects.
- **User Management**: Administer different roles: Administrator, Coordinator, Guide, Mentor, Facilitator, Starter, and Liaison.
- **Tools & Resources**: Includes diagnostics, whiteboards, logs, and templates for project configurations.
- **Report Generation**: Create, assign, and manage custom report templates.
- **Notifications & Messaging**: System-generated emails for registration, approvals, diagnostics, and project updates.

## ⚙️ Installation & Setup

1. Clone the repository:
   ```sh
   git clone https://github.com/your-repository/LinaSys.git
   ```
2. Navigate to the project folder:
   ```sh
   cd LinaSys
   ```
3. Install dependencies (if applicable):
   ```sh
   dotnet restore  # For backend
   ```
4. Configure environment variables.
5. Start the development server:
   ```sh
   dotnet run    # For backend
   ```

## 🚀 Deployment

1. Build the project:
   ```sh
   dotnet publish  # Backend
   ```
2. Deploy to the selected environment.

## 🛠 Tech Stack

- **Frontend**: React.js / Vue.js (Based on implementation)
- **Backend**: ASP.NET Core 8
- **Database**: SQL Server
- **Authentication**: Microsoft Identity
- **Cloud**: Azure (Deployment & Storage)

## 📌 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch:
   ```sh
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```sh
   git commit -m "Added new feature"
   ```
4. Push to your branch:
   ```sh
   git push origin feature-name
   ```
5. Create a Pull Request.

## 📄 License

This project is licensed under the MIT License. (missing License file)

## 📞 Contact

For any inquiries, please contact: [hablemos@programasemilla.com](mailto:hablemos@programasemilla.com)
