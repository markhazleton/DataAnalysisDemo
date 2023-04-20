<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SimplePivot.aspx.vb" Inherits="SimplePivot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- external libs from cdnjs -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
    
    <!-- PivotTable.js libs from ../dist -->
    <link rel="stylesheet" type="text/css" href="./pivottable/dist/pivot.css">
    <script type="text/javascript" src="./pivottable/dist/pivot.js"></script>
    
    <!-- optional: mobile support with jqueryui-touch-punch -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui-touch-punch/0.2.3/jquery.ui.touch-punch.min.js"></script>
    <script type="text/javascript">
        // This example is the most basic usage of pivotUI()
        $(function () {
            $("#output").pivotUI(
                [
                    { color: "blue", shape: "circle" },
                    { color: "red", shape: "triangle" },
                    { color: "red", shape: "triangle" },
                    { color: "red", shape: "circle" }
                ],
                {
                    rows: ["color"],
                    cols: ["shape"]
                }
                );
        });
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">



        <div id="output" style="margin: 30px;"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" Runat="Server">
</asp:Content>

