<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CSV_DisplayTable.ascx.vb" Inherits="controls_CSV_DisplayTable" %>

<div class="table-responsive">
    <h2><asp:Literal ID="tblTitle" runat="server"></asp:Literal></h2>
    <table class="data_table table table-striped">
        <thead>
            <tr>
                <asp:Literal ID="rptHeader" runat ="server" Text=""></asp:Literal>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDataRows" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr><%# Container.DataItem%></tr>
                    </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
