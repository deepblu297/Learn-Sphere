<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LearnSphere.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Register - LearnSphere
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="register-container" style="margin-top:100px; max-width:500px; margin-left:auto; margin-right:auto;">
        <div class="header text-center mb-4">
            <h1>Create Account</h1>
            <p>Join LearnSphere and start your learning journey</p>
        </div>

        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="message mb-3">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </asp:Panel>

        <div class="form-group mb-3">
            <label for="txtFullName">Full Name <span class="required">*</span></label>
            <asp:TextBox ID="txtFullName" runat="server" placeholder="Enter your full name" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" 
                ControlToValidate="txtFullName" 
                ErrorMessage="Full name is required" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>

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
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Create a strong password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                ControlToValidate="txtPassword" 
                ErrorMessage="Password is required" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revPassword" runat="server" 
                ControlToValidate="txtPassword" 
                ErrorMessage="Password must be 8+ characters with uppercase, lowercase, number, and special character" 
                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RegularExpressionValidator>
        </div>

        <div class="form-group mb-3">
            <label for="txtConfirmPassword">Confirm Password <span class="required">*</span></label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm your password" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                ControlToValidate="txtConfirmPassword" 
                ErrorMessage="Please confirm your password" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvPassword" runat="server" 
                ControlToValidate="txtConfirmPassword" 
                ControlToCompare="txtPassword" 
                ErrorMessage="Passwords do not match" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:CompareValidator>
        </div>

        <div class="form-group mb-3">
            <label for="ddlRole">Register As <span class="required">*</span></label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                <asp:ListItem Value="" Selected="True">-- Select Role --</asp:ListItem>
                <asp:ListItem Value="Learner">Learner</asp:ListItem>
                <asp:ListItem Value="Instructor">Instructor</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvRole" runat="server" 
                ControlToValidate="ddlRole" 
                InitialValue=""
                ErrorMessage="Please select a role" 
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Create Account" 
            CssClass="btn btn-primary w-100" OnClick="btnRegister_Click" />

        <div class="login-link text-center mt-3">
            Already have an account? <a href="Login.aspx">Sign In</a>
        </div>

    </div>

</asp:Content>
