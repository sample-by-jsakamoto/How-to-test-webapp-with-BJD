Feature: SpecFlowFeature1
	regist and receive confirmation mail.

Scenario: Attendee Registraton
	Given There is no mails received
	When Open URL http://localhost:49469/
	And Enter text "bar" into #Name
	And Enter text "bar@example.com" into #Email
	And Click #Regist
	Then The text "Complete" is present at #Caption
	And One mail received as bellow
	| field   | value                     |
	| from    | foo@example.com           |
	| to      | bar@example.com           |
	| subject | Registration Comfirmation |
	"""
	Hi bar,
	Thank you for your attend!
	"""
