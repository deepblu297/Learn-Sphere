<%@ Page Title="Explore Courses" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Explore.aspx.cs" Inherits="LearnSphere.Explore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Explore Courses - LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- SEARCH BAR -->
    <section class="py-5">
        <div class="container search-bar" style="max-width:500px; margin-top:50px;">
            <div class="input-group mb-4">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search courses..."></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
            </div>
        </div>
    </section>

    <!-- COURSES GRID -->
    <section class="py-5">
        <div class="container">
            <h2 class="text-center fw-bold mb-5">Explore Courses</h2>

            <div class="row g-4">
                <asp:Repeater ID="rptCourses" runat="server">
                    <ItemTemplate>
                        <div class="col-md-3">
                            <div class="card course-card h-100 shadow-sm">
                                <%# !string.IsNullOrEmpty(Eval("ThumbnailPath").ToString()) 
    ? "<img src='/" + Eval("ThumbnailPath") + "' class='course-thumbnail' alt='Course Thumbnail' />" 
    : "<div class='placeholder-thumbnail'>📚</div>" %>
                                <div class="card-body">
                                    <h5 class="card-title"><%# Eval("CourseName") %></h5>
                                    <p class="card-text"><%# Eval("Description") %></p>
                                    <small class="text-muted">Instructor: <%# Eval("InstructorName") %></small>
                                    <br />
                                    <asp:HyperLink ID="hlViewCourse" runat="server" CssClass="btn btn-outline-primary btn-sm mt-2"
                                        NavigateUrl='<%# "CourseDetails.aspx?CourseID=" + Eval("CourseID") %>' Text="View Course"></asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlNoCourses" runat="server" Visible="false" CssClass="text-center mt-5">
                    <h5>No courses found.</h5>
                </asp:Panel>
            </div>
        </div>
    </section>

</asp:Content>
