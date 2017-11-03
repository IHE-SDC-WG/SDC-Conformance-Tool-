Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Xml.Xsl
Imports System.Xml
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Namespace SDCExtension

    Public Class TraceSoapExtension
        Inherits SoapExtension

        Private oldStream As Stream
        Private newStream As Stream
        Private Shared msg As String 'custom return message
        Private Shared m_xmlRequest As XmlDocument

        Private Shared m_responseWriter As String  ' function that builds response

        Public Shared WriteOnly Property ReturnMessage() As String
            Set(ByVal value As String)
                msg = value
            End Set
        End Property
        Public Shared ReadOnly Property XmlRequest() As XmlDocument
            Get
                Return m_xmlRequest
            End Get
        End Property

       
        Public Shared Property ResponseWriter() As String
            Get
                Return m_responseWriter
            End Get
            Set(value As String)
                m_responseWriter = value
            End Set
        End Property
        Private Shared m_xmlResponse As XmlDocument
        Public Shared ReadOnly Property XmlResponse() As XmlDocument
            Get
                Return m_xmlResponse
            End Get
        End Property

        Public Overrides Function ChainStream(ByVal _stream As Stream) As Stream
            oldStream = _stream
            newStream = New MemoryStream()
            Return newStream
        End Function

        Public Shared Sub Reset()

        End Sub

        Public Overrides Sub ProcessMessage(ByVal message As SoapMessage)
            Select Case (message.Stage)
                'During SoapServerMessage processing, the BeforeSerialize stage occurs after the invocation to the XML Web service method returns, 
                'but prior to the return values being serialized and sent over the wire back to the client.
                Case SoapMessageStage.BeforeSerialize
                    Exit Sub
                    'During SoapServerMessage processing, the AfterSerialize stage occurs after an XML Web service 
                    'method returns and any return values are serialized into XML, 
                    'but prior to the SOAP message containing that XML is sent over the network.
                Case SoapMessageStage.AfterSerialize
                    If (msg = "fault") Then  'we want to display soap message

                    End If


                    m_xmlResponse = GetSoapEnvelope(newStream)
                    'manipulate newStream here?
                    Dim myEncoder As New System.Text.ASCIIEncoding
                    Dim bytes As Byte()
                    If (msg = "fault") Then
                        Dim xml As String = FormatXML(m_xmlResponse.InnerXml)
                        bytes = myEncoder.GetBytes(xml)
                        'ElseIf (ResponseWriter = "CreateRetrieveResponse") Then
                        '    bytes = myEncoder.GetBytes(CreateRetrieveResponse(msg))
                    ElseIf (ResponseWriter = "CreateRetrieveFault") Then
                        bytes = myEncoder.GetBytes(CreateRetrieveFault(msg))
                        'ElseIf (ResponseWriter = "CreateSubmitResponse") Then
                        '    bytes = myEncoder.GetBytes(CreateSubmitResponse(msg))
                        'ElseIf (ResponseWriter = "CreateRetrieveFault") Then
                        '   bytes = myEncoder.GetBytes(CreateRetrieveFault(msg))
                    Else
                        bytes = myEncoder.GetBytes(msg)
                    End If


                    newStream = New MemoryStream(bytes)
                    CopyStream(newStream, oldStream)

                    Exit Sub
                    'During SoapServerMessage processing, the BeforeDeserialize stage occurs after a network request 
                    'containing the SOAP message for an XML Web service method invocation is received by the Web 
                    'server, but prior to the SOAP message being deserialized into an object.
                Case SoapMessageStage.BeforeDeserialize
                    CopyStream(oldStream, newStream)  'oldstream is already filled in chainstream
                    m_xmlRequest = GetSoapEnvelope(newStream)   'xmlRequest should have the soaprequest

                    Exit Sub
                    'During SoapServerMessage processing, the AfterDeserialize stage occurs after a network request 
                    'containing a SOAP message representing an XML Web service method invocation is deserialized 
                    'into an object, but prior to the method on that object representing the XML Web service method 
                    'is called.
                Case SoapMessageStage.AfterDeserialize

                    Exit Sub
            End Select

        End Sub

        Private Function FormatXML(xml As String) As String
            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(xml)
            Dim sb As System.Text.StringBuilder = New Text.StringBuilder()
            Dim settings As System.Xml.XmlWriterSettings = New XmlWriterSettings()

            settings.Indent = True
            settings.IndentChars = "\t"
            settings.NewLineChars = "\r\n"
            settings.NewLineHandling = System.Xml.NewLineHandling.Replace
            settings.OmitXmlDeclaration = True
            Dim writer As System.Xml.XmlWriter
            writer = System.Xml.XmlWriter.Create(sb, settings)
            doc.Save(writer)
            Dim retval As String = sb.ToString()

            'format for html display
            retval = retval.Replace("<", "&lt;")
            retval = retval.Replace(">", "&gt;")
            retval = retval.Replace("\r\n", "<br/>")
            retval = retval.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")

            Return retval

        End Function

       

        Private Function CreateRetrieveResponse(ByVal result As String) As String
            Dim response As XElement = _
                  <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope"
                      xmlns:wsa="http://www.w3.org/2005/08/addressing">
                      <soap:Header>
                          <wsa:To>http://www.sharedapps.net/SDCShare/Services/FormManager</wsa:To>
                          <wsa:MessageID>urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050</wsa:MessageID>
                          <wsa:Action soap:mustUnderstand="1">urn:ihe:iti:rfd:2007:RetrieveFormResponse</wsa:Action>
                      </soap:Header>
                      <soap:Body>
                          <RetrieveFormResponse xmlns="urn:ihe:iti:rfd:2007">
                              <form>
                                  <SDCPackage>
                                      <XMLPackage>
                                          Success
                                      </XMLPackage>
                                  </SDCPackage>
                              </form>
                          </RetrieveFormResponse>
                      </soap:Body>
                  </soap:Envelope>


            Return response.ToString.Replace("Success", msg)
        End Function

        Private Function CreateRetrieveFault(ByVal message As String) As String
            Dim response As XElement = _
                  <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope"
                      xmlns:wsa="http://www.w3.org/2005/08/addressing">
                      <soap:Header>
                          <wsa:To>http://www.sharedapps.net/SDCShare/Services/FormManager</wsa:To>
                          <wsa:MessageID>urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050</wsa:MessageID>
                          <wsa:Action soap:mustUnderstand="1">urn:ihe:iti:rfd:2007:RetrieveFormResponse</wsa:Action>
                      </soap:Header>
                      <soap:Body>
                          <soap:Fault>
                              object
                          </soap:Fault>
                      </soap:Body>
                  </soap:Envelope>


            Return response.ToString.Replace("object", msg)
        End Function
        Private Function CreateSubmitResponse(ByVal msg As String) As String
            Return msg

            'Dim response As XElement = _
            '      <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope"
            '          xmlns:wsa="http://www.w3.org/2005/08/addressing">
            '          <soap:Header>
            '              <wsa:To>http://www.sharedapps.net/SDCShare/Services/FormManager</wsa:To>
            '              <wsa:MessageID>urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050</wsa:MessageID>
            '              <wsa:Action soap:mustUnderstand="1">urn:ihe:iti:rfd:2007:SubmitFormResponse</wsa:Action>
            '          </soap:Header>
            '          <soap:Body>
            '              <SubmitFormResponse xmlns="urn:ihe:iti:rfd:2007">
            '                 Success
            '              </SubmitFormResponse>
            '          </soap:Body>
            '      </soap:Envelope>


            'Return response.ToString.Replace("Success", msg)
        End Function

        Private Function CreateSubmitFault(ByVal message As String) As String
            Dim response As XElement = _
                  <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope"
                      xmlns:wsa="http://www.w3.org/2005/08/addressing">
                      <soap:Header>
                          <wsa:To>http://www.sharedapps.net/SDCShare/Services/FormReceiver</wsa:To>
                          <wsa:MessageID>urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050</wsa:MessageID>
                          <wsa:Action soap:mustUnderstand="1">urn:ihe:iti:rfd:2007:RetrieveFormResponse</wsa:Action>
                      </soap:Header>
                      <soap:Body>
                          <soap:Fault>
                              object
                          </soap:Fault>
                      </soap:Body>
                  </soap:Envelope>


            Return response.ToString.Replace("object", msg)
        End Function

        Private Function GetSoapEnvelope(ByVal instream As Stream) As XmlDocument
            Try
                Dim xml As XmlDocument = New XmlDocument()
                instream.Position = 0
                Dim reader As StreamReader = New StreamReader(instream)
                xml.LoadXml(reader.ReadToEnd())
                instream.Position = 0
                Return xml
            Catch ex As Exception

            End Try
        End Function



        Private Sub CopyStream(ByVal from As Stream, ByVal [to] As Stream)
            Dim reader As TextReader = New StreamReader(from)
            Dim writer As TextWriter = New StreamWriter([to])
            writer.WriteLine(reader.ReadToEnd())
            writer.Flush()
        End Sub



        Public Overrides Function GetInitializer(ByVal methodInfo As LogicalMethodInfo, ByVal attribute As SoapExtensionAttribute) As Object
            Return Nothing
        End Function



        Public Overrides Function GetInitializer(ByVal WebServiceType As Type) As Object
            Return Nothing
        End Function



        Public Overrides Sub Initialize(ByVal initializer As Object)

        End Sub
    End Class
End Namespace