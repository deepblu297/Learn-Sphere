<%@ Page Title="About Us - LearnSphere" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="LearnSphere.AboutUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About Us - LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION -->
    <section class="hero-banner d-flex align-items-center" style="height: 300px; background: linear-gradient(rgba(0,0,0,0.4), rgba(0,0,0,0.4)), url('Assets/images/hero-banner.jpg') center/cover no-repeat;">
        <div class="container text-center text-white">
            <h1 class="fw-bold display-5">About LearnSphere</h1>
            <p class="lead mt-2">Discover our mission, vision, and values</p>
        </div>
    </section>

    <!-- ABOUT CONTENT -->
    <section class="py-5">
        <div class="container">
            <h2 class="fw-bold mb-4 text-center">Who We Are</h2>
            <p class="text-center mb-5">
                LearnSphere is a modern online learning platform dedicated to empowering learners worldwide. 
                Our mission is to provide high-quality, accessible, and engaging courses for students, professionals, 
                and lifelong learners alike. We believe in learning without boundaries, allowing individuals to grow 
                and achieve their full potential.
            </p>

            <div class="row text-center">
                <div class="col-md-4 mb-4">
                    <div class="p-4 shadow-sm rounded">
                        <h4 class="fw-bold">Our Mission</h4>
                        <p>To make learning accessible, interactive, and effective for everyone, everywhere.</p>
                    </div>
                </div>
                <div class="col-md-4 mb-4">
                    <div class="p-4 shadow-sm rounded">
                        <h4 class="fw-bold">Our Vision</h4>
                        <p>To be a leading global platform where knowledge meets opportunity, fostering lifelong learning.</p>
                    </div>
                </div>
                <div class="col-md-4 mb-4">
                    <div class="p-4 shadow-sm rounded">
                        <h4 class="fw-bold">Our Values</h4>
                        <p>Innovation, inclusivity, integrity, and learner-focused excellence in everything we do.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

