
using CommunityToolkit.Mvvm.Messaging.Messages;
using RotiNet.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotiNet.Class
{
    public class MyMessage : ValueChangedMessage<string>
    {
        public Cliente Cliente { get; set; }
        public Proveedor Proveedor { get; set; }

        public Producto Producto { get; set; }

        public Pedido Pedido { get; set; }

        public MyMessage(string value):base(value)
        {
                
        }
    }
}
