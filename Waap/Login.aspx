<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LearnSphere.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login - LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="login-container" style="margin-top:100px; max-width:400px; margin-left:auto; margin-right:auto;">
        <div class="header text-center mb-4">
            <h1>Welcome Back</h1>
            <p>Login to continue your learning journey</p>
        </div>

        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="message mb-3">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>

        <div class="form-group mb-3">
            <label for="txtEmail">Email Address <span class="required">*</span></label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Enter your email" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                ControlToValidate="txtEmail" 
                ErrorMessage="Email is required" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                ControlToValidate="txtEmail" 
                ErrorMessage="Please enter a valid email address" 
                ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RegularExpressionValidator>
        </div>

        <div class="form-group mb-3">
            <label for="txtPassword">Password <span class="required">*</span></label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                ControlToValidate="txtPassword" 
                ErrorMessage="Password is required" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>

        <div class="d-flex justify-content-between align-items-center mb-3">
            <label class="remember-me">
                <asp:CheckBox ID="chkRememberMe" runat="server" />
                Remember me
            </label>
            <a href="#" class="forgot-password">Forgot Password?</a>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" 
            CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />

        <div class="divider text-center my-3">
            <span>OR</span>
        </div>

        <div class="register-link text-center">
            Don't have an account? <a href="Register.aspx">Create Account</a>
        </div>
    </div>

</asp:Content>
