<%@ Page Title="Manage Users"
    Language="C#"
    MasterPageFile="~/Admin.master"
    AutoEventWireup="true"
    CodeBehind="Users.aspx.cs"
    Inherits="LearnSphere.Admin.Users" %>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Manage Users
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <!-- Message Panel -->
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert alert-info mb-4">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Search + Header -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h5 class="mb-0">All Users</h5>

        <div class="d-flex gap-2">
            <asp:TextBox ID="txtSearch" runat="server"
                CssClass="form-control"
                placeholder="Search by name or email..."
                style="width: 260px;" />

            <asp:Button ID="btnSearch" runat="server"
                Text="Search"
                CssClass="btn btn-primary"
                OnClick="btnSearch_Click" />

            <asp:Button ID="btnClearSearch" runat="server"
                Text="Clear"
                CssClass="btn btn-secondary"
                OnClick="btnClearSearch_Click" />
        </div>
    </div>

    <!-- Users Table -->
    <asp:GridView ID="gvUsers" runat="server"
        CssClass="table table-bordered table-hover"
        AutoGenerateColumns="False"
        DataKeyNames="UserID"
        OnRowEditing="gvUsers_RowEditing"
        OnRowUpdating="gvUsers_RowUpdating"
        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
        OnRowDeleting="gvUsers_RowDeleting">

        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />

            <asp:TemplateField HeaderText="Role">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Role") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlEditRole" runat="server"
                        SelectedValue='<%# Bind("Role") %>'>
                        <asp:ListItem>Admin</asp:ListItem>
                        <asp:ListItem>Instructor</asp:ListItem>
                        <asp:ListItem>Learner</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True" ButtonType="Button" />
            <asp:CommandField ShowDeleteButton="True" ButtonType="Button" />
        </Columns>
    </asp:GridView>

</asp:Content>
