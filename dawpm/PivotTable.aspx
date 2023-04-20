<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="PivotTable.aspx.vb" Inherits="PivotTable_Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- external libs from cdnjs -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.css">
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.min.css">

    <!-- PivotTable.js libs from ../dist -->
    <link rel="stylesheet" type="text/css" href="<%=RootPath %>pivottable/dist/pivot.css" />
    <script type="text/javascript" src="<%=RootPath %>pivottable/dist/pivot.js"></script>
    <script type="text/javascript" src="<%=RootPath %>pivottable/dist/export_renderers.js"></script>
    <script type="text/javascript" src="<%=RootPath %>pivottable/dist/d3_renderers.js"></script>
    <script type="text/javascript" src="<%=RootPath %>pivottable/dist/c3_renderers.js"></script>


    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.5/d3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui-touch-punch/0.2.3/jquery.ui.touch-punch.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-csv/0.71/jquery.csv-0.71.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.4.2/chosen.jquery.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.10/c3.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">

    <script type="text/javascript">
        $(function () {

            var derivers = $.pivotUtilities.derivers;

            var renderers = $.extend(
                $.pivotUtilities.renderers,
                $.pivotUtilities.c3_renderers,
                $.pivotUtilities.d3_renderers,
                $.pivotUtilities.export_renderers
                );

            $.get("<%= ActiveCsvPath %>", function (mps) {
                $("#output").pivotUI($.csv.toArrays(mps), {
                    renderers: renderers,
                    <%= PivotParms %>
                    onRefresh: function (config) {
                        var config_copy = JSON.parse(JSON.stringify(config));
                        //delete some values which are functions
                        delete config_copy["renderers"];
                        delete config_copy["aggregators"];
                        delete config_copy["derivedAttributes"];
                        //delete some bulky default values
                        delete config_copy["rendererOptions"];
                        delete config_copy["localeStrings"];
                        //$("#config_json").text(JSON.stringify(config_copy, undefined, 2));
                        $("#tbJSON").text(JSON.stringify(config_copy, undefined, 2));

                    }
                });
            });
        });
    </script>

    <div class="row">
        <div class="col-lg-8 form-inline">
            <div class="form-group">
                <asp:LinkButton ID="cmd_GetCSV" runat="server" Text="Get CSV File" OnClick="cmd_GetCSV_Click" CssClass="btn btn-primary"></asp:LinkButton>
                <label for="tbName">New Name:</label>
                <asp:TextBox ID="tbName" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                <label for="ddlConfig">Select:</label>
                <asp:DropDownList ID="ddlConfig" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlConfig_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                <asp:LinkButton ID="cmd_ReadJSON" runat="server" Text="Save Config" OnClick="cmd_ReadJSON_Click" CssClass="btn btn-primary"></asp:LinkButton>
                <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete Config" OnClick="cmd_Delete_Click" CssClass="btn btn-warning"></asp:LinkButton>
            </div>
        </div>
        <div class="col-lg-4">
        </div>
    </div>
    <div id="output" style="margin: 30px;"></div>

    <div class="row">


        <div class="jumbotron">

            <div class="col-md-12 col-sm-12 col-lg-12">

                <div class="panel panel-primary" id="panels">
                    <div class="panel-heading">
                        <h2>About the Pivot Table</h2>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-6 col-sm-6 col-lg-6">

                            <img src="images/PivotTableDemo.gif" class="img-responsive" />
                            <br />
                            <iframe width="420" height="315" src="https://www.youtube.com/embed/ZbrRrXiWBKc" frameborder="0" allowfullscreen></iframe>


                        </div>
                        <div class="col-md-6 col-sm-6 col-lg-6">
                            <p>
                                PivotTable.js is an open-source Javascript Pivot Table (aka Pivot Grid, Pivot Chart, Cross-Tab) implementation with drag'n'drop functionality written by Nicolas Kruchten at Datacratic. Here are some Link with more information: 
                            </p>
                            <div class="list-group">
                                <a href="https://github.com/nicolaskruchten/pivottable/wiki/UI-Tutorial" class="list-group-item active" target="_blank">UI Tutortial</a>
                                <a href="https://github.com/nicolaskruchten/pivottable/wiki/Frequently-Asked-Questions" class="list-group-item " target="_blank">Frequently Asked Questions</a>
                                <a href="http://nicolas.kruchten.com/pivottable/examples" class="list-group-item "  target="_blank">PivotTable Examples</a>
                            </div>




                        </div>
                    </div>
                    <div class="panel-footer">
                        <a href="http://nicolas.kruchten.com/pivottable/" target="_blank">PivotTable by Nicolas Kruchten</a>
                    </div>
                </div>







            </div>
        </div>

    </div>



    <asp:TextBox ID="tbJSON" runat="server" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>

