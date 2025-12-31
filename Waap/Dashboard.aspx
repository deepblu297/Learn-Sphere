<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="LearnSphere.Learner.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard - LearnSphere</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="Assets/home.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        .course-card {
            transition: transform 0.3s ease;
        }
        .course-card:hover {
            transform: translateY(-5px);
        }
        .course-card img {
            height: 180px;
            object-fit: cover;
            border-radius: 0.5rem 0.5rem 0 0;
        }
        .thumbnail-placeholder {
            height: 180px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 48px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 0.5rem 0.5rem 0 0;
            color: #fff;
        }
        .course-badge {
            font-size: 0.85rem;
            padding: 2px 6px;
            border-radius: 0.5rem;
            background-color: #f0f0f0;
        }
        .course-actions .btn {
            margin-right: 5px;
            margin-top: 5px;
        }
        .sidebar {
            background-color: #111827;
            color: #fff;
            min-height: 100vh;
            padding: 1rem;
        }
        .sidebar a.nav-item {
            color: #fff;
            display: block;
            padding: 0.5rem 0;
            text-decoration: none;
        }
        .sidebar a.nav-item.active {
            font-weight: bold;
            color: #ffc107;
        }
        .main-content {
            padding: 2rem;
        }
    </style>
</head>

<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row min-vh-100">

                <!-- SIDEBAR -->
                <div class="col-12 col-md-3 col-lg-2 sidebar">
                    <h4 class="fw-bold mb-0">LearnSphere</h4>
                    <small class="text-muted">Student Panel</small>
                    <hr class="border-secondary my-3" />

                    <nav class="nav flex-column">
                        <a href="Dashboard.aspx" class="nav-item active"><img src="../Assets/Images/manage.png" class="nav-icon"/>Available Courses</a>
                        <a href="MyCourses.aspx" class="nav-item"><img src="../Assets/Images/enroll.png" class="nav-icon"/> My Enrolled Courses</a>
                    </nav>
                </div>

                <!-- MAIN CONTENT -->
                <div class="col-12 col-md-9 col-lg-10 main-content">

                    <!-- TOP BAR -->
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h2 class="fw-bold mb-0">Available Courses</h2>

                        <!-- Logout Dropdown -->
                        <div class="dropdown">
                            <button class="btn btn-light dropdown-toggle fw-semibold" type="button" data-bs-toggle="dropdown">
                                <img src="../Assets/Images/avatar.png" class="nav-icon"/> <asp:Label ID="lblStudentName" runat="server" />
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

                    <!-- SUCCESS / ERROR MESSAGES -->
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert mb-4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- STATISTICS CARDS -->
                    <div class="row g-3 mb-4">
                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-primary"><img src="../Assets/Images/manage.png" class="nav-icon"/></div>
                                    <div>
                                        <h4 class="mb-0"><asp:Label ID="lblTotalCourses" runat="server">0</asp:Label></h4>
                                        <small class="text-muted">Total Available Courses</small>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-success"><img src="../Assets/Images/en-roll.png" class="nav-icon"/></div>
                                    <div>
                                        <h4 class="mb-0"><asp:Label ID="lblEnrolledCourses" runat="server">0</asp:Label></h4>
                                        <small class="text-muted">My Enrolled Courses</small>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-warning"><img src="../Assets/Images/ava.png" class="nav-icon"/></div>
                                    <div>
                                        <h4 class="mb-0"><asp:Label ID="lblAvailableToEnroll" runat="server">0</asp:Label></h4>
                                        <small class="text-muted">Available to Enroll</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- SEARCH AND FILTER -->
                    <div class="d-flex gap-2 mb-4">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search courses..." />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" />
                    </div>

                    <!-- COURSES GRID -->
                    <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                        <asp:Repeater ID="rptCourses" runat="server" OnItemCommand="rptCourses_ItemCommand">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="card course-card shadow-sm">
                                        <%# !string.IsNullOrEmpty(Eval("ThumbnailPath").ToString()) 
                                            ? "<img src='../" + Eval("ThumbnailPath") + "' class='card-img-top' alt='Course Thumbnail' />"
                                            : "<div class='thumbnail-placeholder'><img src=\"../Assets/Images/Dashboard.png\"/></div>" %>
                                        <div class="card-body">
                                            <h5 class="card-title"><%# Eval("CourseName") %></h5>
                                            <span class="course-badge mb-2 d-inline-block">
                                                <%# Convert.ToBoolean(Eval("IsEnrolled")) ? "Enrolled" : "Available" %>
                                            </span>
                                            <p class="card-text"><%# Eval("Description") %></p>
                                            <div class="d-flex justify-content-between align-items-center mb-2">
                                                <small> <%# Eval("InstructorName") %></small>
                                                <small><%# Eval("EnrollmentCount") %> students</small>
                                            </div>
                                            <small class="text-muted d-block mb-2">Created: <%# Convert.ToDateTime(Eval("CreatedDate")).ToString("MMM dd, yyyy") %></small>
                                            <div class="d-flex flex-wrap gap-2">
                                                <asp:LinkButton ID="btnViewDetails" runat="server" CssClass="btn btn-outline-primary btn-sm flex-grow-1" CommandName="ViewDetails" CommandArgument='<%# Eval("CourseID") %>' CausesValidation="false">View Details</asp:LinkButton>
                                                <asp:LinkButton ID="btnEnroll" runat="server" CssClass='<%# Convert.ToBoolean(Eval("IsEnrolled")) ? "btn btn-success btn-sm flex-grow-1 disabled" : "btn btn-primary btn-sm flex-grow-1" %>' CommandName="EnrollCourse" CommandArgument='<%# Eval("CourseID") %>' Enabled='<%# !Convert.ToBoolean(Eval("IsEnrolled")) %>' OnClientClick='<%# Convert.ToBoolean(Eval("IsEnrolled")) ? "return false;" : "return confirm(\"Are you sure you want to enroll in this course?\");" %>' CausesValidation="false">
                                                    <%# Convert.ToBoolean(Eval("IsEnrolled")) ? "Already Enrolled" : "Enroll Now" %>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- No Courses Panel -->
                    <asp:Panel ID="pnlNoCourses" runat="server" Visible="false" CssClass="text-center mt-4">
                        <p class="text-muted">No courses available at the moment.</p>
                    </asp:Panel>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
