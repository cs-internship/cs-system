# User
Someone who can login to the system. Can be someone that is not involved in learning in the CS Internship program.
 
| Property | Type | Description |
---|---|---
DisplayName | String |  
RolesStr | String  | Learner, Company, CompanyCreator, ContentHero 
Learner | Learner


 # Learner
A user that is a part of CS Internship program and is in the middle of growing journey.

Property | Type | Description
---|---|---
Username | String  | just letters and digits and underscore
FirstName | String   
LastName | String   
Summary | String   
Status  | Enum.LearnerStatus | PendingInfo, Active, Inactive
IsActive | Boolean   
BirthCity | City   
LivingCity | City
Address | String   
AnimalCharacter | String   
MobileNo | String   
TelegramUsername | String   
EmailMicrosoft | String   
EmailGoogle | String   
EmailCrystallineSociety | String   
TwitterUsername | String   
LinkedInUrl | String   
GitHubUsername | String   
StackOverflowUsername | String   
EthereumPubKey | String   
TonPubKey | String   
TagStr | String   

# Badge
A badge is indication of a specific quality of a learner. For example if you participate in writing a document you may get a `document-creator` badge for each of the documents you write. Or if you participate in enough documentations you may get `documentation-guru` badge.
| Property | Type | Description |
---|---|---
Code | String
Title | String
Description | String | Having this badge indicates these qualities
Benefits | String | The benefits of having this badge
IsPermission | Boolean
Permission | String
Prerequisites | String
PrerequisitesJsonSourceUrl | String | Each badge has a github file which will be synced one-way from it.
PrerequisitesJson | String
Level | Enum.BadgeLevel | Bronze, Silver, Gold
IsApprovalRequired | Boolean

# Privilege
| Property | Type | Description |
---|---|---
Code | String
Title | String
Description | String
Power | String
Prerequisites | String
PrerequisitesJson | String
IsApprovalRequired | Boolean

# LearnerAchievement
| Property | Type | Description |
---|---|---
Learner | Learner
AchivementId | Guid | Badge or Previlege
AchieveDate | Date
AchieveType | Enum.AchieveType | Manual, Automatic
ApproverType | AchievementApproverType | Maual, Automatic
Approver | Learner?
Description | String | Why and how this badge achieved.

# LearnerAchievementProof
| Property | Type | Description |
---|---|---
Learner | Learner
AchievementId | Guid
LearnerAchievement | LearnerAchievement
ProofType | Enum.ProofType | `Activity, Badge, Approver`
Title | String | A human undrestanable title generated based on proof information.
Approver | Learner
ApproverNote | String
ProofBadge | LearnerBadge
ProofBadgeJson | String | A copy of ProofLearnerBadge at the moment.
ProofActivity | LearnerActivity
ProofActivityJson | String | A copy of ProofLearnerActivity at the moment.
CreateDateTime | DateTimeOffset | The time the proof is created
ProofDateTime | DateTimeOffSet | The time the proof is considered `Final`
Status | Enum.ProofStatus | `Draft, Final`


# LearnerActivity
| Property | Type | Description |
---|---|---
Learner | Learner
ActivityType | Enum.ActivityType | Unknown, OperationMeetingCoordination
ActivityTypeText | String | If unknown, this is the helping information
Title | String
Description | String
RefUrl | String
RefInfoJson | String | More information about the ref eg. starting second (if video).
ActivityDateTime | DateTimeOffset
ActivityStatus | Enum.ActivityStatus | Pending, Approved, Rejected
Approver | Learner
ApproveDateTime | DateTimeOffset

# Praisal System
The Praisal System is designed to specify the requirements to pick a learner for a specific situation.
## RequirementSet
| Property | Type | Description |
---|---|---
Badges | BadgeRequirement[]
Activities | ActivityRequirement[]
Approvers | ApproverRequirement[]

A `RequirementSet` is a set of requirements that should be satified on a learner to accept for a situation. These requirements are one of these 3 types:
- Badge Requirements
- Activity Requirements
- Approver Requirements

## BadgeRequirement
| Property | Type | Description |
---|---|---
Badge | Badge |
Count | Integer
## ActivityRequirement
| Property | Type | Description |
---|---|---
ActivityType | Enum.ActivityType
Count | Integer
## ApproverRequirement
| Property | Type | Description |
---|---|---
ActivityType | Enum.ActivityType
Badges | BadgeRequirement[]
Activities | ActivityRequirement[]
ApproverCount | Integer