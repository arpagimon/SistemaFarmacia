<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviewCorreo.aspx.cs" Inherits="SistemaFarmacia.PreviewCorreo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .btnPreviewG {
            display: inline-block;
            font-size: 14px;
            font-family: Verdana;
            background: #78407e;
            color: white;
            border: none;
            border-radius: 5px;
            padding: 5px 30px;
        }
        .btnPreviewG:hover{
            cursor:pointer;
        }
    </style>
    <script type="text/javascript">
        function cierraPreview() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divBotonera">
            <button id="btnSalirPreview" class="btnPreviewG" onclick="cierraPreview()">Salir Preview</button>
        </div>
        <div runat="server" id="divCorreo">
        </div>
    </form>
</body>
</html>
