<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LearnSphere.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- HERO SECTION -->
    <section class="hero-banner d-flex align-items-center" style="height: 500px; background: linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.5)), url('Assets/images/hero-banner.jpg') center/cover no-repeat;">
        <div class="container text-center text-white position-relative">
            <h1 class="fw-bold display-5">Learn Anytime, Anywhere</h1>
            <p class="lead mt-2">Empower your learning journey with LearnSphere</p>
            <a href="#courses" class="btn btn-primary btn-lg mt-3">Explore Courses</a>
        </div>
    </section>

    <!-- COURSES SECTION -->
    <section class="py-5" id="courses">
        <div class="container">
            <h2 class="text-center fw-bold mb-5">Explore Our Courses</h2>

            <div class="row g-4">
                <asp:Repeater ID="rptHomeCourses" runat="server">
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
                                    <a href='CourseDetails.aspx?CourseID=<%# Eval("CourseID") %>' class="btn btn-outline-primary btn-sm mt-2">View Course</a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

</asp:Content>
