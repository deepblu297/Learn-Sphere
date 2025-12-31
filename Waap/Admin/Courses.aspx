<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Admin.master"
    CodeBehind="Courses.aspx.cs"
    Inherits="LearnSphere.Admin.Courses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- PAGE TITLE -->
    <div class="mb-4">
        <h1 class="m-0">Manage Courses</h1>
    </div>

    <!-- MESSAGE PANEL -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </asp:Panel>

    <!-- ADD COURSE -->
    <div class="content-section mb-5">
        <h2>Add New Course</h2>

        <div class="row g-3">
            <div class="col-md-6">
                <label class="form-label">Course Name</label>
                <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="txtCourseName"
                    ErrorMessage="Course name required"
                    CssClass="text-danger" />
            </div>

            <div class="col-md-6">
                <label class="form-label">Description</label>
                <asp:TextBox ID="txtDescription" runat="server"
                    TextMode="MultiLine" Rows="3"
                    CssClass="form-control" />
            </div>

            <div class="col-md-12">
                <label class="form-label">Content</label>
                <asp:TextBox ID="txtContent" runat="server"
                    TextMode="MultiLine" Rows="4"
                    CssClass="form-control" />
            </div>

            <div class="col-md-12">
                <asp:Button ID="btnAddCourse" runat="server"
                    Text="Add Course"
                    CssClass="btn btn-primary"
                    OnClick="btnAddCourse_Click" />
            </div>
        </div>
    </div>

    <!-- COURSES TABLE -->
    <div class="content-section">
        <h2>All Courses</h2>

        <asp:GridView ID="gvCourses" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-hover"
            DataKeyNames="CourseID"
            OnRowEditing="gvCourses_RowEditing"
            OnRowCancelingEdit="gvCourses_RowCancelingEdit"
            OnRowUpdating="gvCourses_RowUpdating"
            OnRowDeleting="gvCourses_RowDeleting">

            <Columns>
                <asp:BoundField DataField="CourseID" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="Content" HeaderText="Content" />
                <asp:BoundField DataField="CreatedDate"
                    HeaderText="Created Date"
                    DataFormatString="{0:MMM dd, yyyy}"
                    ReadOnly="True" />

                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
