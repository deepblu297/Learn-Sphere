<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseDetails.aspx.cs" Inherits="LearnSphere.Learner.CourseDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Details - LearnSphere</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Assets/home.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/home.css" rel="stylesheet" />

    <style>
        body { background-color: #f8f9fa; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
        h2, h3 { font-weight: 600; }
        /* Sidebar */
        .sidebar { background-color: #111827; color: #fff; min-height: 100vh; padding: 1rem; }
        .sidebar h4 { font-weight: bold; margin-bottom: 0.2rem; }
        .sidebar small { color: #adb5bd; }
        .sidebar hr { border-color: #495057; margin: 1rem 0; }
        .sidebar a.nav-item { color: #fff; display: block; padding: 0.5rem 0; text-decoration: none; transition: 0.3s; }
        .sidebar a.nav-item:hover, .sidebar a.nav-item.active { color: #ffc107; font-weight: bold; }

        /* Main Content */
        .main-content { padding: 2rem; }
        .thumbnail-placeholder {
            height: 150px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #fff;
            font-size: 3rem;
            border-radius: 0.5rem;
        }
        .card { border-radius: 0.5rem; }
        .card img { border-radius: 0.5rem 0.5rem 0 0; object-fit: cover; height: 250px; width: 100%; }
        .btn-enroll { margin-top: 10px; }
        .alert { font-size: 0.95rem; }
        .resource-card { border-left: 4px solid #28a745; }
        .download-icon { font-size: 1.2rem; margin-right: 0.5rem; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row min-vh-100">
                
                <!-- SIDEBAR -->
                <div class="col-12 col-md-3 col-lg-2 sidebar">
                    <h4>LearnSphere</h4>
                    <small>Student Panel</small>
                    <hr />
                    <nav class="nav flex-column">
                        <a href="Dashboard.aspx" class="nav-item active"><img src="../Assets/Images/manage.png" class="nav-icon"/> Available Courses</a>
                        <a href="MyCourses.aspx" class="nav-item"><img src="../Assets/Images/enroll.png" class="nav-icon"/> My Enrolled Courses</a>
                    </nav>
                </div>

                <!-- MAIN CONTENT -->
                <div class="col-12 col-md-9 col-lg-10 main-content">
                    
                    <!-- Top Bar -->
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h2>Course Details</h2>
                        <a href="Dashboard.aspx" class="btn btn-outline-primary btn-sm">← Back to Courses</a>
                    </div>

                    <!-- Message Panel -->
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert mb-4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- Course Details -->
                    <asp:Panel ID="pnlCourseDetails" runat="server" Visible="false">

                        <!-- Course Name and Enroll -->
                        <div class="d-flex justify-content-between align-items-center mb-4">
                            <h3><asp:Label ID="lblCourseName" runat="server"></asp:Label></h3>
                            <div>
                                <asp:Panel ID="pnlEnrollButton" runat="server" Visible="true">
                                    <asp:Button ID="btnEnroll" runat="server" Text="Enroll" CssClass="btn btn-primary btn-enroll"
                                        OnClick="btnEnroll_Click"
                                        OnClientClick="return confirm('Are you sure you want to enroll in this course?');" />
                                </asp:Panel>
                                <asp:Panel ID="pnlEnrolledBadge" runat="server" Visible="false">
                                    <span class="badge bg-success">✓ Enrolled</span>
                                </asp:Panel>
                            </div>
                        </div>

                        <!-- Thumbnail -->
                        <div class="mb-4">
                            <asp:Image ID="imgCourseThumbnail" runat="server" CssClass="img-fluid shadow-sm" AlternateText="Course Thumbnail" Visible="false" />
                            <div id="thumbnailPlaceholder" runat="server" class="thumbnail-placeholder" visible="false"></div>
                        </div>

                        <!-- Stats Cards -->
                        <div class="row g-3 mb-4">
                            <div class="col-md-4">
                                <div class="card shadow-sm text-center">
                                    <div class="card-body">
                                        <h5 class="card-title">Instructor</h5>
                                        <p class="card-text"><asp:Label ID="lblInstructor" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card shadow-sm text-center">
                                    <div class="card-body">
                                        <h5 class="card-title">Students Enrolled</h5>
                                        <p class="card-text"><asp:Label ID="lblEnrollmentCount" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card shadow-sm text-center">
                                    <div class="card-body">
                                        <h5 class="card-title">Created On</h5>
                                        <p class="card-text"><asp:Label ID="lblCreatedDate" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Description & Content -->
                        <div class="row g-3 mb-4">
                            <div class="col-md-6">
                                <div class="card shadow-sm h-100">
                                    <div class="card-header">Course Description</div>
                                    <div class="card-body">
                                        <p><asp:Label ID="lblDescription" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card shadow-sm h-100">
                                    <div class="card-header">Course Content</div>
                                    <div class="card-body">
                                        <p><asp:Label ID="lblContent" runat="server"></asp:Label></p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Resource Download Section (Only for Enrolled Students) -->
                        <asp:Panel ID="pnlResourceDownload" runat="server" Visible="false">
                            <div class="card shadow-sm resource-card mb-4">
                                <div class="card-header bg-success text-white">
                                    <h5 class="mb-0">Course Resources</h5>
                                </div>
                                <div class="card-body">
                                    <p class="text-muted mb-3">Download course materials and resources below:</p>
                                    <asp:Button ID="btnDownloadResource" runat="server" 
                                        Text="Download Course Resource" 
                                        CssClass="btn btn-success btn-lg" 
                                        OnClick="btnDownloadResource_Click" />
                                    <small class="d-block mt-2 text-muted">
                                        <asp:Label ID="lblResourceName" runat="server"></asp:Label>
                                    </small>
                                </div>
                            </div>
                        </asp:Panel>

                        <!-- No Resource Message (for enrolled students) -->
                        <asp:Panel ID="pnlNoResource" runat="server" Visible="false">
                            <div class="alert alert-info">
                                <strong>Note:</strong> No downloadable resources are currently available for this course.
                            </div>
                        </asp:Panel>

                    </asp:Panel>

                    <!-- Course Not Found -->
                    <asp:Panel ID="pnlNotFound" runat="server" Visible="false" CssClass="alert alert-danger text-center">
                        <h4>Course not available</h4>
                        <p>The course you're looking for doesn't exist or has been removed.</p>
                        <a href="Dashboard.aspx" class="btn btn-primary">Back to Courses</a>
                    </asp:Panel>

                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>