<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $(".regular").slick({
                dots: true,
                infinite: true,
                slidesToShow: 3,
                slidesToScroll: 3
            });
        });
    </script>

    <link rel="stylesheet" type="text/css" href="<%= RootPath %>/slick/slick.css">
    <link rel="stylesheet" type="text/css" href="<%= RootPath %>/slick/slick-theme.css">
    <style type="text/css">
        .slider {
            width: 50%;
            margin: 100px auto;
        }

        .slick-slide {
            margin: 0px 20px;
            height: auto;
        }

            .slick-slide img {
                height: auto;
                width: 100%;
            }

        .slick-prev:before,
        .slick-next:before {
            color: black;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="row">
        <div class="col-md-8 col-sm-12 col-lg-8">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h2>Data Set Analysis</h2>
                </div>
                <div class="panel-body">
                    <div class="alert alert-success">
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        &nbsp;<h3>1. Select Data Set To Analyze</h3>
                        <div class="table-responsive">
                            <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="data_table table table-striped">
                                <alternatingrowstyle backcolor="White" forecolor="#284775" />
                                <columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="FileName" HeaderText="Data Set" SortExpression="FileName" />
                                    <asp:BoundField DataField="FileDate" HeaderText="Update Date" SortExpression="FileDate" />
                                    <asp:BoundField DataField="FileSize" HeaderText="FileSize" SortExpression="FileSize" />
                                </columns>
                                <editrowstyle backcolor="#999999" />
                                <footerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                <headerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                <pagerstyle backcolor="#284775" forecolor="White" horizontalalign="Center" />
                                <rowstyle backcolor="#F7F6F3" forecolor="#333333" />
                                <selectedrowstyle backcolor="#E2DED6" font-bold="True" forecolor="#333333" />
                                <sortedascendingcellstyle backcolor="#E9E7E2" />
                                <sortedascendingheaderstyle backcolor="#506C8C" />
                                <sorteddescendingcellstyle backcolor="#FFFDF8" />
                                <sorteddescendingheaderstyle backcolor="#6F8DAE" />
                            </asp:GridView>
                        </div>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetCsvFileList" TypeName="dawpmPage"></asp:ObjectDataSource>
                    </div>

                    <div class="alert alert-success">
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        <h3>2. Selected Data Set Analysis</h3>
                        <strong>
                            <asp:TextBox ID="tbCsvFileName" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox></strong>
                        <asp:Button ID="cmd_PivotTable" runat="server" ClientIDMode="Static" CssClass="btn btn-default" Text="Analyse With Pivot Table" OnClick="cmd_PivotTable_Click" />
                        <asp:DropDownList ID="myFileListBox" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="myFileListBox_SelectedIndexChanged">
                        </asp:DropDownList>
                        <div class="table-responsive">
                            <asp:GridView ID="gvCsvColumns" runat="server" AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="data_table table table-striped">
                                <alternatingrowstyle backcolor="White" forecolor="#284775" />
                                <editrowstyle backcolor="#999999" />
                                <footerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                <headerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                <pagerstyle backcolor="#284775" forecolor="White" horizontalalign="Center" />
                                <rowstyle backcolor="#F7F6F3" forecolor="#333333" />
                                <selectedrowstyle backcolor="#E2DED6" font-bold="True" forecolor="#333333" />
                                <sortedascendingcellstyle backcolor="#E9E7E2" />
                                <sortedascendingheaderstyle backcolor="#506C8C" />
                                <sorteddescendingcellstyle backcolor="#FFFDF8" />
                                <sorteddescendingheaderstyle backcolor="#6F8DAE" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-12 col-lg-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h2>Data Set Charts from <asp:Label ID="lblChartHeading" runat="server" ></asp:Label></h2>
                </div>
                <div class="panel-body">
                    <asp:Repeater runat="server" ID="rptCharts"></asp:Repeater>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>

