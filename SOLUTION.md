Design Pattern Used:

	MVC design pattern has been used to develop this LEADER BOARD MVP Web application.

Main Features incorporated:

	Leaderboard MVP Web application has been derived from a Web App template in Visual Studio with invidual user accounts authentication
	support. The following features has been incorporated as per the requirements.

	Two tables were added to the database
	a) Send Invitations
	b) Leaderboard Table

	1) Leaderboard MVP view to display competitors and allow referees to add, update, delete Competitors details.

	2) Everybody can see competitors details, who scored max, min, draw details but only Referees can perform actions.

	3) Referee's should be able to send private invitations using SMTP Client
		a) Render NotificationEmail.cshtml to string. This cshtml contains a link which will direct User to register.
		b) Register URL + Guid for each user and this will be dispalyed in Send Invitations Page.

	Leaderboard View:
		This view has been developed to display all competitors details along with competitor with max points, less points,
		and drawn points. The registered users i.e Referees can be able to do all operations on competitor information.

		View: ~/View/LeaderBoardTable/*.cshtml
		Model: ~/Models/LeaderBoardTable.cs
		Controller: ~/Controller/LeaderBoardTablesController.cs

		Methods - Index() and Create()

	Send Invitations:
		This view has been developed to display users for whom the e-mail invitations has been sent.

		View: ~/View/SendInvitation/*.cshtml
		Model: ~/Models/SendInvitations.cs
		Controller: ~/Controller/SendInvitationController.cs

		Methods - Create()

Assumptions:
	
	Referee are invidual users who has accounts to manage leader boards of competitors

	Referee's are only able to access leaderboard to create, edit, delete competiros information

	Referee's email will used as from e-mail id while sending private invitations to subscribers 
	who can be able to further register for this applications.

	Assumed H2H Contest is another Competitor name.

	Web.Config

		AppSettings

		a) Smtp Server need to be configured
		b) Register URL should be configured properly