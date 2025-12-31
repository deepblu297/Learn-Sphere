<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCourses.aspx.cs" Inherits="LearnSphere.Instructor.ManageCourses" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Courses - LearnSphere</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="Assets/home.css" rel="stylesheet" />
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

                <!-- SIDEBAR -->
                <div class="col-12 col-md-3 col-lg-2 bg-dark text-white p-4">
                    <h4 class="fw-bold mb-0">LearnSphere</h4>
                    <small class="text-secondary">Instructor Panel</small>

                    <hr class="border-secondary my-4" />

                    <ul class="nav flex-column gap-2">
                        <li class="nav-item">
                            <a class="nav-link text-white" href="InstructorDashboard.aspx"><img src="../Assets/Images/Dashboard.png" class="nav-icon"/>Dashboard</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-white fw-bold" href="ManageCourses.aspx"><img src="../Assets/Images/manage.png" class="nav-icon"/>My Courses</a>
                        </li>
                    </ul>
                </div>

                <!-- MAIN CONTENT -->
                <div class="col-12 col-md-9 col-lg-10 p-4">

                    <!-- TOP BAR -->
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h2 class="fw-bold mb-0">Manage Courses</h2>

                        <div class="dropdown">
                            <button class="btn btn-outline-dark dropdown-toggle" type="button" data-bs-toggle="dropdown">
                                <img src="../Assets/Images/avatar.png" class="nav-icon"/><asp:Label ID="lblInstructorName" runat="server" />
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li>
                                    <asp:LinkButton ID="btnLogout"
                                        runat="server"
                                        CssClass="dropdown-item text-danger"
                                        OnClick="btnLogout_Click"
                                        CausesValidation="false">
                                        <img src="../Assets/Images/logout.png" class="nav-icon"/>Logout
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>

                    <!-- SUCCESS / ERROR MESSAGES -->
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert mb-4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- ADD / EDIT COURSE FORM -->
                    <div class="card shadow-sm border-0 mb-4">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <h5 class="fw-bold mb-0">
                                    <asp:Label ID="lblFormTitle" runat="server" Text="Add New Course" />
                                </h5>
                                <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="btn btn-secondary btn-sm"
                                            OnClick="btnCancelEdit_Click" Visible="false" CausesValidation="false" />
                            </div>

                            <asp:HiddenField ID="hfCourseID" runat="server" Value="0" />

                            <div class="mb-3">
                                <label class="form-label">Course Name <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" placeholder="Enter course name" MaxLength="100" />
                                <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" ControlToValidate="txtCourseName" ErrorMessage="Course name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CourseValidation" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Description <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter course description" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CourseValidation" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Course Content <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" placeholder="Enter course content, learning materials, syllabus, etc." />
                                <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent" ErrorMessage="Course content is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CourseValidation" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Course Thumbnail (Image)</label>
                                <asp:FileUpload ID="fuThumbnail" runat="server" CssClass="form-control" accept="image/*" />
                                <small class="text-muted">Upload a thumbnail image (JPG, PNG, GIF)</small>
                                <asp:Label ID="lblCurrentThumbnail" runat="server" CssClass="d-block mt-1 text-info" Visible="false" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Course Resources (Document)</label>
                                <asp:FileUpload ID="fuResource" runat="server" CssClass="form-control" accept=".pdf,.doc,.docx" />
                                <small class="text-muted">Upload course materials (PDF, DOC, DOCX)</small>
                                <asp:Label ID="lblCurrentResource" runat="server" CssClass="d-block mt-1 text-info" Visible="false" />
                            </div>

                            <div class="d-flex gap-2">
                                <asp:Button ID="btnSaveCourse" runat="server" Text="Save Course" CssClass="btn btn-primary" OnClick="btnSaveCourse_Click" ValidationGroup="CourseValidation" />
                                <asp:Button ID="btnClearForm" runat="server" Text="Clear Form" CssClass="btn btn-secondary" OnClick="btnClearForm_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>

                    <!-- COURSES LIST -->
                    <div class="card shadow-sm border-0">
                        <div class="card-body">
                            <h5 class="fw-bold mb-3">My Courses</h5>

                            <div class="table-responsive">
                                <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover align-middle"
                                    OnRowCommand="gvCourses_RowCommand" DataKeyNames="CourseID">
                                    <Columns>
                                        <asp:BoundField DataField="CourseID" HeaderText="ID" />
                                        <asp:TemplateField HeaderText="Thumbnail">
                                            <ItemTemplate>
                                                <asp:Image ID="imgThumbnail" runat="server"
                                                    ImageUrl='<%# !string.IsNullOrEmpty(Eval("ThumbnailPath").ToString()) ? "~/" + Eval("ThumbnailPath") : "~/Assets/default-course.png" %>'
                                                    Width="60px" Height="40px" CssClass="img-thumbnail" AlternateText="Course Thumbnail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="EnrollmentCount" HeaderText="Students" />
                                        <asp:TemplateField HeaderText="Resources">
                                            <ItemTemplate>
                                                <%# !string.IsNullOrEmpty(Eval("ResourcePath").ToString()) ? "Available" : "No file" %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:MMM dd, yyyy}" />
                                        <asp:TemplateField HeaderText="Actions">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditCourse" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-sm btn-outline-primary me-1" CausesValidation="false">
                                                    Edit
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteCourse" CommandArgument='<%# Eval("CourseID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Are you sure you want to delete this course? All enrollments will also be deleted.');" CausesValidation="false">
                                                    Delete
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="text-center py-4">No courses found. Create your first course above!</div>
                                    </EmptyDataTemplate>
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
