﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data_Loading_Tool.GeographicalNeedsService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GeographicalNeedsService.IGeographicalNeedsService")]
    public interface IGeographicalNeedsService {
        
        // CODEGEN: Parameter 'GetDataResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetDataResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Data_Loading_Tool.GeographicalNeedsService.GetDataResponse GetData(Data_Loading_Tool.GeographicalNeedsService.GetDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetDataResponse")]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataResponse> GetDataAsync(Data_Loading_Tool.GeographicalNeedsService.GetDataRequest request);
        
        // CODEGEN: Parameter 'GetDataUsingDataContractResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetDataUsingDataContractResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse GetDataUsingDataContract(Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetDataUsingDataContractResponse")]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse> GetDataUsingDataContractAsync(Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest request);
        
        // CODEGEN: Parameter 'GetViewDataResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetViewData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetViewDataResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse GetViewData(Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetViewData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetViewDataResponse")]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse> GetViewDataAsync(Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetData", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public int value;
        
        public GetDataRequest() {
        }
        
        public GetDataRequest(int value) {
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetDataResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string GetDataResult;
        
        public GetDataResponse() {
        }
        
        public GetDataResponse(string GetDataResult) {
            this.GetDataResult = GetDataResult;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/DataAccessServices")]
    public partial class CompositeType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool boolValueField;
        
        private bool boolValueFieldSpecified;
        
        private string stringValueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool BoolValue {
            get {
                return this.boolValueField;
            }
            set {
                this.boolValueField = value;
                this.RaisePropertyChanged("BoolValue");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BoolValueSpecified {
            get {
                return this.boolValueFieldSpecified;
            }
            set {
                this.boolValueFieldSpecified = value;
                this.RaisePropertyChanged("BoolValueSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string StringValue {
            get {
                return this.stringValueField;
            }
            set {
                this.stringValueField = value;
                this.RaisePropertyChanged("StringValue");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetDataUsingDataContract", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetDataUsingDataContractRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Data_Loading_Tool.GeographicalNeedsService.CompositeType composite;
        
        public GetDataUsingDataContractRequest() {
        }
        
        public GetDataUsingDataContractRequest(Data_Loading_Tool.GeographicalNeedsService.CompositeType composite) {
            this.composite = composite;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetDataUsingDataContractResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetDataUsingDataContractResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Data_Loading_Tool.GeographicalNeedsService.CompositeType GetDataUsingDataContractResult;
        
        public GetDataUsingDataContractResponse() {
        }
        
        public GetDataUsingDataContractResponse(Data_Loading_Tool.GeographicalNeedsService.CompositeType GetDataUsingDataContractResult) {
            this.GetDataUsingDataContractResult = GetDataUsingDataContractResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetViewData", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetViewDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public int viewId;
        
        public GetViewDataRequest() {
        }
        
        public GetViewDataRequest(int viewId) {
            this.viewId = viewId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetViewDataResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetViewDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Data.DataTable GetViewDataResult;
        
        public GetViewDataResponse() {
        }
        
        public GetViewDataResponse(System.Data.DataTable GetViewDataResult) {
            this.GetViewDataResult = GetViewDataResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGeographicalNeedsServiceChannel : Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GeographicalNeedsServiceClient : System.ServiceModel.ClientBase<Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService>, Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService {
        
        public GeographicalNeedsServiceClient() {
        }
        
        public GeographicalNeedsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GeographicalNeedsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeographicalNeedsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeographicalNeedsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Data_Loading_Tool.GeographicalNeedsService.GetDataResponse Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetData(Data_Loading_Tool.GeographicalNeedsService.GetDataRequest request) {
            return base.Channel.GetData(request);
        }
        
        public string GetData(int value) {
            Data_Loading_Tool.GeographicalNeedsService.GetDataRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetDataRequest();
            inValue.value = value;
            Data_Loading_Tool.GeographicalNeedsService.GetDataResponse retVal = ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetData(inValue);
            return retVal.GetDataResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataResponse> Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetDataAsync(Data_Loading_Tool.GeographicalNeedsService.GetDataRequest request) {
            return base.Channel.GetDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataResponse> GetDataAsync(int value) {
            Data_Loading_Tool.GeographicalNeedsService.GetDataRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetDataRequest();
            inValue.value = value;
            return ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetDataUsingDataContract(Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest request) {
            return base.Channel.GetDataUsingDataContract(request);
        }
        
        public Data_Loading_Tool.GeographicalNeedsService.CompositeType GetDataUsingDataContract(Data_Loading_Tool.GeographicalNeedsService.CompositeType composite) {
            Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest();
            inValue.composite = composite;
            Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse retVal = ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetDataUsingDataContract(inValue);
            return retVal.GetDataUsingDataContractResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse> Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetDataUsingDataContractAsync(Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest request) {
            return base.Channel.GetDataUsingDataContractAsync(request);
        }
        
        public System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractResponse> GetDataUsingDataContractAsync(Data_Loading_Tool.GeographicalNeedsService.CompositeType composite) {
            Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetDataUsingDataContractRequest();
            inValue.composite = composite;
            return ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetDataUsingDataContractAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetViewData(Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest request) {
            return base.Channel.GetViewData(request);
        }
        
        public System.Data.DataTable GetViewData(int viewId) {
            Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest();
            inValue.viewId = viewId;
            Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse retVal = ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetViewData(inValue);
            return retVal.GetViewDataResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse> Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService.GetViewDataAsync(Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest request) {
            return base.Channel.GetViewDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<Data_Loading_Tool.GeographicalNeedsService.GetViewDataResponse> GetViewDataAsync(int viewId) {
            Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest inValue = new Data_Loading_Tool.GeographicalNeedsService.GetViewDataRequest();
            inValue.viewId = viewId;
            return ((Data_Loading_Tool.GeographicalNeedsService.IGeographicalNeedsService)(this)).GetViewDataAsync(inValue);
        }
    }
}