using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Serialization;

namespace MIDIator.Web
{
    public class SignalRContractResolver : IContractResolver
    {
        private readonly string _assemblyName;
        private readonly IContractResolver _injectedContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver(IContractResolver injectedContractResolver)
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _injectedContractResolver = injectedContractResolver;
            _assemblyName = "SignalR";
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.FullName.ToLower().Contains(_assemblyName.ToLower()))
            {
                return _defaultContractSerializer.ResolveContract(type);

            }

            return _injectedContractResolver.ResolveContract(type);
        }

    }
}
