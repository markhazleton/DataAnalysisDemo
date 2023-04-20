<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChartConfiguration.ascx.vb" Inherits="controls_ChartConfiguration" %>
<div class="panel panel-info">

    <div class="panel-heading">
        <h3>Chart Configuration</h3>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm=6 col-xs-12">
                <div class="form-group">
                    <label>Title:</label>
                    <asp:TextBox ID="tbTitle" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>SubTitle:</label>
                    <asp:TextBox ID="tbSubTitle" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-6 col-md-6 col-sm=6 col-xs-12">
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
                <div class="form-group">
                    <label>Background Image:</label>
                    <asp:DropDownList ID="ddlBackgroundImage" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="">No Image</asp:ListItem>
                        <asp:ListItem Value="~/images/WorldGlobe.jpg">WorldGlobe</asp:ListItem>
                        <asp:ListItem Value="~/images/slider_img_1.jpg">Control Origins 1</asp:ListItem>
                        <asp:ListItem Value="~/images/slider_img_2.jpg">Control Origins 2</asp:ListItem>
                        <asp:ListItem Value="~/images/slider_img_3.jpg">Control Origins 3</asp:ListItem>
                        <asp:ListItem Value="~/images/slider_img_4.jpg">Control Origins 4</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer">
        <h3></h3>
    </div>
</div>
