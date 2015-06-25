using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Xml;

[AttributeUsage(AttributeTargets.Method)]
public class RegistryExtensionAttribute : SoapExtensionAttribute
{
    #region "--Variables--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Methods--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    public override Type ExtensionType
    {
        get { return typeof(RegistryExtension); }
    }

    public override int Priority
    {
        get
        {
            return 1;
        }
        set
        {
            this.Priority = value;
        }
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}

public class RegistryExtension : SoapExtension
{
    #region "--Variables--"

    #region "--Private--"

    private System.IO.Stream _outwardStream;
    private System.IO.Stream _inwardStream;

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Methods--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
    {
        return null;
    }

    public override object GetInitializer(Type serviceType)
    {
        return null;
    }

    public override void Initialize(object initializer)
    {
    }

    public override Stream ChainStream(Stream stream)
    {
       this._outwardStream = stream;
       this._inwardStream = new MemoryStream();
       return this._inwardStream;
    }

    public override void ProcessMessage(SoapMessage message)
    {
        string SOAPMessage;
        StreamReader Reader;
        StreamWriter Writer;
        XmlDocument SOAPPayload;
        XmlNodeList Root, Fault, OriginalFaultString;
        XmlNode FaultCode, FaultString, FaultActor, Detail;
        string FaultCodeValue, FaultStringValue, FaultActorValue;
        bool HandleErrorFlag;

        HandleErrorFlag = false;
        FaultCodeValue = string.Empty;
        FaultStringValue = string.Empty;
        FaultActorValue = string.Empty;

        switch (message.Stage)
        {
            case SoapMessageStage.BeforeSerialize:
                break;
            case SoapMessageStage.AfterSerialize:
                this._inwardStream.Position = 0;

                Reader = new StreamReader(this._inwardStream);
                Writer = new StreamWriter(this._outwardStream);

                SOAPMessage = Reader.ReadToEnd();
                SOAPPayload = new XmlDocument();
                SOAPPayload.LoadXml(SOAPMessage);
                Fault = SOAPPayload.GetElementsByTagName(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SOAP + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.Fault);

                if (Fault != null && Fault.Count > 0)
                {
                    OriginalFaultString = SOAPPayload.GetElementsByTagName(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.faultstring);

                    if (OriginalFaultString != null && OriginalFaultString.Count > 0)
                    {
                        if (OriginalFaultString[0].InnerText.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message))
                        {
                            HandleErrorFlag = true;
                            FaultCodeValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Code;
                            FaultStringValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message;
                        }
                        else if (OriginalFaultString[0].InnerText.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Message))
                        {
                            HandleErrorFlag = true;
                            FaultCodeValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Code;
                            FaultStringValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Message;
                        }
                        else if (OriginalFaultString[0].InnerText.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message))
                        {
                            HandleErrorFlag = true;
                            FaultCodeValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Code;
                            FaultStringValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message;
                        }
                        else if (OriginalFaultString[0].InnerText.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message))
                        {
                            HandleErrorFlag = true;
                            FaultCodeValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Code;
                            FaultStringValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message;
                        }
                        else
                        {
                            HandleErrorFlag = true;
                            FaultCodeValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Code;
                            FaultStringValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Message;
                        }
                    }

                    if (HandleErrorFlag == true)
                    {
                        Fault[0].InnerXml = string.Empty;
                        FaultActorValue = DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXWebservice + ":" + message.Action.Replace(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.URIs.SDMXWebservice, string.Empty);

                        FaultCode = SOAPPayload.CreateNode(XmlNodeType.Element, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.faultcode, string.Empty);
                        FaultCode.InnerText = FaultCodeValue;
                        Fault[0].AppendChild(FaultCode);

                        FaultString = SOAPPayload.CreateNode(XmlNodeType.Element, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.faultstring, string.Empty);
                        FaultString.InnerText = FaultStringValue;
                        Fault[0].AppendChild(FaultString);

                        FaultActor = SOAPPayload.CreateNode(XmlNodeType.Element, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.faultactor, string.Empty);
                        FaultActor.InnerText = FaultActorValue;
                        Fault[0].AppendChild(FaultActor);

                        Root = SOAPPayload.GetElementsByTagName(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SOAP + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.Envelope);

                        if (Root != null && Root.Count > 0)
                        {
                            ((XmlElement)Root[0]).SetAttribute(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.xmlns + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXError, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.URIs.SDMXError);
                            ((XmlElement)Root[0]).SetAttribute(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.xmlns + ":" + DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.Prefixes.SDMXWebservice, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Namespaces.URIs.SDMXWebservice);
                        }

                        Detail = SOAPPayload.CreateNode(XmlNodeType.Element, DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.SOAPTags.detail, string.Empty);
                        Detail.InnerText = SOAPPayload.OuterXml;
                        Fault[0].AppendChild(Detail);
                    }
                }

                Writer.Write(SOAPPayload.OuterXml);
                Writer.Flush();
                break;
            case SoapMessageStage.BeforeDeserialize:
                Reader = new StreamReader(this._outwardStream);
                Writer = new StreamWriter(this._inwardStream);

                SOAPMessage = Reader.ReadToEnd();

                Writer.Write(SOAPMessage);
                Writer.Flush();

                this._inwardStream.Position = 0;
                break;
            case SoapMessageStage.AfterDeserialize:
                break;
        }
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}