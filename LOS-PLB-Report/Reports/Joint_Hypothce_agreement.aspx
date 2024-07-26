<%@ Page Title="Joint Hypothce agreement" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Joint_Hypothce_agreement.aspx.cs" Inherits="LOS_PLB_Report.Reports.Joint_Hypothce_agreement" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="responsive-viewer">
        <center> 
            <rsweb:ReportViewer KeepSessionAlive="false" AsyncRendering="false" ProcessingMode="Local" ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" ShowPrintButton="true" ShowBackButton="true" BackColor="#CCCCCC" CssClass="printer" 
            InternalBorderStyle="None" PageCountMode="Actual" ShowZoomControl="False"></rsweb:ReportViewer>
        </center>
    </div>
</asp:Content>
