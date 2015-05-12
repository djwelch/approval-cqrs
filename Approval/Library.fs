namespace Approval

type Command = 
    | RequireApprovalFromParticipant of Id:System.Guid*ParticipantId:System.Guid
    | ParticipantApproves of Id:System.Guid*ParticipantId:System.Guid
    | CloseApproval of Id:System.Guid
    
type Event  = 
    | ParticipantApprovalRequired of Id:System.Guid*ParticipantId:System.Guid
    | ParticipantApproved of Id:System.Guid*ParticipantId:System.Guid
    | ApprovalClosed of Id:System.Guid
    | ApprovalComplete of Id:System.Guid
   
exception ParticipantNotFoundException of string
exception ParticipantAlreadyRequiresApprovalException of string
exception ParticipantAlreadyApprovedException of string
exception ApprovalNotFoundException of string
exception ApprovalClosedException of string

module Domain =

    type private State = {
        Id: System.Guid;
        Closed: bool;
        Participants: Set<System.Guid>;
        Approvals: Set<System.Guid>;
    }
    
    let private apply item event = 
        match item, event with
            | None,    ParticipantApprovalRequired(id, participantId) -> { Id=id; Closed=false; Participants = [participantId] |> Set.ofList; Approvals = [] |> Set.ofList }
            | Some(a), ParticipantApprovalRequired(id, participantId) -> { a with Participants = a.Participants.Add(participantId) }
            | Some(a), ParticipantApproved(id, participantId)         -> { a with Approvals = a.Approvals.Add(participantId) }
            | Some(a), ApprovalClosed(id)                             -> { a with Closed = true }
            | Some(a), ApprovalComplete(id)                           -> a
            | _, _                                                    -> raise (System.Exception("approval not found"))
           
    let private (|ClosedApproval|_|) (o) =
        match o with
            | Some({ Id = _; Closed = true }) -> Some(o)
            | _ -> None
             
    let private (|CompleteApproval|_|) (o) =
        match o with
            | Some(o) -> if o.Participants.Count = o.Approvals.Count then Some(o) else None
            | _       -> None      

    let private (|AlmostCompleteApproval|_|) (o) =
        if o.Participants.Count = o.Approvals.Count+1 && o.Closed then Some(o) else None 

    let private apply2 acc v e =
         match v with
             | Some(i) -> (Some(apply acc e), Some(i+1))
             | None -> (Some(apply acc e), Some(0))

    let internal Handle events command = 
        let (item, version) = events |> Seq.fold (fun (acc, v) e -> (apply2 acc v e)) (None, None)
        
        let newEvents = 
            match item, command with
                | None, RequireApprovalFromParticipant(id, participantId) -> [ ParticipantApprovalRequired(id, participantId) ]

                | None, _ -> raise( ApprovalNotFoundException("approval not found"))
                
                | Some(a), ParticipantApproves(id, participantId) 
                    -> match a.Participants.Contains(participantId), a.Approvals.Contains(participantId), a with
                        | true, false, AlmostCompleteApproval(a) -> [ ParticipantApproved(id, participantId); ApprovalComplete(id) ]
                        | true, false, _                         -> [ ParticipantApproved(id, participantId) ]
                        | true, true, _                          -> raise( ParticipantAlreadyApprovedException("participant already approved"))
                        | false, _, _                            -> raise( ParticipantNotFoundException("participant not found"))
            
                | ClosedApproval(a), _ -> raise( ApprovalClosedException("approval is closed"))

                | Some(a), RequireApprovalFromParticipant(id, participantId)          
                    -> match a.Participants.Contains(participantId) with
                        | true  -> raise( ParticipantAlreadyRequiresApprovalException("participant already requires approval"))
                        | false -> [ ParticipantApprovalRequired(id, participantId) ]

                | CompleteApproval(a), CloseApproval(id) -> [ ApprovalClosed(id); ApprovalComplete(id) ]
                
                | Some(a), CloseApproval(id) -> [ ApprovalClosed(id) ]

        (newEvents, version)
    
    let Execute id command = 
        let store = Domain.Infrastructure.EventStore<Event>.Instance
        let prevEvents = store.Read (id)
        let (events, expectedVersion) = Handle prevEvents command
        if expectedVersion.IsNone then
            store.Append (id, System.Nullable(), events)
        else
            store.Append (id, System.Nullable(expectedVersion.Value), events)