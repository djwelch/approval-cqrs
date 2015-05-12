using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FSharp.Collections;
using System.Linq;

namespace Approval.Test
{
    [TestClass]
    public class ProfileTest
    {
        [TestMethod]
        public void Profile()
        {
            for (int approval = 0; approval < 100; ++approval)
            {
                var events = Enumerable.Empty<Event>();
                var approvalId = Guid.NewGuid();
                for (int participant = 0; participant < 10; ++participant)
                {
                    var participantId = Guid.NewGuid();
                    events = events.Union(Domain.Handle(events, Command.NewRequireApprovalFromParticipant(approvalId, participantId)).Item1);
                    if (participant % 2 == 0 || approval % 5 == 1)
                    {
                        events = events.Union(Domain.Handle(events, Command.NewParticipantApproves(approvalId, participantId)).Item1);
                    }
                }
                events = events.Union(Domain.Handle(events, Command.NewCloseApproval(approvalId)).Item1);
            }

        }

        [TestMethod, Ignore]
        public void Stress()
        {
            for (int approval = 0; approval < 100; ++approval)
            {
                var approvalId = Guid.NewGuid();
                for (int participant = 0; participant < 10; ++participant)
                {
                    var participantId = Guid.NewGuid();
                    Domain.Execute(approvalId, Command.NewRequireApprovalFromParticipant(approvalId, participantId));

                    if (participant % 2 == 0 || approval % 5 == 1)
                    {
                        Domain.Execute(approvalId, Command.NewParticipantApproves(approvalId, participantId));
                    }
                }
                Domain.Execute(approvalId, Command.NewCloseApproval(approvalId));
            }

        }

        [TestMethod, Ignore]
        public void Infrastructure()
        {
            var approvalId = Guid.NewGuid();
            var participantId = Guid.NewGuid();
            Domain.Execute(approvalId, Command.NewRequireApprovalFromParticipant(approvalId, participantId));
            Domain.Execute(approvalId, Command.NewParticipantApproves(approvalId, participantId));
            Domain.Execute(approvalId, Command.NewCloseApproval(approvalId));
        }
    }
}
