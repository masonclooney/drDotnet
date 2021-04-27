using System.Collections.Generic;

namespace drDotnet.Services.SignalrHub.MessageObjects.Contact
{
    public class UpdateContacts
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public List<long> Ids { get; set; }
    }
}