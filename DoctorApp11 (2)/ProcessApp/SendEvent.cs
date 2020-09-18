using Prism.Events;
using ProcessApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessApp
{
    public class SendEvent : PubSubEvent<SearchParam>
    {
    }
}
