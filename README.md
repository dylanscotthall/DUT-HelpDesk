# DUT-HelpDesk

## Team BitBots
|Members|
|---|
|Jishen Harilal|
|Evan Teague|
|Dylan Hall|
|Ricardo Sánchez|
|Fawwaz Osman|
|Deylin Nair|
|Keval Rohith|
|Ayrton Mulqueeny|
---
## DUT-HELPDESK | TICKET STATUS RUNDOWN

> Statuses need to be added to the statuses table.

### Statuses:
|No.|Name|Check|
|---|---|---|
|0|Available/Pending|0 technicians assigned|
|1|Active|>0 technicians assigned|
|2|Closed|Ticket.ClosedDate != null|


### Status needs to be checked for update when a ticket is:

1. Submitted (set to Available)
2. Accepted by technician (set to Active)
3. Unaccepted by technician (check if technicianCount <1, then set to available... else stays Active)
4. Closed by a technician (set to Closed)

### Questions:
```
Q1. Can tickets be reopened? 
A: No
```
```
Q2. Are there any other statuses? 
A: It's possible that we may introduce other statuses in the future, code with that in mind.
```

> any more questions to list....?
