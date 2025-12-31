<%@ Page Title="Contact Us - LearnSphere" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="LearnSphere.ContactUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contact Us - LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION -->
    <section class="hero-banner d-flex align-items-center" style="height: 300px; background: linear-gradient(rgba(0,0,0,0.4), rgba(0,0,0,0.4)), url('Assets/images/hero-banner.jpg') center/cover no-repeat;">
        <div class="container text-center text-white">
            <h1 class="fw-bold display-5">Find Us Here</h1>
            <p class="lead mt-2">Locate our office on the map</p>
        </div>
    </section>

    <!-- GOOGLE MAP -->
    <section class="py-5">
        <div class="container">
            <h2 class="fw-bold mb-4 text-center">Our Location</h2>
            <div class="row justify-content-center">
                <div class="col-md-10">
                    <div class="ratio ratio-16x9 shadow rounded">
                        <iframe 
                            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3532.789437222694!2d85.32298707539843!3d27.717245732784176!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x39eb1908a60f8eaf%3A0x86f5673fa1f2edb4!2sKathmandu%2C%20Nepal!5e0!3m2!1sen!2sus!4v1700000000000!5m2!1sen!2sus" 
                            width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade">
                        </iframe>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
