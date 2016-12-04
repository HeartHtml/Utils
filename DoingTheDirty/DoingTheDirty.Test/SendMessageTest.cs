using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoingTheDirty.Test
{
    [TestClass]
    public class SendMessageTest
    {
        [TestMethod]
        public void SendTest()
        {
            DetermineNightMessage determineNightMessage = new DetermineNightMessage();

            determineNightMessage.Send();
        }
    }
}
