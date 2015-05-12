using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Approval.Test.CloseApproval
{
    [TestClass]
    public class CannotCloseAnApprovalThatDoesNotExist : DomainTest
    {
        private readonly Guid Id = Guid.NewGuid();
        
        protected override Command When()
        {
            return Command.NewCloseApproval(Id);
        }

        [TestMethod]
        public void AnExceptionWasThrown()
        {
            Assert.IsInstanceOfType(this.Exception, typeof(ApprovalNotFoundException));
        }

        [TestMethod]
        public void NoEventIsRaised()
        {
            Assert.AreEqual(0, Events.Length);
        }
    }
}
