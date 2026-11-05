Imports System.IO
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.Collections.Generic
'Imports DocumentFormat.OpenXml.Packaging
'Imports DocumentFormat.OpenXml.Wordprocessing

Namespace MailMerge
    Public Class MailMerger
        '     Public Property Template() As FileInfo
        '         Get
        '             Return m_Template
        '         End Get
        '         Private Set(value As FileInfo)
        '             m_Template = Value
        '         End Set
        '     End Property
        '     Private m_Template As FileInfo
        '     Public ReadOnly Property XMLNS() As XNamespace
        '         Get
        '             Return XNamespace.[Get](WORDMLNS)
        '         End Get
        '     End Property

        '     Public Const WORDMLNS As String = "http://schemas.openxmlformats.org/wordprocessingml/2006/main"

        '     Private Enum MergeStatus
        '         None
        '         HaveOld
        '         HaveNew
        '         HaveBegin
        '         HaveName
        '         HaveSeperator
        '         HaveData
        '     End Enum

        '     Public Sub New(templatePath As String)
        '         If String.IsNullOrEmpty(templatePath) Then
        '             Throw New ArgumentNullException("The Template cannot be null.")
        '         End If

        '         Dim template__1 As New FileInfo(templatePath)
        '         If Not template__1.Exists Then
        '             Throw New ArgumentNullException("The Template File does not exists.")
        '         End If
        '         If template__1.Extension <> ".docx" Then
        '             Throw New ArgumentException("The Template must be of type docx.")
        '         End If
        '         Template = template__1
        '     End Sub

        '     Private Function GetFieldData(text As String) As String
        '         Dim newText As String = text
        '         If newText.Length > 0 AndAlso newText.Substring(0, 1) = """" Then
        '             newText = newText.Substring(1)
        '             If newText.Length > 1 AndAlso newText.Substring(newText.Length - 1, 1) = """" Then
        '                 newText = newText.Substring(0, newText.Length - 1)
        '             End If
        '         End If

        '         Return newText
        '     End Function

        '     Public Function MergeReplace(newBody As XElement, row As DataRow) As XElement
        '         ' Get all Mail Merge Fields 
        'Dim mailMergeFields As IList(Of XElement) = (From el In newBody.Descendants() Where (el.Name = (XMLNS + "r") OrElse el.Attribute(XMLNS + "instr") IsNot Nothing)el).ToList()

        '         Dim status As MergeStatus = MergeStatus.None
        '         Dim status0 As XElement = Nothing, status1 As XElement = Nothing, status2 As XElement = Nothing, status3 As XElement = Nothing, status4 As XElement = Nothing
        '         Dim fieldName As String = ""
        '         ' Replace all merge fields with Data 
        '         For Each field As XElement In mailMergeFields
        '             If (status = MergeStatus.None) AndAlso (field.Element(XMLNS + "fldChar") IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType") IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType").Value = "begin") Then
        '                 status = MergeStatus.HaveBegin
        '                 status0 = field
        '             ElseIf (status = MergeStatus.None) AndAlso (field.Attribute(XMLNS + "instr") IsNot Nothing) Then
        '                 status = MergeStatus.HaveOld
        '                 status0 = field
        '                 fieldName = GetFieldData(field.Attribute(XMLNS + "instr").Value.Replace("MERGEFIELD", String.Empty).Trim())
        '             ElseIf (status = MergeStatus.HaveBegin) AndAlso (field.Element(XMLNS + "instrText") IsNot Nothing) AndAlso (field.Element(XMLNS + "instrText").Value IsNot Nothing) AndAlso (field.Element(XMLNS + "instrText").Value.Trim().Length > 10) AndAlso (field.Element(XMLNS + "instrText").Value.Trim().Substring(0, 10) = "MERGEFIELD") Then
        '                 status = MergeStatus.HaveName
        '                 status1 = field
        '                 fieldName = GetFieldData(field.Element(XMLNS + "instrText").Value.Replace("MERGEFIELD", String.Empty).Trim())
        '             ElseIf (status = MergeStatus.HaveName) AndAlso (field.Element(XMLNS + "fldChar") IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Value IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType").Value <> "") AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType").Value.Trim() = "separate") Then
        '                 status = MergeStatus.HaveSeperator
        '                 status2 = field
        '             ElseIf (status = MergeStatus.HaveSeperator) AndAlso (field.Element(XMLNS + "rPr") IsNot Nothing) AndAlso (field.Element(XMLNS + "rPr").NextNode IsNot Nothing) AndAlso ((TryCast(field.Element(XMLNS + "rPr").NextNode, XElement).Value IsNot Nothing)) Then
        '                 status = MergeStatus.HaveData
        '                 status3 = field
        '             ElseIf (status = MergeStatus.HaveData) AndAlso (field.Element(XMLNS + "fldChar") Is Nothing) AndAlso (field.Element(XMLNS + "rPr") IsNot Nothing) AndAlso (status3 IsNot Nothing) Then
        '                 status3.Element(XMLNS + "rPr").Add(field.Element(XMLNS + "rPr").Nodes())
        '                 field.Remove()
        '             ElseIf (status = MergeStatus.HaveData) AndAlso (field.Element(XMLNS + "fldChar") IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType") IsNot Nothing) AndAlso (field.Element(XMLNS + "fldChar").Attribute(XMLNS + "fldCharType").Value = "end") Then
        '                 status = MergeStatus.HaveNew
        '                 status4 = field
        '             Else
        '                 status = MergeStatus.None
        '                 status0 = Nothing
        '                 status1 = Nothing
        '                 status2 = Nothing
        '                 status3 = Nothing
        '                 status4 = Nothing
        '                 fieldName = ""
        '             End If

        '             If status = MergeStatus.HaveOld OrElse status = MergeStatus.HaveNew Then
        '                 If row.Table.Columns.Contains(fieldName) Then
        '                     Dim newElement As XElement = Nothing, newElement2 As XElement = Nothing
        '                     If status = MergeStatus.HaveOld Then
        '                         newElement = field.Descendants(XMLNS + "r").First()
        '                         newElement.Descendants(XMLNS + "t").First().Value = row.Field(Of String)(fieldName)
        '                     Else
        '                         newElement = New XElement(XMLNS + "fldSimple")
        '                         newElement.SetAttributeValue(XMLNS + "instr", Convert.ToString("MERGEFIELD ") & fieldName)
        '                         newElement2 = New XElement(XMLNS + "t", row.Field(Of String)(fieldName))
        '                         newElement.Add(newElement2)
        '                     End If
        '                     If status = MergeStatus.HaveOld Then
        '                         status0.ReplaceWith(newElement)
        '                     Else
        '                         status3.Element(XMLNS + "rPr").NextNode.ReplaceWith(newElement)
        '                         status0.Remove()
        '                         status1.Remove()
        '                         status2.Remove()
        '                         status4.Remove()
        '                     End If
        '                 End If
        '                 status = MergeStatus.None
        '             End If
        '         Next
        '         Return newBody
        '     End Function

        '     Public Sub MailMerge(row As DataRow, targetFileName As String)
        '         If row Is Nothing Then
        '             Throw New ArgumentNullException("The Data Row cannot be null.")
        '         End If
        '         If String.IsNullOrEmpty(targetFileName) Then
        '             Throw New ArgumentNullException("The Target File Name cannot be null.")
        '         End If
        '         ' if (new FileInfo(targetFileName).Exists) throw new ArgumentOutOfRangeException("The Target File Name already exists."); 

        '         ' Open Template 
        '         Dim sourceBytes As Byte() = File.ReadAllBytes(Template.FullName)
        '         Using _workingMemoryStream As New MemoryStream()
        '             ' Load into memory 
        '             _workingMemoryStream.Write(sourceBytes, 0, sourceBytes.Length)

        '             Using wordDocument As WordprocessingDocument = WordprocessingDocument.Open(_workingMemoryStream, True)
        '                 Dim newBody As XElement = XElement.Parse(wordDocument.MainDocumentPart.Document.Body.OuterXml)
        '                 newBody = MergeReplace(newBody, row)
        '                 wordDocument.MainDocumentPart.Document.Body = New Body(newBody.ToString())
        '                 wordDocument.MainDocumentPart.Document.Save()

        '                 For Each x As HeaderPart In wordDocument.MainDocumentPart.HeaderParts
        '                     newBody = XElement.Parse(x.Header.OuterXml)
        '                     newBody = MergeReplace(newBody, row)
        '                     x.Header = New Header(newBody.ToString())
        '                     x.Header.Save()
        '                 Next

        '                 For Each x As FooterPart In wordDocument.MainDocumentPart.FooterParts
        '                     newBody = XElement.Parse(x.Footer.OuterXml)
        '                     newBody = MergeReplace(newBody, row)
        '                     x.Footer = New Footer(newBody.ToString())
        '                     x.Footer.Save()
        '                 Next

        '                 Dim settings As XElement = Nothing
        '                 Dim settingsPart As DocumentSettingsPart = Nothing
        '                 If wordDocument.MainDocumentPart.GetPartsOfType(Of DocumentSettingsPart)().Count() > 0 Then
        '                     settingsPart = wordDocument.MainDocumentPart.GetPartsOfType(Of DocumentSettingsPart)().First()
        '                     If settingsPart IsNot Nothing Then
        '                         If settingsPart.GetPartsOfType(Of MailMergeRecipientDataPart)().Count() > 0 Then
        '                             Dim mmrPart As MailMergeRecipientDataPart = settingsPart.GetPartsOfType(Of MailMergeRecipientDataPart)().First()
        '                             settingsPart.DeletePart(mmrPart)
        '                             ' Delete refrence to Mail Merge Data sources 
        '                             settings = XElement.Parse(settingsPart.RootElement.OuterXml)
        '                         End If
        '                     End If
        '                 End If

        '                 If settings IsNot Nothing Then
        '			Dim mailMergeElements As IList(Of XElement) = (From el In settings.Descendants() Where el.Name = (XMLNS + "mailMerge")el).ToList()

        '                     For Each field As XElement In mailMergeElements
        '                         field.Remove()
        '                     Next
        '                 End If

        '                 If settingsPart IsNot Nothing AndAlso settings IsNot Nothing Then
        '                     settingsPart.RootElement.InnerXml = settings.ToString()
        '                     settingsPart.RootElement.Save()
        '                 End If

        '                 ' Save in output directory 
        '                 ' Create a new document based on updated template 
        '                 Using fileStream As New FileStream(targetFileName, FileMode.Create)
        '                     _workingMemoryStream.WriteTo(fileStream)
        '                 End Using
        '             End Using
        '         End Using
        '     End Sub
    End Class
End Namespace
