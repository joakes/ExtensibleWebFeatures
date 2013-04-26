<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebChat.ascx.cs" Inherits="WebFeatures.UserControls.WebChat" %>
<%@ Register TagPrefix="custom" Namespace="WebFeatures.Infrastructure" Assembly="WebFeatures" %>

<h2>Web Chat</h2>
<a ID="ChatAnchor" runat="server" href="#" target="_blank">Chat now!</a>

<custom:ScriptReference runat="server" Script="webChat.js" />