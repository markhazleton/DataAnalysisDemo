<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ViewChart.aspx.vb" Inherits="ViewChart" %>

<%@ Register Src="~/controls/Chart.ascx" TagName="Chart" TagPrefix="uc1" %>

<%@ Register src="controls/ChartConfiguration.ascx" tagname="ChartConfiguration" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6 form-inline">
                    <h2>Select Configuration</h2>
                    <asp:DropDownList ID="ddlConfig" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlConfig_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                    <asp:LinkButton ID="cmd_refresh" runat="server" ClientIDMode="Static" OnClick="cmd_refresh_Click" Text="Update Chart Settings" CssClass="btn btn-info"></asp:LinkButton>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-md-6 col-sm-12 col-lg-6">
                <uc1:Chart ID="Chart1" runat="server" />
            </div>
            <div class="col-md-6 col-sm-12 col-lg-6">
                <uc2:ChartConfiguration ID="ChartConfiguration1" runat="server" />
            </div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="Server">
</asp:Content>

