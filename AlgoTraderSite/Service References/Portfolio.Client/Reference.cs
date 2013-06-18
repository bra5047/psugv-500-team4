﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlgoTraderSite.Portfolio.Client {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Portfolio.Client.IPortfolioManager")]
    public interface IPortfolioManager {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/GetOpenPositions", ReplyAction="http://tempuri.org/IPortfolioManager/GetOpenPositionsResponse")]
        AlgoTrader.portfolio.PositionMessage[] GetOpenPositions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/GetOpenPositions", ReplyAction="http://tempuri.org/IPortfolioManager/GetOpenPositionsResponse")]
        System.Threading.Tasks.Task<AlgoTrader.portfolio.PositionMessage[]> GetOpenPositionsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/GetPosition", ReplyAction="http://tempuri.org/IPortfolioManager/GetPositionResponse")]
        AlgoTrader.portfolio.PositionMessage GetPosition(string SymbolName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/GetPosition", ReplyAction="http://tempuri.org/IPortfolioManager/GetPositionResponse")]
        System.Threading.Tasks.Task<AlgoTrader.portfolio.PositionMessage> GetPositionAsync(string SymbolName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/sell", ReplyAction="http://tempuri.org/IPortfolioManager/sellResponse")]
        void sell(string symbolName, int quantity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/sell", ReplyAction="http://tempuri.org/IPortfolioManager/sellResponse")]
        System.Threading.Tasks.Task sellAsync(string symbolName, int quantity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/buy", ReplyAction="http://tempuri.org/IPortfolioManager/buyResponse")]
        void buy(string symbolName, int quantity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/buy", ReplyAction="http://tempuri.org/IPortfolioManager/buyResponse")]
        System.Threading.Tasks.Task buyAsync(string symbolName, int quantity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/getAvailableCash", ReplyAction="http://tempuri.org/IPortfolioManager/getAvailableCashResponse")]
        double getAvailableCash();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPortfolioManager/getAvailableCash", ReplyAction="http://tempuri.org/IPortfolioManager/getAvailableCashResponse")]
        System.Threading.Tasks.Task<double> getAvailableCashAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPortfolioManagerChannel : AlgoTraderSite.Portfolio.Client.IPortfolioManager, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PortfolioManagerClient : System.ServiceModel.ClientBase<AlgoTraderSite.Portfolio.Client.IPortfolioManager>, AlgoTraderSite.Portfolio.Client.IPortfolioManager {
        
        public PortfolioManagerClient() {
        }
        
        public PortfolioManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PortfolioManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PortfolioManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PortfolioManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AlgoTrader.portfolio.PositionMessage[] GetOpenPositions() {
            return base.Channel.GetOpenPositions();
        }
        
        public System.Threading.Tasks.Task<AlgoTrader.portfolio.PositionMessage[]> GetOpenPositionsAsync() {
            return base.Channel.GetOpenPositionsAsync();
        }
        
        public AlgoTrader.portfolio.PositionMessage GetPosition(string SymbolName) {
            return base.Channel.GetPosition(SymbolName);
        }
        
        public System.Threading.Tasks.Task<AlgoTrader.portfolio.PositionMessage> GetPositionAsync(string SymbolName) {
            return base.Channel.GetPositionAsync(SymbolName);
        }
        
        public void sell(string symbolName, int quantity) {
            base.Channel.sell(symbolName, quantity);
        }
        
        public System.Threading.Tasks.Task sellAsync(string symbolName, int quantity) {
            return base.Channel.sellAsync(symbolName, quantity);
        }
        
        public void buy(string symbolName, int quantity) {
            base.Channel.buy(symbolName, quantity);
        }
        
        public System.Threading.Tasks.Task buyAsync(string symbolName, int quantity) {
            return base.Channel.buyAsync(symbolName, quantity);
        }
        
        public double getAvailableCash() {
            return base.Channel.getAvailableCash();
        }
        
        public System.Threading.Tasks.Task<double> getAvailableCashAsync() {
            return base.Channel.getAvailableCashAsync();
        }
    }
}
