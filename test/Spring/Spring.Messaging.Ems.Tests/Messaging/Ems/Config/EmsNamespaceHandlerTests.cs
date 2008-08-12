#region License

/*
 * Copyright � 2002-2007 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System.Collections;
using NUnit.Framework;
using Rhino.Mocks;
using Spring.Context;
using Spring.Context.Support;
using Spring.Messaging.Ems.Listener;
using Spring.Objects.Factory.Xml;
using TIBCO.EMS;

#endregion

namespace Spring.Messaging.Ems.Config
{
    /// <summary>
    /// This class contains tests for 
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <version>$Id:$</version>
    [TestFixture]
    public class EmsNamespaceHandlerTests
    {

        private static string DEFAULT_CONNECTION_FACTORY = "ConnectionFactory";

        private static string EXPLICIT_CONNECTION_FACTORY = "testConnectionFactory";


        private IApplicationContext ctx;

        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            NamespaceParserRegistry.RegisterParser(typeof(EmsNamespaceParser));
            ctx = new XmlApplicationContext(ReadOnlyXmlTestResource.GetFilePath("EmsNamespaceHandlerTests.xml", GetType()));
            mocks = new MockRepository();
        }

        [Test]
        public void Registered()
        {
            Assert.IsNotNull(NamespaceParserRegistry.GetParser("http://www.springframework.net/ems"));
        }

        [Test]
        public void ObjectsCreated()
        {
            IDictionary containers = ctx.GetObjectsOfType(typeof(SimpleMessageListenerContainer));
            Assert.AreEqual(3, containers.Count);
        }

        [Test]
        public void ContainerConfiguration()
        {
            IDictionary containers = ctx.GetObjectsOfType(typeof (SimpleMessageListenerContainer));
            ConnectionFactory defaultConnectionFactory = (ConnectionFactory) ctx.GetObject(DEFAULT_CONNECTION_FACTORY);
            ConnectionFactory explicitConnectionFactory = (ConnectionFactory) ctx.GetObject(EXPLICIT_CONNECTION_FACTORY);
            

            int defaultConnectionFactoryCount = 0;
		    int explicitConnectionFactoryCount = 0;
            foreach (DictionaryEntry dictionaryEntry in containers)
            {
                SimpleMessageListenerContainer container = (SimpleMessageListenerContainer) dictionaryEntry.Value;
                if (container.ConnectionFactory.Equals(defaultConnectionFactory))
                {
                    defaultConnectionFactoryCount++;
                }
                else if (container.ConnectionFactory.Equals(explicitConnectionFactory))
                {
                    explicitConnectionFactoryCount++;
                }
            }

            Assert.AreEqual(1, defaultConnectionFactoryCount, "1 container should have the default connectionFactory");
            Assert.AreEqual(2, explicitConnectionFactoryCount, "2 containers should have the explicit connectionFactory");

        }

 

    }
}