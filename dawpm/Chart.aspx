<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Chart.aspx.vb" Inherits="Chart" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #ChartDiv {
            height: 100%;
            width: 100%;
        }

            #ChartDiv img {
                max-width: 100%;
                max-height: 100%;
                margin: auto;
                display: block;
                height: 100%;
                width: 100%;
                object-fit: contain;
            }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="row">
        <div class="col-lg-8 form-inline">
            <div class="form-group">
                <asp:LinkButton ID="cmd_GetCSV" runat="server" Text="Get CSV File" OnClick="cmd_GetCSV_Click" CssClass="btn btn-primary"></asp:LinkButton>
                <label for="tbName">New Name:</label>
                <asp:TextBox ID="tbName" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                <label for="ddlConfig">Select:</label>
                <asp:DropDownList ID="ddlConfig" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlConfig_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                <asp:LinkButton ID="cmd_Delete" runat="server" Text="Delete Config" OnClick="cmd_Delete_Click" CssClass="btn btn-warning"></asp:LinkButton>
            </div>
        </div>
        <div class="col-lg-4">
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div id="ChartDiv">
                <asp:Chart ID="myChart" ClientIDMode="Static" runat="server" Height="1200px" Width="1600px" BackSecondaryColor="White" BorderlineDashStyle="Solid" BorderlineWidth="5" CssClass="img-responsive" BorderlineColor="Transparent" ImageStorageMode="UseHttpHandler" ViewStateContent="Appearance" ValidateRequestMode="Disabled" ViewStateMode="Disabled" BackImageWrapMode="Scaled" BackImageTransparentColor="White">
                </asp:Chart>
            </div>

        </div>
        <div class="col-md-6">

            <asp:Panel ID="pnlChartConfiguration" runat="server" CssClass="form panel panel-default">
                <div class="panel-heading">
                    <h3>Chart Configuration</h3>
                    <asp:LinkButton ID="cmd_SetChartConfig" runat="server" CssClass="btn btn-primary" OnClick="cmd_SetChartConfig_Click">Update Chart</asp:LinkButton>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                            <div class="form-group">
                                <label>Title:</label>
                                <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>SubTitle:</label>
                                <asp:TextBox ID="tbSubTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                            <div class="form-group">
                                <label>Chart Value:</label>
                                <asp:DropDownList ID="ddlValueColumn" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Value Calculation Method:</label>
                                <asp:DropDownList ID="ddlCalc" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-4 col-sm=4 col-xs-12">
                            <div class="form-group">
                                <label>Chart Type:</label>
                                <asp:DropDownList ID="ddlChartType" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Chart Palette:</label>
                                <asp:DropDownList ID="ddlChartPalette" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Chart Style:</label>
                                <asp:DropDownList ID="ddlChartStyle" runat="server" CssClass="form-control">
                                    <asp:ListItem>2D</asp:ListItem>
                                    <asp:ListItem>3D</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm=6 col-xs-12">
                            <div class="form-group">
                                <label>Series:</label>
                                <asp:DropDownList ID="ddlSeries" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlSeries_SelectedIndexChanged"></asp:DropDownList>
                                <asp:HiddenField ID="hfSeries" runat="server" />
                                <asp:CheckBoxList ID="cbSeries" runat="server" Height="70px">
                                </asp:CheckBoxList>
                                <asp:LinkButton ID="lbSeriesData" runat="server" Text="Select Series Data" OnClick="lbSeriesData_Click" CssClass="btn btn-primary"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm=6 col-xs-12">
                            <div class="form-group">
                                <label>Axis:</label>
                                <asp:DropDownList ID="ddlAxis" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlAxis_SelectedIndexChanged"></asp:DropDownList>
                                <asp:HiddenField ID="hfAxis" runat="server" />
                                <asp:CheckBoxList ID="cbAxis" runat="server" Height="70px">
                                </asp:CheckBoxList>
                                <asp:LinkButton ID="lbAxisData" runat="server" Text="Select Axis Data" OnClick="lbAxisData_Click" CssClass="btn btn-primary"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm=6 col-xs-12">
                            <div class="form-group">
                                <label>Filter:</label>
                                <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged"></asp:DropDownList>
                                <asp:HiddenField ID="hfFilter" runat="server" />
                                <asp:CheckBoxList ID="cbFilter" runat="server" Height="70px">
                                </asp:CheckBoxList>
                                <asp:LinkButton ID="lbFilterData" runat="server" Text="Select Filter Data" OnClick="lbFilterData_Click" CssClass="btn btn-primary"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <h3></h3>
                </div>
            </asp:Panel>
        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>

