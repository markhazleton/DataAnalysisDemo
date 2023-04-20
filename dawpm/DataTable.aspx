<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="DataTable.aspx.vb" Inherits="DataTable_Page" %>

<%@ Register Src="~/controls/CSV_DisplayTable.ascx" TagPrefix="uc1" TagName="DisplayTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.css">

    <script type="text/javascript" src="//cdn.datatables.net/1.10.7/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="//cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.js"></script>

    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('table.data_table').dataTable();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <asp:LinkButton ID="cmd_GetCSV" runat="server" Text="Get CSV File"  OnClick="cmd_GetCSV_Click"  CssClass="btn btn-primary"></asp:LinkButton>
    <uc1:DisplayTable runat="server" ID="dtList" />
</asp:Content>

<asp:Content ID="ContentFooter" ContentPlaceHolderID="footer" runat="server">

    <!-- Page-Level Scripts - Tables -->
    <script>
        $(document).ready(function () {
            $('#dataTables-admin').dataTable();
        });
    </script>

</asp:Content>
