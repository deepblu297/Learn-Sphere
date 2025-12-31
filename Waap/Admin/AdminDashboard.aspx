<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Admin.master"
    CodeBehind="AdminDashboard.aspx.cs"
    Inherits="LearnSphere.Admin.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- DASHBOARD STAT CARDS -->
    <div class="row g-4 mb-4">

        <div class="col-md-6 col-xl-3">
            <div class="card shadow-sm border-0">
                <div class="card-body d-flex align-items-center gap-3">
                    <div class="fs-2"><img src="../Assets/Images/people-together.png" class="nav-icon"/></div>
                    <div>
                        <h4 class="mb-0">
                            <asp:Label ID="lblTotalUsers" runat="server" Text="0" />
                        </h4>
                        <small class="text-muted">Total Users</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6 col-xl-3">
            <div class="card shadow-sm border-0">
                <div class="card-body d-flex align-items-center gap-3">
                    <div class="fs-2"><img src="../Assets/Images/enroll.png" class="nav-icon"/></div>
                    <div>
                        <h4 class="mb-0">
                            <asp:Label ID="lblTotalCourses" runat="server" Text="0" />
                        </h4>
                        <small class="text-muted">Total Courses</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6 col-xl-3">
            <div class="card shadow-sm border-0">
                <div class="card-body d-flex align-items-center gap-3">
                    <div class="fs-2"><img src="../Assets/Images/online-learning.png" class="nav-icon"/></div>
                    <div>
                        <h4 class="mb-0">
                            <asp:Label ID="lblTotalEnrollments" runat="server" Text="0" />
                        </h4>
                        <small class="text-muted">Enrollments</small>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6 col-xl-3">
            <div class="card shadow-sm border-0">
                <div class="card-body d-flex align-items-center gap-3">
                    <div class="fs-2"><img src="../Assets/Images/training.png" class="nav-icon"/></div>
                    <div>
                        <h4 class="mb-0">
                            <asp:Label ID="lblTotalInstructors" runat="server" Text="0" />
                        </h4>
                        <small class="text-muted">Instructors</small>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- RECENT USERS -->
    <div class="card shadow-sm border-0 mb-4">
        <div class="card-body">
            <h5 class="fw-bold mb-3">Recent Users</h5>

            <div class="table-responsive">
                <asp:GridView ID="gvRecentUsers"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover align-middle">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="ID" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Role" HeaderText="Role" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- RECENT COURSES -->
    <div class="card shadow-sm border-0">
        <div class="card-body">
            <h5 class="fw-bold mb-3">Recent Courses</h5>

            <div class="table-responsive">
                <asp:GridView ID="gvRecentCourses"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped table-hover align-middle">
                    <Columns>
                        <asp:BoundField DataField="CourseID" HeaderText="ID" />
                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                        <asp:BoundField DataField="CreatedDate"
                            HeaderText="Created Date"
                            DataFormatString="{0:MMM dd, yyyy}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
