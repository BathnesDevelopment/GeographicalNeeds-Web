﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data_Loading_Tool.GeographicalNeedsService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GeographicalNeedsService.IGeographicalNeedsService")]
    public interface IGeographicalNeedsService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetViewData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetViewDataResponse")]
        System.Data.DataTable GetViewData(int viewId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGeographicalNeedsService/GetViewData", ReplyAction="http://tempuri.org/IGeographicalNeedsService/GetViewDataResponse")]
        System.Threading.Tasks.Task<System.Data.DataTable> GetViewDataAsync(int viewId);
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
        
        public System.Data.DataTable GetViewData(int viewId) {
            return base.Channel.GetViewData(viewId);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> GetViewDataAsync(int viewId) {
            return base.Channel.GetViewDataAsync(viewId);
        }
    }
}
