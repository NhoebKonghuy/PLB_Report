<%@ Page Title="Receive & Return Title Deed Receipt" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Receive_&_Return_Title_Deed_Receipt.aspx.cs" Inherits="LOS_PLB_Report.Reports.Colleteral_Receipt_Update" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="responsive-viewer">
        <center> 
            <rsweb:ReportViewer  KeepSessionAlive="true" AsyncRendering="false" ProcessingMode="Local" ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" ShowPrintButton="true" ShowBackButton="true" BackColor="#CCCCCC" CssClass="printer" 
            InternalBorderStyle="None" PageCountMode="Actual" ShowZoomControl="False"></rsweb:ReportViewer>
        </center>
    </div>
</asp:Content>

