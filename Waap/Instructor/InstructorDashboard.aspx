<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstructorDashboard.aspx.cs" Inherits="LearnSphere.Instructor.InstructorDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Instructor Dashboard - LearnSphere</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../Assets/home.css" rel="stylesheet"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <style>
    .nav-icon {
    width: 30px;
    height: 30px;
    margin-right: 5px;
    vertical-align: middle;
}
      </style>
</head>

<body class="bg-light">
    <form id="form1" runat="server">

        <div class="container-fluid">
            <div class="row min-vh-100">

                <!-- ========== SIDEBAR ========== -->
                <div class="col-12 col-md-3 col-lg-2 bg-dark text-white p-4">
                    <h4 class="fw-bold mb-0">LearnSphere</h4>
                    <small class="text-secondary">Instructor Panel</small>

                    <hr class="border-secondary my-4" />

                    <ul class="nav flex-column gap-2">
                        <li class="nav-item">
                            <a class="nav-link text-white fw-semibold" href="InstructorDashboard.aspx">
                                <img src="../Assets/Images/Dashboard.png" class="nav-icon"/>Dashboard
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-white fw-semibold" href="ManageCourses.aspx">
                               <img src="../Assets/Images/manage.png" class="nav-icon"/>My Courses
                            </a>
                        </li>
                    </ul>
                </div>

                <!-- ========== MAIN CONTENT ========== -->
                <div class="col-12 col-md-9 col-lg-10 p-4">

                    <!-- TOP BAR -->
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h2 class="fw-bold mb-0">Instructor Dashboard</h2>

                        <div class="dropdown">
                            <button class="btn btn-outline-dark dropdown-toggle"
                                    type="button"
                                    data-bs-toggle="dropdown">
                                <img src="../Assets/Images/avatar.png" class="nav-icon"/><asp:Label ID="lblInstructorName" runat="server" />
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li>
                                    <asp:LinkButton ID="btnLogout"
                                        runat="server"
                                        CssClass="dropdown-item text-danger"
                                        OnClick="btnLogout_Click">
                                        <img src="../Assets/Images/logout.png" class="nav-icon"/>Logout
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <!-- STATISTICS CARDS -->
                    <div class="row g-4 mb-4">
                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-success"><img src="../Assets/Images/manage.png" class="nav-icon"/></div>
                                    <div>
                                        <h4 class="mb-0">
                                            <asp:Label ID="lblMyCourses" runat="server">0</asp:Label>
                                        </h4>
                                        <small class="text-muted">My Courses</small>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-primary"><img src="../Assets/Images/avatar.png" class="nav-icon"/></div>
                                    <div>
                                        <h4 class="mb-0">
                                            <asp:Label ID="lblTotalStudents" runat="server">0</asp:Label>
                                        </h4>
                                        <small class="text-muted">Total Students Enrolled</small>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="card shadow-sm border-0">
                                <div class="card-body d-flex align-items-center gap-3">
                                    <div class="fs-2 text-warning"></div>
                                    <div>
                                        <h4 class="mb-0">
                                            <asp:Label ID="lblPopularCourse" runat="server">N/A</asp:Label>
                                        </h4>
                                        <small class="text-muted">Most Popular Course</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- MY COURSES SECTION -->
                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <h5 class="fw-bold mb-0">My Courses</h5>
                                <a href="ManageCourses.aspx" class="btn btn-primary btn-sm">+ Add New Course</a>
                            </div>

                            <div class="table-responsive">
                                <asp:GridView ID="gvMyCourses" runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="table table-striped table-hover align-middle">
                                    <Columns>
                                        <asp:BoundField DataField="CourseID" HeaderText="ID" />
                                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="EnrollmentCount" HeaderText="Enrolled Students" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:MMM dd, yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!-- RECENT ENROLLMENTS SECTION -->
                    <div class="card shadow-sm border-0">
                        <div class="card-body">
                            <h5 class="fw-bold mb-3">Recent Enrollments</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvRecentEnrollments" runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="table table-striped table-hover align-middle">
                                    <Columns>
                                        <asp:BoundField DataField="FullName" HeaderText="Student Name" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                        <asp:BoundField DataField="CourseName" HeaderText="Course" />
                                        <asp:BoundField DataField="EnrolledDate" HeaderText="Enrolled Date" DataFormatString="{0:MMM dd, yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>
