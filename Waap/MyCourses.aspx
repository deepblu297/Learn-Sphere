<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyCourses.aspx.cs" Inherits="LearnSphere.Learner.MyCourses" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Enrolled Courses - LearnSphere</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/home.css" rel="stylesheet" />
    <style>
        /* Sidebar */
        .sidebar {
            height: 100vh;
            background-color: #111827; 
            padding: 1rem;
        }
        .sidebar h4, .sidebar p {
            color: #fff;
            text-align: center;
        }
        .nav-link {
            color: #fff;
        }
        .nav-link.active {
            font-weight: bold;
            background-color: #495057;
            border-radius: 5px;
        }

        /* Stats Cards */
        .stat-card {
            background-color: #fff;
            border-radius: 0.5rem;
            padding: 1rem;
            text-align: center;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
            transition: transform 0.3s ease;
        }
        .stat-card:hover { transform: translateY(-5px); }
        .stat-icon { font-size: 2rem; margin-bottom: 0.5rem; }

        /* Course Cards */
        .courses-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
            gap: 1.5rem;
        }
        .course-card {
            background-color: #fff;
            border-radius: 0.5rem;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }
        .course-card:hover { transform: translateY(-5px); box-shadow: 0 8px 16px rgba(0,0,0,0.2); }
        .course-card img {
            width: 100%;
            height: 180px;
            object-fit: cover;
        }
        .course-card-body { padding: 1rem; }
        .enrolled-badge {
            background-color: #28a745;
            color: #fff;
            padding: 0.2rem 0.6rem;
            border-radius: 0.3rem;
            font-size: 0.8rem;
        }
        .course-actions .btn {
            margin-right: 0.5rem;
            margin-top: 0.5rem;
        }
        .empty-state { text-align: center; padding: 2rem; }
    </style>
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <!-- Sidebar -->
                <nav class="col-12 col-md-3 col-lg-2 sidebar">
                    <div class="mb-4 text-white text-center">
                    <h4 class="fw-bold mb-0">LearnSphere</h4>
                    <small class="text-muted">Student Panel</small>
                    <hr class="border-secondary my-3" />
                    </div>
                    <ul class="nav flex-column">
                        <li class="nav-item mb-2">
                            <a class="nav-link" href="Dashboard.aspx"><img src="../Assets/Images/manage.png" class="nav-icon"/> Available Courses</a>
                        </li>
                        <li class="nav-item mb-2">
                            <a class="nav-link active" href="MyCourses.aspx"><img src="../Assets/Images/enroll.png" class="nav-icon"/> My Enrolled Courses</a>
                        </li>
                    </ul>
                </nav>

                <!-- Main Content -->
                <main class="col-md-10 ms-sm-auto col-lg-10 px-md-4 py-4">
                    <!-- Top Bar -->
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h2>My Enrolled Courses</h2>
                        <!-- Top Right: Student Name + Dropdown Logout -->
                        <div class="dropdown">
                            <button class="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <img src="../Assets/Images/avatar.png" class="nav-icon"/><asp:Label ID="lblStudentName" runat="server"></asp:Label>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li>
                                    <asp:LinkButton ID="btnLogout" runat="server" CssClass="dropdown-item text-danger" OnClick="btnLogout_Click" CausesValidation="false">
                                       <img src="../Assets/Images/logout.png" class="nav-icon"/> Logout
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <!-- Success/Error Messages -->
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- Stats -->
                    <div class="row mb-4 g-3">
                        <div class="col-md-4">
                            <div class="stat-card">
                                <div class="stat-icon green"><img src="../Assets/Images/en-roll.png" class="nav-icon"/></div>
                                <h3><asp:Label ID="lblEnrolledCount" runat="server">0</asp:Label></h3>
                                <p>Enrolled Courses</p>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="stat-card">
                                <div class="stat-icon blue"><img src="../Assets/Images/books.png" class="nav-icon"/></div>
                                <h3><asp:Label ID="lblAvailableCount" runat="server">0</asp:Label></h3>
                                <p>More Available</p>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="stat-card">
                                <div class="stat-icon purple"><img src="../Assets/Images/calendar.png" class="nav-icon"/></div>
                                <h3><asp:Label ID="lblLatestEnrollment" runat="server">N/A</asp:Label></h3>
                                <p>Latest Enrollment</p>
                            </div>
                        </div>
                    </div>

                    <!-- Courses Grid -->
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4>Your Learning Journey</h4>
                        <a href="Dashboard.aspx" class="btn btn-primary btn-sm">+ Explore More Courses</a>
                    </div>

                    <div class="courses-grid">
                        <asp:Repeater ID="rptEnrolledCourses" runat="server" OnItemCommand="rptEnrolledCourses_ItemCommand">
                            <ItemTemplate>
                                <div class="course-card enrolled">
                                    <%# !string.IsNullOrEmpty(Eval("ThumbnailPath").ToString()) 
                                        ? "<img src=\"../" + Eval("ThumbnailPath") + "\" alt=\"Course Thumbnail\" />" 
                                        : "<div class=\"thumbnail-placeholder\"><img src=\"../Assets/Images/manage.png\"/></div>" %>
                                    <div class="course-card-body">
                                        <h5><%# Eval("CourseName") %> <span class="enrolled-badge">Enrolled</span></h5>
                                        <p><%# Eval("Description") %></p>
                                        <small> <%# Eval("InstructorName") %></small><br />
                                        <small>Enrolled: <%# Convert.ToDateTime(Eval("EnrolledDate")).ToString("MMM dd, yyyy") %></small>
                                        <div class="course-actions mt-2">
                                            <asp:LinkButton ID="btnViewCourse" runat="server" CssClass="btn btn-outline-primary btn-sm"
                                                CommandName="ViewCourse" CommandArgument='<%# Eval("CourseID") %>' CausesValidation="false">
                                                View Course
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnUnenroll" runat="server" CssClass="btn btn-outline-danger btn-sm"
                                                CommandName="Unenroll" CommandArgument='<%# Eval("EnrollmentID") %>'
                                                OnClientClick="return confirm('Are you sure you want to unenroll?');" CausesValidation="false">
                                                Unenroll
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- No Enrollments -->
                    <asp:Panel ID="pnlNoEnrollments" runat="server" Visible="false">
                        <div class="empty-state">
                            <h2>You haven't enrolled in any courses yet!</h2>
                            <p>Start your learning journey by exploring our available courses.</p>
                            <a href="Dashboard.aspx" class="btn btn-primary btn-lg">Browse Courses</a>
                        </div>
                    </asp:Panel>
                </main>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
