using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Inasync;

namespace HookAppUnitTest.Models
{
    public abstract class TestBase
    {

        [TestInitialize]
        public void Initialize()
        {
            //PrimitiveAssert : 常にログを出力する
            PrimitiveAssert.ConsoleLogging = true;
        }


    }
}
