# DUT-HelpDesk

## 🤖 Team BitBots
|<sub>Jishen Harilal|<sub>Evan Teague|<sub>Dylan Hall|<sub>Ricardo Sánchez|<sub>Fawwaz Osman|<sub>Deylin Nair|<sub>Keval Rohith|<sub>Ayrton Mulqueeny|
|---|-|-|-|-|-|-|-|

---
### ✅ POE Rubric Responsibility
#### 🧑‍💻 Can be assigned to individual:
|Section|Assigned Member|
|-------|-----|
|Business Communication        ||
|Systems Analysis and Design   ||
|Architecture                  ||
|Application security          ||
|Operations                    ||
|Secure application development||
|DevOps                        ||

#### 🏢 Group Effort
|Section|
|-------|
|Digital Law and Ethics         |
|Basic Programming Skills       |
|Intermediate Programming Skills|
|Advanced Programming Skills    |

---
### DUT-HELPDESK | TICKET STATUS RUNDOWN
> This section in the Readme is temporary.  
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
