using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public class Class1 : StyleSelector
    {
        public Style FileMessageStyle { get; set; }
        public Style TextMessageStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ChatMessage chatMessage)
            {
                if (chatMessage.MessageType == MessageType.File)
                {
                    return FileMessageStyle;
                }
            }

            return TextMessageStyle; // Default style for other message types
        }
    }
}
